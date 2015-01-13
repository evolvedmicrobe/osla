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
    /// The form has several tabs, the first of which is the main one which shows the currently loaded protocols,
    /// The state of the execution process, which protocols are running etc.
    /// 
    /// The second tab is a recovery tab that allows users to recover instruments that have broken.  Every instrument
    /// not derived from the VirtualInstrument class gets a recovery button
    /// 
    /// The third tab demonstrates controlling an instrument directly from the GUI.  In this case it is an 
    /// incubator, whose shaking speed can be changed, etc.  
    /// 
    /// This form generally shows the engine of Clarity, it is designed to be a front end for the engine,
    /// which is implemented as an InstrumentManagerClass.  Most of the execution of protocols takes place on 
    /// a separate thread behind this class.  
    ///
    /// Note that the Clarity Suite is slightly agnostic when it comes to the division of labor amongst threads.  
    /// Conceptually and in code, the engine that runs the protocols (ClarityEngine, an instance of the 
    /// InstrumentManagerClass) is distinct.  Most of it's work also takes place on a background worker thread.
    /// 
    /// However, it does not maintain a separate message queue and does not exist without this controlling GUI.
    /// The reason for this is both so that the code is easier to integrate, though this can be easily divided in the
    /// future if need be.  
    /// 
    /// The code below is commented, read through to learn how to make a Clarity GUI!
    /// 
    /// 
    /// </summary>
    public partial class ClarityForm : Form    {

        /// <summary>
        /// This is the backend engine that runs the protocols and manages the instruments
        /// </summary>
        private InstrumentManagerClass ClarityEngine;
        //These is an instruments we want to have the GUI gain access to
        //Demonstrated on the Incubator tab.
        private IncubatorServ Incubator;
        /// <summary>
        /// For certain controls we might only want some users to have access, this is the password setting.
        /// </summary>
        public const string PASSWORD = "PASSWORD";
        //This region should not part be a part of the final release, can be ignored.
        #region ToRemove
        private string pErrorEmails = "ndelaney@fas.harvard.edu;3039210411@vtext.com";
        public string NSFErrorEmails
        {
            get { return pErrorEmails; }
            set { pErrorEmails = value; }
        }
        int[] ExcludedIncubatorPositions = { 9, 19, 27, 38 };
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
                    //NSFExperiment tmp = new NSFExperiment();
                    foreach (int plateslot in lstNSFPlates.SelectedItems)
                    {
                        Protocol NewProt = new Protocol();
                        NewProt.ErrorEmailAddress = "";
                        string baseName = "NSF-" + txtNSFName.Text + "-" + txtNSFTransferNumber.Text.ToString();
                        NewProt.ProtocolName = baseName + "_" + plateslot.ToString();
                        StaticProtocolItem SP = new StaticProtocolItem();
                        SP.MethodName = "CreateProtocol";
                        //SP.InstrumentName = tmp.Name;
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
        //A splash screen to show while the instruments load
        WelcomeForm WF;
        //Basic constructor, makes the GUI+
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

            //First load up any XML Settings for the GUI, any fields in this class can be 
            //customized here for local deployment.
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
                ShowErrorSameThread("Could not load settings for interface from XML.\nError is:" + thrown.Message);
                this.Close();
                Application.Exit();
            }
             
            //Now Load up the instruments
            bool InstrumentsLoadedSuccessfully = LoadUpClarityEngine();
            //Give the Splash screen time to load, since it is being created on a separate thread
            //it could not be ready before we set the WF.StayAlive value to false, which signals the form to close
            if (WF == null)
            {
                Thread.Sleep(1000);
            }
            WF.StayAlive = false;

            if (ClarityEngine != null)
            {
                UpdateInstrumentStatus();
                CreateRecoveryPanel();
            }
            try
            {
                //Load web pages with instructions into windows on the tabs
                //to orient the users
                string CurDirec = System.Environment.CurrentDirectory;
                Uri recovAdd = new Uri(CurDirec + @"\AttemptRecoveryDocument.htm", UriKind.Absolute);
                Uri growthinst = new Uri(CurDirec + @"\GrowthRateInstructionsl.htm", UriKind.Absolute);
                wBrowRecovInstructions.Url = recovAdd;
                wBrowGrowthRate.Url = growthinst;
            }
            catch { }
            //Finally some code related to the tab to control incubators, this loads it with slots
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
            //Throw an error if problem loading
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
                //Any GUI should register for all of these events, note that most of these will have to redirect
                // to this Forms thread, any interface to the engine should subscribe to all of these
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
                //Quite simple, just request it by type!  (note, this does mean we tend to only have one type of instrument)                
                return true;
            }
            catch (Exception thrown)
            {
                //These are thrown by the interfaces to the instruments.
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
        /// Displays an error message to the user on a separate thread
        /// </summary>
        /// <param name="ErrorMessage"></param>
        private void ShowError(string ErrorMessage)
        {
            ErrorMessage = "\nNew Error at " + DateTime.Now.ToString() + " \n" + ErrorMessage;
            AddErrorLogText(ErrorMessage);
            //Not sure why, but I am showing this error box on a separate thread, may change this later, 
            //I worry that Clarity will hang somehow if I am not careful about keeping the main thread free,
            //not sure where that would be yet though...
            ShowMessage sm = MessageBox.Show;
            ThreadStart TS = new ThreadStart(() => { sm(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); });
            Thread t = new Thread(TS);
            t.IsBackground=true;
            t.Start();
            //MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ShowErrorSameThread(string ErrorMessage)
        {
            ErrorMessage = "\nNew Error at " + DateTime.Now.ToString() + " \n" + ErrorMessage;
            AddErrorLogText(ErrorMessage);
            MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public  delegate DialogResult ShowMessage(string s,string s2,MessageBoxButtons b ,MessageBoxIcon i);
        /// <summary>
        /// Do not call asynchronously, shows a dialog and adds the error to a textbox so
        /// must be called from main thread.
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <param name="Error"></param>
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
        #region ClarityEngine Event Handlers
        //Note that each of these methods has one trick to it.
        //Windows forms can't update the UI from any thread but the one controlling it
        //so we redirect if it's a different thread to the controllng thread using 
        // this.invoke if that is the case, as a separate thread might be responsible for the call.
        delegate void IM(InstrumentManager s, EventArgs e2);
        delegate void IME(InstrumentManager s, Exception e);
        delegate void IMT(InstrumentManager s, TimeSpan t);
      
        
        void ClarityEngine_OnProtocolExecutionUpdates(InstrumentManager Source, EventArgs e)
        {
            if (this.InvokeRequired)
            {
               
                IM myDel=ClarityEngine_OnProtocolExecutionUpdates; 
                this.Invoke(myDel,new object[] {Source,e});
            }
            else
            {
                lock (Source.LoadedProtocols)
                {
                    try
                    {
                        UpdateForm();
                        try
                        {
                            string toUse = Source.LoadedProtocols.CurrentProtocolInUse.Instructions[Source.LoadedProtocols.CurrentProtocolInUse.NextItemToRun - 1].ToString();
                            StatusLabel.Text = "Performing operation: " +toUse;
                        }
                        catch { }
                    }
                    catch { }
                }
            }
        }

        void ClarityEngine_OnErrorDuringProtocolExecution(InstrumentManager Source, Exception thrown)
        {
            if (this.InvokeRequired)
            {
                IME myDel= ClarityEngine_OnErrorDuringProtocolExecution;
                this.Invoke(myDel, new object[] { Source, thrown});
            }
            else
            {
                StatusLabel.Text = "Procedure ended with errors";
                UpdateInstrumentStatus();
                
                btnRetryLastInstruction.Enabled = true;
                pnlFailure.Visible = true;
                lblFailure.Text = "The Last Instruction Failed To Run, Please recover the machines and retry or delete the protocol, Do not reattempt a macro instruction";
                if (Source.GetLastFailedInstruction() != null)
                {
                    lblFailureInstructionName.Text = "Would you like to reattempt: " + Source.GetLastFailedInstruction().ToString();
                }
                else
                {
                    btnRetryLastInstruction.Enabled = false;
                    AddErrorLogText("\nWeird command failure, the last instruction is not available");
                    
                }
                btnExecuteProtocols.Enabled = true;
                btnCancelProtocolExecution.Enabled = false;
                TimeToGo.Stop();
                TimeToGo.Text = "Error";
                if (thrown is ProtcolExcecutionError)
                {
                    this.AddErrorLogText(((ProtcolExcecutionError)thrown).MakeErrorReport());
                }
               
                ShowError("Failed To Run Protocol", thrown); 
            }
        }

        void ClarityEngine_OnProtocolSuccessfullyCancelled(InstrumentManager Source, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                IM myDel = ClarityEngine_OnProtocolSuccessfullyCancelled;
                this.Invoke(myDel, new object[] { Source, e });
            }
            else
            {
                btnCancelProtocolExecution.Enabled = false;
                btnExecuteProtocols.Enabled = true;
                StatusLabel.Text = "Cancelled Protocol";
                UpdateForm();
            }
        }

        void ClarityEngine_OnGenericError(InstrumentManager Source, Exception thrown)
        {
            if (this.InvokeRequired)
            {
                IME myDel = ClarityEngine_OnGenericError;
                this.Invoke(myDel, new object[] { Source, thrown });
            }
            else
            {
                btnExecuteProtocols.Enabled = true;
                btnCancelProtocolExecution.Enabled = false;
                ShowError("Unspecified Error", thrown);
                UpdateForm();

                string ErrorMessage = thrown.Message;
                AddErrorLogText(ErrorMessage);
            }
        }

        void ClarityEngine_OnAllProtocolsEnded(InstrumentManager Source, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                IM myDel = ClarityEngine_OnAllProtocolsEnded;
                this.Invoke(myDel, new object[] { Source, e });
            }
            else
            {
                StatusLabel.Text = "Finished Running Protocols";
                UpdateForm();
            }
        }

        void ClarityEngine_OnProtocolPaused(InstrumentManager Source, TimeSpan TS)
        {
            if (this.InvokeRequired)
            {
                IMT myDel = ClarityEngine_OnProtocolPaused;
                this.Invoke(myDel, new object[] { Source, TS });
            }
            else
            {

                StatusLabel.Text = "Waiting Until It Is Time To Run The Next Protocol Instruction";
                TimeToGo.Start((int)TS.TotalMilliseconds);
                UpdateForm();
            }
        }

        void ClarityEngine_OnProtocolStarted(InstrumentManager Source, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                IM myDel = ClarityEngine_OnProtocolStarted;
                this.Invoke(myDel, new object[] { Source, e });
            }
            else
            {
                pnlFailure.Visible = false;
                btnExecuteProtocols.Enabled = false;
                btnCancelProtocolExecution.Enabled = true;
                btnRetryLastInstruction.Enabled = false;
                //probably should try some checks here
                TimeToGo.Stop();
                TimeToGo.Text = "Protocol Running";
            }
            
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
                    txtResponse.Text = Incubator.PerformCommandUnsafe(txtCommand.Text, IncubatorServ.Password);
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
                Incubator.InitializeShaker(Incubator.STARTING_SPEED);
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
        //When we make buttons, name them and give them a generic event handler, 
        //which parses the buttons name to decide what to do based on this delimiter
        public const char INSTRUMENT_NAME_DELIMITER = '_';
        /// <summary>
        /// This creates a panel of buttons that give access to 
        /// a basic "Complete reboot after failure" method for 
        /// each instrument.  All instruments should implement this.
        /// </summary>
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
                NewButton.Click += new EventHandler(FreeResourceButton_Click);
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
        /// <summary>
        /// Updates the instrument list to say who is working 
        /// and who isn't
        /// </summary>
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
        /// <summary>
        /// This makes a GUI that allows users to create protocols, follow the code to
        /// see how!
        /// </summary>
        private void DisplayMakeProtocolsForm()
        {
            // Use full reference to avoid conflict with Skype
            System.Windows.Forms.Application.Run(new MakeProtocols(ClarityEngine.InstrumentCollection));
        }
        /// <summary>
        /// This is a generic method to update all the windows forms controls 
        /// related to which programs are running and 
        /// </summary>
        private void UpdateLoadedProtocols()
        {
            lstSelectedProtocol.Items.Clear();
            lstLoadedProtocols.Items.Clear();
            lstCurrentProtocol.Items.Clear();
            foreach (object o in ClarityEngine.LoadedProtocols.Protocols)
            {
                lstLoadedProtocols.Items.Add(o);
            }
        }
        /// <summary>
        /// This is a generic method that basically updates EVERYTHING
        /// based on the currrent state of the runtime engine.
        /// </summary>
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
        /// <summary>
        /// When a protocol is selected with the listbox, display its instructions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Historical method to add error messages to the textbox
        /// could have been done directly but at the time separate
        /// threads were required
        /// </summary>
        /// <param name="ErrorInfo"></param>
        private void AddErrorLogText(string ErrorInfo)
        {
            txtErrorLog.Text = txtErrorLog.Text+ ErrorInfo;
        }
        /// <summary>
        /// If you solved the problem, let everyone else know!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmailOkay_Click(object sender, EventArgs e)
        {
            try
            {
                ClarityEngine.ReportErrorRecovery();
                MessageBox.Show("Clarity is sending a message to all users");

            }
            catch
            {
                MessageBox.Show("Could not send messages to all");
            }
        }
        /// <summary>
        /// This method creates a protocol to run a certain type of 
        /// experiment for the user, in this case an experiment that moves plates
        /// from an incubator to a plate reader periodically.
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
                else if (ClarityEngine.UseAlarm && ClarityEngine.Clarity_Alarm!=null && ClarityEngine.Clarity_Alarm.Connected && !ClarityEngine.Clarity_Alarm.ValidNumbers(textbox_number.Text))
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
                       
                        ClarityEngine.AddProtocol(NewProt);
                        UpdateLoadedProtocols();
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
                        ProtocolRemoveResult result = ClarityEngine.RemoveProtocol(toRemove);
                        //protocol is running
                        if (result == ProtocolRemoveResult.WasCurrentlyRunning)
                        {
                            MessageBox.Show("Cannot delete currently executing protocol, please stop the protocol and then delete it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        UpdateForm();
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

        /// <summary>
        /// Some commands we want system admin types to be able to use
        /// if the errors for them are not robustly handled, this 
        /// validates that only people with the not-so-cryptic password
        /// perform such commands.
        /// </summary>
        /// <returns></returns>
        private bool CheckPassword()
        {
            if (txtPassword.Text.ToUpper() == PASSWORD) { return true; }
            else { MessageBox.Show("You do not have permission to make the advanced controls"); return false; }
        }
        /// <summary>
        /// Request the engine to stop running protocols.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    ClarityEngine.RequestProtocolCancellation();

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
                            SelectedProtocol.NextExecutionTimePoint = DateTime.Now.Subtract(new TimeSpan(1000000,0,0));
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
       
       
        
        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version : 1.0");
        }
        private void btnDeleteProtocol_Click(object sender, EventArgs e)
        {
            RemoveSelectedProtocol();
        }
        //Ignore these methods, they involve which plate a protocol should read
        //and are specific to one lab or people doing growth curves
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

        private void lstIncubatorSlots_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
      
    }
}

