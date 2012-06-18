using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMlrCtls;
using System.Resources;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Compatibility;
using MlrServ;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
//This form has been superseded by the class library in the hosting app

//THE TRICK TO THIS CODE IS INTERFACES!!!!

//Always use an interface, cast things as an interace using "as" eg
// State = InstrumentServ.GetState() as MlrServ.State;, cast to an interface
//when going FROM AND TO the COM component.

namespace VictorRemoteServer
{
    public delegate void delVoid_Void();
    public enum InstrumentState {Ready,NotReady,Busted};

    public partial class VictorForm : Form, IDisposable
    {
        //This form is designed to run a simple plate read, it assumes that two things will happen when it 
        //attempts this, the plate will unload following a success, or it will throw an error
        private bool _disposed;
        public object MonitorPulseObject = new object();//used to detect timeouts between the two programs
        public InstrumentState CurrentStatus;
        List<int> CompletedAssayIDs = new List<int>();
        Assay m_Assay;//The assay to be run
        public string NextPlateName;//again used in place of argument passing
        public int NextProtocolIdentifier;// this is the next protocol id, used in place of argument passing, looking at this on 11/3/08 not sure why I made
        //this choice, figure there was some problem earlier
        //that the last operation has an error, when an error is handled by my event, I will
        //change this variable, then before any method returns a value, it will check this one
        public Boolean isInitialized=false;//indicates whether it is intialized
        public delVoid_Void del_RunParameterizedProtocol;
        public CommandResult LastCommandResult;

        string DataDirectory = @"C:\Growth_Curve_Data\";
        //Ax is from the Aximp.exe utility, see msdn documentation (google for it)
        //it is necessary to make the old victor controls manageable        
        //This control has to be of the windows form type, so is ax
        AxMlrCtls.AxVictor Victor;//this is victor.ocx
        //I don't think that applies to this one though, and so it is not
        //Interfaces are a little above me too I might add (not really getting these)
        //apparently this all seems to work better if I stick with the interfaces instead of the classes
        MlrServ.InstrumentServer InstrumentServ; //I believe this is the mlrsrv control

