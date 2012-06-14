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

namespace Clarity
{    
    /// <summary>
    /// This is a generic template form which shows the basics of running Clarity.
    /// This form shows you how to load up and run instruments.  
    /// In general, the first tab shows the display of all things that run.
    /// The second tab allows one to recover errors when something goes wrong.  
    /// There is a third tab the "Incubator tab" which allows for GUI control of an instrument.
    /// This can be useful for various tasks.
    /// </summary>
    public partial class ClarityForm : Form, InstrumentManager
    {

        InstrumentManager Instrument;
        private IncubatorServ Incubator;
        private Twister Robot;
        private TransferStation TransStation;
        private VictorManager PlateReader;
        /// <summary>
        /// This can be set by the files XML, this is the directory to look for data.
        /// </summary>
        public string AppDataDirectory
        {
            get { return pAppDataDirectory; }
            set { pAppDataDirectory = value; }
        }
        WelcomeForm WF;
        public ClarityForm()
        {   
            RecoveryProtocolFile = AppDataDirectory + "Last_Protocol.nfd";
            InitializeComponent();  
        }
        private static InstrumentManagerClass ClarityEngine;
        //Form Loading/Closing methods 
        public void LoadUpForm1()
        {
            try
            {
                //Make a collection to hold everything
                string Filename = BaseInstrumentClass.GetXMLSettingsFile();
                XmlDocument XmlDoc = new XmlDocument();
                XmlTextReader XReader = new XmlTextReader(Filename);
                XmlDoc.Load(XReader);
                //first node is xml, second is the protocol, this is assumed and should be the case
                XmlNode SettingsXML = XmlDoc.SelectSingleNode("//InterfaceSettings");
                BaseInstrumentClass.SetPropertiesByXML(SettingsXML, this);
                XReader.Close();
                
                //Now to load up the instrument manager, remember to register events before initializing it
                ClarityEngine = new InstrumentManagerClass();

                ClarityEngine.OnProtocolStarted += new InstrumentManagerEventHandler(ClarityEngine_OnProtocolStarted);
                ClarityEngine.OnProtocolPaused += new InstrumentManagerEventHandler(ClarityEngine_OnProtocolPaused);
                ClarityEngine.OnProtocolEnded += new InstrumentManagerEventHandler(ClarityEngine_OnProtocolEnded);
                Incubator = this.ReturnInstrumentType<IncubatorServ>();
                TransStation = this.ReturnInstrumentType<TransferStation>();
                PlateReader = this.ReturnInstrumentType<VictorManager>();
            }
            catch (Exception thrown)
            {
                if (thrown is InstrumentError)
                {
                    InstrumentError IE = thrown as InstrumentError;
                    ShowError("Problem Loading: "+IE.InstrumentInError.Name+"\n"
                        +IE.ErrorDescription);
                }
                else
                {
                         ShowError("Problem initializing instruments\n\n" + thrown.Message);
                }
            }
            
        }

        void ClarityEngine_OnProtocolEnded(InstrumentManager Source, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void ClarityEngine_OnProtocolPaused(InstrumentManager Source, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void ClarityEngine_OnProtocolStarted(InstrumentManager Source, EventArgs e)
        {
            pnlFailure.Visible = false;
            btnExecuteProtocols.Enabled = false;
            btnCancelProtocolExecution.Enabled = true;
            btnRetryLastInstruction.Enabled = false;
            //probably should try some checks here
            TimeToGo.Text = "Protocol Running";
        }
        /// <summary>
        /// Shows a splash screen during load to explain that the instruments are working.
        /// The splash screen simply displays the html file WelcomInstructions.htm
        /// </summary>
        public void ShowWelcomeForm()
        {
            // Use full reference or it'll conflict with Skype
            System.Windows.Forms.Application.Run(WF = new WelcomeForm());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            if (UseAlarm)
            {
                Clarity_Alarm = new Alarm();
            }
            
            //Show the welcome form
            Thread LoadingThread = new Thread(ShowWelcomeForm);
            LoadingThread.SetApartmentState(ApartmentState.STA);
            LoadingThread.IsBackground = true;
            LoadingThread.Start();

            //if this delegate is set before the instance is initialized it will fail
            Thread.Sleep(1000);//give it time to initialize the WF              
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
                  
                    lstGrowthRatesProtocol.Items.Add(i);
                }
            }
            cmbShakeSpeed.SelectedIndex = 6;
            CreateRecoveryPanel();

            try
            {
                string CurDirec=System.Environment.CurrentDirectory;
                Uri recovAdd = new Uri(CurDirec+@"\AttemptRecoveryDocument.htm",UriKind.Absolute);
                Uri growthinst = new Uri(CurDirec + @"\GrowthRateInstructionsl.htm",UriKind.Absolute);
                wBrowRecovInstructions.Url = recovAdd;
                wBrowGrowthRate.Url = growthinst;
            }

            catch { }

        }
        /// <summary>
        /// Method called on exit, reports to everyone that the GUI has been closed, and 
        /// releases all the instrument resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
                try { LoadedProtocols.ReportToAllUsers("The Robot Software Has Been Closed"); }
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
        private void KillOldProcesses()
        {
            //really not the best here, but out with the old so the new can load
            string[] ToKill = { "Device Control Unit", "TwisterII Robot ICP", "DCU", "CommDispatcher","ZymarkRobotICP","ScicloneICP","SciPEM","SciRabbitVexta","CavroDeviceController","Consumables" };
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
            catch { }
        } 


