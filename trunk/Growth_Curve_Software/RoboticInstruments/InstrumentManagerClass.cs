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
    /// This class is the runtime engine for Clarity
    /// </summary>
    public class InstrumentManagerClass : InstrumentManager
    {
        public event InstrumentManagerEventHandler OnProtocolStarted;
        public event ProtocolPauseEventHandler OnProtocolPaused;
        public event InstrumentManagerEventHandler OnProtocolEnded;
        public event InstrumentManagerErrorHandler OnError;
        
        public string RecoveryProtocolFile;
        private string pAppDataDirectory = Directory.GetCurrentDirectory()+"\\";
        BackgroundWorker WorkerToRunRobots;
        static public ProtocolManager LoadedProtocols;
        static public ProtocolEventCaller ProtocolEvents;
        //This should not be static in the future
        static public Alarm Clarity_Alarm;
        private bool pUseAlarm = true;
        public bool UseAlarm
        {
            get { return pUseAlarm; }
            set { pUseAlarm = value; }
        }
        static private StaticProtocolItem LastFailedInstruction;
        static public List<BaseInstrumentClass> InstrumentCollection;
        Dictionary<string, BaseInstrumentClass> NamesToBICs;
        //protocol execution
        private void StartProtocolExecution()
        {

            //probably should try some checks here
            if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Protocol Running");
            }
            if (WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
            { ShowError("A Protocol Is Already Running"); }
            else
            {
                InitializeWorkerToRunRobots();
                WorkerToRunRobots.RunWorkerAsync();
            }
        }
        private void ThrowError(Exception thrown)
        {
            if (OnError != null)
            {
                OnError(this, thrown);
            }
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
        private void InitializeWorkerToRunRobots()
        {
            WorkerToRunRobots = new BackgroundWorker();
            WorkerToRunRobots.WorkerReportsProgress = true;
            WorkerToRunRobots.WorkerSupportsCancellation = true;
            WorkerToRunRobots.DoWork += new DoWorkEventHandler(WorkerToRunRobots_DoWork);
            WorkerToRunRobots.ProgressChanged += new ProgressChangedEventHandler(WorkerToRunRobots_ProgressChanged);
            WorkerToRunRobots.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerToRunRobots_RunWorkerCompleted);
        }
        private void DealWithErrorDuringProtocolRun(Exception thrown)
        {
            LoadedProtocols.ReportToAllUsers();
            StatusLabel.Text = "Procedure ended with errors";
            if (UseAlarm)
            {
                Clarity_Alarm.TurnOnAlarm("Procedure ended with errors");
            }
            btnRetryLastInstruction.Enabled = true;
            ShowError("Failed To Run Protocol", thrown);
            pnlFailure.Visible = true;
            lblFailure.Text = "The Last Instruction Failed To Run, Please recover the machines and retry or delete the protocol, Do not reattempt a macro instruction";
            if (LastFailedInstruction != null)
            {
                lblFailureInstructionName.Text = "Would you like to reattempt: " + LastFailedInstruction.ToString();
            }
            else
            {
                btnRetryLastInstruction.Enabled = false;
                StringDel newDel = this.AddErrorLogText;
                object[] myarr = new object[1] { "\nWeird command failure, the last instruction is not available" };
                this.Invoke(newDel, myarr);
            }
        }
        void WorkerToRunRobots_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    btnCancelProtocolExecution.Enabled = false;
                    btnExecuteProtocols.Enabled = true;
                    StatusLabel.Text = "Cancelled Protocol";
                    TimeToGo.Text = "Nothing Running";
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("Nothing Running");
                    }
                    UpdateForm();
                }
                else if (e.Error != null)
                {
                    DealWithErrorDuringProtocolRun(e.Error);
                    TimeToGo.Text = "Nothing Running";
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("Nothing Running");
                    }
                    btnExecuteProtocols.Enabled = true;
                    btnCancelProtocolExecution.Enabled = false;
                }
                else if (e.Result is StaticProtocolItem)
                {
                    StaticProtocolItem SP = e.Result as StaticProtocolItem;

                    Exception Excep = new Exception("Failed to run instruction " + SP.ToString() + " for instrument " + SP.InstrumentName);
                    DealWithErrorDuringProtocolRun(Excep);
                    TimeToGo.Text = "Nothing Running";
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("Nothing Running");
                    }
                    btnExecuteProtocols.Enabled = true;
                    btnCancelProtocolExecution.Enabled = false;

                }
                else if (e.Result == null)
                {
                    StatusLabel.Text = "Finished Running Protocols";
                    TimeToGo.Text = "";
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("Protocols Finished");
                    }
                    btnExecuteProtocols.Enabled = true;
                    btnCancelProtocolExecution.Enabled = false;
                    UpdateForm();
                }
                else if (e.Result is Int32)
                {//then is the milliseconds until the next instruction occurs
                    int delay = (int)e.Result;
                    SetDelayAndStartWait(delay);
                }

            }
            catch (Exception thrown)
            {
                LoadedProtocols.ReportToAllUsers("There was an error in the protocol manager, and the system is down.");
                btnCancelProtocolExecution.Enabled = false;
                btnExecuteProtocols.Enabled = true;
                StatusLabel.Text = "Problem in Execution Management";
                TimeToGo.Text = "Nothing Running";
                ShowError(thrown.Message);
            }
        }
        private void SetDelayAndStartWait(int Delay)
        {
            if (Delay <= 0)
            {
                throw new ArgumentOutOfRangeException("Can't place a 0 or negative delay between protocols.");
            }
            StatusLabel.Text = "Waiting Until It Is Time To Run The Next Protocol Instruction";
            if (UseAlarm)
            {
                Clarity_Alarm.ChangeStatus("Waiting Until It Is Time To Run The Next Protocol Instruction");
            }
            NextInstructionTimer.Interval = Delay;
            NextInstructionTimer.Start();
            TimeToGo.Start(Delay);
            UpdateForm();
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
        private void btnExecuteProtocols_Click(object sender, EventArgs e)
        {

            StartProtocolExecution();

        }
        private void btnCancelProtocolExecution_Click(object sender, EventArgs e)
        {
            try
            {
                if (WorkerToRunRobots is BackgroundWorker && WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
                {
                    WorkerToRunRobots.CancelAsync();
                    btnCancelProtocolExecution.Enabled = false;
                    StatusLabel.Text = "Status is: Attempting To Cancel Operation";
                }
                else if (NextInstructionTimer.Enabled)
                {
                    NextInstructionTimer.Enabled = false;
                    StatusLabel.Text = "Cancelled Operation";
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("User stopped protocol execution");
                    }
                    btnExecuteProtocols.Enabled = true;
                    btnCancelProtocolExecution.Enabled = false;
                }
                else { btnCancelProtocolExecution.Enabled = false; btnExecuteProtocols.Enabled = true; }
                TimeToGo.Stop();
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not cancel operation.\n\n" + thrown.Message);
            }
        }
        private void StopClockWaitAndExecute()
        {
            NextInstructionTimer.Stop();
            TimeToGo.Stop();
            StartProtocolExecution();
        }
        private void NextInstructionTimer_Tick(object sender, EventArgs e)
        {
            StopClockWaitAndExecute();
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
        public InstrumentManagerClass()
        {
            InstrumentCollection = new List<BaseInstrumentClass>();
            LoadedProtocols = new ProtocolManager(this);
            ProtocolEvents = new ProtocolEventCaller();
            LoadUpInstruments();

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
