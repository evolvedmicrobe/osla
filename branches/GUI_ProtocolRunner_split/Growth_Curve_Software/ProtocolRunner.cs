using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using System.Net.Mail;
using System.Net;
using Growth_Curve_Software;

namespace Growth_Curve_Software
{
    class ProtocolRunner : InstrumentManager
    {
        private string pAppDataDirectory = Directory.GetCurrentDirectory() + "\\";
        public string AppDataDirectory
        {
            get { return pAppDataDirectory; }
            set { pAppDataDirectory = value; }
        }
        public string RecoveryProtocolFile = "Last_Protocol.nfd"; 
        // This needs to get concatenated with the AppDataDirectory
        //RecoveryProtocolFile = AppDataDirectory + "Last_Protocol.nfd";


        static bool Debugging = false;
        //might use this later to kill a processs
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int TerminateProcess(int hProcess, int uExitCode);
        //supposedly another way to terminate
        static public List<BaseInstrumentClass> InstrumentCollection;
        Dictionary<string, BaseInstrumentClass> NamesToBICs;
        private IncubatorServ Incubator;
        private Twister Robot;
        private TransferStation TransStation;
        private VictorManager PlateReader;

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
        private bool pRequireProtocolValidation;
        public bool RequireProtocolValidation
        { get; set; }
        // TODO: Settings file
        private string pErrorEmails = "ndelaney@fas.harvard.edu;4158234767@vtext.com";
        public string NSFErrorEmails
        {
            get { return pErrorEmails; }
            set { pErrorEmails = value; }
        }
        BackgroundWorker WorkerToRunRobots;
        static private StaticProtocolItem LastFailedInstruction;
        private int pWELL48_PLATE_PROTOCOL_ID = 2000095;
        public int WELL48_PLATE_PROTOCOL_ID
        {
            get { return pWELL48_PLATE_PROTOCOL_ID; }
            set { pWELL48_PLATE_PROTOCOL_ID = value; }
        }
        private int pGBO_PLATE_PROTOCOL_ID = 2000128;
        public int GBO_PLATE_PROTOCOL_ID
        {
            get { return pGBO_PLATE_PROTOCOL_ID; }
            set { pGBO_PLATE_PROTOCOL_ID = value; }
        }
        // TODO: Settings file
        int[] ExcludedIncubatorPositions = { 19, 38 };

