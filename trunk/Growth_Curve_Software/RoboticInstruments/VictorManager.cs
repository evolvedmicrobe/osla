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
using System.Threading;
using VictorRemoteServer;
using System.Diagnostics;

namespace Growth_Curve_Software
{


    //This class formerly was used to control the victor when it wasn't automated, now that it is, we are changing it to 
    //by creating a new class,this one is outdated and so will be renamed....
        public class VictorManager : BaseInstrumentClass
        {            

            //Variables ported over from remote server
            static VictorForm PlateReader;
            public string LastErrorMessage;
            public delegate void StatusChangeHandler(bool NewStatus);
            public event StatusChangeHandler StatusChange;
            static Thread ThreadToRunForm;
            string ThreadName = "VictorFormThread";
            public bool StatusOKVictor;
           
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
                    try
                    {
                        InitializeVictorLocal();
                        //StatusChange(true);
                    }
                    catch (Exception thrown)
                    {
                        StatusChange(false);
                        //StatusOK = false;
                        throw new InstrumentError("Could not initialize plate reader", this, thrown);
                    }                
            }
            void Victor_StatusChange(bool NewStatus)
            {
                StatusOK = NewStatus;
            }
            [UserCallableMethod()]
            public void ReadPlate2(string PlateName, int ProtocolID)
            {
                try
                {
                    //status ok should have already been checked, but just to be sure
                    if (StatusOK) //&& Victor.StatusOKVictor)
                    {
                        bool value;
                        value = ReadPlateLocal(PlateName, ProtocolID);
                        if (!value)
                        {
                                throw new InstrumentError("Could not read plate: " + this.LastErrorMessage, false, this);
                        }
                    }
                    else
                    {
                        StatusOK = false;
                        throw new InstrumentError("Plate status is not okay", true, this);
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
                return AttemptRecovery();
            }
            public override bool AttemptRecovery()
            {
                //this will attempt to recover the server
                try
                {
                    CloseAndFreeUpResources();
                    Initialize();
                    //InitializeVictorLocal();
                    //StatusOK = true;
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
                {
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
                        if (WaitCount > 150) { StatusChange(false); return; }//took too long, is a problem
                    }
                }
            }
            private void InitializePlateReader()
            {
                //seperate method to be called by a seperate thread, this keeps the form
                //with its own message queue
                Application.Run(PlateReader = new VictorForm());
            }
            [UserCallableMethod()]
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
                            //wait for 10 minutes for a completion
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
                        StatusChange(false);
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
            [UserCallableMethod()]
            public void KillForm()
            {
                StatusChange(false);
                try
                {
                    if (PlateReader != null)
                    {
                        delVoidVoid closeform = PlateReader.Close;
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
            [UserCallableMethod()]
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


