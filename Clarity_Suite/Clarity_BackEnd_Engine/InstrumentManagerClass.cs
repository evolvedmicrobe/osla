using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
using System.Threading.Tasks;


namespace Clarity
{
    /// <summary>
    /// These are the various options the engine is in, depending on what it is currently doing
    /// </summary>
    public enum RunningStates { Idle, Running, WaitingForNextExecutionTimePoint }
    public enum ProtocolRemoveResult { WasCurrentlyRunning, RemovedSuccessfully, Error };
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
        /// <summary>
        /// This event happens when all protocols have completed their current tasks
        /// and are just waiting for another time point.
        /// </summary>
        public event ProtocolPauseEventHandler OnProtocolPaused;
        public event InstrumentManagerEventHandler OnAllRunningProtocolsEnded;
        public event InstrumentManagerErrorHandler OnGenericError;
        public event InstrumentManagerErrorHandler OnErrorDuringProtocolExecution;
        public event InstrumentManagerEventHandler OnProtocolSuccessfullyCancelled;
        public event InstrumentManagerEventHandler OnProtocolExecutionUpdates;
        public event InstrumentManagerEventHandler OnInstrumentStatusUpdate;
        public event InstrumentManagerEventHandler OnProtocolsStarted;

        private bool CurrentlyAttemptingInstrumentRecovery = false;
        System.Windows.Forms.Timer NextInstructionTimer= new System.Windows.Forms.Timer();
        public string RecoveryProtocolFile;
        private string pAppDataDirectory = Directory.GetCurrentDirectory() + "\\";
        /// <summary>
        /// This can be set by the files XML, this is the directory to look for data.
        /// </summary>
        public string AppDataDirectory
        {
            get { return pAppDataDirectory; }
            set { pAppDataDirectory = value; }
        }
        public bool LoadInstrumentsInParallel {get;set;}


        private bool pRequireProtocolValidation;
        public bool RequireProtocolValidation
        {
            get { return pRequireProtocolValidation; }
            set { pRequireProtocolValidation = value; }
        }

        BackgroundWorker WorkerToRunRobots;
        ProtocolManager pLoadedProtocols;
        public ProtocolManager LoadedProtocols
        {
            get{return pLoadedProtocols;}
        }
        public ProtocolEventCaller ProtocolEvents;
        //This should not be static in the future
        static public Alarm pClarity_Alarm;
        public Alarm Clarity_Alarm
        {
            get { return pClarity_Alarm; }
        }
        private bool pUseAlarm = true;
        public bool UseAlarm
        {
            get { return pUseAlarm; }
            set { pUseAlarm = value; }
        }
        StaticProtocolItem LastFailedInstruction;
        public StaticProtocolItem GetLastFailedInstruction()
        {
            return LastFailedInstruction;
        }
        public List<BaseInstrumentClass> InstrumentCollection;
        Dictionary<string, BaseInstrumentClass> NamesToBICs;

        private void FireProtocolExecutionStarted()
        {
            if (OnProtocolsStarted != null)
            {
                OnProtocolsStarted(this, null);
            }
        }
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
        private void FireErrorDuringProtocolExecution(Exception thrown)
        {
            this.CurrentRunningState = RunningStates.Idle;
            if (OnErrorDuringProtocolExecution != null)
            {
                OnErrorDuringProtocolExecution(this, thrown);
            }
            pLoadedProtocols.ReportToAllUsers();
            if (UseAlarm)
            {
                pClarity_Alarm.ChangeStatus("Nothing Running");
                pClarity_Alarm.TurnOnAlarm("Procedure ended with errors");
            }
        }
        private void FireSuccessfulCancellation()
        {
            this.CurrentRunningState = RunningStates.Idle;
            if (UseAlarm)
            {
                pClarity_Alarm.ChangeStatus("Nothing Running");
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
                pClarity_Alarm.ChangeStatus("Protocols Finished");
            }
            if (OnAllRunningProtocolsEnded != null)
            { OnAllRunningProtocolsEnded(this, null); }
            //Fire a really really long pause event
            ProtocolEvents.FirePauseEvent(this, new TimeSpan(10000,0,0));
        }
        private void FireProtocolDelayEvent(DateTime NextExecutionTime)
        {
            
            TimeSpan ts = NextExecutionTime.Subtract(DateTime.Now);
            //Perhaps I should throw an error here instead
            if (ts.TotalMilliseconds <= 0)
            {
                StartProtocolExecution();
                //throw new ArgumentOutOfRangeException("Can't place a 0 or negative delay between protocols.");
            }
            else
            {
                CurrentRunningState = RunningStates.WaitingForNextExecutionTimePoint;
                if (UseAlarm)
                {
                    pClarity_Alarm.ChangeStatus("Waiting Until It Is Time To Run The Next Protocol Instruction");
                }
                NextInstructionTimer.Interval = (int)ts.TotalMilliseconds;
                NextInstructionTimer.Start();
                ProtocolEvents.FirePauseEvent(this, ts);
                if (OnProtocolPaused != null)
                {
                    OnProtocolPaused(this, ts);
                }
            }
        }
        private void FireProtocolUpdate()
        {
            if (UseAlarm && LoadedProtocols.CurrentProtocolInUse!=null)
            {
                pClarity_Alarm.ChangeStatus("Current Running Protocol - " + LoadedProtocols.CurrentProtocolInUse.ToString());
            }
            
            if (OnProtocolExecutionUpdates != null)
                OnProtocolExecutionUpdates(this, null);
        }