        public void LoadUpForm1()
        {
            try
            {
                //Make a collection to hold everything
                InstrumentCollection = new List<BaseInstrumentClass>();
                LoadedProtocols = new ProtocolManager(this);
                ProtocolEvents = new ProtocolEventCaller();
                //out with the old
                if (!Debugging)
                {
                    KillOldProcesses();
                    //in with the new
                }
                //First get an instance of every possible base instrument class
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
                //now to set all of the variables for this instance to the created variables,
                //this is a bit sloppy, and depends on any classes needed by the interface being
                //present in the xml, in essence this replaces code like 
                //Incubator = new IncubatorServ();
                //Robot = new Twister(); etc. etc.
                Type BICType = typeof(BaseInstrumentClass);
                foreach (FieldInfo FI in this.GetType().GetFields())
                {
                    if (FI.FieldType.IsSubclassOf(BICType))
                    {
                        var InstanceToSet = TypesToInstruments[FI.FieldType.Name];
                        FI.SetValue(this, InstanceToSet);
                    }
                }
                //Now to load and initialize the instruments from XML
                //string Filename = @"C:\Users\Nigel_Administrator\Desktop\ConfigurationFile.xml";
                string Filename = BaseInstrumentClass.GetXMLSettingsFile();
                XmlDocument XmlDoc = new XmlDocument();
                XmlTextReader XReader = new XmlTextReader(Filename);
                XmlDoc.Load(XReader);
                //first node is xml, second is the protocol, this is assumed and should be the case
                XmlNode SettingsXML = XmlDoc.SelectSingleNode("//InterfaceSettings");
                BaseInstrumentClass.SetPropertiesByXML(SettingsXML, this);

                XmlNode InstrumentsNode = XmlDoc.SelectSingleNode("//Instruments");
                //Loop through all of the 
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
                            instrument.InitializeFromParsedXML(instNode);
                        }
                    }
                }
                Incubator = this.ReturnInstrumentType<IncubatorServ>();
                TransStation = this.ReturnInstrumentType<TransferStation>();
                PlateReader = this.ReturnInstrumentType<VictorManager>();
            }
            catch (Exception thrown)
            {
                if (thrown is InstrumentError)
                {
                    InstrumentError IE = thrown as InstrumentError;
                    ShowError("Problem Loading: " + IE.InstrumentInError.Name + "\n"
                        + IE.ErrorDescription);
                }
                else
                {
                    ShowError("Problem initializing instruments\n\n" + thrown.Message);
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            if (UseAlarm)
            {
                Clarity_Alarm = new Alarm();
            }
            Thread LoadingThread = new Thread(ShowWelcomeForm);
            LoadingThread.SetApartmentState(ApartmentState.STA);
            LoadingThread.IsBackground = true;
            LoadingThread.Start();

            //if this delegate is set before the instance is initialized it will fail
            Thread.Sleep(1000);//give it time to initialize the WF\                  
            LoadUpForm1();
            WF.StayAlive = false;

            UpdateInstrumentStatus();

            for (int i = 38; i > 0; i--)
            {
                if (ExcludedIncubatorPositions.Contains(i))
                { continue; }
                else
                {
                    lstIncubatorSlots.Items.Add(i);
                    lstNSFPlates.Items.Add(i);
                    lstGrowthRatesProtocol.Items.Add(i);
                }
            }
            cmbShakeSpeed.SelectedIndex = 6;
            CreateRecoveryPanel();

            try
            {
                string CurDirec = System.Environment.CurrentDirectory;
                Uri recovAdd = new Uri(CurDirec + @"\AttemptRecoveryDocument.htm", UriKind.Absolute);
                Uri growthinst = new Uri(CurDirec + @"\GrowthRateInstructionsl.htm", UriKind.Absolute);
                wBrowRecovInstructions.Url = recovAdd;
                wBrowGrowthRate.Url = growthinst;
            }

            catch { }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Debugging)
            {
                try { LoadedProtocols.EmailErrorMessageToAllUsers("The Robot Software Has Been Closed"); }
                catch { }
                try { if (UseAlarm) { Clarity_Alarm.TurnOnAlarm("The software was closed"); } }
                catch { }
                this.Cursor = Cursors.WaitCursor;
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
                finally { this.Cursor = Cursors.Default; }
            }
        }
        private void KillOldProcesses()
        {
            //really not the best here, but out with the old so the new can load
            string[] ToKill = { "Device Control Unit", "TwisterII Robot ICP", "DCU", "CommDispatcher", "ZymarkRobotICP", "ScicloneICP", "SciPEM", "SciRabbitVexta", "CavroDeviceController", "Consumables" };
            foreach (string str in ToKill)
            {
                try { KillProcessAttempt(str); }
                catch { }

            }
        }
        public static void KillProcessAttempt(string ProcessNameWithoutExeEnding)
        {
            try
            {
                Process[] processList = Process.GetProcesses();
                var x = from p in processList select p.ProcessName;
                if (x.Contains(ProcessNameWithoutExeEnding))
                {
                    string output = String.Empty;
                    System.Diagnostics.Process proc = new Process();
                    ProcessStartInfo myStartInfo = new ProcessStartInfo();
                    myStartInfo.RedirectStandardInput = false;
                    myStartInfo.UseShellExecute = false;
                    myStartInfo.RedirectStandardOutput = false;
                    myStartInfo.Arguments = "/IM " + ProcessNameWithoutExeEnding + ".exe";
                    myStartInfo.CreateNoWindow = true;
                    myStartInfo.FileName = @"C:\WINDOWS\SYSTEM32\TASKKILL.EXE ";
                    proc.StartInfo = myStartInfo;

                    proc.Start();
                    proc.WaitForExit(3000);
                    output = proc.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception thrown) { }
        }

        //protocol execution
        private void StartProtocolExecution()
        {
            pnlFailure.Visible = false;
            btnExecuteProtocols.Enabled = false;
            btnCancelProtocolExecution.Enabled = true;
            btnRetryLastInstruction.Enabled = false;
            //probably should try some checks here
            TimeToGo.Text = "Protocol Running";
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
            try
            {
                LoadedProtocols.EmailErrorMessageToAllUsers();
            }
            catch { }
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
                LoadedProtocols.EmailErrorMessageToAllUsers("There was an error in the protocol manager, and the system is down.");
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
        public delegate void StringDel(string firstArg);
        public delegate void StringErrorDel(string arg1, Exception arg2);
        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version : 5.0");
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
        private void btnDeleteProtocol_Click(object sender, EventArgs e)
        {
            RemoveSelectedProtocol();
        }
        private void chkGBO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGBO.Checked)
            {
                chk48WellPlate.Checked = false;
            }
        }
        private void chk48WellPlate_CheckedChanged(object sender, EventArgs e)
        {
            if (chk48WellPlate.Checked)
            { chkGBO.Checked = false; }
        }
        private void btnGenerateNSFData_Click(object sender, EventArgs e)
        {
            try
            {
                int tranNum; string Name;
                List<Protocol> ProtsToAdd = new List<Protocol>();
                //this will create and start a protocol, first to check for errors
                if (txtNSFTransferNumber.Text == "" | txtNSFName.Text == "")
                {
                    ShowError("You did not fill out all of the required fields");
                }
                else if (lstNSFPlates.SelectedIndex == -1)
                {
                    ShowError("You did not select any slots");
                }
                else
                {
                    try { tranNum = Convert.ToInt32(txtNSFTransferNumber.Text); }
                    catch { ShowError("Could not convert your transfer number into an integer, please enter an appropriate value"); return; }
                    NSFExperiment tmp = new NSFExperiment();
                    foreach (int plateslot in lstNSFPlates.SelectedItems)
                    {
                        Protocol NewProt = new Protocol();
                        NewProt.ErrorEmailAddress = "";
                        string baseName = "NSF-" + txtNSFName.Text + "-" + txtNSFTransferNumber.Text.ToString();
                        NewProt.ProtocolName = baseName + "_" + plateslot.ToString();
                        StaticProtocolItem SP = new StaticProtocolItem();
                        SP.MethodName = "CreateProtocol";
                        SP.InstrumentName = tmp.Name;
                        SP.Parameters = new object[2] { baseName, plateslot };
                        NewProt.Instructions.Add(SP);
                        ProtsToAdd.Add(NewProt);
                    }
                    Protocol Watcher = new Protocol();
                    Watcher.ProtocolName = "NSF-Error-" + txtNSFTransferNumber.Text.ToString();
                    Watcher.ErrorEmailAddress = this.NSFErrorEmails;
                    DelayTime DT = new DelayTime();
                    DT.minutes = 60 * 70;
                    Watcher.Instructions.Add(DT);
                    ProtsToAdd.Add(Watcher);
                }
                DialogResult DR = MessageBox.Show("Are you sure you have entered the right values and are ready to load this protocol?", "Final Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DR == DialogResult.Yes)
                {
                    foreach (Protocol p in ProtsToAdd)
                    {
                        LoadedProtocols.AddProtocol(p);
                    }
                    UpdateLoadedProtocols();
                    if (NextInstructionTimer.Enabled == true)//this timer is running while a protocol is waiting to go
                    {
                        DialogResult DR2 = MessageBox.Show("Would you like to start your protocols immediately?", "Begin Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DR2 == DialogResult.Yes)
                        {
                            LoadedProtocols.CurrentProtocolInUse = ProtsToAdd.First();
                            StopClockWaitAndExecute();
                        }
                    }
                }
            }

            catch { ShowError("Weird error occurred"); }
        }
        private void btnInstrumentRefresh_Click(object sender, EventArgs e)
        {
            UpdateInstrumentStatus();
        }
        // TODO: Nothing calls this method, should we get rid of it? 
        private void button1_Click_2(object sender, EventArgs e)
        {
            Protocol P = new Protocol();
            P.ProtocolName = "tmp";
            StaticProtocolItem ReturnInstruction = new StaticProtocolItem();
            ReturnInstruction.MethodName = "ModifyGrowthProtocol";
            ReturnInstruction.InstrumentName = "NSFExperiment";
            //Last item is passed in by the parser
            ReturnInstruction.Parameters = new object[3] { "NSF-111204-0", 31, null };
            P.Instructions.Add(ReturnInstruction);
            LoadedProtocols.AddProtocol(P);
            UpdateLoadedProtocols();
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
