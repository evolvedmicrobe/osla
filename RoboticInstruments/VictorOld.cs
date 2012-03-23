using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;
using System.Collections;
//using PlateReader;
using System.Threading;
using VictorRemoteServer;
using System.Diagnostics;

//This class formerly was used to control the victor when it wasn't automated, now that it is, we are changing it to 
//by creating a new class,this one is outdated and so will be renamed....
namespace Growth_Curve_Software
{
    [Serializable]
    public class VictorManager : BaseInstrumentClass
    {
        VictorRemoteServer.VictorStandAloneToDll Victor;
        
        //Variables ported over from remote server
        TcpClientChannel channel;
        static VictorForm PlateReader;
        public string LastErrorMessage;
        public delegate void StatusChangeHandler(bool NewStatus);
        public event StatusChangeHandler StatusChange;
        static Thread ThreadToRunForm;
        string ThreadName = "VictorFormThread";

        public bool StatusOKVictor;
        private string pIPAddress = "192.168.0.1:8080";
        public string IPAddress
        {
            get
            { return pIPAddress; }
            set { pIPAddress = value; }
        }
        private bool pRunningLocal = false;
        public bool RunningLocal
        {
            get { return pRunningLocal; }
            set { pRunningLocal = value; }
        }
        /// <summary>
        /// An integer that specifies the number of milliseconds to wait before a request times out. 
        /// 0 or -1 indicates an infinite timeout period. The default is Infinite. 
        /// </summary>
        private int pTimeOut = -1;
        public int TimeOut
        {
            get { return pTimeOut; }
            set {
                if (value > -2)
                    pTimeOut = value;
                else
                    throw new ArgumentOutOfRangeException("Timeout values must be greater then -1");
            }
        }
        public VictorManager()
        { StatusChange += ChangeStatusOKVictor; }
        public override string Name
        {
            get
            {
                return "PlateReader";
            }
        }
        public override void Initialize()
        {
            if (!pRunningLocal)
            {
                try
                {
                    System.Collections.IDictionary properties = new Hashtable();
                    properties["timeout"] = pTimeOut;
                    channel = new TcpClientChannel(properties, null);
                    ChannelServices.RegisterChannel(channel, false);
                    Victor = (VictorStandAloneToDll)Activator.GetObject(typeof(VictorStandAloneToDll), "tcp://" + pIPAddress + "/VictorRemote", null);
                    Thread.Sleep(2000);
                    //Victor = new VictorStandAloneToDll();
                    Victor.InitializePlateReaderPublic();
                    //Victor.StatusChange += new VictorStandAloneToDll.StatusChangeHandler(Victor_StatusChange);
                    StatusOK = true;
                }
                catch (Exception thrown)
                {
                    StatusOK = false;
                    throw new InstrumentError("Could not initialize plate reader", this, thrown);
                }
            }
            else
            {
                try
                {
                    InitializeVictorLocal();
                    StatusOK = true;
                }
                catch (Exception thrown)
                {
                    StatusOK = false;
                    throw new InstrumentError("Could not initialize plate reader", this, thrown);
                }

            }
        }
        void Victor_StatusChange(bool NewStatus)
        {
            StatusOK = NewStatus;
        }
        public void ReadPlate(string PlateName)
        {
            try
            {

                //status ok should have already been checked, but just to be sure
                if (StatusOK) //&& Victor.StatusOKVictor)
                {
                    bool value;
                    if (!pRunningLocal)
                    {
                        value = ReadPlateLocal(PlateName);
                    }
                    else
                    {
                        value = Victor.ReadPlate(PlateName);
                    }
                    if (!value)
                    {
                        StatusOK = false;
                        string errorMsg;
                        if (!RunningLocal)
                        {
                            errorMsg = Victor.LastErrorMessage;
                        }
                        else { errorMsg = LastErrorMessage; }
                        throw new InstrumentError(errorMsg, false, this);
                    }
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Plate status is not okay",false,this);
                }
            }
            catch (Exception thrown)
            {
                StatusOK = false;
                string ToReport = thrown.Message;
                if (thrown is InstrumentError)
                {
                    InstrumentError IE = thrown as InstrumentError;
                    ToReport += "\n" + IE.ErrorDescription;
                }
                throw new InstrumentError("Could not read plate \n" + ToReport+"\n", this,thrown);
            }
        }
        public void ReadPlate2(string PlateName, int ProtocolID)
        {
            try
            {
                //status ok should have already been checked, but just to be sure
                if (StatusOK) //&& Victor.StatusOKVictor)
                {
                    bool value;
                    if (!pRunningLocal)
                    {
                        value = Victor.ReadPlate(PlateName, ProtocolID);
                        if (!value)
                        {
                            StatusOK = false;
                            throw new InstrumentError("Could not read plate: " + Victor.LastErrorMessage, false, this);
                        }
                    }
                    else 
                    { 
                        value = ReadPlateLocal(PlateName, ProtocolID);
                        if (!value)
                        {
                            throw new InstrumentError("Could not read plate: " + this.LastErrorMessage, false, this);
                        }
                    }
                  
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Plate status is not okay",true,this);
                }
            }
            catch (Exception thrown)
            {
                StatusOK = false;
                if (thrown is InstrumentError)
                {
                    throw thrown;
                }
                else
                {
                    throw new InstrumentError("Could not read plate \n" + thrown.Message, false, this);
                }
            }
        }
        public override bool AttemptRecovery(InstrumentError Error)
        {
            StatusOK = false;
            try
            {
                CloseAndFreeUpResources();
                Initialize();
                return true;
            }
            catch
            {
                StatusOK = false;
                return false;
            }
        }
        public override bool AttemptRecovery()
        {
            //this will attempt to recover the server
            try
            {
                StatusOK = false;
                try { Victor.KillForm(); }
                catch { }
                try { Victor.ForceablyKillServer(); }
                catch { }
                CloseAndFreeUpResources();
                Initialize();
                Victor.InitializePlateReaderPublic();
                StatusOK = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override bool CloseAndFreeUpResources()
        {
            StatusOK = false;
            if (!pRunningLocal)
            {
                try
                {
                    Victor.KillForm();
                }
                catch { }
                try
                {
                    ChannelServices.UnregisterChannel(channel);

                    //Victor = null;
                    //channel = null;
                }
                catch
                {
                    return false;
                }
                GC.Collect();
            }
            else {
                try { KillForm(); }
                catch { }
                try { ForceablyKillServer(); }
                catch { }
            }
            return true;
        }

        //Method stolen from older form
        #region LocalRunningVictorServer
        private void InitializeVictorLocal()
        {
            if (!StatusOKVictor && PlateReader == null)
            {
                ThreadToRunForm = new Thread(InitializePlateReader);
                ThreadToRunForm.SetApartmentState(ApartmentState.STA);//must be STA for activex control, otherwise remoting will come in on an MTA thread
                ThreadToRunForm.Name = ThreadName;
                ThreadToRunForm.IsBackground = true;
                ThreadToRunForm.Start();
                Thread.Sleep(1000);//give it time to initalize variables
                int WaitCount = 0;
                while (true)
                {
                    try
                    {
                        if (PlateReader.isInitialized)
                        {
                            StatusChange(true);
                            return;
                        }
                    }
                    catch { }
                    delVoid_Void stayuptodate = Application.DoEvents;
                    try { PlateReader.Invoke(stayuptodate); }
                    catch { }

                    Thread.Sleep(600);
                    WaitCount++;
                    if (WaitCount > 100) { StatusChange(false); return; }//took too long, is a problem
                }
            }
        }
        private void InitializePlateReader()
        {
            //seperate method to be called by a seperate thread, this keeps the form
            //with its own message queue
            Application.Run(PlateReader = new VictorForm());
        }
        public bool ReadPlateLocal(string DirectoryName, int ProtocolID)
        {
            //This is a cut and paste of exactly what was below, with one change to alter the protocol, 
            //(namely it invokes a different method, and sets one more parameter)
            //If this class changes anymore, it should be rewritten into one protocol, 

            //returns true if method successful, false otherwise
            //first to clear last error message
            LastErrorMessage = "Not yet set";
            try
            {
                if (StatusOKVictor)
                {
                    PlateReader.NextPlateName = DirectoryName;
                    PlateReader.NextProtocolIdentifier = ProtocolID;
                    PlateReader.BeginInvoke(PlateReader.del_RunParameterizedProtocol);
                    //now to wait until this object is pulsed by the other program indicating
                    //that it is done 
                    bool Success = false;
                    lock (PlateReader.MonitorPulseObject)
                    {
                        //wait for two minutes for a completion
                        Success = Monitor.Wait(PlateReader.MonitorPulseObject, 60 * 1000 * 10);
                    }
                    if (Success)//no timeout
                    {
                        if (PlateReader.LastCommandResult.HowFinished == FinishedHow.Ok)
                        {
                            return true;//success
                        }
                        else
                        {
                            LastErrorMessage = PlateReader.LastCommandResult.Error;
                            StatusChange(false);
                            return false;
                        }
                    }
                    else//timeout
                    {
                        LastErrorMessage = "Timed out waiting for response from Victor";
                        StatusChange(false);
                        return false;
                    }
                }
                else
                {
                    LastErrorMessage = "Victor Status is not okay";
                    return false;
                }
            }
            catch
            {
                LastErrorMessage = "Unhandled Exception";
                return false;
            }

        }
        public bool ReadPlateLocal(string PlateName)
        {
            //returns true if method successful, false otherwise
            //first to clear last error message
            LastErrorMessage = "Not yet set";
            try
            {
                if (StatusOKVictor)
                {
                    PlateReader.NextPlateName = PlateName;
                    PlateReader.BeginInvoke(PlateReader.del_RunProtocol);
                    //now to wait until this object is pulsed by the other program indicating
                    //that it is done 
                    bool Success = false;
                    lock (PlateReader.MonitorPulseObject)
                    {
                        //wait for two minutes for a completion
                        Success = Monitor.Wait(PlateReader.MonitorPulseObject, 60 * 1000 * 2);
                    }
                    if (Success)//no timeout
                    {
                        if (PlateReader.LastCommandResult.HowFinished == FinishedHow.Ok)
                        {
                            return true;//success
                        }
                        else
                        {
                            LastErrorMessage = PlateReader.LastCommandResult.Error;
                            StatusChange(false);
                            return false;
                        }
                    }
                    else//timeout
                    {
                        LastErrorMessage = "Timed out waiting for response from Victor";
                        StatusChange(false);
                        return false;
                    }
                }
                else
                {
                    LastErrorMessage = "Victor Status is not okay";
                    return false;
                }
            }
            catch
            {
                LastErrorMessage = "Unhandled Exception";
                return false;
            }
        }
        public void KillForm()
        {
            StatusChange(false);
            try
            {
                if (PlateReader != null)
                {
                    VoidVoid closeform = PlateReader.Close;
                    PlateReader.Invoke(closeform);
                    int timeLoop = 0;
                    if (ThreadToRunForm != null && ThreadToRunForm.IsAlive && timeLoop < 100)
                    {
                        timeLoop++;
                        Thread.Sleep(1000);
                    }
                    PlateReader = null;
                    GC.Collect();
                }
                if (ThreadToRunForm.IsAlive)
                {
                    ThreadToRunForm.Abort();
                    ThreadToRunForm = null;
                    PlateReader = null;
                    GC.Collect();
                }
            }
            catch { PlateReader = null; GC.Collect(); }
        }
        public void ForceablyKillServer()
        {
            //This method is designed to forceably kill the mlrserver in preparation for a 
            //restart, it should be used sparingly if at all;
            try
            {
                string ToKill = "MlrServ";
                string output = String.Empty;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                ProcessStartInfo myStartInfo = new ProcessStartInfo();
                myStartInfo.RedirectStandardInput = false;
                myStartInfo.UseShellExecute = false;
                myStartInfo.RedirectStandardOutput = false;
                myStartInfo.Arguments = "/IM " + ToKill + ".exe";
                myStartInfo.CreateNoWindow = true;
                myStartInfo.FileName = @"C:\WINDOWS\SYSTEM32\TASKKILL.EXE";
                proc.StartInfo = myStartInfo;
                proc.Start();
                output = proc.StandardOutput.ReadToEnd();
            }
            catch { }

        }
        private void ChangeStatusOKVictor(bool val)
        {
            StatusOK = val;
            StatusOKVictor = val;
        }
        #endregion
    }
}

