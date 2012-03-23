using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Growth_Curve_Software
{
    class ProtocolRunner
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
        WelcomeForm WF;
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
    }
}