        private void FireInstrumentStatusUpdate()
        {
            if (OnInstrumentStatusUpdate != null)
            {
                OnInstrumentStatusUpdate(this, null);
            }
        }
        private void InitializeWorkerToRunRobots()
        {
            LoadedProtocols.UpdateCurrentProtocol();
            WorkerToRunRobots = new BackgroundWorker();
            WorkerToRunRobots.WorkerReportsProgress = true;
            WorkerToRunRobots.WorkerSupportsCancellation = true;
            WorkerToRunRobots.DoWork += new DoWorkEventHandler(WorkerToRunRobots_DoWork);
            WorkerToRunRobots.ProgressChanged += new ProgressChangedEventHandler(WorkerToRunRobots_ProgressChanged);
            WorkerToRunRobots.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerToRunRobots_RunWorkerCompleted);
        }

        public bool RunInstrumentMethod(string InstrumentName, string MethodName, object[] Parameters, bool RequireStatusOK, Protocol curProtocol = null)
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
                FireErrorDuringProtocolExecution(thrown);

            }
            return false;
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
               else if (e.Result is DateTime)
               {
                   //then is the milliseconds until the next instruction occurs
                   DateTime delay = (DateTime)e.Result;
                   FireProtocolDelayEvent(delay);
               }
           }
           catch (Exception thrown)
           {
               pLoadedProtocols.ReportToAllUsers("There was an error in the protocol manager, and the system is down.");
               Exception toThrow = new Exception("Problem interpretting the results of an executing protocol, the system is down", thrown);
               FireGenericError(toThrow);
           }
       }
        void WorkerToRunRobots_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FireProtocolUpdate();
           
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
                while (!Worker.CancellationPending && pLoadedProtocols != null)
                {
                    object NextInstruction = pLoadedProtocols.GetNextProtocolObject();
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
                                Worker.ReportProgress(pLoadedProtocols.CurrentProtocolInUse.NextItemToRun);
                            }
                        }
                        else if (NextInstruction is DateTime)
                        {
                            //this is the time when the next protocol should run
                            OutputRecoveryFile(RecoveryProtocolFile);
                            e.Result = NextInstruction;
                            return;
                        }
                        else if (NextInstruction == null)
                        {
                            OutputRecoveryFile(RecoveryProtocolFile);
                            e.Result = null;
                            return;
                        }
                        else { throw new Exception("Protocol item was not an instruction, datetime or null"); }
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
            if (NextInstructionTimer != null && NextInstructionTimer.Enabled)
            { NextInstructionTimer.Stop(); }
            if (LoadedProtocols.Protocols.Count < 1)
            {
                FireGenericError("Tried to start protocols when none were loaded");
            }
            else
            {
                //First to check if a protocol is running
                double Delay = LoadedProtocols.GetMilliSecondsTillNextRunTime();
                //Run now
                if (Delay < 0)
                {
                    if (WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
                    { FireGenericError("Tried to start a protocol when one was already running"); }
                    else
                    {
                        if (UseAlarm)
                        {
                            pClarity_Alarm.ChangeStatus("Protocol Running");
                        }
                        InitializeWorkerToRunRobots();
                        CurrentRunningState = RunningStates.Running;
                        WorkerToRunRobots.RunWorkerAsync();
                    }
                    FireProtocolExecutionStarted();
                }
                //Delay for later
                else
                {
                    FireProtocolDelayEvent(LoadedProtocols.CurrentProtocolInUse.NextExecutionTimePoint);
                }
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
                        pClarity_Alarm.ChangeStatus("User stopped protocol execution");
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
            try {
                if(pLoadedProtocols!=null)
                pLoadedProtocols.ReportToAllUsers("The Robot Software Has Been Closed");
            }
            catch { }
            try { if (UseAlarm &&pClarity_Alarm!=null) { pClarity_Alarm.TurnOnAlarm("The software was closed"); } }
            catch { }
            try
            {
                if (InstrumentCollection != null)
                {

                    Parallel.ForEach(InstrumentCollection, o =>
                    {
                        if (o != null)
                        {
                            try
                            {
                                o.CloseAndFreeUpResources();
                            }
                            catch
                            { }
                        }
                    });
                }
            }
            catch
            { }
        }
        
        public ProtocolRemoveResult RemoveProtocol(Protocol toRemove)
        {
            ProtocolRemoveResult toReturn;
            try
            {
                bool ResetTimerAfterwards = false;//will be used to determine if we need to reset the timer
                if (WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
                {
                    //protocol is running
                    if (LoadedProtocols.CurrentProtocolInUse == toRemove)
                    {
                        toReturn = ProtocolRemoveResult.WasCurrentlyRunning;
                        return toReturn;
                    }
                }
                if (LoadedProtocols.CurrentProtocolInUse == toRemove)
                {
                    ResetTimerAfterwards = true;
                }
                LoadedProtocols.RemoveProtocol(toRemove);
                if (ResetTimerAfterwards && LoadedProtocols.Protocols.Count>0)
                {
                    StartProtocolExecution();
                }
                toReturn = ProtocolRemoveResult.RemovedSuccessfully;
                return toReturn;
            }
            catch (Exception thrown)
            {
                Exception e = new Exception("Could not remove protcol: " + thrown.Message, thrown);
                FireGenericError(e);
                toReturn = ProtocolRemoveResult.Error;
                return toReturn;
            }
        }
        public void AddProtocol(Protocol toAdd)
        {
            LoadedProtocols.AddProtocol(toAdd);
            if (CurrentRunningState == RunningStates.WaitingForNextExecutionTimePoint)
            {
                StartProtocolExecution();
            }

        }
        public void RetryLastFailedInstruction()
        {
            try
            {
                if (CurrentRunningState == RunningStates.Running)
                    FireGenericError("Can't retry instructions while protocol is running");
                if (LastFailedInstruction == null)
                    FireGenericError("No last failed instruction set, can't try again.");
                else
                {
                    CurrentRunningState = RunningStates.Running;
                    RunInstrumentMethod(LastFailedInstruction.InstrumentName, LastFailedInstruction.MethodName, LastFailedInstruction.Parameters, true, LastFailedInstruction.ContainingProtocol);
                    CurrentRunningState = RunningStates.Idle;
                }
            }
            catch (Exception thrown)
            {
                FireGenericError(thrown);


            }
        }
        public void TryToRecoverInstrument(string InstrumentName)
        {
            try
            {
                if (CurrentlyAttemptingInstrumentRecovery)
                {
                    FireGenericError("Currently attempting a recovery, please wait for the results");
                }
                else
                {
                    Action<string> toRun = AttemptRecoveryMethod;
                    Thread toRecover = new Thread(() => toRun(InstrumentName));
                    toRecover.IsBackground = true;
                    toRecover.Name = "Instrument Recovery thread";
                    toRecover.Start();
                }
            }
            catch (Exception thrown)
            { FireGenericError(thrown); CurrentlyAttemptingInstrumentRecovery = false; }
        }
        private void AttemptRecoveryMethod(string InstrumentName)
        {
            try
            {
                BaseInstrumentClass toRecover = this.NamesToBICs[InstrumentName];
                toRecover.AttemptRecovery();
                CurrentlyAttemptingInstrumentRecovery = false;
            }
            catch (Exception thrown)
            {
                CurrentlyAttemptingInstrumentRecovery = false;
                FireGenericError(thrown);
            }
        }

        public void FreeResourcesForInstrument(string InstrumentName)
        {
            if (!this.NamesToBICs.ContainsKey(InstrumentName))
                FireGenericError("Instrument name " + InstrumentName + " is not loaded");
            else
            {
                try
                {
                    BaseInstrumentClass toFree = this.NamesToBICs[InstrumentName];
                    toFree.CloseAndFreeUpResources();
                }
                catch (Exception thrown)
                {
                    FireGenericError(thrown);
                }
            }
        }
        public void ReinitializeAlarm()
        {
            try
            {
                if (UseAlarm)
                {
                     pClarity_Alarm = new Alarm();
                    if (pClarity_Alarm.Connected) { LoadedProtocols.UpdateAlarmProtocols(); }
                }
                else
                    FireGenericError("Alarm disabled at startup");
            }
            catch (Exception thrown) { FireGenericError(thrown.Message); }
        }
        public void ReportErrorRecovery()
        {
            try
            {
                LoadedProtocols.ReportToAllUsers("The Clarity System Is Okay Now ");
                if (UseAlarm)
                { Clarity_Alarm.TurnOffAlarm(); }
            }
            catch (Exception thrown)
            {
                FireGenericError("Could not send messages to all: \n" + thrown.Message);
            }
        }
       


        private void NextInstructionTimer_Tick(object sender, EventArgs e)
        {
            StartProtocolExecution();
        }
       
        //Backup and Recovery Stuff
        public void OutputRecoveryFile(string FileNameToWrite)
        {
            try
            {
                if (pLoadedProtocols.Protocols.Count > 0)
                {
                    FileStream f = new FileStream(FileNameToWrite, FileMode.Create);
                    BinaryFormatter b = new BinaryFormatter();
                    pLoadedProtocols.manager = null;
                    b.Serialize(f, pLoadedProtocols);
                    pLoadedProtocols.manager = this;
                    f.Close();
                }
            }
            catch (Exception thrown)
            {
                Exception e = new Exception("Could Not Write Recovery Data File\n\nError is:" + thrown.Message,thrown);
                FireGenericError(e);             
            }
        }
        public void InputRecoveryFile(string FileNameToRead)
        {
            FileStream f = null;
            try
            {
                f = new FileStream(FileNameToRead, FileMode.Open);
                BinaryFormatter b = new BinaryFormatter();
                //This next bit seems odd to me, got it off a forum as a way to correct an end of stream error
                f.Seek(0, 0);
                ProtocolManager ReplacementProtocols = (ProtocolManager)b.Deserialize(f);
                pLoadedProtocols = ReplacementProtocols;
                pLoadedProtocols.manager = this;
                f.Close();
            }
            catch (Exception thrown)
            {
                Exception e = new Exception( "Could Not Load Previous File\n\nError is:" + thrown.Message);
                FireGenericError(e);
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
            //TODO: Much better error handling and reporting here
            //First get an instance of every possible base instrument class by searching for DLLs in the
            //current directory
            List<BaseInstrumentClass> BICs = InstrumentFinder.GetAllInstrumentClasses();
            //now make a dictionary of these types
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
            //Loop through all of the Instruments

            if (!LoadInstrumentsInParallel)
            {
                //For some reason it couldn't do type inference without making a list like this
                //not sure why, but it seemed easiest.
                List<XmlNode> toUse = new List<XmlNode>();
                foreach (XmlNode instNode in InstrumentsNode)
                {
                    toUse.Add(instNode);
                }
                var allExceptions = new ConcurrentQueue<Exception>();
                Parallel.ForEach(toUse, instNode =>
                {
                    try
                    {
                        string instName = instNode.Name;
                        if (NamesToBICs.ContainsKey(instName))
                        {
                            BaseInstrumentClass instrument = NamesToBICs[instName];
                            //now to check if it has a no load flag
                            var Skip = instNode.Attributes.GetNamedItem("SkipLoad");
                            if (Skip == null || Skip.Value.ToUpper().Trim() != "TRUE")
                            {
                                instrument.InitializeFromParsedXML(instNode);
                            }
                        }
                    }
                    catch (Exception thrown)
                    {
                       allExceptions.Enqueue(new Exception("Could not Initialize Instrument: " + instNode.Name + "\n" + thrown.Message));
                    }
                      
                });
                if (allExceptions.Count > 0)
                {
                    string Error = "Could not Initialize Instruments in Parrallel\n";
                    foreach (Exception e in allExceptions)
                    {
                        Error += e.Message + "\n\n";
                    }
                    throw new Exception(Error);
                }
            }
            else
            {
                foreach (XmlNode instNode in InstrumentsNode)
                {
                    string instName = instNode.Name;
                    if (NamesToBICs.ContainsKey(instName))
                    {
                        BaseInstrumentClass instrument = NamesToBICs[instName];
                        //now to check if it has a no load flag
                        var Skip = instNode.Attributes.GetNamedItem("SkipLoad");
                        if (Skip == null || Skip.Value.ToUpper().Trim() != "TRUE")
                        {
                            //if not load it
                            try
                            {
                                instrument.InitializeFromParsedXML(instNode);
                            }
                            catch (Exception thrown)
                            {
                                throw new Exception("Could not Initialize Instrument: " + instrument.Name + "\n" + thrown.Message);
                            }
                        }
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
            RecoveryProtocolFile = AppDataDirectory + "Last_Protocol.nfd";
            CurrentRunningState = RunningStates.Idle;
            InstrumentCollection = new List<BaseInstrumentClass>();
            pLoadedProtocols = new ProtocolManager(this);
            ProtocolEvents = new ProtocolEventCaller();
            NextInstructionTimer.Tick+=new EventHandler(NextInstructionTimer_Tick);
            try
            {
                SetEngineSettingsBasedOnXML();
            }
            catch(Exception thrown) { throw new Exception("Could not load XML Settings for Clarity Engine\n" + thrown.Message); }
            try
            {
                if (UseAlarm)
                {
                    pClarity_Alarm = new Alarm();
                }
            }
            catch (Exception thrown)
            {
                this.UseAlarm = false;
                throw new Exception("Could not create Alarm for Clarity Engine\n" + thrown.Message);
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
            return pClarity_Alarm;
        }
        #endregion

      
    }
}
