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
    /// Any Automation GUI can be created off of this as a template.
    /// 
    /// The form has several tabs, the first of which is the main one which shows the executions and current
    /// state of the automation process, which protocols are running etc.
    /// 
    /// The second tab is a recovery tab that allows users to recover instruments that have broken.
    /// 
    /// The third tab is a demonstration tab that creates protocols to be run of a specific type for a user.
    /// 
    /// The fourth tab demonstrates controlling an instrument directly from the GUI.  In this case it is an 
    /// This form shows you how to load up and run instruments.  
    /// In general, the first tab shows the display of all things that run.
    /// The second tab allows one to recover errors when something goes wrong.  
    /// There is a third tab the "Incubator tab" which allows for GUI control of an instrument.
    /// This can be useful for various tasks.
    /// </summary>
    public partial class ClarityForm : Form    {

        /// <summary>
        /// This is the engine that runs the protocols and manages the instruments
        /// </summary>
        private InstrumentManagerClass ClarityEngine;
        //These is an instruments we want to have the GUI gain access to
        //Demonstrated on the Incubator tab.
        private IncubatorServ Incubator;
        /// <summary>
        /// For certain controls we might only want some users to have access, this is the password setting.
        /// </summary>
        public const string PASSWORD = "PASSWORD";
        //This region is not part of the final release, can be ignored.
        #region ToRemove
        private string pErrorEmails = "ndelaney@fas.harvard.edu;4158234767@vtext.com";
        public string NSFErrorEmails
        {
            get { return pErrorEmails; }
            set { pErrorEmails = value; }
        }
        int[] ExcludedIncubatorPositions = { 19, 38 };
        private void btnGenerateNSFData_Click(object sender, EventArgs e)
        {
            try
            {
                int tranNum;
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
                    Watcher.ErrorPhoneNumber = "4158234767";
                    Watcher.MaxIdleTimeBeforeAlarm = 50;
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
                        ClarityEngine.AddProtocol(p);
                    }
                    UpdateLoadedProtocols();
                    if (ClarityEngine.CurrentRunningState==RunningStates.Idle)//this timer is running while a protocol is waiting to go
                    {
                        DialogResult DR2 = MessageBox.Show("Would you like to start your protocols immediately?", "Begin Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DR2 == DialogResult.Yes)
                        {
                            ClarityEngine.StartProtocolExecution();
                        }
                    }
                }
            }

            catch { ShowError("Weird error occurred"); }
        }
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
        #endregion
        WelcomeForm WF;
        public ClarityForm()
        {   
            InitializeComponent();  
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
       
        /// <summary>
        /// Load up the GUI and initialize the runtime engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUI_Load(object sender, EventArgs e)
        {
            //Create a splash screen on a separate thread to let the user know we are loading.
            Thread LoadingThread = new Thread(ShowWelcomeForm);
            LoadingThread.SetApartmentState(ApartmentState.STA);
            LoadingThread.IsBackground = true;
            LoadingThread.Start();

            //First load up any XML Settings for the GUI
            try
            {
                string Filename = BaseInstrumentClass.GetXMLSettingsFile();
                XmlDocument XmlDoc = new XmlDocument();
                XmlTextReader XReader = new XmlTextReader(Filename);
                XmlDoc.Load(XReader);
                //first node is xml, second is the protocol, this is assumed and should be the case
                XmlNode SettingsXML = XmlDoc.SelectSingleNode("//InterfaceSettings");
                BaseInstrumentClass.SetPropertiesByXML(SettingsXML, this);
                XReader.Close();
            }
            catch (Exception thrown)
            {
                WF.StayAlive = false;
                ShowError("Could not load settings for interface from XML.\nError is:" + thrown.Message);
                this.Close();
                Application.Exit();
            }
            //Give the Splash screen time to load
            Thread.Sleep(1000);//give it time to initialize the WF              
            //Now Load up the 
            bool InstrumentsLoadedSuccessfully = LoadUpClarityEngine();
            WF.StayAlive = false;



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
            if (ClarityEngine != null)
            {
                UpdateInstrumentStatus();
                CreateRecoveryPanel();
            }
            try
            {
                string CurDirec = System.Environment.CurrentDirectory;
                Uri recovAdd = new Uri(CurDirec + @"\AttemptRecoveryDocument.htm", UriKind.Absolute);
                Uri growthinst = new Uri(CurDirec + @"\GrowthRateInstructionsl.htm", UriKind.Absolute);
                wBrowRecovInstructions.Url = recovAdd;
                wBrowGrowthRate.Url = growthinst;
            }

            catch { }
            if (!InstrumentsLoadedSuccessfully)
                ShowError("There was a problem during loading, please read any errors and then exit.  Clarity will not work.");
        }
     
        /// <summary>
        /// This method loads up the Clarity Engine and initializes all the instruments
        /// Errors here should lead to shutdown/exits.
  /// </summary>
  /// <returns>True if load successful</returns>
        public bool LoadUpClarityEngine()
        {
            try
            {   
                //Now to load up the instrument manager
                try
                {
                     ClarityEngine = new InstrumentManagerClass();
                }
                catch(Exception thrown)
                {
                    ExitDueToError("Could not create Clarity Engine ",thrown);
                }
                //Any GUI should register for all of these events
                ClarityEngine.OnProtocolsStarted += new InstrumentManagerEventHandler(ClarityEngine_OnProtocolStarted);
                ClarityEngine.OnProtocolPaused += new ProtocolPauseEventHandler(ClarityEngine_OnProtocolPaused);
                ClarityEngine.OnAllRunningProtocolsEnded += new InstrumentManagerEventHandler(ClarityEngine_OnAllProtocolsEnded);
                ClarityEngine.OnGenericError += new InstrumentManagerErrorHandler(ClarityEngine_OnGenericError);
                ClarityEngine.OnProtocolSuccessfullyCancelled += new InstrumentManagerEventHandler(ClarityEngine_OnProtocolSuccessfullyCancelled);
                ClarityEngine.OnErrorDuringProtocolExecution += new InstrumentManagerErrorHandler(ClarityEngine_OnErrorDuringProtocolExecution);
                ClarityEngine.OnProtocolExecutionUpdates += new InstrumentManagerEventHandler(ClarityEngine_OnProtocolExecutionUpdates);
                //Make sure we can load the instruments we need to.
                try
                {
                    ClarityEngine.LoadUpInstruments();
                }
                catch(Exception thrown)
                {
                    ExitDueToError("Could not initialize the instruments",thrown);
                }
                //Finally, if we wanted our GUI to have control of some instruments for the user,
                //we can get references to them from the engine like so:
                Incubator = ClarityEngine.ReturnInstrumentType<IncubatorServ>();
                
                return true;
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
                return false;
            }
            
        }
        /// <summary>
        /// Displays an error message to the user
        /// </summary>
        /// <param name="ErrorMessage"></param>
        private void ShowError(string ErrorMessage)
        {
            ErrorMessage = "\nNew Error at " + DateTime.Now.ToString() + " \n" + ErrorMessage;
            AddErrorLogText(ErrorMessage);
            MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ShowError(string ErrorMessage, Exception Error)
        {
            if (Error != null && Error.InnerException != null && Error.InnerException.Message != null)
            {
                Exception tempExcep = Error;
                int InnerExceptionCounter = 0;
                while (InnerExceptionCounter < 4)
                {
                    ErrorMessage += "\n\n" + tempExcep.Message;
                    if (tempExcep.InnerException == null || tempExcep.InnerException.Message == null)
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
        /// <summary>
        /// Call when an error is so bad the program should exit.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="thrown"></param>
        private void ExitDueToError(string Message, Exception thrown)
        {
            ShowError(Message+"\nError is: " + thrown.Message);
            this.Close();
            Application.Exit();
        }


        #region ClarityEngine Event Handler
        void ClarityEngine_OnProtocolExecutionUpdates(InstrumentManager Source, EventArgs e)
        {
            lock (Source.LoadedProtocols)
            {
                try
                {
                    UpdateForm();
                    try
                    {
                        StatusLabel.Text = "Performing operation: " + Source.LoadedProtocols.CurrentProtocolInUse.Instructions[Source.LoadedProtocols.CurrentProtocolInUse.NextItemToRun - 1].ToString();
                    }
                    catch { }
                }
                catch { }
            }
        }

        void ClarityEngine_OnErrorDuringProtocolExecution(InstrumentManager Source, Exception thrown)
        {
            StatusLabel.Text = "Procedure ended with errors";
            btnRetryLastInstruction.Enabled = true;
            ShowError("Failed To Run Protocol", thrown);
            pnlFailure.Visible = true;
            lblFailure.Text = "The Last Instruction Failed To Run, Please recover the machines and retry or delete the protocol, Do not reattempt a macro instruction";
            if (Source.GetLastFailedInstruction() != null)
            {
                lblFailureInstructionName.Text = "Would you like to reattempt: " + Source.GetLastFailedInstruction().ToString();
            }
            else
            {
                btnRetryLastInstruction.Enabled = false;
                StringDel newDel = this.AddErrorLogText;
                object[] myarr = new object[1] { "\nWeird command failure, the last instruction is not available" };
                this.Invoke(newDel, myarr);
            }
            btnExecuteProtocols.Enabled = true;
            btnCancelProtocolExecution.Enabled = false;

            if (thrown is ProtcolExcecutionError)
            {
                StringDel newDel = this.AddErrorLogText;
                object[] myarr = new object[1] { ((ProtcolExcecutionError)thrown).MakeErrorReport() };
                this.Invoke(newDel, myarr);
            }
        }

        void ClarityEngine_OnProtocolSuccessfullyCancelled(InstrumentManager Source, EventArgs e)
        {
            btnCancelProtocolExecution.Enabled = false;
            btnExecuteProtocols.Enabled = true;
            StatusLabel.Text = "Cancelled Protocol";
            TimeToGo.Text = "Nothing Running";
            UpdateForm();
        }

        void ClarityEngine_OnGenericError(InstrumentManager Source, Exception thrown)
        {
            TimeToGo.Text = "Nothing Running";
            btnExecuteProtocols.Enabled = true;
            btnCancelProtocolExecution.Enabled = false;
            ShowError("Unspecified Error", thrown);
            UpdateForm();

            string ErrorMessage = thrown.Message;
            StringDel newDel = this.AddErrorLogText;
            object[] myarr = new object[1] { ErrorMessage };
            this.Invoke(newDel, myarr);
        }

        void ClarityEngine_OnAllProtocolsEnded(InstrumentManager Source, EventArgs e)
        {
            StatusLabel.Text = "Finished Running Protocols";
            TimeToGo.Text = "";
        }

        void ClarityEngine_OnProtocolPaused(InstrumentManager Source, TimeSpan TS)
        {
            StatusLabel.Text = "Waiting Until It Is Time To Run The Next Protocol Instruction";
            TimeToGo.Start((int)TS.TotalMilliseconds);
            UpdateForm();
        }

        void ClarityEngine_OnProtocolStarted(InstrumentManager Source, EventArgs e)
        {
            pnlFailure.Visible = false;
            btnExecuteProtocols.Enabled = false;
            btnCancelProtocolExecution.Enabled = true;
            btnRetryLastInstruction.Enabled = false;
            //probably should try some checks here
            TimeToGo.Stop();
            TimeToGo.Text = "Protocol Running";
            
        } 
        #endregion

       

        
        
        /// <summary>
        /// Method called on exit, reports to everyone that the GUI has been closed, and 
        /// releases all the instrument resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (ClarityEngine != null)
            {
                ClarityEngine.ShutdownEngine();
            }
            this.Cursor = Cursors.Default;
        }

        // Incubator Controls, demonstrates a tab that controls an instrument.    
        #region IncubatorTabCode
        private void CreateIncubatorCommands()
        {
            for (int i = 38; i > 0; i--)
            {
                lstIncubatorSlots.Items.Add(i);
                lstGrowthRatesProtocol.Items.Add(i);

            }
            cmbShakeSpeed.SelectedIndex = 6;

        }
        private void btnStartShaking_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Incubator.StartShaking();
            }
            catch (Exception thrown)
            {
                ShowError("Could not start shaker\n\n", thrown);
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
                ShowError("Could not stop shaker\n\n", thrown);
            }
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
            catch (Exception thrown)
            {
                ShowError("Could not load plate\n", thrown);
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
                ShowError(error, thrown);
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
                    txtResponse.Text = Incubator.PerformCommandUnsafe(txtCommand.Text, "donkey");
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
            catch (Exception thrown)
            {
                ShowError("Could not reinitialize incubator", thrown);
            }
            finally { this.Cursor = Cursors.Default; UpdateInstrumentStatus(); }
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
        #endregion

        //Form update and addition methods
        public const char INSTRUMENT_NAME_DELIMITER = '_';
        private void CreateRecoveryPanel()
        {
            //this method will create a tab for every instrument, and fill that tab with 
            System.Drawing.Size DefaultButtonSize = new Size(200, 25);
            int maxDepth = 600;//how far down a button can be placed
            int StartX = 20;
            int StartY = 25;
            System.Drawing.Point StartPoint = new Point(StartX, StartY);
            foreach (object c in ClarityEngine.InstrumentCollection)
            {
                if (c is VirtualInstrument)
                { continue; }
                BaseInstrumentClass Instr = (BaseInstrumentClass)c;
                Type InstType = c.GetType();
              

                Button NewButton = new Button();
                NewButton.Name = Instr.Name + INSTRUMENT_NAME_DELIMITER + "AttemptRecovery";

                NewButton.Size = DefaultButtonSize;
                NewButton.AutoSize = true;
                NewButton.Text = "Try to Recover " + Instr.Name;
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
            StartPoint.Y += 160;
            foreach (object c in ClarityEngine.InstrumentCollection)
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
            lstInstrumentStatus.Items.Clear();
            if (ClarityEngine!=null && ClarityEngine.InstrumentCollection != null)
            {
                lock (ClarityEngine.InstrumentCollection)
                {
                    foreach (object o in ClarityEngine.InstrumentCollection)
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
        }
        private void DisplayMakeProtocolsForm()
        {
            // Use full reference to avoid conflict with Skype
            System.Windows.Forms.Application.Run(new MakeProtocols(ClarityEngine.InstrumentCollection));
        }
        private void UpdateLoadedProtocols()
        {
            //int SelectedIndex = lstLoadedProtocols.SelectedIndex;
            lstSelectedProtocol.Items.Clear();
            lstLoadedProtocols.Items.Clear();
            lstCurrentProtocol.Items.Clear();
            foreach (object o in ClarityEngine.LoadedProtocols.Protocols)
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
                if (ClarityEngine.CurrentRunningState == RunningStates.Running)
                {
                    TimeToGo.Stop();
                    TimeToGo.Text = "Protocol Running";
                    
                }
                if (ClarityEngine.CurrentRunningState == RunningStates.Idle)
                {
                    TimeToGo.Stop();
                    TimeToGo.Text = "Nothing Running";
                    
                }
                if (ClarityEngine.LoadedProtocols.CurrentProtocolInUse != null)
                {
                    Protocol selectedProtocol = (Protocol)ClarityEngine.LoadedProtocols.CurrentProtocolInUse;
                    int FinishedUPTO = selectedProtocol.NextItemToRun;
                    lblCurrentRunningProtocol.Text = "Current Running Protocol - " + selectedProtocol.ToString();
                    for (int j = 0; j < selectedProtocol.Instructions.Count; j++)
                    {
                        if (j < FinishedUPTO)
                        { lstCurrentProtocol.Items.Add(selectedProtocol.Instructions[j].ToString() + "   --  Finished"); }
                        else { lstCurrentProtocol.Items.Add(selectedProtocol.Instructions[j].ToString() + "   --   To Be Executed"); }
                    }
                    if (FinishedUPTO < lstCurrentProtocol.Items.Count)
                    { lstCurrentProtocol.SelectedIndex = FinishedUPTO; }
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
        private void btnEmailOkay_Click(object sender, EventArgs e)
        {
            try
            {
                ClarityEngine.ReportErrorRecovery();
                MessageBox.Show("Clarity has sent a message to all users");

            }
            catch
            {
                MessageBox.Show("Could not send messages to all");
            }
        }

        
        /// <summary>
        /// This method creates a protocol to run a certain type of 
        /// experiment for the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                else if (!ClarityEngine.Clarity_Alarm.ValidNumbers(textbox_number.Text))
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
                        UpdateLoadedProtocols();
                        ClarityEngine.AddProtocol(NewProt);
                        if (ClarityEngine.CurrentRunningState == RunningStates.Idle)//this timer is running while a protocol is waiting to go
                        {
                            DialogResult DR2 = MessageBox.Show("Would you like to start your protocol immediately?", "Begin Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DR2 == DialogResult.Yes)
                            {
                                ClarityEngine.StartProtocolExecution();
                            }
                        }
                    }
                }
            }
            catch { ShowError("Weird error occurred"); }
        }

        //UI Methods
        /// <summary>
        /// This is a method that creates separate tabs for each instrument method that
        /// is available in all the instruments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewAdvancedControls_Click(object sender, EventArgs e)
        {
            //this method will create a tab for every instrument, and fill that tab with 
            if (CheckPassword())
            {
                System.Drawing.Size DefaultButtonSize = new Size(200, 25);
                int maxDepth = 500;//how far down a button can be placed
                int StartX = 50;
                int StartY = 50;
                foreach (object c in ClarityEngine.InstrumentCollection)
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

                    }
                }
            }
           
        }
        /// <summary>
        /// This method will run the given method when the button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleAdvancedButton(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //This is the sister method to the one above, for a given button that is named
            //Instrument_Method this will run that method, note only methods with no parameters should be called this way
            Button myButton = (Button)sender;
            string[] InsMet = myButton.Name.Split(INSTRUMENT_NAME_DELIMITER);
            string InstrumentName = InsMet[0];
            string MethodName = InsMet[1];
            if (myButton.Name.IndexOf("@") != -1)
            {
                MethodName = MethodName.Split('@')[0];
                if (myButton.Name.Split('@')[1] == "IncubatorServ")
                {
                    object[] Params = new object[1] { Incubator };
                    ClarityEngine.RunInstrumentMethod(InstrumentName, MethodName, Params,true);
                }
            }
            else
            {
                ClarityEngine.RunInstrumentMethod(InstrumentName, MethodName,null,true);
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

                    ClarityEngine.AddProtocol(NewProtocol);
                    UpdateLoadedProtocols();
                    if (ClarityEngine.CurrentRunningState==RunningStates.Idle)//this timer is running while a protocol is waiting to go
                    {
                        DialogResult DR2 = MessageBox.Show("Would you like to start your protocol immediately?", "Begin Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DR2 == DialogResult.Yes)
                        {
                            ClarityEngine.LoadedProtocols.CurrentProtocolInUse = NewProtocol;
                            ClarityEngine.StartProtocolExecution();
                        }
                    }
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not load your protocol.  Error is:\n\n" + thrown.Message, "Unable to Load Protocol", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExecuteProtocols_Click(object sender, EventArgs e)
        {
            ClarityEngine.StartProtocolExecution();
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
            string[] InsMet = myButton.Name.Split(INSTRUMENT_NAME_DELIMITER);
            string InstrumentName = InsMet[0];
            ClarityEngine.TryToRecoverInstrument(InstrumentName);
            this.Cursor = Cursors.Default;
        }
        private void FreeResourceButton_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;
            Button myButton = (Button)sender;
            string[] InsMet = myButton.Name.Split(INSTRUMENT_NAME_DELIMITER);
            string InstrumentName = InsMet[0];
            ClarityEngine.FreeResourcesForInstrument(InstrumentName);
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
                        InstrumentManagerClass.ProtocolRemoveResult result = ClarityEngine.RemoveProtocol(toRemove);
                        //protocol is running
                        if (result == InstrumentManagerClass.ProtocolRemoveResult.WasCurrentlyRunning)
                        {
                            MessageBox.Show("Cannot delete currently executing protocol, please stop the protocol and then delete it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                ClarityEngine.RetryLastFailedInstruction();     
            }
            catch { MessageBox.Show("Last instruction failed"); }
        }

        
        private bool CheckPassword()
        {
            if (txtPassword.Text.ToUpper() == PASSWORD) { return true; }
            else { MessageBox.Show("You do not have permission to make the advanced controls"); return false; }
        }


        private void btnCancelProtocolExecution_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClarityEngine.CurrentRunningState == RunningStates.Running)
                {
                    ClarityEngine.RequestProtocolCancellation();
                    btnCancelProtocolExecution.Enabled = false;
                    StatusLabel.Text = "Status is: Attempting To Cancel Operation";
                }
                else if (ClarityEngine.CurrentRunningState == RunningStates.WaitingForNextExecutionTimePoint)
                {
                    StatusLabel.Text = "Cancelled Operation";
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
        private void saveCurrentProtocolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClarityEngine.LoadedProtocols.Protocols.Count == 0)
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
                        ClarityEngine.OutputRecoveryFile(SFD.FileName);
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
                if (ClarityEngine.LoadedProtocols.Protocols.Count > 0)
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
                        ClarityEngine.InputRecoveryFile(OFD.FileName);
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
                            if (ClarityEngine.CurrentRunningState==RunningStates.Idle || ClarityEngine.CurrentRunningState==RunningStates.WaitingForNextExecutionTimePoint)//this timer is running while a protocol is waiting to go
                            {
                                ClarityEngine.LoadedProtocols.CurrentProtocolInUse = SelectedProtocol;
                                ClarityEngine.StartProtocolExecution();
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
                if (ClarityEngine.LoadedProtocols.Protocols.Count > 0)
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
                    ClarityEngine.InputRecoveryFile(ClarityEngine.RecoveryProtocolFile);
                    UpdateLoadedProtocols();
                }
            }

            catch (Exception thrown)
            {
                MessageBox.Show("Could not load previous protocols.  \n\n" + thrown.Message);
            }
        }
        
        #region Alarm Server Relevant Code
        private void getAlarmStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClarityEngine.UseAlarm)
            {
                Alarm.AlarmState CurStatus = ClarityEngine.Clarity_Alarm.IsAlarmOn();
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
            if (ClarityEngine.UseAlarm)
                ClarityEngine.Clarity_Alarm.TurnOnAlarm("User activated alarm");
            else
                MessageBox.Show("Alarm disabled at startup");
        }
        private void turnOffAlarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClarityEngine.UseAlarm)
                ClarityEngine.Clarity_Alarm.TurnOffAlarm("User Ended Alarm");
            else
                MessageBox.Show("Alarm disabled at startup");
        }
        private void reconnectToAlarmServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClarityEngine.ReinitializeAlarm();
        } 
        #endregion
       
       
        

        public delegate void StringDel(string firstArg);
        public delegate void StringErrorDel(string arg1, Exception arg2);
        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version : 1.0");
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