        /// <summary>
        /// Startup, object called from calling thread to indicate a failure to load
        /// </summary>
        /// <param name="FailedToLoad"></param>
        public VictorForm(BooleanReferenceType FailedToLoad)
        {
            lock (this)
            {
                try
                {
                    isInitialized = false;
                    //MessageBox.Show(Thread.CurrentThread.Name);
                    InitializeComponent();
                    del_RunParameterizedProtocol = RunDifferentProtocol;
                    StartVictor();
                    isInitialized = true;
                    FailedToLoad.FailedToLoad = false;
                    FailedToLoad.ErrorMessage = null;
                    FailedToLoad.thrown = null;
                }
                catch (Exception thrown)
                {
                    FailedToLoad.FailedToLoad = true;
                    FailedToLoad.ErrorMessage = thrown.Message;
                    FailedToLoad.thrown = thrown;
                    this.txtErrors.Text += thrown.Message;
                    this.CurrentStatus = InstrumentState.Busted;
                    isInitialized = false;
                    Application.ExitThread();                    
                }
            }
        }
        public void StartVictor()
        {
            //EVERYTHING HERE IS CRITICAL!!!!!!!
            //this code is very difficult to work with because the Victor server is so damn out of date
            //there is an automatic wrapper class the VS generated, and these AxInterops should be used
            //YOU MUST ADD THE VICTOR TO THE CONTROL OF THE THING!!! THIS IS VITAL
            System.Resources.ResourceManager resources = new ResourceManager(typeof(VictorForm));
            this.Victor = new AxMlrCtls.AxVictor();
            ((System.ComponentModel.ISupportInitialize)Victor).BeginInit();
            Victor.OcxState = (System.Windows.Forms.AxHost.State)resources.GetObject("Victor.OcxState");
            this.Victor.Location = new System.Drawing.Point(388, 172);
            this.Victor.Name = "Victor";
            this.Victor.Size=new System.Drawing.Size(18,17);
            Victor.PerformLayout();
            Victor.OcxState = (System.Windows.Forms.AxHost.State)resources.GetObject("Victor.OcxState");
            this.Controls.Add(Victor);
            //Victor.CreateControl();-this appears to work just as well
            ((System.ComponentModel.ISupportInitialize)this.Victor).EndInit();
            this.PerformLayout();
            Victor.StartServer(0);
            Victor.ResultEventsEnabled=true;
            Victor.PlateEventsEnabled=true;
            Victor.StateEventsEnabled=true;
            Victor.AssayEventsEnabled=true;
            Victor.StateEventsEnabled=true;

            Victor.ServerState +=new EventHandler(Victor_ServerState);
            Victor.ServerAssayEnd+=new _DVictorEvents_ServerAssayEndEventHandler(Victor_ServerAssayEnd);
            Victor.ServerPlateUnload += new _DVictorEvents_ServerPlateUnloadEventHandler(Victor_ServerPlateUnload);
                    }
        private void Form1_Load(object sender, EventArgs e)
        {
            //StartVictor();
            if (CurrentStatus != InstrumentState.Busted)
            {
                InstrumentServ = new InstrumentServerClass();
                InstrumentServ.OnError += new IInstrumentEvents_OnErrorEventHandler(InstrumentServ_OnError);
            }
        }
        //UI Methods 
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                txtProtInfo.Text = ProtocolTree.SelectedItem.ProtocolID.ToString();
            }
            catch (Exception thrown)
            { txtErrors.Text = thrown.Message; }
        }
        //Status Updates
        private void UpdateStatus()
        {
            MlrServ.State State;
            if (Victor.IsServerUpAndRunning())
            {
                //way more then three options here
                State = InstrumentServ.GetState() as MlrServ.State;
                if (State.IsIdle())
                {
                    CurrentStatus = InstrumentState.Ready;
                    lblStatus.Text = "Status: Machine Ready";
                    lblStatus.ForeColor = Color.Black;
                }
                else if (State.IsConnected() && State.IsWaiting())
                {
                    //check this
                    CurrentStatus = InstrumentState.NotReady;
                    lblStatus.Text = "Status: Not ready yet, waiting for the next plate";
                    lblStatus.ForeColor = Color.Blue;
                }
                else if (State.IsRunning())
                {
                    CurrentStatus = InstrumentState.NotReady;
                    lblStatus.Text = "Status: Running";
                    lblStatus.ForeColor = Color.Blue;
                }
                else if (State.IsUnloading())
                {
                    CurrentStatus = InstrumentState.NotReady;
                    lblStatus.Text = "Status: Unloading";
                    lblStatus.ForeColor = Color.Blue;
                }
                else if (State.IsLoading())
                {
                    CurrentStatus = InstrumentState.NotReady;
                    lblStatus.Text = "Status: Loading";
                    lblStatus.ForeColor = Color.Blue;
                }
                else if (State.IsError())
                {
                    CurrentStatus = InstrumentState.Busted;
                    lblStatus.Text = "Status: Error";
                    lblStatus.ForeColor = Color.Red;
                    txtErrors.Text += "ERROR REPORTED AT: " + System.DateTime.Now.ToString() + "\n";
                    txtErrors.Text += InstrumentServ.GetLastErrorText() + "\n";
                }
                else
                {
                    CurrentStatus = InstrumentState.Busted;
                    lblStatus.Text = "Status: Busted";
                    lblStatus.ForeColor = Color.Red;
                }
            }
        }
        //Victor/Form Events
        void Victor_ServerPlateUnload(object sender, _DVictorEvents_ServerPlateUnloadEvent e)
        {
           //I hope the error event happens before this one
           //If this happens I am assuming the assay finished successfully
            if(!CompletedAssayIDs.Contains(e.assayID))
            {
                EventFinishedSuccessfully();
            }
        }
        void Victor_ServerAssayEnd(object sender, _DVictorEvents_ServerAssayEndEvent e)
        {
            //EventFinishedSuccessfully();
        }        
        void Victor_ServerState(object o,EventArgs e)
        {
            UpdateStatus();
        }
        void InstrumentServ_OnError(string ErrorMessage, int ErrorCode, int Choices, ref ErrorAction Action)
        {
            //This is vital and I believe means that the dang thing will no longer throw
            //a dialog box up, thus blocking the code (which of course would be unknown to the other programs)     
            Action = ErrorAction.Abort;
            LastCommandResult = new CommandResult(ErrorMessage);
            PulseObject();
            //by setting the action I effectively make a decision, I expect this program to usually work
            //and so am always going to abort, in the future I could try the default options, or ok, or whatever
        }      
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.InternalDispose();
        }      

        //Method run and monitoring
        private void PulseObject()
        {
            lock (MonitorPulseObject) Monitor.Pulse(MonitorPulseObject);
        }
        private void EventFinishedSuccessfully()
        {
            try
            {
                if (m_Assay != null && LastCommandResult != null && !LastCommandResult.ErrorSetAlready)
                {
                    string Filename = m_Assay.GetAssayID().ToString();
                    MovePlateDataToNewDirectory(NextPlateName, Filename);
                    LastCommandResult = new CommandResult();
                    if (CompletedAssayIDs.Count > 5) { CompletedAssayIDs.Clear(); }
                    CompletedAssayIDs.Add(m_Assay.GetAssayID());
                }
            }
            catch { txtErrors.Text += "\n\nProblem Moving File and Ending Assay\n"; }
            finally { PulseObject(); txtErrors.Text += "\nEvent Finish Call at: " + DateTime.Now.ToString(); }
            //
        }
        private void RunProtocol(int ProtocolID, string PlateName)
        {
            counter = 0;
            //int ProtocolID = 2000103;//This is my OD Protocol                
            //returns true if successful, false otherwise
            try
            {
                //First have to make sure the machine is working
                UpdateStatus();
                if (CurrentStatus != InstrumentState.Ready)
                {//if the instrument isn't ready, try for 10 seconds to make it ready, then abort
                    for (int i = 0; i < 20; i++)
                    {
                        UpdateStatus();
                        if (CurrentStatus == InstrumentState.Ready)
                        {
                            break;
                        }
                        Thread.Sleep(500);
                    }
                    //if this fails, return false
                    if (CurrentStatus != InstrumentState.Ready)
                    {
                        LastCommandResult = new CommandResult("Instrument Status Was Never Ready");
                        PulseObject();
                        return; 
                    }
                }
                //Onwards with the protocol
                MlrServ.AssayRunDefinitionClass AssayRunDef = new AssayRunDefinitionClass();
                AssayRunDef.ProtocolID = ProtocolID;
                AssayRunDef.LoadFirstPlate = true;
                AssayRunDef.Notes = NextPlateName;

                //Create new m_Assay (and run if in demo mode)
                m_Assay = InstrumentServ.NewAssay(AssayRunDef as AssayRunDefinition) as Assay;
                 //Check that the object was created
                if (m_Assay == null)
                {
                    txtErrors.Text += "\nUnable to start new m_Assay (" + InstrumentServ.GetLastErrorText() + ")";
                    LastCommandResult = new CommandResult("Was unable to start assay");
                    PulseObject();
                    return;
                }              
            }
            catch
            {
                txtErrors.Text += "\nUnexplained Error During Reading"+DateTime.Now.ToString()+"\n";
                LastCommandResult = new CommandResult("Unexplained Error");
                PulseObject();
                CurrentStatus = InstrumentState.Busted;
                return;
            }
            return;
        }
        public void RunDifferentProtocol()
        {
            //This method is used to run a non-standard protocol, (anyone that does not use OD600, very similar to the one above)
            try
            {
                LastCommandResult = new CommandResult("Object Never Was Initialized Except At The Start Of Method, Big Problem");
                LastCommandResult.ErrorSetAlready = false;
                RunProtocol(NextProtocolIdentifier, NextPlateName);
            }
            catch
            {
            }
        }    
        private void ValidateName(string PossibleName)
        {
            string regexString = "[" + Regex.Escape(Path.GetInvalidPathChars().ToString()) + Regex.Escape(Path.GetInvalidFileNameChars().ToString()) + "\\" + "]";
            Regex containsABadCharacter = new Regex(regexString);
            if (containsABadCharacter.IsMatch(PossibleName))
            {
                throw new IOException("Filename is not valid " + PossibleName);
            }
        }
        int counter;
        private void MovePlateDataToNewDirectory(string PlateName,string FileName)
        {
            string TimeOutError = "File Did Not Appear Within The Required Time";
            string DirectoryToPlace = DataDirectory + PlateName;
            try
            {
                if (!Directory.Exists(DirectoryToPlace))
                {
                    Directory.CreateDirectory(DirectoryToPlace);
                }
                //Now to grab the file
                string OldFileName = DataDirectory + FileName + ".txt";
                string OldFileName2 = DataDirectory + FileName + ".xls";
                string NewFileName = DataDirectory + PlateName + "\\" + PlateName + "_" + FileName + ".txt";
                string NewFileName2 = DataDirectory + PlateName + "\\" + PlateName + "_" + FileName + ".xls";   
                int MaxCounter = 14;
                if (counter == MaxCounter)
                {
                    LastCommandResult = new CommandResult(TimeOutError);
                    LastCommandResult.ErrorSetAlready = true;
                    
                    txtErrors.Text += "Failed to Move DataFile: " + FileName + " for " + PlateName+"\n"+TimeOutError;
                    throw new Exception(TimeOutError);
                }
                while (!File.Exists(OldFileName) && !File.Exists(OldFileName2) && counter < MaxCounter)//the program often fires the event finished event before it finishes, so this attempts this several times
                {
                    Thread.Sleep(3000);
                    counter++;
                }
              //keep it in xls if needed
                if (!File.Exists(OldFileName))
                {
                    OldFileName = OldFileName2;
                    NewFileName = NewFileName2;
                }
                ValidateName(NewFileName);
                File.Move(OldFileName, NewFileName);
                counter = 0;
            }
            catch(Exception thrown)
            {
                if (thrown.Message == TimeOutError || thrown.Message.Contains("not valid"))
                    throw thrown;
                else
                {
                    Thread.Sleep(15000);//wait 10 seconds in case the victor has a file lock on it
                    MovePlateDataToNewDirectory(PlateName, FileName);
                }
            }
        }
        //Data moving-Old methods below, no longer used
        private void OldMovePlateDataToNewDirectory(string PlateName, string FileName)
        {
            string DirectoryToPlace = DataDirectory + PlateName;
            try
            {
                if (!Directory.Exists(DirectoryToPlace))
                {
                    Directory.CreateDirectory(DirectoryToPlace);
                }
                //Now to grab the file
                string OldFileName = DataDirectory + FileName + ".txt";
                string NewFileName = DataDirectory + PlateName + "\\" + PlateName + "_" + FileName + ".txt";
                if (!File.Exists(OldFileName))//the program often fires the event finished event before it finishes, so this attempts this several times
                {
                    OldMovePlateDataToNewDirectory(PlateName, FileName, 0);
                    return;
                }
                

                File.Move(OldFileName, NewFileName);
            }
            catch
            {
                Thread.Sleep(10000);//wait 10 seconds in case the victor has a file lock on it
                OldMovePlateDataToNewDirectory(PlateName, FileName,0);
            }
        }
        private void OldMovePlateDataToNewDirectory(string PlateName, string FileName, int RecursionNumber)
        {
            //overloaded method, doesn't try again if it fails
            string OldFileName = DataDirectory + FileName + ".txt";
            string NewFileName = DataDirectory + PlateName + "\\" + PlateName + "_" + FileName + ".txt";
               
            if (RecursionNumber > 6) 
            {
                txtErrors.Text += "\nCOULD NOT FIND EXPECTED DATA FILE: " + OldFileName;
                OldMovePlateDataToNewDirectory(PlateName, FileName,0);return; 
            }
            string DirectoryToPlace = DataDirectory + PlateName;
            try
            {
                if (!Directory.Exists(DirectoryToPlace))
                {
                    Directory.CreateDirectory(DirectoryToPlace);
                }
                //Now to grab the file
                if (!File.Exists(OldFileName))
                {//simply report error if file does not exists
                    Thread.Sleep(3000);
                    RecursionNumber++;
                    OldMovePlateDataToNewDirectory(PlateName, FileName, RecursionNumber);
                    return;
                }
                File.Move(OldFileName, NewFileName);
            }
            catch
            {
                txtErrors.Text += "Failed to Move DataFile: " + FileName + " for " + PlateName;
            }
        }
        private void btnRefreshProtocols_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DT = new DataTable();
                DataColumn DC = new DataColumn("Protocol Name", "".GetType());
                DT.Columns.Add(DC);
                DC = new DataColumn("Protocol ID", "".GetType());
                DT.Columns.Add(DC);

                foreach (MlrCtls.Protocol P in ProtocolTree.Protocols)
                {
                    DataRow DR = DT.NewRow();
                    DR[0] = P.ProtocolName;
                    DR[1] = P.ProtocolID;
                    DT.Rows.Add(DR);
                    txtProtInfo.Text = P.ProtocolID.ToString();
                }
                dataGridView1.DataSource = DT;
            }
            catch
            {
                MessageBox.Show("Could not load all of the protocols, the underlying software is not responding to the request, not sure what the error is, try again in a bit, or tell Nigel to call perkin elmer");
            }
        }
        private void btnDisplayProt_Click(object sender, EventArgs e)
        {

        }
        private void tabBackup_Click(object sender, EventArgs e)
        {

        }
        ~VictorForm()
        {
            this.InternalDispose();
            //is base finalizer called to?
            
        }
        #region IDisposable Members

        public void InternalDispose()
        {
            InternalDispose(true);
            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            //GC.SuppressFinalize(this);
        }

        protected void InternalDispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Victor != null)
                    {
                        try
                        {
                            Victor.StopServer();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(InstrumentServ);
                            Thread.Sleep(1000);//give it time to close
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(Victor);
                        }
                        catch { }
                    }
                    if (ProtocolTree != null)
                    {
                        try
                        {
                            Marshal.ReleaseComObject(ProtocolTree);
                        }
                        catch { }

                    }
                    if (m_Assay != null)
                    {
                        try
                        {
                            Marshal.ReleaseComObject(m_Assay);
                        }
                        catch { }
                    }
                    Victor = null;
                    ProtocolTree = null;
                    m_Assay = null;
                    InstrumentServ = null;
                    GC.Collect();//call disposers
                    GC.Collect();//finalize
                    _disposed = true;
                }


                
            }
        }

        #endregion
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.RunProtocol(2000103, "testPlate");
        }
    }
    public enum FinishedHow { Ok, Error }
    public class CommandResult
    {
       
        public FinishedHow HowFinished;
        public string Error;
        public bool ErrorSetAlready;//check to see if some other event has triggered this
        public CommandResult() 
        { 
            HowFinished = FinishedHow.Ok;
            ErrorSetAlready = true;
        }
        public CommandResult(string ErroMessage)
        {
            Error = ErroMessage;
            HowFinished = FinishedHow.Error;
            ErrorSetAlready = true;

        }
    }
    public class BooleanReferenceType
            {
                public bool FailedToLoad=false;
                public string ErrorMessage;
                public Exception thrown;
            }
}
