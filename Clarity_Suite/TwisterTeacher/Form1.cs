using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Clarity;


namespace Clarity
{
    
    public sealed partial class TwisterTeacher : Form
    {
        public Dictionary<string,Twister.TwisterPosition> TwistDict;
        Twister Robot;
        bool SimulationMode;
        ZyRobot_ICP.RobotServer RobotServ;

        //This Array will contain all of the Various Postions for the rack, each "row" will have the safety position and the 
        //pick position
        Twister.TwisterPosition[,] RackPositions;
        string FilePath;

        /// <summary>
        /// Nothing in this area is important, it was only placed here to aid in debugging
        /// </summary>
        Twister.TwisterPosition IncubatorSafe;//the location where it is safe to move the twister
        Twister.TwisterPosition IncubatorOverPlate;//the position over the DCU transfer out
        Twister.TwisterPosition IncubatorPlate;
        Twister.TwisterPosition ReaderSafe;
        Twister.TwisterPosition ReaderOverPlate;
        Twister.TwisterPosition ReaderPlate;
        Twister.TwisterPosition GarbageLocation;
        Twister.TwisterPosition GlobalSafe;
        Twister.TwisterPosition ScicloneSafe;
        List<Twister.TwisterPosition> ListOfSpots;
        Twister.TwisterPosition[,] SciCloneSpots;

