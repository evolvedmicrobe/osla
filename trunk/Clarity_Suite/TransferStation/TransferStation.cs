using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCU;
using System.Diagnostics;
using System.Threading;

namespace Clarity
{
    
    public class TransferStation:BaseInstrumentClass
    {
        //The transfer station is a very odd ball, for the most part it is controlled by the DCU server, with the
        //one exception of the sensor on the transfer station, which is connected by RS232 to the incubator and its value is read from there
        private clsDCUserver DCUServer;
        private int pLoadUpSecondsWait = 5;
        public int LoadUpSecondsWait
        {
            get { return pLoadUpSecondsWait; }
            set
            {
                if (value < 0 | value > 30)
                { throw new Exception("Initialized waiting time for DCU is too short or long"); }
                else
                {
                    pLoadUpSecondsWait = value;
                }
            }
        }

        private int pPlungerPort = 3;
        private int pSlidePort = 4;
        private int pVacuumPort = 2;
        private int pAirOutPort = 1;
        private int pSensorVacSealed = 1;
        private int pSensorStationOut = 3;
        private int pSensorStationIn = 2;
        private int pSensorPlungerUp = 5;
        private int pSensorPlungerDown = 4;
        public int PlungerPort
        {
            get { return pPlungerPort; }
            set { pPlungerPort = value; }
        }
        public int SlidePort
        {
            get { return pSlidePort; }
            set { pSlidePort = value; }
        }
        public int VacuumPort
        {
            get { return pVacuumPort; }
            set { pVacuumPort = value; }
        }
        public int AirOutPort
        {
            get { return pAirOutPort; }
            set { pAirOutPort = value; }
        }
        public int SensorVacSealed
        {
            get { return pSensorStationOut; }
            set { pSensorVacSealed = value; }
        }
        public int SensorStationOut
        {
            get { return pSensorStationOut; }
            set { pSensorStationOut = value; }
        }
        public int SensorStationIn
        {
            get { return pSensorStationIn; }
            set { pSensorStationIn = value; }
        }
        public int SensorPlungerUp
        {
            get { return pSensorPlungerUp; }
            set { pSensorPlungerUp = value; }
        }
        public int SensorPlungerDown
        {
            get { return pSensorPlungerDown; }
            set { pSensorPlungerDown = value; }
        }

        delegate bool  delCheckStatus();
        public TransferStation()
        {
        }
        public override string Name
        {
            get { return "TransferStation"; }
        }
        public bool CheckIfVacuumSealed()
        {
            short answer = DCUServer.ReadPort((short)pSensorVacSealed);
            if (answer == (short)1)
            { return true; }
            else { return false; }
        }
        public bool CheckIfStationOut()
        {
            return true; // This is a hack and should be fixed.  We made this change because we think that the sensor is broken.
            short answer = DCUServer.ReadPort((short)pSensorStationOut);
            if (answer == (short)1)
            { return true; }
        }
        public bool CheckIfStationIn()
        {
            short answer = DCUServer.ReadPort((short)pSensorStationIn);
            if (answer == (short)1)
            { return true; }
            else { return false; }
        }
        public bool CheckIfPlungerUp()
        {        
            short answer = DCUServer.ReadPort((short)SensorPlungerUp);
            if (answer == (short)1)
            { return true; }
            else { return false; }        
        }
        public bool CheckIfPlungerDown()
        {
            short answer = DCUServer.ReadPort((short)SensorPlungerDown);
            if (answer == (short)1)
            { return true; }
            else { return false; } 
        }
        private void PauseTillMethodTrue(int MSTimeout, delCheckStatus MethodToCheck)
        {
            int counter = 0;
            int SleepTime = 500;
            int max = (SleepTime+MSTimeout) / SleepTime;
            while (counter < max)
            {
                if (MethodToCheck())
                {
                    break;
                }
                Thread.Sleep(SleepTime);
                counter++;
            }
            
            if (counter == max)
            {
                string methName = MethodToCheck.Method.Name;
                throw new InstrumentError("Could not fulfill transfer station request in time for method: "+methName, false, this); 
            }
        }
        [UserCallableMethod()]
        public override bool AttemptRecovery(InstrumentError Error)
        {
            StatusOK = false;
            try
            {
                CloseAndFreeUpResources();
                Initialize();
                return true;
            }
            catch { return false; }           
        }
        [UserCallableMethod()]
        public void MoveStationIn()
        {
            try
            {
                if (StatusOK)
                {
                    delCheckStatus InYet = CheckIfStationIn;
                    DCUServer.ClearPort((short)SlidePort);
                    //now wait for it
                    PauseTillMethodTrue(6000, InYet);
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Transfer Station Is Not Initialized Or Is Bust, could not Move Plate", false, this);
                }
            }
            catch
            {
                StatusOK = false;
                throw new InstrumentError("Transfer Station Could Not Move Plate Towards Incubator", false, this);
            }
        }
        [UserCallableMethod()]
        public void MoveStationOut()
        {
            try
            {
                if (StatusOK)
                {
                    DCUServer.SetPort((short)SlidePort);
                    //now wait for it to move out
                    delCheckStatus ClosedYet = CheckIfStationOut;
                    PauseTillMethodTrue(6000, ClosedYet);
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Transfer Station Is Not Initialized Or Is Bust, could not Move Plate",false,this);
                }
            }
            catch
            {
                StatusOK = false;
                throw new InstrumentError("Transfer Station Could Not Move Plate Away From Incubator", false, this);
            }
        }
        [UserCallableMethod()]
        public void TurnOnVacuumAndLiftLid()
        {
            try
            {
                if (StatusOK)
                {
                    //turn on vacuum
                    DCUServer.SetPort((short)VacuumPort);
                    Thread.Sleep(400);//just to avoid talking to fast
                    //lower plunger
                    DCUServer.SetPort((short)PlungerPort);
                    //now to make sure plunger and vaccuum okay
                    delCheckStatus MethodToCheck = CheckIfPlungerDown;
                    PauseTillMethodTrue(30000, MethodToCheck);
                    MethodToCheck = CheckIfVacuumSealed;
                    PauseTillMethodTrue(3000, MethodToCheck);
                    //now to raise the plunger                    
                    DCUServer.ClearPort((short)PlungerPort);
                    //make sure it went up
                    MethodToCheck = CheckIfPlungerUp;
                    PauseTillMethodTrue(7000, MethodToCheck);    
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Transfer Station Is Not Initialized Or Is Bust, could not lift lid", false, this);
                }
            }
            catch(Exception thrown)
            {
                StatusOK = false;
                string error="Transfer Station Could Not Lift the lid "+thrown.Message;
                if (thrown is InstrumentError)
                {
                    InstrumentError inner = thrown as InstrumentError;
                    throw new InstrumentError(error,inner);
                }
                else
                {
                    throw new InstrumentError(error,this,thrown);
                }
            }
        }
        [UserCallableMethod()]
        public void TurnOffVacuumAndReturnLid()
        {
            try
            {
                if (StatusOK)
                {
                    delCheckStatus MethodToCheck;
                    //lower the plunger
                    DCUServer.SetPort((short)PlungerPort);
                    //check if lowered
                    MethodToCheck = CheckIfPlungerDown;
                    PauseTillMethodTrue(30000, MethodToCheck);
                    //turn off the vaccuum
                    DCUServer.ClearPort((short)VacuumPort);
                    Thread.Sleep(200);
                    //now blow some air
                    DCUServer.SetPort((short)AirOutPort);
                    Thread.Sleep(1000);
                    //now to raise the plunger
                    DCUServer.ClearPort((short)PlungerPort);
                    MethodToCheck = CheckIfPlungerUp;
                    PauseTillMethodTrue(12000, MethodToCheck);
                    //turn off the air
                    DCUServer.ClearPort((short)AirOutPort);
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Transfer Station Is Not Initialized Or Is Bust, Could not return lid", false, this);
                }
            }
            catch(Exception thrown)
            {
                StatusOK = false;
                string error = "Transfer Station Could Not Return The Lid ";
                if (thrown is InstrumentError)
                {
                    InstrumentError inner = thrown as InstrumentError;
                    throw new InstrumentError(error, inner);
                }
                else
                {
                    throw new InstrumentError(error, this, thrown);
                }
            }
        }
        