        // Incubator Controls    
        private void btnStartShaking_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Incubator.StartShaking();
            }
            catch (Exception thrown)
            {
                ShowError("Could not start shaker\n\n",thrown);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }
        private void btnStopShaking_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Incubator.StopShaking();
            }
            catch (Exception thrown)
            {
                ShowError("Could not stop shaker\n\n",thrown);            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }
        private void btnLoadPlate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lstIncubatorSlots.SelectedIndex != -1)
                {
                    int Slot = (int)lstIncubatorSlots.SelectedItem;
                    Incubator.LoadPlate(Slot);
                }
                else
                {
                    MessageBox.Show("Please Select a Slot First");
                }
            }
            catch(Exception thrown)
            {  
                ShowError("Could not load plate\n",thrown);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }
        private void btnUnloadPlate_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lstIncubatorSlots.SelectedIndex != -1)
                {
                    int Slot = (int)lstIncubatorSlots.SelectedItem;
                    Incubator.UnloadPlate(Slot);
                }
                else
                {
                    MessageBox.Show("Please Select a Slot First");
                }
            }
            catch (Exception thrown)
            {
                string error = "Could not unload, error was: ";
                ShowError(error,thrown);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }
        private void btnPerformCommand_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckPassword())
                {
                    this.Cursor = Cursors.WaitCursor;
                    txtResponse.Text = Incubator.PerformCommandUnsafe(txtCommand.Text,"donkey");
                }
            }
            catch (Exception thrown)
            {
                ShowError("Could not perform Command, error was:\n\n" + thrown.Message);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }
        private void btnChangeShakingSpeed_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cmbShakeSpeed.SelectedIndex == -1)
                {
                    MessageBox.Show("You need to select a speed from the drop down menu above the button");
                }
                else
                {

                    int ShakerSpeed = Convert.ToInt32(cmbShakeSpeed.SelectedItem.ToString());
                    if (ShakerSpeed > 1200 | ShakerSpeed < 0)
                    {
                        MessageBox.Show("You have a bad shaker speed please select a better one");
                    }
                    else
                    {
                        Incubator.ChangeShakingSpeed(ShakerSpeed);
                    }
                }
            }
            catch (Exception thrown)
            {
                ShowError("Could not change speed\n\n", thrown);
            }
            finally { UpdateInstrumentStatus(); this.Cursor = Cursors.Default; }
        }
        private void btnResetIncubator_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Incubator.ResetIncubator();
            }
            catch (Exception thrown)
            {

                ShowError("Could not reset incubator, error was:\n\n", thrown);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }
        private void btnReinitializeIncubator_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Incubator.Initialize(Incubator.STARTING_SPEED);                
                MessageBox.Show("Incubator is now initialized");
            }
            catch(Exception thrown)
            {
                ShowError("Could not reinitialize incubator",thrown);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
        }

        //Form update and addition methods
        public const string INSTRUMENT_NAME_DELIMITER = "_";
        private void CreateRecoveryPanel()
        {
             //this method will create a tab for every instrument, and fill that tab with 
            System.Drawing.Size DefaultButtonSize = new Size(200, 25);
            int maxDepth = 600;//how far down a button can be placed
            int StartX = 20;
            int StartY = 25;
            System.Drawing.Point StartPoint = new Point(StartX, StartY);
            foreach (object c in InstrumentCollection)
            {
                if (c is VirtualInstrument)
                { continue; }
                BaseInstrumentClass Instr = (BaseInstrumentClass)c;
                Type InstType = c.GetType();
                if (Instr.Name == "Macros") { continue; }
                
                Button NewButton = new Button();
                NewButton.Name = Instr.Name + INSTRUMENT_NAME_DELIMITER + "AttemptRecovery";
                
                NewButton.Size = DefaultButtonSize;
                NewButton.AutoSize = true;
                NewButton.Text = "Try to Recover "+Instr.Name;
                NewButton.Location = new Point(StartPoint.X, StartPoint.Y);
                NewButton.Click +=new EventHandler(RecoveryButton_Click);
                tabRecovery.Controls.Add(NewButton);
                this.Refresh();
                StartPoint.Y += DefaultButtonSize.Height+10;
                if (StartPoint.Y >= maxDepth)
                {
                    StartPoint.Y = StartY;
                    StartPoint.X = StartX + 5 + DefaultButtonSize.Width;
                }
            }
            StartPoint.Y += 160;
            foreach (object c in InstrumentCollection)
            {
                if (c is VirtualInstrument)
                { continue; }
                BaseInstrumentClass Instr = (BaseInstrumentClass)c;
                Type InstType = c.GetType();
                Button NewButton = new Button();
                NewButton.Name = Instr.Name + INSTRUMENT_NAME_DELIMITER + "CloseAndFreeUpResources";
                NewButton.Size = DefaultButtonSize;
                NewButton.AutoSize = true;
                NewButton.Text = "Release Resources: " + Instr.Name;
                NewButton.Location = new Point(StartPoint.X, StartPoint.Y);
                NewButton.Click += new EventHandler(RecoveryButton_Click);
                tabRecovery.Controls.Add(NewButton);
                this.Refresh();
                StartPoint.Y += DefaultButtonSize.Height + 10;
                if (StartPoint.Y >= maxDepth)
                {
                    StartPoint.Y = StartY;
                    StartPoint.X = StartX + 5 + DefaultButtonSize.Width;
                }
            }
           
        }                   
        private void UpdateInstrumentStatus()
        {
            lock (InstrumentCollection)
            {
                lstInstrumentStatus.Items.Clear();
                foreach (object o in InstrumentCollection)
                {
                    if (o is VirtualInstrument)
                    { continue; }
                    BaseInstrumentClass BC = (BaseInstrumentClass)o;
                    if (BC.StatusOK != true)
                    {
                        lstInstrumentStatus.Items.Add(BC.Name + " is not working");
                    }
                    else
                    {
                        lstInstrumentStatus.Items.Add(BC.Name + " is working");
                    }
                }
            }
        }
        private void DisplayMakeProtocolsForm()
        {
            // Use full reference to avoid conflict with Skype
            System.Windows.Forms.Application.Run(new MakeProtocols(InstrumentCollection));
        }
        private void UpdateLoadedProtocols()
        {
            //int SelectedIndex = lstLoadedProtocols.SelectedIndex;
            lstSelectedProtocol.Items.Clear();
            lstLoadedProtocols.Items.Clear();
            lstCurrentProtocol.Items.Clear();
            foreach (object o in LoadedProtocols.Protocols)
            {
                lstLoadedProtocols.Items.Add(o);
            }
            //lstLoadedProtocols.SelectedIndex = SelectedIndex;
        }
        private void UpdateForm()
        {
            try
            {
                UpdateInstrumentStatus();
                UpdateLoadedProtocols();
                lstCurrentProtocol.Items.Clear();
                if (LoadedProtocols.CurrentProtocolInUse != null)
                {
                    Protocol selectedProtocol = (Protocol)LoadedProtocols.CurrentProtocolInUse;
                    int FinishedUPTO = selectedProtocol.NextItemToRun;
                    lblCurrentRunningProtocol.Text = "Current Running Protocol - " + selectedProtocol.ToString();
                    if (UseAlarm)
                    {

                        Clarity_Alarm.ChangeStatus("Current Running Protocol - " + selectedProtocol.ToString());
                    }
                    for (int j = 0; j < selectedProtocol.Instructions.Count; j++)
                    {
                        if (j < FinishedUPTO)
                        { lstCurrentProtocol.Items.Add(selectedProtocol.Instructions[j].ToString() + "   --  Finished"); }
                        else { lstCurrentProtocol.Items.Add(selectedProtocol.Instructions[j].ToString() + "   --   To Be Executed"); }
                    }
                    if (FinishedUPTO < lstCurrentProtocol.Items.Count)
                    { lstCurrentProtocol.SelectedIndex = FinishedUPTO; }
                }
                else
                {
                    if (UseAlarm)
                    {
                        Clarity_Alarm.ChangeStatus("Nothing Running");
                    }
                }

            }
            catch (Exception thrown)
            {
                txtErrorLog.Text += "\nCould not update user interface when requested at " + DateTime.Now.ToString() + ".\nError is:" + thrown.Message + "\n";
            }

        }
        private void lstLoadedProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstSelectedProtocol.Items.Clear();
            if (lstLoadedProtocols.SelectedIndex != -1)
            {
                try
                {
                    Protocol selectedProtocol = (Protocol)lstLoadedProtocols.SelectedItem;
                    int FinishedUPTO = selectedProtocol.NextItemToRun;
                    for (int j = 0; j < selectedProtocol.Instructions.Count; j++)
                    {
                        if (j < FinishedUPTO)
                        { lstSelectedProtocol.Items.Add(selectedProtocol.Instructions[j].ToString() + "   --  Finished"); }
                        else { lstSelectedProtocol.Items.Add(selectedProtocol.Instructions[j].ToString() + "   --   To Be Executed"); }
                    }
                }
                catch { MessageBox.Show("Could not display the protocol"); }
            }
        }
        private void AddErrorLogText(string ErrorInfo)
        {
            txtErrorLog.Text = txtErrorLog.Text+ ErrorInfo;
        }


        //UI Methods
        private void btnViewAdvancedControls_Click(object sender, EventArgs e)
        {
            //this method will create a tab for every instrument, and fill that tab with 
            if (CheckPassword())
            {
                System.Drawing.Size DefaultButtonSize = new Size(200, 25);
                int maxDepth = 500;//how far down a button can be placed
                int StartX = 50;
                int StartY = 50;
                foreach (object c in InstrumentCollection)
                {
                    BaseInstrumentClass Instr = (BaseInstrumentClass)c;
                    TabPage NewTab = new TabPage(Instr.Name);
                    Type InstType = c.GetType();
                    System.Drawing.Point StartPoint = new Point(StartX, StartY);
                    MainTab.TabPages.Add(NewTab);
                    foreach (MethodInfo MI in InstType.GetMethods())
                    {
                        if (MI.GetParameters().Length == 0)
                        {
                            //Make a button
                            Button NewButton = new Button();
                            NewButton.Name = Instr.Name + INSTRUMENT_NAME_DELIMITER + MI.Name;
                            NewButton.Size = DefaultButtonSize;
                            NewButton.Text = MI.Name;
                            NewButton.Location = new Point(StartPoint.X, StartPoint.Y);
                            NewButton.Click += new System.EventHandler(handleAdvancedButton);
                            NewTab.Controls.Add(NewButton);
                            StartPoint.Y += DefaultButtonSize.Height;
                            if (StartPoint.Y >= maxDepth)
                            {
                                StartPoint.Y = StartY;
                                StartPoint.X = StartX + 5 + DefaultButtonSize.Width;
                            }
                        }
                        else if (MI.GetParameters()[0].ParameterType.Name == "IncubatorServ")
                        {
                            //Make a button
                            Button NewButton = new Button();
                            NewButton.Name = Instr.Name + INSTRUMENT_NAME_DELIMITER + MI.Name + "@" + "IncubatorServ";
                            NewButton.Size = DefaultButtonSize;
                            NewButton.Text = MI.Name;
                            NewButton.Location = new Point(StartPoint.X, StartPoint.Y);
                            NewButton.Click += new System.EventHandler(handleAdvancedButton);
                            NewTab.Controls.Add(NewButton);
                            StartPoint.Y += DefaultButtonSize.Height;
                            if (StartPoint.Y >= maxDepth)
                            {
                                StartPoint.Y = StartY;
                                StartPoint.X = StartX + 5 + DefaultButtonSize.Width;
                            }
                        }
                    }
                }
            }
           
        }
        private void handleAdvancedButton(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //This is the sister method to the one above, for a given button that is named
            //Instrument_Method this will run that method, note only methods with no parameters should be called this way
            Button myButton = (Button)sender;
            string[] InsMet = myButton.Name.Split(INSTRUMENT_NAME_DELIMITER.ToCharArray()[0]);
            string InstrumentName = InsMet[0];
            string MethodName = InsMet[1];
            if (myButton.Name.IndexOf("@") != -1)
            {
                MethodName = MethodName.Split('@')[0];
                if (myButton.Name.Split('@')[1] == "IncubatorServ")
                {
                    object[] Params = new object[1] { Incubator };
                    RunInstrumentMethod(InstrumentName, MethodName, Params,true);
                }
            }
            else
            {
                RunInstrumentMethod(InstrumentName, MethodName,null,true);
            }
            delVoidVoid del_Update = UpdateInstrumentStatus;
            this.Invoke(del_Update);
            this.Cursor = Cursors.Default;
        }
        private void loadProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Title = "Select your protocol file to load it";
                OFD.DefaultExt = ".xml";
                OFD.Filter = "XML file (*.xml)|*.xml";
                DialogResult DR = OFD.ShowDialog();
                if (DR == DialogResult.OK)
                {
                    string Filename = OFD.FileName;
                    Protocol NewProtocol = ProtocolConverter.XMLFileToProtocol(Filename);
                    NewProtocol.Instructions = ProtocolConverter.ProtocolWithRepeatsToProtocolWithout(NewProtocol.Instructions);

                    LoadedProtocols.AddProtocol(NewProtocol);
                    UpdateLoadedProtocols();
                    if (NextInstructionTimer.Enabled == true)//this timer is running while a protocol is waiting to go
                    {
                        DialogResult DR2 = MessageBox.Show("Would you like to start your protocol immediately?", "Begin Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DR2 == DialogResult.Yes)
                        {
                            LoadedProtocols.CurrentProtocolInUse = NewProtocol;
                            StopClockWaitAndExecute();
                        }
                    }
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not load your protocol.  Error is:\n\n" + thrown.Message, "Unable to Load Protocol", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowError(string ErrorMessage)
        {
            ErrorMessage= "\nNew Error at " + DateTime.Now.ToString() + " \n" + ErrorMessage;
            AddErrorLogText(ErrorMessage);
            MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ShowError(string ErrorMessage, Exception Error)
        {
            if (Error!=null && Error.InnerException != null  && Error.InnerException.Message!=null)
            {
                Exception tempExcep = Error;
                int InnerExceptionCounter=0;
                while (InnerExceptionCounter < 4)
                {
                    ErrorMessage += "\n\n" + tempExcep.InnerException.Message;
                    if (tempExcep.InnerException == null || tempExcep.InnerException.Message==null)
                    { break; }
                    else { tempExcep = tempExcep.InnerException; }
                    InnerExceptionCounter++;                
                }
                ShowError(ErrorMessage);               
            }
            else if (Error != null && Error.Message == null)
            {
                ShowError(ErrorMessage += "\nException was null, no message");
            }
            else if (Error == null)
            {
                ShowError(ErrorMessage);
            }
            else
            {
                ShowError(ErrorMessage += "\n" + Error.Message);
            }
        }       
        private void btnMakeProtocols_Click(object sender, EventArgs e)
        {
            Thread MakeProtocolsThread = new Thread(DisplayMakeProtocolsForm);
            MakeProtocolsThread.IsBackground = true;
            MakeProtocolsThread.SetApartmentState(ApartmentState.STA);
            MakeProtocolsThread.Start();
        }
        private void RecoveryButton_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;
            Button myButton = (Button)sender;
            string[] InsMet = myButton.Name.Split(INSTRUMENT_NAME_DELIMITER.ToCharArray()[0]);
            string InstrumentName = InsMet[0];
            string MethodName = InsMet[1];
            RunInstrumentMethod(InstrumentName,MethodName,null,false);
            delVoidVoid del_Update = UpdateInstrumentStatus;
            this.Invoke(del_Update);
            this.Cursor = Cursors.Default;
        }
        private void RemoveSelectedProtocol()
        {
            try
            {
                if (lstLoadedProtocols.SelectedIndex != -1)
                {
                    Protocol toRemove = (Protocol)lstLoadedProtocols.SelectedItem;
                    DialogResult DR = MessageBox.Show("Do you want to delete this protocol?  Please only delete your own protocols.", "Delete Protocol?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (DR == DialogResult.OK)
                    {
                        bool OkayToRemove = true;
                        bool ResetTimerAfterwards = false;//will be used to determine if we need to reset the timer
                        if (WorkerToRunRobots != null && WorkerToRunRobots.IsBusy)
                        {
                            //protocol is running
                            if (LoadedProtocols.CurrentProtocolInUse == toRemove)
                            {
                                OkayToRemove = false;
                                MessageBox.Show("Cannot delete currently executing protocol, please stop the protocol and then delete it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if (LoadedProtocols.CurrentProtocolInUse == toRemove)
                        {
                            ResetTimerAfterwards = true;
                        }
                        if (OkayToRemove)
                        {
                            LoadedProtocols.RemoveProtocol(toRemove);
                            UpdateLoadedProtocols();
                        }
                        if (btnExecuteProtocols.Enabled==false && ResetTimerAfterwards && OkayToRemove)
                        {
                            double timeMS=LoadedProtocols.GetMilliSecondsTillNextRunTime();
                            TimeSpan delay = new TimeSpan(10000*(Convert.ToInt64(timeMS) + 1));
                            ProtocolEvents.FirePauseEvent(delay);
                            int Delay = Convert.ToInt32(timeMS) + 1;//always make sure the value is greater then 0
                            SetDelayAndStartWait(Delay);
                            //StopClockWaitAndExecute();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Protocol Selected");
                }
            }
            catch { MessageBox.Show("Could not delete protocol, odd error"); }
        }
        private void lstLoadedProtocols_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyData == Keys.Delete)
                {
                    RemoveSelectedProtocol();
                }           
           
        }
        private void btnRetryLastInstruction_Click(object sender, EventArgs e)
        {
            try
            {
                if (LastFailedInstruction != null)
                {
                    RunInstrumentMethod(LastFailedInstruction.InstrumentName, LastFailedInstruction.MethodName, LastFailedInstruction.Parameters, true,LastFailedInstruction.ContainingProtocol);
                }
            }
            catch { MessageBox.Show("Last instruction failed"); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StaticProtocolItem Instruction = LoadedProtocols.GetNextProtocolObject() as StaticProtocolItem;
            MessageBox.Show(Instruction.InstrumentName + "," + Instruction.Parameters.ToString());
            //then we run the protocol item
            bool result = RunInstrumentMethod(Instruction.InstrumentName, Instruction.MethodName, Instruction.Parameters, true);

        }
        private void btnStartGrowthRate_Click(object sender, EventArgs e)
        {
            try
            {
                //this will create and start a protocol, first to check for errors
                int minuteDelay = 0;
                int cycles = 0;
                if (txtGrowthRateEmail.Text == "" | txtGrowthRateExperimentName.Text == "" | txtGrowthRateMinutes.Text == "" | txtGrowthRateTimesToMeasure.Text == "")
                {
                    ShowError("You did not fill out all of the required fields");
                }
                else if (lstGrowthRatesProtocol.SelectedIndex == -1)
                {
                    ShowError("You did not select any slots");
                }
                // For SkypeAlarm
                else if (! Clarity_Alarm.ValidNumbers(textbox_number.Text))
                {
                    ShowError("Your number is not valid!");
                }
                else
                {
                    try { minuteDelay = Convert.ToInt32(txtGrowthRateMinutes.Text); cycles = Convert.ToInt32(txtGrowthRateTimesToMeasure.Text); }
                    catch { ShowError("Could not convert your delay or measurement into an integer, please enter an appropriate value"); return; }
                    if (minuteDelay < 9 | minuteDelay > 25 * 60)
                    {
                        ShowError("Your delay time is  weird, please make it greater then 9 minutes or less than 25 hours");
                        return;
                    }
                    if (cycles < 0 | cycles > 200)
                    {
                        ShowError("You seem to have selected less than 0 or more than 200 readings, please pick an alternative");
                        return;
                    }
                    Protocol NewProt = new Protocol();
                    NewProt.ProtocolName = txtGrowthRateExperimentName.Text;
                    NewProt.ErrorEmailAddress = txtGrowthRateEmail.Text;
                    NewProt.ErrorPhoneNumber = textbox_number.Text;
                    NewProt.Instructions = new List<ProtocolInstruction>();
                    for (int i = 0; i < cycles; i++)
                    {
                        foreach (int plateslot in lstGrowthRatesProtocol.SelectedItems)
                        {
                            //move from incubator to platereader 
                            StaticProtocolItem SP = new StaticProtocolItem();
                            SP.InstrumentName = "Macros";
                            SP.Parameters = new object[1] { plateslot };
                            SP.MethodName = "MovePlateFromIncubatorToVictor";
                            NewProt.Instructions.Add(SP);
                            //read plate
                            StaticProtocolItem SP2;
                            if (chk48WellPlate.Checked)
                            {
                                SP2 = new StaticProtocolItem();
                                SP2.MethodName = "ReadPlate2";
                                SP2.InstrumentName = "PlateReader";
                                SP2.Parameters = new object[2] { NewProt.ProtocolName + INSTRUMENT_NAME_DELIMITER + plateslot.ToString(), WELL48_PLATE_PROTOCOL_ID };
                                NewProt.Instructions.Add(SP2);

                            }
                            else if (chkGBO.Checked)
                            {
                                SP2 = new StaticProtocolItem();
                                SP2.MethodName = "ReadPlate2";
                                SP2.InstrumentName = "PlateReader";
                                SP2.Parameters = new object[2] { NewProt.ProtocolName + INSTRUMENT_NAME_DELIMITER + plateslot.ToString(), GBO_PLATE_PROTOCOL_ID };
                                NewProt.Instructions.Add(SP2);
                            }
                            else
                            {
                                SP2 = new StaticProtocolItem();
                                SP2.MethodName = "ReadPlate";
                                SP2.InstrumentName = "PlateReader";
                                SP2.Parameters = new object[1] { NewProt.ProtocolName + INSTRUMENT_NAME_DELIMITER + plateslot.ToString() };
                                NewProt.Instructions.Add(SP2);
                            }
                            //now to move the old plate back
                            SP2 = SP.Clone();
                            SP2.MethodName = "MovePlateFromVictorToIncubatorWithLidOnTransferStation";
                            NewProt.Instructions.Add(SP2);
                        }
                        DelayTime DT = new DelayTime();
                        DT.minutes = minuteDelay;
                        NewProt.Instructions.Add(DT);
                    }
                    DialogResult DR = MessageBox.Show("Are you sure you have entered the right values and are ready to load this protocol?", "Final Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        LoadedProtocols.AddProtocol(NewProt);
                        UpdateLoadedProtocols();
                        if (NextInstructionTimer.Enabled == true)//this timer is running while a protocol is waiting to go
                        {
                            DialogResult DR2 = MessageBox.Show("Would you like to start your protocol immediately?", "Begin Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DR2 == DialogResult.Yes)
                            {
                                LoadedProtocols.CurrentProtocolInUse = NewProt;
                                StopClockWaitAndExecute();
                            }
                        }
                    }
                }
            }
            catch { ShowError("Weird error occurred"); }
        }
        private bool CheckPassword()
        {
            if (txtPassword.Text.ToUpper() == "DONKEY") { return true; }
            else { MessageBox.Show("You do not have permission to make the advanced controls"); return false; }
        }
        private void btnReleaseCom_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckPassword())
                {
                    Incubator.ReleaseComPort("donkey");
                }
            }
            catch (Exception thrown)
            {
                ShowError("Could not release com", thrown);
            }
        }
        private void btnStopIncubatorShaking_Click(object sender, EventArgs e)
        {
            try
            {
                Incubator.StopIncubatorShakingWhenNothingElseWill("");

            }
            catch (Exception thrown)
            {
                ShowError("Shaking Stop failed", thrown);
            }
        }
        private void btnEmailOkay_Click(object sender, EventArgs e)
        {
            try
            {
                LoadedProtocols.ReportToAllUsers("The Robot System Is Okay Now ");
                MessageBox.Show("Clarity has sent a message to all users");
                if (UseAlarm)
                { Clarity_Alarm.TurnOffAlarm(); }
            }
            catch
            {
                MessageBox.Show("Could not send messages to all");
            }
        }
        private void saveCurrentProtocolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (LoadedProtocols.Protocols.Count == 0)
                {
                    MessageBox.Show("There are no currently loaded protocols");
                }
                else
                {
                    SaveFileDialog SFD = new SaveFileDialog();
                    SFD.Title = "Select the Old Protocols file to load it";
                    SFD.DefaultExt = ".cpm";
                    SFD.Filter = "CPM file (*.cpm)|*.cpm";
                    SFD.AddExtension = true;

                    SFD.Title = "Create a File To Save the Loaded Protocols";
                    DialogResult DR = SFD.ShowDialog();
                    if (DR == DialogResult.OK)
                    {
                        OutputRecoveryFile(SFD.FileName);
                        MessageBox.Show("File Saved");
                    }
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could Not Save File, Error is " + thrown.Message);
            }
        }
        private void loadPreviousSystemStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult DR;
                if (LoadedProtocols.Protocols.Count > 0)
                {
                    DR = MessageBox.Show("Are you sure you want to load another set of protocols?  Doing so will remove the current set, and prevent a recovery of their state."
                       + "  If there are no loaded protocols, feel free to hit okay.", "Confirm Protocol Load", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    DR = DialogResult.OK;
                }
                if (DR == DialogResult.OK)
                {
                    OpenFileDialog OFD = new OpenFileDialog();
                    OFD.Title = "Select the Old Protocols file to load it";
                    OFD.DefaultExt = ".cpm";
                    OFD.Filter = "Clarity Protocols File (*.cpm)|*.cpm";
                    DR = OFD.ShowDialog();
                    if (DR == DialogResult.OK)
                    {
                        InputRecoveryFile(OFD.FileName);
                        UpdateLoadedProtocols();
                    }
                }
            }

            catch (Exception thrown)
            {
                MessageBox.Show("Could not load previous protocols.  \n\n" + thrown.Message);
            }
        }
        private void btnChangeCurrentProtPosition_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstLoadedProtocols.SelectedIndex == -1 | lstSelectedProtocol.SelectedIndex == -1)
                {
                    ShowError("There are no loaded protocols to change, or you have not selected an instruction.");
                }
                else
                {
                    Protocol SelectedProtocol = (Protocol)lstLoadedProtocols.SelectedItem;
                    DialogResult DR = MessageBox.Show("Are you sure you want to change the protocol so that it performs your instruction next?", "Confirm Protocol Change", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (DR == DialogResult.OK)
                    {
                        SelectedProtocol.NextItemToRun = lstSelectedProtocol.SelectedIndex;
                        int SelectProtIndex = lstLoadedProtocols.SelectedIndex;
                        int SelectInstruIndex = lstSelectedProtocol.SelectedIndex;
                        UpdateLoadedProtocols();
                        lstLoadedProtocols.SelectedIndex = SelectProtIndex;
                        lstSelectedProtocol.SelectedIndex = SelectInstruIndex;
                        DR = MessageBox.Show("Should We Make This Instruction Run ASAP?", "Confirm Protocol Change", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (DR == DialogResult.OK)
                        {
                            SelectedProtocol.NextExecutionTimePoint = DateTime.Now;
                            if (NextInstructionTimer.Enabled == true)//this timer is running while a protocol is waiting to go
                            {
                                LoadedProtocols.CurrentProtocolInUse = SelectedProtocol;
                                StopClockWaitAndExecute();
                            }
                        }
                    }
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Error: could not load your protocol\n\n" + thrown.Message);
            }
        }
        private void recoverLastProtcolInstructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult DR;
                if (LoadedProtocols.Protocols.Count > 0)
                {
                    DR = MessageBox.Show("Are you sure you want to load the most recent set of protocols?  Doing so will remove the current set, and prevent a recovery of their state."
                       + "  If there are no loaded protocols, feel free to hit okay.", "Confirm Protocol Load", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    DR = DialogResult.OK;
                }
                if (DR == DialogResult.OK)
                {
                    InputRecoveryFile(RecoveryProtocolFile);
                    UpdateLoadedProtocols();
                }
            }

            catch (Exception thrown)
            {
                MessageBox.Show("Could not load previous protocols.  \n\n" + thrown.Message);
            }
        }
        private void getAlarmStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UseAlarm)
            {
                Alarm.AlarmState CurStatus = Clarity_Alarm.IsAlarmOn();
                if (CurStatus == Alarm.AlarmState.On)
                {
                    MessageBox.Show("Connected and On");
                }
                else if (CurStatus == Alarm.AlarmState.Off)
                {
                    MessageBox.Show("Connected and Off");
                }
                else
                {
                    MessageBox.Show("Not Connected to Monitoring Software");
                }
            }
            else
            {
                MessageBox.Show("Software was initialized with the alarm disabled");
            }
        }
        private void turnOnAlarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UseAlarm)
                Clarity_Alarm.TurnOnAlarm("User activated alarm");
            else
                MessageBox.Show("Alarm disabled at startup");
        }
        private void turnOffAlarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(UseAlarm)
                Clarity_Alarm.TurnOffAlarm("User Ended Alarm");
             else
                MessageBox.Show("Alarm disabled at startup");
        }
        private void reconnectToAlarmServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (UseAlarm)
                {
                    Clarity_Alarm = new Alarm();
                    if (Clarity_Alarm.Connected) { LoadedProtocols.UpdateAlarmProtocols(); }
                }
                else
                    MessageBox.Show("Alarm disabled at startup");
            }
            catch (Exception thrown) { MessageBox.Show(thrown.Message); }
        }
        private void btnResetLid_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckPassword())
                {
                    this.Cursor = Cursors.WaitCursor;
                    RunInstrumentMethod("Macros", "AttemptToReseatLid", null, true);
                }
            }
            catch (Exception thrown)
            {
                ShowError("Could not perform Command, error was:\n\n" + thrown.Message);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
            
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
            FileStream f=null;
            try
            {
                    f = new FileStream(FileNameToRead, FileMode.Open);
                    BinaryFormatter b = new BinaryFormatter();
                //This next bit seems odd to me, got it off a forum as a way to correct an end of stream error
                    f.Seek(0, 0);
                    ProtocolManager ReplacementProtocols=(ProtocolManager)b.Deserialize(f);
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
            if(chk48WellPlate.Checked)
            { chkGBO.Checked = false; }
        }
        
        private void btnInstrumentRefresh_Click(object sender, EventArgs e)
        {
            UpdateInstrumentStatus();
        }
      
    }
}

