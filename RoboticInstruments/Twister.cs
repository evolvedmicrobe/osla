using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyRobot_ICP;
using ZyRobotAdapter;
//using System.Windows.Forms;
//using ZyRobotAdapter;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Growth_Curve_Software
{
    //As a broad note, the most important method in the twister robot class seems to be 
    //the execute method, this method takes the following.
    //Format is module name (Robot is always provided), MacroName, (optional) Arguments ... as | delimited string

    //Another useful thing, read the file Integration guide in the zyrobot file, this will give you all the information
    //on how to use the .xpd and .pos files 

    public class Twister : BaseInstrumentClass
    {
        //not sure which mix of these are needed to run the robot, going to load them all for now, 
        //cls server seems most important, not sure what is up with robotserver
        //ZyLicensing.clsLicensingClass LicenseClass;
        private ZyRobot_ICP.clsServer TwisterClsServer;
        private ZyRobot_ICP.RobotServer RobotServ;

        private float GripTolerance=4;//This is how it is set in the twister macro
        
       //private ZyRobotAdapter.CServerClass RoboAdapterCserv;        
        
        //first to get the positions where the thing needs to go for the incubator,
        //it should go first to safe, then over, then grab plate and back
        TwisterPosition IncubatorSafe;//the location where it is safe to move the twister
        TwisterPosition IncubatorOverPlate;//the position over the DCU transfer out
        TwisterPosition IncubatorPlate;
        TwisterPosition ReaderSafe;
        TwisterPosition ReaderOverPlate;
        TwisterPosition ReaderPlate;
        TwisterPosition GarbageLocation;
        TwisterPosition GlobalSafe;
        TwisterPosition ScicloneSafe;
        TwisterPosition RestSafe;
        TwisterPosition RestLocation;
        IncubatorServ Incubator;

        //public const string IncubatorPositionsFile = @"C:\Clarity\TwistPos.nfd";
        private string pIncubatorPostionsFile = @"C:\Clarity\TwistPos.nfd";
        public string IncubatorPositionsFile
        {
            get { return pIncubatorPostionsFile; }
            set { pIncubatorPostionsFile = value; }
        }

        private string pInitiationFile = "Twister.ini";
        public string InitiationFile
        {
            get { return pInitiationFile; }
            set { pInitiationFile = value; }
        }
        private int pMinutesRequiredForVerticalMotorShutdown= 5;
        public int MinutesRequiredForVerticalMotorShutdown
        {
            get { return pMinutesRequiredForVerticalMotorShutdown; }
            set { if(value>0) pMinutesRequiredForVerticalMotorShutdown = value; }
        }
        TwisterPosition[,] SciCloneSpots;
        //This Array will contain all of the Various Postions for the rack, each "row" will have the safety position and the 
        //pick position
        TwisterPosition[,] RackPositions;
       
        [Serializable]
        public class TwisterPosition
        {
            public TwisterPosition(string name,string type, float vertical, float reach, float rotary, float wrist,short PercentSpeedToMoveAt)
            {
                Name = name;
                Vertical = vertical;
                Type = type;
                Reach = reach;
                Rotary = rotary;
                Wrist = wrist;
                Track=0;
                SpeedToMoveAt = PercentSpeedToMoveAt;
            }
            public string Name;
            public float Vertical;
            public string Type;
            public float Reach;
            public float Rotary;
            public float Wrist;
            public float Track;
            public short SpeedToMoveAt;

            public override string ToString()
            {
                return Name;
            }
        }
        bool SimulationMode;
        private void initializeTwisterPositions()
        {
            //First to adjust positions
            Dictionary<string, Twister.TwisterPosition> TwistDict = new Dictionary<string, Twister.TwisterPosition>();
            FileStream f = new FileStream(IncubatorPositionsFile, FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            TwistDict = (Dictionary<string, Twister.TwisterPosition>)b.Deserialize(f);
            f.Close();

            ReaderSafe = TwistDict["Reader Safe"];
            ReaderOverPlate =TwistDict["Reader Clearance"];
            ReaderPlate = TwistDict["Reader Place"];
            
            //now for the incubator, actually the process incubator in Clara
            IncubatorSafe = TwistDict["Incubator Safe"];
            IncubatorOverPlate = TwistDict["Incubator Clearance"];
            IncubatorPlate = TwistDict["Incubator Place"];
            //Now for the Garbage Location
            GarbageLocation = TwistDict["Garbage Safe"];
            GlobalSafe = TwistDict["Global Safe"];
            //now for twister rest spots
            RestSafe = TwistDict["Rest Safe"];
            RestLocation = TwistDict["Rest Position"];
 
            //Now to give all the coordinates of the rack positions
            RackPositions = new Twister.TwisterPosition[6, 2];
            for (int i = 0; i < 6; i++)
            {
                string Key2 = "Rack " + (i+1).ToString(); 
                string Key1 = Key2+ " Clearance";
                RackPositions[i, 0] = TwistDict[Key1];
                RackPositions[i, 1] = TwistDict[Key2];
            }
            //Now for the SciClonePositions
            //Very important!!! This will hold the positions labelled 1-5, with 1 being closest 
            //to the computer.  Position 1 is for the pippette tips, and so will have a special location,
            //while position 4 holdsthe shaker.
            //only position one will have the move absolute rule enforced
            //Note that the first spot is the "safe" position, and the second is the "down" position
            //also note that I am defining one "safety position" for all sciclone moves" 
            ScicloneSafe = TwistDict["Sci Safe"];
            SciCloneSpots = new Twister.TwisterPosition[4, 2];
            for (int i = 0; i < 4; i++)
            {
                string Key2 = "SciClone " + (i + 1).ToString();
                string Key1 = Key2 + " Clearance";
                SciCloneSpots[i, 0] = TwistDict[Key1];
                SciCloneSpots[i, 1] = TwistDict[Key2];
            }
            
        }

        public Twister()
        {
            //the constructor is not doing anything right now
            SimulationMode = false;
            
        }
        public override void Initialize()
        {
            initializeTwisterPositions();
            try
            {                            
                TwisterClsServer = new clsServer();
                string iniFile = TwisterClsServer.ApplicationPath + "\\" + InitiationFile;//"Twister.ini";//hope this is right one
                TwisterClsServer.ConfigureRobot(iniFile, ref SimulationMode);
                TwisterClsServer.Initialize();
                //THE TWISTER SERVE MUST BE INITIALIZED BEFORE IT WILL GIVE A ROBOT REF PROPERLY
                RobotServ = TwisterClsServer.GetRobotRef();//this line must follow the server initialization
                RobotServ.HomeAllAxes();
                //BIG ERROR IF NOT FOLLOWED!!!!
                short windowState = 1;//0 normal, 1 minimized, 2 maximized
                TwisterClsServer.ShowWindow(ref windowState);
                RobotServ.SpeedAsPercentMax = (short)50;                               
                StatusOK = true;                
            }
            catch(Exception thrown)
            {
                 StatusOK = false;
                 throw new InstrumentError("Could not initialize Twister: "+thrown.Message, false, this);    
            }
        }
        public void SetIncubator(IncubatorServ incubator)
        {
            //For methods that require the incubator, this must be set
            Incubator=incubator;
        }
        public void HomeAllAxes()
        {
            try
            {
                RobotServ.HomeAllAxes();
            }
            catch
            {
                StatusOK=false;
                throw new InstrumentError("Unable to Home Twister Axes",false,this);
            }
        }        
        public void MovePlateFromPlateReaderToIncubator()
        {
            //first to move to the incubator an pick up the plate
            try
            {
                
                //go to reader
                MoveToPosition(ReaderSafe);
                MoveToPosition(ReaderOverPlate);
                FindMaterialAndGripIt();
                //MoveToPosition(ReaderPlate);
                //RobotServ.GripMaterial(4);
                //back to safe
                MoveToPosition(ReaderOverPlate);
                MoveToPosition(ReaderSafe);
 
                MoveToPosition(IncubatorSafe);
                MoveToPosition(IncubatorOverPlate);
                FindMaterialAndReleaseGrip();
                
                //return to safety
 
                MoveToPosition(IncubatorOverPlate);
                MoveToPosition(IncubatorSafe);            
            }
            catch(Exception thrown)
            {
                //if it fails, try to move the arm back to a safe position
                RobotServ.Abort();
                RobotServ.SpeedAsPercentMax = (short)5;
                throw new InstrumentError("Could not get plate and move it"+thrown.Message,true,this);
            }          
        }

        public override void RegisterEventsWithProtocolManager(ProtocolEventCaller PEC)
        {
             PEC.onProtocolPause += new ProtocolPauseEventHandler(PM_onProtocolPause);
             base.RegisterEventsWithProtocolManager(PEC);
        }

        void PM_onProtocolPause(TimeSpan TS)
        {
            if (TS.TotalMinutes >= pMinutesRequiredForVerticalMotorShutdown)
            {
                DisableVerticalMotor();
            }
        }

        //Sciclone methods
        public void GetItemInSciclonePosition(int BayNumber)
        {
            //need to convert to zero based system
            //note that for now I am "finding" the material, and not using absolute positions
            try
            {
                //First move to a safe location, otherwise the extended arm can crash into the
                //plate reader and incubator as it moves around
                MoveToPosition(ScicloneSafe);
                //now go to the spot
                MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
                FindMaterialAndGripIt();
                MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
                MoveToPosition(ScicloneSafe);
            }
            catch (Exception thrown)
            {
                RobotServ.Abort();
                throw new InstrumentError("Could not place material", false, this);
            }
        }
        public void PlaceItemInSciclonePosition(int BayNumber)
        {
            //need to convert to zero based system
            //note that for now I am "finding" the material, and not using absolute positions
            try
            {
                //First move to a safe location, otherwise the extended arm can crash into the
                //plate reader and incubator as it moves around
                MoveToPosition(ScicloneSafe);
                //now go to the spot
                MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
                FindMaterialAndReleaseGrip();
                MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
                MoveToPosition(ScicloneSafe);
            }
            catch (Exception thrown)
            {
                RobotServ.Abort();
                throw new InstrumentError("Could not place material", false, this);
            }
        }
        public void MoveTwisterToSafePosition()
        {
            try
            {
                MoveToPosition(GlobalSafe);
            }
            catch (Exception thrown)
            {
                RobotServ.Abort();
                throw new InstrumentError("Could not move to safe haven", false, this);
            }
        }
        public void DropItemInGarbage()
        {
            try
            {
                MoveToPosition(GlobalSafe);
                MoveToPosition(GarbageLocation);
                RobotServ.GripOpen();
                MoveToPosition(GlobalSafe);
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("Could Not Drop Item\n\n" + thrown.Message, false, this);
            }
        }
        public void EnableVerticalMotor()
        {
            try
            {
                bool enabled = true;
                RobotServ.Vertical.set_MotorEnabled(ref enabled);
                RobotServ.Vertical.HomeAxis();
            }
            catch (Exception thrown)
            { 
                throw new InstrumentError("Could not initialize vertical motor", this,thrown);
            }
        }
        public void DisableVerticalMotor()
        {
            try
            {
                MoveToPosition(RestSafe);
                MoveToPosition(RestLocation);
                bool disabled = false;
                RobotServ.Vertical.set_MotorEnabled(ref disabled);
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("Could not disable vertical motor", this, thrown);
            }
        }
        //Private methods
        private void MoveToPosition(TwisterPosition Position)
        {
            MoveToPosition(Position, Position.SpeedToMoveAt);
        }
        private void MoveToPosition(TwisterPosition Position, short MoveSpeed)
        {
            if (!RobotServ.Vertical.get_MotorEnabled())
            {
                EnableVerticalMotor();
            }
            RobotServ.SpeedAsPercentMax = MoveSpeed;
            RobotServ.MoveAbsolute(Position.Rotary, Position.Reach, Position.Vertical, Position.Wrist, Position.Track);
        
        }
        private void FindMaterialAndGripIt()
        {
            RobotServ.FindMaterial((object)14, (object)0, (object)0);//settings from twister macros
            RobotServ.GripMaterial((object)4);
        }
        private void FindMaterialAndReleaseGrip()
        {
            RobotServ.FindMaterial((object)14, (object)0, (object)0);//settings from twister macros
            RobotServ.GripOpen();
        }
        public void MoveToGlobalSafe()
        {
            MoveToPosition(GlobalSafe);
        }
        public void MovePlateFromTransferStationToPlateReader()
        {
            //first to move to the incubator an pick up the plate
            if (!Incubator.CheckIfSomethingOnTransferOutStation())
            {

                throw new InstrumentError("Nothing on plate reader", true, this);
            }
            try
            {
                MoveToPosition(IncubatorSafe);
                MoveToPosition(IncubatorOverPlate);
                FindMaterialAndGripIt();
                MoveToPosition(IncubatorOverPlate);
                MoveToPosition(IncubatorSafe);
            }
            catch(Exception thrown)
            {
                //if it fails, try to move the arm back to a safe position
                //RobotServ.Abort();
                MoveToPosition(IncubatorOverPlate, (short)5);
               // RobotServ.SpeedAsPercentMax = (short)5;
                //RobotServ.MoveAbsolute(IncubatorOverPlate.Rotary, IncubatorOverPlate.Reach, IncubatorOverPlate.Vertical, IncubatorOverPlate.Wrist, IncubatorOverPlate.Track);
                throw new InstrumentError("Could not get material from plate", this,thrown);
            }
            try
            {
                MoveToPosition(ReaderSafe);
                MoveToPosition(ReaderOverPlate);
                //MoveToPosition(ReaderPlate);
                //RobotServ.GripOpen();
                FindMaterialAndReleaseGrip();
                MoveToPosition(ReaderOverPlate);
                MoveToPosition(ReaderSafe);
            }
            catch(Exception thrown)
            {
                //if it fails, try to move the arm back to a safe position
                RobotServ.Abort();
                RobotServ.SpeedAsPercentMax = (short)5;
                throw new InstrumentError("Could not get material from plate",this,thrown);
            }
        }
        public void GrabItemFromRack(int RackNumber)
        {
            try
            {
                TwisterPosition Safe = RackPositions[RackNumber - 1, 0];
                TwisterPosition Poised = RackPositions[RackNumber - 1, 1];

                TwisterPosition[] ToMoveTo = new TwisterPosition[2] { Safe, Poised };
                foreach (TwisterPosition Position in ToMoveTo)
                {
                    MoveToPosition(Position);
                    //RobotServ.SpeedAsPercentMax = Position.SpeedToMoveAt;
                    //RobotServ.MoveAbsolute(Position.Rotary, Position.Reach, Position.Vertical, Position.Wrist, Position.Track);
                }
                //now to get the item
                FindMaterialAndGripIt();

                ToMoveTo = new TwisterPosition[2] { Poised, Safe };
                foreach (TwisterPosition Position in ToMoveTo)
                {
                    MoveToPosition(Position);
                    //RobotServ.SpeedAsPercentMax = Position.SpeedToMoveAt;
                    //RobotServ.MoveAbsolute(Position.Rotary, Position.Reach, Position.Vertical, Position.Wrist, Position.Track);
                }
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("Could not get item\n\n" + thrown.Message, this,thrown);
            }            
        }
        public void GrabItemOnTransferStation()
        {
            //This method assumes that a plate with a lid on it is outside the transfer station
            if (!Incubator.CheckIfSomethingOnTransferOutStation())
            {
                throw new InstrumentError("Nothing on plate reader", true, this);
            }
            try
            {
                MoveToPosition(IncubatorSafe);
                MoveToPosition(IncubatorOverPlate);
                FindMaterialAndGripIt();
                MoveToPosition(IncubatorOverPlate);
                MoveToPosition(IncubatorSafe);
            }
            catch(Exception thrown)
            {
                //if it fails, try to move the arm back to a safe position
                RobotServ.Abort();
                RobotServ.SpeedAsPercentMax = (short)5;
                throw new InstrumentError("Failed to put item on plate\n\n" + thrown.Message, false, this);
            }           
        }
        public void PlaceItemOnTransferStation()
        {
            //This method assumes that a plate with a lid on it is outside the transfer station
            if (Incubator.CheckIfSomethingOnTransferOutStation())
            {
                throw new InstrumentError("Something already on transfer station", true, this);
            }
            try
            {

                MoveToPosition(IncubatorSafe);
                MoveToPosition(IncubatorOverPlate);
                FindMaterialAndReleaseGrip();
                
                MoveToPosition(IncubatorOverPlate);
                MoveToPosition(IncubatorSafe);
            }
            catch(Exception thrown)
            {
                //if it fails, try to move the arm back to a safe position
                RobotServ.Abort();
                RobotServ.SpeedAsPercentMax = (short)5;
                throw new InstrumentError("Failed to put item on plate\n\n" + thrown.Message, false, this);
            }  
        }
        public void PlaceItemInRack(int RackNumber)
        {
            try
            {
                TwisterPosition Safe = RackPositions[RackNumber - 1, 0];
                TwisterPosition Poised = RackPositions[RackNumber - 1, 1];
                TwisterPosition[] ToMoveTo = new TwisterPosition[2] { Safe, Poised };
                foreach (TwisterPosition Position in ToMoveTo)
                {
                    MoveToPosition(Position);
                    //RobotServ.SpeedAsPercentMax = Position.SpeedToMoveAt;
                   // RobotServ.MoveAbsolute(Position.Rotary, Position.Reach, Position.Vertical, Position.Wrist, Position.Track);
                }
                //now to place the item
                FindMaterialAndReleaseGrip();

                ToMoveTo = new TwisterPosition[2] { Poised, Safe };
                foreach (TwisterPosition Position in ToMoveTo)
                {
                    MoveToPosition(Position);
                    //RobotServ.SpeedAsPercentMax = Position.SpeedToMoveAt;
                    //RobotServ.MoveAbsolute(Position.Rotary, Position.Reach, Position.Vertical, Position.Wrist, Position.Track);
                }
            }
            catch (Exception thrown)
            {
                RobotServ.Abort();
                throw new InstrumentError("Could not place item in rack\n\n" + thrown.Message, false, this);
            }
        }        
        public override bool AttemptRecovery(InstrumentError Error)
        {
            //ditch the old stuff
            StatusOK = false;
            try
            {
                CloseAndFreeUpResources();
                Initialize();
                initializeTwisterPositions();
                return true;
            }
            catch { return false; }
        }
        ~Twister()
        {
            if (TwisterClsServer != null || RobotServ != null)
            { CloseAndFreeUpResources(); }
        }
        public override bool CloseAndFreeUpResources()
        {
            //in vb6 these things are closed by setting them equal to none, not so in C# or .NET
            //this setup is terrible
            StatusOK = false;
            try { DisableVerticalMotor(); }
            catch { }
            try
            {
                if (TwisterClsServer != null)
                {                    
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(TwisterClsServer);
                    TwisterClsServer = null;
                }
                    //GC.Collect();
            }
            catch{}
            try
            {
                if (RobotServ != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(RobotServ);
                    RobotServ = null;
                }
                //GC.Collect();
            }
            catch{}
            string[] ToKill = new string[2]{"TwisterII Robot ICP","ZymarkRobotICP"};
            foreach (string str in ToKill)
            {
                KillProcessAttempt(str);
            }
            return true;         
        }


        //public void GetLidInSciclonePosition(int BayNumber)
        //{
        //    This method is a hold over from before when the grip was replaced
        //    It should not be used anymore
        //    //Same as below except with a more nuance move
        //    try
        //    {
        //        //First move to a safe location, otherwise the extended arm can crash into the
        //        //plate reader and incubator as it moves around
        //        MoveToPosition(ScicloneSafe);
        //        //now go to the spot
        //        MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
        //        RobotServ.FindMaterial((object)5, (object)10, (object)0);//no idea what grip offset does, its in user units (microns??)
        //        GripPlateLid();
        //        MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
        //        MoveToPosition(ScicloneSafe);
        //    }
        //    catch (Exception thrown)
        //    {
        //        throw new InstrumentError("Could not place material", false, this);
        //    }
        //}
        //public void GetLidInSciclonePosition(int BayNumber)
        //{
        //    This method is a hold over from before when the grip was replaced
        //    It should not be used anymore
        //    //Same as below except with a more nuance move
        //    try
        //    {
        //        //First move to a safe location, otherwise the extended arm can crash into the
        //        //plate reader and incubator as it moves around
        //        MoveToPosition(ScicloneSafe);
        //        //now go to the spot
        //        MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
        //        RobotServ.FindMaterial((object)5, (object)10, (object)0);//no idea what grip offset does, its in user units (microns??)
        //        GripPlateLid();
        //        MoveToPosition(SciCloneSpots[BayNumber - 1, 0]);
        //        MoveToPosition(ScicloneSafe);
        //    }
        //    catch (Exception thrown)
        //    {
        //        throw new InstrumentError("Could not place material", false, this);
        //    }
        //}
        //public void PlaceLidOnTransferStation()
        //{
        //    //Old method from previous gripper style, is no more

        //    //same as other method, with no check for a lid
        //    ArrayList PositionsToMoveTo = new ArrayList();
        //    try
        //    {

        //        PositionsToMoveTo.Add(IncubatorSafe);
        //        PositionsToMoveTo.Add(IncubatorOverPlate);
        //        PositionsToMoveTo.Add(IncubatorPlate);
        //        foreach (TwisterPosition Position in PositionsToMoveTo)
        //        {
        //            RobotServ.SpeedAsPercentMax = Position.SpeedToMoveAt;
        //            RobotServ.MoveAbsolute(Position.Rotary, Position.Reach, Position.Vertical, Position.Wrist, Position.Track);
        //        }
        //        ResetGrip();
        //        PositionsToMoveTo.Clear();
        //        MoveToPosition(IncubatorOverPlate);
        //        MoveToPosition(IncubatorSafe);

        //    }
        //    catch (Exception thrown)
        //    {
        //        //if it fails, try to move the arm back to a safe position
        //        RobotServ.Abort();
        //        RobotServ.SpeedAsPercentMax = (short)5;
        //        RobotServ.MoveAbsolute(IncubatorSafe.Rotary, IncubatorSafe.Reach, IncubatorSafe.Vertical, IncubatorSafe.Wrist, IncubatorSafe.Track);
        //        throw new InstrumentError("Failed to put item on plate\n\n" + thrown.Message, false, this);
        //    }  
        //}
        //public void GripPlateLid()
        //{
        //    //Old method used with busted reader
        //    //This method is used because if I just invoke "Grip Plate" 
        //    //then the thing will occasionally squeeze so hard that it lifts both the lid and the plate
        //    //by specifying the grip, I avoid this problem
        //    float LidGripValue = -5000;//got this by testing in twister software
        //    ZyRobot_ICP.Axis AC = RobotServ.Grip;
        //    AC.set_TargetPosition(ref LidGripValue);
        //    RobotServ.TriggerMoveAndWait(20000);            
        //    //please reset hand after this!!!
        //}

        //public void ResetGrip()
        //{
        //    //Old method that was a workaround for a broken grip, should never ever be called.
        //    RobotServ.GripOpen();

        //    while (RobotServ.Busy)
        //    {
        //        Thread.Sleep(300);
        //    }
        //    ZyRobot_ICP.Axis AC = RobotServ.Grip;
        //    float Target = -3000;
        //    AC.set_TargetPosition(ref Target);
        //    RobotServ.TriggerMoveAndWait(50000);
        //    RobotServ.Grip.HomeAxis();
        //    int counter = 0;
        //    while (RobotServ.Busy & counter<200)
        //    {
        //        counter++;
        //        Thread.Sleep(300);
        //    }
        //    if (counter >= 200) { throw new InstrumentError("Grip axis did not reset quickly enough", false, this); }
        //}  
      
        //private void OLD_OLD_OLD_initializeTwisterPositions()
        //{

        //    //These positions will change in the new location, you can find them by using the twister software from caliper
        //    //that software trains a position, and by typing them here you are a-ok, trying to get them programmatically was a pain
        //    ReaderSafe = new TwisterPosition("Safe", "Absolute", 2955, -1938, 76960, 137482, 80);
        //    ReaderOverPlate = new TwisterPosition("PlaceClearance1", "Absolute", 2825, 97301, 76954, 137781, 60);
        //    ReaderPlate = new TwisterPosition("Place1", "Absolute", -86579, 97301, 76954, 137781, 15);
        //    //now for the incubator, actually the process incubator in Clara
        //    IncubatorSafe = new TwisterPosition("Safe", "Absolute", 2818, -1938, 118087, -23783, 80);
        //    IncubatorOverPlate = new TwisterPosition("PlaceClearance1", "Absolute", 2825, 84559, 117459, -21274, 60);
        //    //IncubatorOverPlate = new TwisterPosition("PlaceClearance1", "Absolute", 2825, 85560, 117905, -21274, 60);
        //    IncubatorPlate = new TwisterPosition("Place1", "Absolute", -70000, 84559, 117459, -21274, 15);
        //    //IncubatorPlate = new TwisterPosition("Place1", "Absolute", -70000, 85560, 117905, -21274, 15);

        //    //Now for the Garbage Location
        //    GarbageLocation = new TwisterPosition("GarbageSafe", "Absolute", 2900,49999, 209223, -5121, 20);

        //    //Now for a safe spot 
        //    GlobalSafe = new TwisterPosition("GlobalSafe", "Absolute", 2900, -5732, 179861, -5121, 45);
        //    //Now to give all the coordinates of the rack positions
        //    RackPositions = new TwisterPosition[6, 2];
        //    short RackMoveSpeed=30;
        //    RackPositions[0, 0] = new TwisterPosition("PickClearance1", "Absolute", 3000, -2435, 150443, -4285, RackMoveSpeed);
        //    RackPositions[0, 1] = new TwisterPosition("Pick1", "Absolute", -54329, -2435, 150443, -4285, RackMoveSpeed);
        //    RackPositions[1, 0] = new TwisterPosition("PickClearance2", "Absolute", 3000, 61010, 176446, -4076, RackMoveSpeed);
        //    RackPositions[1, 1] = new TwisterPosition("Pick2", "Absolute", -49473, 61010, 176446, -4076, RackMoveSpeed);
        //    RackPositions[2, 0] = new TwisterPosition("PickClear3", "Absolute", 3000, -1582, 202995, -5644, RackMoveSpeed);
        //    RackPositions[2,1]= new TwisterPosition("Pick3","Absolute",-54628,-1582,202995,-5644,RackMoveSpeed);
        //    //fourth positions are on the other side
        //    RackPositions[3, 0] = new TwisterPosition("PickClear4", "Absolute", 3000, -2939, 22129, -3137, RackMoveSpeed);
        //    RackPositions[3, 1] = new TwisterPosition("Pick4", "Absolute", -54171, -2939, 22129, -3137, RackMoveSpeed);
        //    RackPositions[4, 0] = new TwisterPosition("PickClear5", "Absolute", 3000, 61478, -3754, -4289, RackMoveSpeed);
        //    RackPositions[4, 1] = new TwisterPosition("Pick5", "Absolute", -54361, 61478, -3754, -4289, RackMoveSpeed);
        //    RackPositions[5, 0] = new TwisterPosition("PickClear6", "Absolute", 3000, -1856, -29650, -2875, RackMoveSpeed);
        //    RackPositions[5, 1] = new TwisterPosition("Pick6", "Absolute", -54107, -1856, -29650, -2875, RackMoveSpeed);

        //    //Now for the SciClonePositions
        //    //Very important!!! This will hold the positions labelled 1-5, with 1 being closest 
        //    //to the computer.  Position 1 is for the pippette tips, and so will have a special location,
        //    //while position 4 holdsthe shaker.
        //    //only position one will have the move absolute rule enforced
        //    //Note that the first spot is the "safe" position, and the second is the "down" position

        //    //also note that I am defining one "safety position" for all sciclone moves" 
        //    ScicloneSafe = new TwisterPosition("SciSafe", "Absolute", 2825, -1938, 274934, -23783, 30);

        //    SciCloneSpots = new TwisterPosition[4, 2];
        //    SciCloneSpots[0, 0] = new TwisterPosition("OverPippetteSafe", "Absolute", 2759, 210403, 239658, -93685, 20);
        //    SciCloneSpots[1, 1] = new TwisterPosition("InPipetteSpot", "Absolute", -85132, 210403, 239658, -93685, 5);
        //    SciCloneSpots[1, 0] = new TwisterPosition("OverTwoSafe", "Absolute", -48051, 160782, 255749, -115764, 20);
        //    SciCloneSpots[1, 1] = new TwisterPosition("OverTwoPlace", "Absolute", -125830, 1160782, 255749, -115764, 20);
        //    SciCloneSpots[2, 0] = new TwisterPosition("OverThreeSafe", "Absolute", -53682, 153407, 273898, -142338, 20);
        //    SciCloneSpots[2, 1] = new TwisterPosition("OverThreePlace", "Absolute", -128136, 153407, 273898, -142338, 5);
        //    //Now the shaker
        //    SciCloneSpots[3, 0] = new TwisterPosition("SafeOverShaker", "Absolute", -48057, 192265, 290704, -166918, 20);
        //    SciCloneSpots[3, 1] = new TwisterPosition("InShaker", "Absolute", -95143, 192265, 290704, -166918, 5);

        //}
    }
}