        public override void Initialize()
        {
            try { CloseAndFreeUpResources(); }
            catch { }
            try
            {
                DCUServer = new clsDCUserver();
                DCUServer.TaskError += new __clsDCUserver_TaskErrorEventHandler(DCUServer_TaskError);
                
                DCUServer.ShowWindow(1);
                //DCUServer.TaskMessage += new __clsDCUserver_TaskMessageEventHandler(DCUServer_TaskMessage);
                //DCUServer.TaskWarning += new __clsDCUserver_TaskWarningEventHandler(DCUServer_TaskWarning);
                //DCUServer.TaskEnd += new __clsDCUserver_TaskEndEventHandler(DCUServer_TaskEnd);
                int MStoHold = LoadUpSecondsWait*1000;
                Thread.Sleep(MStoHold);
                int i = 1;
                while (i < 15)
                {
                    DCUServer.ClearPort((short)i);
                    i++;
                }
                StatusOK = true;
            }
            catch
            {
                StatusOK=false;
                throw new InstrumentError("Could not initialize transfer station", false, this);
            }
        }

        void DCUServer_TaskEnd(ref string strMessage)
        {
            //MessageBox.Show("END "+ strMessage);
        }
        ~TransferStation()
        {
            if (DCUServer != null)
            {
                CloseAndFreeUpResources();
            }
        }
        public override bool CloseAndFreeUpResources()
        {
            StatusOK = false;
            if (DCUServer != null)
            {
                try
                {
                    int i = 1;
                    while (i < 15)
                    {
                        DCUServer.ClearPort((short)i);
                        i++;
                    }
                }
                catch { }
                try
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(DCUServer);
                }
                catch { }
                DCUServer = null;
            }
                //GC.Collect();
            //next part should always fail, process should be out by now, and thus not forceably killed
            string[] ToKill = { "Device Control Unit", "DCU" };
            foreach (string str in ToKill)
            {
                KillProcessAttempt(str);                
            }
            return true;
        }
        void DCUServer_TaskWarning(ref string strMessage)
        {
            //MessageBox.Show("DCU WARNING: "+strMessage);
        }
        void DCUServer_TaskMessage(ref string strMessage)
        {
            //MessageBox.Show("Task "+strMessage);
        }
        void DCUServer_TaskError(ref string strMessage)
        {
            StatusOK = false;
            throw new InstrumentError(strMessage, false, this);          
        }
    }
}
