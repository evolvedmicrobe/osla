using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml;
using System.Net.Mail;
using System.Net;


namespace Clarity
{
    /// <summary>
    /// These are the various options the engine is in, depending on what it is currently doing
    /// </summary>
    public enum RunningStates { Idle, Running, WaitingForNextExecutionTimePoint }
    /// <summary>
    /// This class is the runtime engine for Clarity
    /// </summary>
    public class InstrumentManagerClass : InstrumentManager
    {
        
        /// <summary>
        /// The current system state.
        /// </summary>
        public RunningStates CurrentRunningState;
        //Various, hopefully self explanatory events that a GUI
        //working with this manager should subscribe to.
        public event InstrumentManagerEventHandler OnProtocolStarted;
        /// <summary>
        /// This event happens when all protocols have completed their current tasks
        /// and are just waiting for another time point.
        /// </summary>
        public event ProtocolPauseEventHandler OnProtocolPaused;
        public event InstrumentManagerEventHandler OnAllRunningProtocolsEnded;
        public event InstrumentManagerErrorHandler OnGenericError;
        public event InstrumentManagerErrorHandler OnErrorDuringProtocolExecution;
        public event InstrumentManagerEventHandler OnProtocolSuccessfullyCancelled;
        
        System.Windows.Forms.Timer NextInstructionTimer= new System.Windows.Forms.Timer();
        public string RecoveryProtocolFile;
        private string pAppDataDirectory = Directory.GetCurrentDirectory()+"\\";
        BackgroundWorker WorkerToRunRobots;
        public ProtocolManager LoadedProtocols;
        public ProtocolEventCaller ProtocolEvents;
        //This should not be static in the future
        static public Alarm Clarity_Alarm;
        private bool pUseAlarm = true;
        public bool UseAlarm
        {
            get { return pUseAlarm; }
            set { pUseAlarm = value; }
        }
        static public StaticProtocolItem LastFailedInstruction;
        static public List<BaseInstrumentClass> InstrumentCollection;
        Dictionary<string, BaseInstrumentClass> NamesToBICs;
        
        
        private void FireGenericError(Exception thrown)
        {
            if (OnGenericError != null)
            {
                OnGenericError(this, thrown);
            }
        }
        private void FireGenericError(string ErrorMessage)
        {
            Exception GenericException = new Exception(ErrorMessage);
            FireGenericError(GenericException);
        }
        private bool RunInstrumentMethod(string InstrumentName, string MethodName, object[] Parameters, bool RequireStatusOK, Protocol curProtocol = null)
        {
            //flips through the instruments, until it finds the appropriate method/name, then invokes it
            //all failed instrument methods should throw an instrument error
            //returns true if method worked, false otherwise
            try
            {
                object[] ParametersToPass = Parameters;
                Type T = typeof(UserCallableMethod);
                foreach (object c in InstrumentCollection)
                {
                    BaseInstrumentClass Instr = (BaseInstrumentClass)c;
                    if (Instr.Name == InstrumentName)
                    {
                        if ((Instr.StatusOK == true | !RequireStatusOK) || MethodName == "Initialize")
                        {
                            Type InstType = c.GetType();
                            //var q = InstType.GetMethods();
                            foreach (MethodInfo MI in InstType.GetMethods())
                            {
                                if (MI.Name == MethodName)
                                {
                                    //need to worry more about overloaded methods here

                                    //Some methods might need access to other instruments
                                    //or to the protocol that is calling them
                                    //we check the attributes and add another argument to the object array if this is the 
                                    //case
                                    object[] attrs = MI.GetCustomAttributes(T, false);

                                    if (attrs.Length > 0)
                                    {
                                        UserCallableMethod attr = (UserCallableMethod)attrs[0];
                                        AdditionalMethodArguments extraArgs = new AdditionalMethodArguments();
                                        if (attr.RequiresCurrentProtocol || attr.RequiresInstrumentManager)
                                        {
                                            if (attr.RequiresCurrentProtocol)
                                            {
                                                //Protocol curProtocol = LoadedProtocols.CurrentProtocolInUse;
                                                if (curProtocol == null)
                                                { throw new ArgumentNullException("Method requires a protocol but one isn't set as the current one"); }
                                                extraArgs.CallingProtocol = curProtocol;
                                            }
                                            if (attr.RequiresInstrumentManager)
                                            {
                                                extraArgs.InstrumentCollection = this;
                                            }
                                            //Add addtional arguments
                                            int currentLength = ParametersToPass == null ? 0 : ParametersToPass.Length;
                                            ParametersToPass = new object[currentLength + 1];
                                            if (ParametersToPass != null) Parameters.CopyTo(ParametersToPass, 0);
                                            ParametersToPass[ParametersToPass.Length - 1] = extraArgs;
                                        }
                                    }
                                    if (MI.GetParameters().Length == 0 && ParametersToPass == null)
                                    { MI.Invoke(c, null); return true; }
                                    else if (ParametersToPass != null && MI.GetParameters().Length == ParametersToPass.Length)
                                    {
                                        MI.Invoke(c, ParametersToPass); return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception thrown)
            {
                //thrown should always be of type System.Reflection.TargetInvocationException
                //the inner exception can be the instrument error if this was the cause
                string ErrorReport = "Unhandled error occurred during invocation of method " + MethodName
                + " on instrument " + InstrumentName + "\nException error is: " + thrown.Message + "\n\n";
                Exception leftToReportOn = thrown.InnerException;
                if (thrown.InnerException != null)
                {
                    object Test = (object)thrown.InnerException;
                    if (Test is InstrumentError)
                    {
                        InstrumentError IE = (InstrumentError)Test;
                        ErrorReport += "Problem thrown by instrument " + IE.InstrumentInError.ToString() + "\n";
                        ErrorReport += "Problem occured at " + IE.TimeThrown.ToString() + "\n";
                        ErrorReport += "During invokation of method " + MethodName + "\n";
                        ErrorReport += "Exception message is: " + IE.ErrorDescription + "\n";
                        if (IE.Message != null)
                        { ErrorReport += "Exception inner messages is: " + IE.Message + "\n\n\n"; }


                        leftToReportOn = IE.InnerException;
                    }
                    else
                    {
                        ErrorReport += thrown.Message;
                    }
                }
                StringDel newDel = this.AddErrorLogText;
                object[] myarr = new object[1] { ErrorReport };
                this.Invoke(newDel, myarr);
            }
            return false;
        }
        private void FireErrorDuringProtocolExecution(Exception thrown)
        {
            this.CurrentRunningState = RunningStates.Idle;
            if (OnErrorDuringProtocolExecution != null)
            {
                OnErrorDuringProtocolExecution(this, thrown);
            }
            LoadedProtocols.ReportToAllUsers();
            if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Nothing Running");
                Clarity_Alarm.TurnOnAlarm("Procedure ended with errors");
            }
        }
        private void FireSuccessfulCancellation()
        {
            this.CurrentRunningState = RunningStates.Idle;
            if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Nothing Running");
            }
            if (OnProtocolSuccessfullyCancelled != null)
            {
                OnProtocolSuccessfullyCancelled(this, null);
            }
        }
        private void FireAllProtocolsCompleted()
        {
            CurrentRunningState = RunningStates.Idle;
            if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Protocols Finished");
            }
            if (OnAllRunningProtocolsEnded != null)
            { OnAllRunningProtocolsEnded(this, null); }
        }
        private void FireProtocolDelayEvent(int Delay)
        {
            CurrentRunningState = RunningStates.WaitingForNextExecutionTimePoint;
            if (Delay <= 0)
            {
                throw new ArgumentOutOfRangeException("Can't place a 0 or negative delay between protocols.");
            }
           if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Waiting Until It Is Time To Run The Next Protocol Instruction");
            }
            NextInstructionTimer.Interval = Delay;
            NextInstructionTimer.Start();
          
        }

        private void InitializeWorkerToRunRobots()
        {
            WorkerToRunRobots = new BackgroundWorker();
            WorkerToRunRobots.WorkerReportsProgress = true;
            WorkerToRunRobots.WorkerSupportsCancellation = true;
            WorkerToRunRobots.DoWork += new DoWorkEventHandler(WorkerToRunRobots_DoWork);
            WorkerToRunRobots.ProgressChanged += new ProgressChangedEventHandler(WorkerToRunRobots_ProgressChanged);
            WorkerToRunRobots.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerToRunRobots_RunWorkerCompleted);
        }
        void WorkerToRunRobots_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
       {
           try
           {
               if (e.Cancelled)
               {
                   FireSuccessfulCancellation();
               }
               else if (e.Error != null)
               {
                   FireErrorDuringProtocolExecution(e.Error);
               }
               else if (e.Result is StaticProtocolItem)
               {
                   StaticProtocolItem SP = e.Result as StaticProtocolItem;
                   Exception Excep = new Exception("Failed to run instruction " + SP.ToString() + " for instrument " + SP.InstrumentName);
                   FireErrorDuringProtocolExecution(Excep);
               }
               else if (e.Result == null)
               {
                   FireAllProtocolsCompleted();
               }
               else if (e.Result is Int32)
               {
                   //then is the milliseconds until the next instruction occurs
                   int delay = (int)e.Result;
                   FireProtocolDelayEvent(delay);
               }
           }
           catch (Exception thrown)
           {
               LoadedProtocols.ReportToAllUsers("There was an error in the protocol manager, and the system is down.");
               Exception toThrow = new Exception("Problem interpretting the results of an executing protocol, the system is down", thrown);
               FireGenericError(toThrow);
           }
       }
        void WorkerToRunRobots_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lock (LoadedProtocols)
            {
                try
                {
                    UpdateForm();
                    try
                    {
                        StatusLabel.Text = "Performing operation: " + LoadedProtocols.CurrentProtocolInUse.Instructions[LoadedProtocols.CurrentProtocolInUse.NextItemToRun - 1].ToString();
                    }
                    catch { }
                }
                catch { }
            }
        }
        void WorkerToRunRobots_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            RunThroughProtocols(bw, e);//don't try and return anything but an integers
        }
        private void RunThroughProtocols(BackgroundWorker Worker, DoWorkEventArgs e)
        {
            //okay this will loop through all the instructions and finish them off one by one
            try
            {
                while (!Worker.CancellationPending && LoadedProtocols != null)
                {
                    object NextInstruction = LoadedProtocols.GetNextProtocolObject();
                    {
                        if (NextInstruction is StaticProtocolItem)
                        {
                            Worker.ReportProgress(0);
                            StaticProtocolItem Instruction = (StaticProtocolItem)NextInstruction;
                            //then we run the protocol item
                            bool result = RunInstrumentMethod(Instruction.InstrumentName, Instruction.MethodName, Instruction.Parameters, true, Instruction.ContainingProtocol);
                            if (result == false)
                            {
                                LastFailedInstruction = Instruction;
                                e.Result = Instruction;
                                return;
                            }
                            else
                            {
                                OutputRecoveryFile(RecoveryProtocolFile);
                                Worker.ReportProgress(LoadedProtocols.CurrentProtocolInUse.NextItemToRun);
                            }
                        }
                        else if (NextInstruction is double)
                        {
                            //this is the milliseconds until the next protocol runs
                            e.Result = Convert.ToInt32(NextInstruction) + 1;//always make sure the value is greater then 0
                            OutputRecoveryFile(RecoveryProtocolFile);
                            TimeSpan delay = new TimeSpan(10000 * (Convert.ToInt64(NextInstruction) + 1));
                            ProtocolEvents.FirePauseEvent(delay);
                            return;
                        }
                        else if (NextInstruction == null)
                        {
                            TimeSpan delay = new TimeSpan(1000000, 0, 0);
                            ProtocolEvents.FirePauseEvent(delay);
                            e.Result = null;
                            return;
                        }
                        else { throw new Exception("Protocol item was not an instruction, double or null"); }
                    }
                }
                if (Worker.CancellationPending) { e.Cancel = true; OutputRecoveryFile(RecoveryProtocolFile); }
                return;
            }
            catch (Exception thrown)
            {
                throw thrown;
            }
        }
        /// <summary>
        /// Starts running the loaded protocols
        /// </summary>
        public void StartProtocolExecution()
        {

            //probably should try some checks here
            if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Protocol Running");
            }
            if (WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
            { FireGenericError("Tried to start a protocol when one was already running"); }
            else
            {
                InitializeWorkerToRunRobots();
                CurrentRunningState = RunningStates.Running;
                WorkerToRunRobots.RunWorkerAsync();
            }
        }
        /// <summary>
        /// Request that any running protocols stop running
        /// </summary>
        public void RequestProtocolCancellation()
        {
            try
            {
                if (WorkerToRunRobots is BackgroundWorker && WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
                {
                    WorkerToRunRobots.CancelAsync();
                }
                else if (NextInstructionTimer.Enabled)
                {
                    NextInstructionTimer.Enabled = false;
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("User stopped protocol execution");
                    }
                    CurrentRunningState = RunningStates.Idle;
                }
            }
            catch (Exception thrown)
            {
                FireGenericError("Could not complete cancelation request.\n\n" + thrown.Message);
            }
        }
        /// <summary>
        /// Ends the protocol run and closes everything, usually called after the front end GUI closes
        /// 
        /// This will attempt to close out each instruments resources to.
        /// </summary>
        public void ShutdownEngine()
        {
            try { LoadedProtocols.ReportToAllUsers("The Robot Software Has Been Closed"); }
            catch { }
            try { if (UseAlarm) { Clarity_Alarm.TurnOnAlarm("The software was closed"); } }
            catch { }
            try
            {
                foreach (object o in InstrumentCollection)
                {
                    BaseInstrumentClass BC = (BaseInstrumentClass)o;
                    try
                    {
                        BC.CloseAndFreeUpResources();
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }

        private void StopClockWaitAndExecute()
        {
            NextInstructionTimer.Stop();
            
            StartProtocolExecution();
        }
        private void NextInstructionTimer_Tick(object sender, EventArgs e)
        {
            StopClockWaitAndExecute();
        }

        //Backup and Recovery Stuff
        private void OutputRecoveryFile(string FileNameToWrite)
        {
            try
            {
                if (LoadedProtocols.Protocols.Count > 0)
                {
                    FileStream f = new FileStream(FileNameToWrite, FileMode.Create);
                    BinaryFormatter b = new BinaryFormatter();
                    LoadedProtocols.manager = null;
                    b.Serialize(f, LoadedProtocols);
                    LoadedProtocols.manager = this;
                    f.Close();
                }

            }
            catch (Exception thrown)
            {
                string ErrorMessage = "Could Not Write Recovery Data File\n\nError is:" + thrown.Message;
                StringDel newDel = this.AddErrorLogText;
                object[] myarr = new object[1] { ErrorMessage };
                this.Invoke(newDel, myarr);
            }
        }
        private void InputRecoveryFile(string FileNameToRead)
        {
            FileStream f = null;
            try
            {
                f = new FileStream(FileNameToRead, FileMode.Open);
                BinaryFormatter b = new BinaryFormatter();
                //This next bit seems odd to me, got it off a forum as a way to correct an end of stream error
                f.Seek(0, 0);
                ProtocolManager ReplacementProtocols = (ProtocolManager)b.Deserialize(f);
                LoadedProtocols = ReplacementProtocols;
                LoadedProtocols.manager = this;
                f.Close();
            }
            catch (Exception thrown)
            {
                string ErrorMessage = "Could Not Load Previous File\n\nError is:" + thrown.Message;
                StringDel newDel = this.AddErrorLogText;
                object[] myarr = new object[1] { ErrorMessage };
                this.Invoke(newDel, myarr);
                try { f.Close(); }
                catch { }
            }
        }
        private void SetEngineSettingsBasedOnXML()
        {
            string Filename = BaseInstrumentClass.GetXMLSettingsFile();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader XReader = new XmlTextReader(Filename);
            XmlDoc.Load(XReader);
            //first node is xml, second is the protocol, this is assumed and should be the case
            XmlNode SettingsXML = XmlDoc.SelectSingleNode("//EngineSettings");
            BaseInstrumentClass.SetPropertiesByXML(SettingsXML, this);
            XReader.Close();
        }
        /// <summary>
        /// Load up and initialize the instruments, based on XML settings
        /// </summary>
        public void LoadUpInstruments()
        {
            //First get an instance of every possible base instrument class by searching for DLLs in the
            //current directory
            List<BaseInstrumentClass> BICs = InstrumentFinder.GetAllInstrumentClasses();
            //now make a dictionary of it
            NamesToBICs = new Dictionary<string, BaseInstrumentClass>();
            Dictionary<string, BaseInstrumentClass> TypesToInstruments = new Dictionary<string, BaseInstrumentClass>();
            //now make them searchable
            foreach (BaseInstrumentClass bic in BICs)
            {
                InstrumentCollection.Add(bic);
                bic.RegisterEventsWithProtocolManager(ProtocolEvents);
                NamesToBICs[bic.Name] = bic;
                TypesToInstruments[bic.GetType().Name] = bic;
            }
            //Code immediately below is designed to allow instruments to talk amongst each other.
            //For example if instrument A wants to control instrument B, then in the class
            //for A there might be a field for B, if so, this code will identify this field,
            //and set the value in A to the reference for the current copy of B.
            //In essence this replaces code like 
            //Incubator = new IncubatorServ();
            //Robot = new Twister(); 
            //Robot.Incu=Incubator;
            Type BICType = typeof(BaseInstrumentClass);            
            foreach (FieldInfo FI in this.GetType().GetFields())
            {
                if(FI.FieldType.IsSubclassOf(BICType))
                {   
                    var InstanceToSet=TypesToInstruments[FI.FieldType.Name];
                    FI.SetValue(this,InstanceToSet);
                }
            }
            //Now to load and initialize the instruments from XML
            //This should be a ConfigurationFile.xml
            string Filename = BaseInstrumentClass.GetXMLSettingsFile();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader XReader = new XmlTextReader(Filename);
            XmlDoc.Load(XReader);
            //first node is xml, second is the protocol, this is assumed and should be the case
            XmlNode InstrumentsNode = XmlDoc.SelectSingleNode("//Instruments");
            //Loop through all of the 
            foreach (XmlNode instNode in InstrumentsNode)
            {
                string instName = instNode.Name;
                if(NamesToBICs.ContainsKey(instName))
                {
                    BaseInstrumentClass instrument = NamesToBICs[instName];
                    //now to check if it has a no load flag
                    var Skip=instNode.Attributes.GetNamedItem("SkipLoad");
                    if (Skip == null || Skip.Value.ToUpper().Trim() != "TRUE")
                    {
                        //if not load it
                        instrument.InitializeFromParsedXML(instNode);                        
                    }
                }
            }
            XReader.Close();
        }
        /// <summary>
        /// Initializes a new instance of the instrument manager class, and loads itself and instruments
        /// based on the XML configuration file.
        /// </summary>
        public InstrumentManagerClass()
        {
            CurrentRunningState = RunningStates.Idle;
            InstrumentCollection = new List<BaseInstrumentClass>();
            LoadedProtocols = new ProtocolManager(this);
            ProtocolEvents = new ProtocolEventCaller();
            NextInstructionTimer.Tick+=new EventHandler(NextInstructionTimer_Tick);
            SetEngineSettingsBasedOnXML();

            LoadUpInstruments();
            if (UseAlarm)
            {
                Clarity_Alarm = new Alarm();
            }
        }
        #region InstrumentManager Members
        public BaseInstrumentClass ReturnInstrument(string InstrumentName)
        {
            if (NamesToBICs.ContainsKey(InstrumentName))
                return NamesToBICs[InstrumentName];
            else throw new ArgumentException("Instrument " + InstrumentName + " not in the collection of instruments");
        }
        public T ReturnInstrumentType<T>() where T : BaseInstrumentClass
        {
            //Linear search, get type and return
            Type toGet = typeof(T);
            foreach (BaseInstrumentClass bc in InstrumentCollection)
            {
                if (bc.GetType() == toGet)
                    return (T)bc;
            }
            throw new ArgumentException("Could not find requested instrument type: " + toGet.ToString());
        }
        public Alarm GiveAlarmReference()
        {
            return Clarity_Alarm;
        }
        #endregion

      
    }
}