        public TwisterTeacher()
        {
            InitializeComponent();
          
        }
        private void Never_Call_initializeTwisterPositions()
        {
            ListOfSpots = new List<Twister.TwisterPosition>();
            //These positions will change in the new location, you can find them by using the twister software from caliper
            //that software trains a position, and by typing them here you are a-ok, trying to get them programmatically was a pain
            ReaderSafe = new Twister.TwisterPosition("Reader Safe", "Absolute", 2955, -1938, 76960, 137482, 80);
            ListOfSpots.Add(ReaderSafe);
            ReaderOverPlate = new Twister.TwisterPosition("Reader Clearance", "Absolute", 2825, 97301, 76954, 137781, 60);
            ListOfSpots.Add(ReaderOverPlate);
            ReaderPlate = new Twister.TwisterPosition("Reader Place", "Absolute", -86579, 97301, 76954, 137781, 15);
            ListOfSpots.Add(ReaderPlate);
            //now for the incubator, actually the process incubator in Clara
            IncubatorSafe = new Twister.TwisterPosition("Incubator Safe", "Absolute", 2818, -1938, 118087, -23783, 80);
            ListOfSpots.Add(IncubatorSafe);
            IncubatorOverPlate = new Twister.TwisterPosition("Incubator Clearance", "Absolute", 2825, 84559, 117459, -21274,60);
            ListOfSpots.Add(IncubatorOverPlate);
            //IncubatorOverPlate = new TwisterPosition("PlaceClearance1", "Absolute", 2825, 85560, 117905, -21274, 60);
            IncubatorPlate = new Twister.TwisterPosition("Incubator Place", "Absolute", -70000, 84559, 117459, -21274, 15);
            ListOfSpots.Add(IncubatorPlate);
            //IncubatorPlate = new TwisterPosition("Place1", "Absolute", -70000, 85560, 117905, -21274, 15);
            

            //Now for the Garbage Location
            GarbageLocation = new Twister.TwisterPosition("Garbage Safe", "Absolute", 2900, 49999, 209223, -5121, 20);
            ListOfSpots.Add(GarbageLocation);
            //Now for a safe spot 
            GlobalSafe = new Twister.TwisterPosition("Global Safe", "Absolute", 2900, -5732, 179861, -5121, 45);
            ListOfSpots.Add(GlobalSafe);
            //Now to give all the coordinates of the rack positions
            RackPositions = new Twister.TwisterPosition[6, 2];
            short RackMoveSpeed = 30;
            RackPositions[0, 0] = new Twister.TwisterPosition("Rack 1 Clearance", "Absolute", 3000, -2435, 150443, -4285, RackMoveSpeed);
            RackPositions[0, 1] = new Twister.TwisterPosition("Rack 1", "Absolute", -54329, -2435, 150443, -4285, RackMoveSpeed);
            RackPositions[1, 0] = new Twister.TwisterPosition("Rack 2 Clearance", "Absolute", 3000, 61010, 176446, -4076, RackMoveSpeed);
            RackPositions[1, 1] = new Twister.TwisterPosition("Rack 2", "Absolute", -49473, 61010, 176446, -4076, RackMoveSpeed);
            RackPositions[2, 0] = new Twister.TwisterPosition("Rack 3 Clearance", "Absolute", 3000, -1582, 202995, -5644, RackMoveSpeed);
            RackPositions[2, 1] = new Twister.TwisterPosition("Rack 3", "Absolute", -54628, -1582, 202995, -5644, RackMoveSpeed);
            //fourth positions are on the other side
            RackPositions[3, 0] = new Twister.TwisterPosition("Rack 4 Clearance", "Absolute", 3000, -2939, 22129, -3137, RackMoveSpeed);
            RackPositions[3, 1] = new Twister.TwisterPosition("Rack 4", "Absolute", -54171, -2939, 22129, -3137, RackMoveSpeed);
            RackPositions[4, 0] = new Twister.TwisterPosition("Rack 5 Clearance", "Absolute", 3000, 61478, -3754, -4289, RackMoveSpeed);
            RackPositions[4, 1] = new Twister.TwisterPosition("Rack 5", "Absolute", -54361, 61478, -3754, -4289, RackMoveSpeed);
            RackPositions[5, 0] = new Twister.TwisterPosition("Rack 6 Clearance", "Absolute", 3000, -1856, -29650, -2875, RackMoveSpeed);
            RackPositions[5, 1] = new Twister.TwisterPosition("Rack 6", "Absolute", -54107, -1856, -29650, -2875, RackMoveSpeed);
            foreach (Twister.TwisterPosition pos in RackPositions)
            {
                ListOfSpots.Add(pos);
            }
            //Now for the SciClonePositions
            //Very important!!! This will hold the positions labelled 1-5, with 1 being closest 
            //to the computer.  Position 1 is for the pippette tips, and so will have a special location,
            //while position 4 holdsthe shaker.
            //only position one will have the move absolute rule enforced
            //Note that the first spot is the "safe" position, and the second is the "down" position

            //also note that I am defining one "safety position" for all sciclone moves" 
            ScicloneSafe = new Twister.TwisterPosition("Sci Safe", "Absolute", 2825, -1938, 274934, -23783, 30);
            ListOfSpots.Add(ScicloneSafe);
    

            //SciCloneSpots = new Twister.TwisterPosition[4, 2];
            //SciCloneSpots[0, 0] = new Twister.TwisterPosition("SciClone 1 Clearance", "Absolute", 2759, 210403, 239658, -93685, 20);
            //SciCloneSpots[0, 1] = new Twister.TwisterPosition("SciClone 1", "Absolute", -85132, 210403, 239658, -93685, 5);
            //SciCloneSpots[1, 0] = new Twister.TwisterPosition("SciClone 2 Clearance", "Absolute", -48051, 160782, 255749, -115764, 20);
            //SciCloneSpots[1, 1] = new Twister.TwisterPosition("SciClone 2", "Absolute", -125830, 1160782, 255749, -115764, 20);
            //SciCloneSpots[2, 0] = new Twister.TwisterPosition("SciClone 3 Clearance", "Absolute", -53682, 153407, 273898, -142338, 20);
            //SciCloneSpots[2, 1] = new Twister.TwisterPosition("SciClone 3", "Absolute", -128136, 153407, 273898, -142338, 5);
            ////Now the shaker
            //SciCloneSpots[3, 0] = new Twister.TwisterPosition("SciClone 4 Clearance", "Absolute", -48057, 192265, 290704, -166918, 20);
            //SciCloneSpots[3, 1] = new Twister.TwisterPosition("SciClone 4", "Absolute", -95143, 192265, 290704, -166918, 5);
            //foreach(Twister.TwisterPosition pos in SciCloneSpots)
            //{
            //    ListOfSpots.Add(pos);
            //}  
            TwistDict = new Dictionary<string, Twister.TwisterPosition>();
            foreach (Twister.TwisterPosition pos in ListOfSpots)
            {
                lstPositions.Items.Add(pos);
                TwistDict[pos.Name]= pos;
            }
        }
        private void DeSerializeList()
        {           
                TwistDict=new Dictionary<string,Twister.TwisterPosition>();
                FileStream f=new FileStream(FilePath,FileMode.Open);
                BinaryFormatter b=new BinaryFormatter();
                TwistDict = (Dictionary<string,Twister.TwisterPosition>)b.Deserialize(f);
                f.Close();
                foreach(Twister.TwisterPosition pos in TwistDict.Values)
                {
                    lstPositions.Items.Add(pos);
                }
                Twister.TwisterPosition TestPos = new Twister.TwisterPosition("Test", "Absolute", 0, 0, 0, 0, 15);
                lstPositions.Items.Add(TestPos);       
        }
        private void Never_Called_SerializeList()
        {
            TwistDict.Clear();
            ListOfSpots.TrimExcess();
            foreach(Twister.TwisterPosition pos in ListOfSpots)
            {
                
                if (pos.Rotary!=0.0)
                {
                    TwistDict[pos.Name] = pos;
                }
            }
            FileStream f=new FileStream(FilePath,FileMode.Create);
            //Stream s=f.Open(FileMode.Create);
            BinaryFormatter b=new BinaryFormatter();
            b.Serialize(f,TwistDict);
            f.Close();
            
        }
        private string GetPositionsFile()
        {
            try
            {
                string fileName = BaseInstrumentClass.GetXMLSettingsFile();
                //now to get the file for the data
                XmlDocument XmlDoc = new XmlDocument();
                XmlTextReader XReader = new XmlTextReader(fileName);
                XmlDoc.Load(XReader);
                System.Xml.XmlNode InstrumentsNode = XmlDoc.SelectSingleNode("//Instruments/Twister/IncubatorPositionsFile");
                return InstrumentsNode.InnerText;
            }
            catch
            {
                throw new Exception("Could not determine the positions file location from the xml file");
            }


        }
        private void TwisterTeacher_Load(object sender, EventArgs e)
        {
            try
            {
                Twister Robot = new Twister();
                Robot.Initialize();

                //set up the file
                SimulationMode = false;
                TwistDict = new Dictionary<string, Twister.TwisterPosition>();
                FilePath = GetPositionsFile();
                Never_Call_initializeTwisterPositions();
                //DeSerializeList();                
                
                
                //THE TWISTER SERVE MUST BE INITIALIZED BEFORE IT WILL GIVE A ROBOT REF PROPERLY
                RobotServ = Robot.RobotServ;//this line must follow the server initialization
                RobotServ.SpeedAsPercentMax = (short)20;
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could Not Gain Control Of Twister \n\n"+thrown.Message);
                Application.ExitThread();
            }              
        }
        private void btnGetPosition_Click(object sender, EventArgs e)
        {
            //need order right
            //new TwisterPosition("OverTwoSafe", "Absolute", -48051, 160782, 255749, -115764, 20);
            float vert=RobotServ.Vertical.CurrentPosition;
            float reach = RobotServ.Reach.CurrentPosition;
            float rotary = RobotServ.Rotary.CurrentPosition;
            float wrist = RobotServ.Wrist.CurrentPosition;
            txtPosition.Text = vert.ToString() + "," + reach.ToString() + "," + rotary.ToString() + "," + wrist.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            float target = -800;
            RobotServ.Grip.set_TargetPosition(ref target);
            RobotServ.TriggerMoveAndWait(20000);
   
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RobotServ.Grip.HomeAxis();
            RobotServ.TriggerMoveAndWait(20000);
           while (RobotServ.Busy)
            {
                MessageBox.Show("The Server is Busy");
            }
        }
        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstPositions.SelectedIndex > -1)
                {
                    Twister.TwisterPosition Pos = (Twister.TwisterPosition) lstPositions.SelectedItem;
                    RobotServ.MoveAbsolute(Pos.Rotary, Pos.Reach, Pos.Vertical, Pos.Wrist, Pos.Track);
                    txtReach.Text = Pos.Reach.ToString();
                    txtVertical.Text = Pos.Vertical.ToString();
                    txtRotary.Text = Pos.Rotary.ToString();
                    txtWrist.Text = Pos.Wrist.ToString();                    
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show(thrown.Message);
            }
        }
        private void btnChangePosition_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstPositions.SelectedIndex > -1)
                {
                    Twister.TwisterPosition OldPos = (Twister.TwisterPosition)lstPositions.SelectedItem;
                    float vert = RobotServ.Vertical.CurrentPosition;
                    float reach = RobotServ.Reach.CurrentPosition;
                    float rotary = RobotServ.Rotary.CurrentPosition;
                    float wrist = RobotServ.Wrist.CurrentPosition;
                    OldPos.Vertical = vert;
                    OldPos.Wrist = wrist;
                    OldPos.Reach = reach;
                    OldPos.Rotary = rotary;
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show(thrown.Message);
            }
        }
        private void saveAllPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TwistDict.Clear();
                ListOfSpots = new List<Twister.TwisterPosition>();

                foreach (object o in lstPositions.Items)
                {
                    Twister.TwisterPosition pos = o as Twister.TwisterPosition;
                    TwistDict[pos.Name.Trim()] = pos;
                }
                //FileStream f = new FileStream(Twister.IncubatorPositionsFile, FileMode.Create);
                FileStream f = new FileStream(GetPositionsFile(), FileMode.Create);
                
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(f, TwistDict);
                f.Close();
                MessageBox.Show("Twister Positions Saved");
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Unable to save positions\n\n"+thrown.Message);
            }
        }
        private void btnGripMaterial_Click(object sender, EventArgs e)
        {
            RobotServ.GripMaterial((object)4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Robot.DisableVerticalMotor();
        }

    }
}
