using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace Growth_Curve_Software
{
    public class IncubatorServ:BaseInstrumentClass
    {
        //This is a class for intefacing with the liconic incubator\
        //It handles three main errors 
        //1-Random Communication Error
        //2-Plate Load/Unload Error
        SerialPort IncubatorPort;
        private bool pUseShovelSensor = true;
        public bool UseShovelSensor
        {
            get { return pUseShovelSensor; }
            set { pUseShovelSensor = value; }
        }
        private string pComPort = "COM5";
        public string COM_PORT
        {
            get { return pComPort; }
            set { pComPort = value; }
        }
        private int pStartingSpeed = 650;
        public int STARTING_SPEED
        {
            get { return pStartingSpeed; }
            set
            {
                if (value < 0 || value > 1200)
                {
                    throw new Exception("Tried to set starting speed outside of the allowable range");
                }
                else
                {
                    pStartingSpeed = value;
                }

            }
        }
        private string[] PossibleCommandStarts={"WS","WR","RS","ST"};
        
        private string pInitCommands = "";
        public string InitCommands
        {
            get { return pInitCommands; }
            set { pInitCommands = value; }
        }


       // public const int STARTING_SPEED = 650;
        const string Password = "donkey";
        
       
        
        public IncubatorServ()
        { }
        public override string  Name
        {
         get { return "Incubator"; }
        }
        public override void Initialize()
        {
            IncubatorPort = new SerialPort(COM_PORT);
            IncubatorPort.BaudRate = 9600;
            IncubatorPort.ReadTimeout = 30000;
            IncubatorPort.WriteTimeout = 10000;
            IncubatorPort.Parity = Parity.Even;
            IncubatorPort.DataBits = 8;
            IncubatorPort.StopBits = StopBits.One;
            IncubatorPort.NewLine = "\r\n";
            IncubatorPort.Encoding = Encoding.ASCII;
            StatusOK = true;           
        }
        public override void InitializeFromParsedXML(System.Xml.XmlNode instrumentNode)
        {
            base.InitializeFromParsedXML(instrumentNode);
            this.Initialize(STARTING_SPEED);
        }
        private List<string> CreateAdditionalInitializationCommands()
        {
            List<string> toReturn = new List<string>();
            string[] fullList = InitCommands.Split('\n');
            foreach (string psbCmd in fullList)
            {
                string npsbCmd=psbCmd.Replace("\r","");
                npsbCmd = psbCmd.Trim();
                if (npsbCmd.Length > 3)
                {
                    string startCmd = npsbCmd.Substring(0, 2);
                    if (PossibleCommandStarts.Contains(startCmd))
                    {
                        npsbCmd = npsbCmd.Split(';')[0];
                        npsbCmd = npsbCmd.Trim();
                        toReturn.Add(npsbCmd);
                    }
                }
            }
            return toReturn;
        }
        [UserCallableMethod()]
        public override void Initialize(int ShakerSpeed)
        {
            if (ShakerSpeed > 1200 | ShakerSpeed < 0)
            {
                throw new InstrumentError("Your Shaking Speed Was Not Appropriate", true, this);
            }
            ShakerSpeed = ShakerSpeed / 10;//RPMS to goofy liconic notation
            if (StatusOK)
            {
                
                List<string> InitializeCommands = new List<string>();
                InitializeCommands.Add("WR DM39 " + ShakerSpeed.ToString());
                InitializeCommands.AddRange(CreateAdditionalInitializationCommands());
                InitializeCommands.Add("ST 1900");
                InitializeCommands.Add("RS 1913");
                InitializeCommands.Add("ST 1801");
                if (!IncubatorPort.IsOpen)
                { IncubatorPort.Open(); }
                IncubatorPort.ReadExisting();

                IncubatorPort.WriteLine("CR");
                Thread.Sleep(210);

                //try { PerformCommand("WR DM22 3320"); } //THIS COMMAND GAVE ME A LOT OF TROUBLE, IT SETS THE Z POSITION FOR THE TRANSFER STATION, 42000 IS AT THE TOP, AND 1000 IS TOO LOW, TRY 4000 AND WORK DOWN IF THIS SI OFF
                //catch { }
                //try { PerformCommand("ST 1900"); }//reset from error if it exists
                //catch { }//do nothing if no error
                //string[] InitializeCommands = {  
                //                                  "WR DM89 10000",//NO IDEA WHAT THIS DOES, IN ORIGNIAL FILE THOUGH
                //                                  "WS T61 20000",//ALSO NO IDEA, SENT BY ZYMARK THOUGH
                //                                  //In the old software DM48 set the number of levels, in the new one it sets the max speed
                //                                  //"WR DM48 22", //number of levels in carousal
                //                                  //in the new software it sets the max speed
                //                                  "WR DM48 100",//set speed to 100, should go to 120 but wes was unsure if it now read as a percent or as rpm
                //                                  "WR DM23 1925",//STACKER PITCH, SET BY ZYMARK
                //                                  "WR DM82 3510",//read handler transfer position, default 3500, ALSO 3510 IN ANOTHER FILE
                //                                  "WR DM27 200",//set bcr z-lift read position
                //                                  "WR DM22 1520",//set transfer station out z position
                //                                  "WR DM24 1520",
                //                                  "WR DM80 80",
                //                                  "WR DM81 933",
                //                                  "WR DM39 "+ShakerSpeed.ToString(), //Set shake speed 
                //                                  "RS 1913",//start incubator shaker off
                //                                  "ST 1801" };//INITIALIZE INCUBATOR
                //if (Run48WellRacks)
                //{
                //    string[] NewCommands= {  
                //                                  "WR DM89 10000",//NO IDEA WHAT THIS DOES, IN ORIGNIAL FILE THOUGH
                //                                  "WS T61 20000",//ALSO NO IDEA, SENT BY ZYMARK THOUGH
                //                                  //In the old software DM48 set the number of levels, in the new one it sets the max speed
                //                                  //"WR DM48 22", //number of levels in carousal
                //                                  //in the new software it sets the max speed
                //                                  "WR DM48 100",//set speed to 100, should go to 120 but wes was unsure if it now read as a percent or as rpm
                //                                  "WR DM23 2255",//STACKER PITCH, SET BY ZYMARK
                //                                  "WR DM25 19",//max levels
                //                                  "WR DM21 400",//pitch
                //                                  "WR DM20 1010",
                //                                  "WR DM35 1010",
                //                                  "WR DM82 3510",//read handler transfer position, default 3500, ALSO 3510 IN ANOTHER FILE
                //                                  "WR DM27 200",//set bcr z-lift read position
                //                                  "WR DM22 1520",//set transfer station out z position
                //                                  "WR DM24 1520",

                //                                  "WR DM39 "+ShakerSpeed.ToString(), //Set shake speed 
                //                                  "RS 1913",//start incubator shaker off
                //                                  "ST 1801" };//INITIALIZE INCUBATOR
                //    InitializeCommands = NewCommands;
                //}

                foreach (string com in InitializeCommands)
                {
                    PerformCommand(com);
                }
                StatusOK = true;
            }
            else
            {
                throw new InstrumentError("Method Called Before Instrument Initialized", false, this);
            }
        }
        /// <summary>
        /// Checks if the incubator is ready to receive a command
        /// </summary>
        private bool CheckIfReady()
        {
            //this method will see if the incubator is okay, and if so let the program know, it will check at least 100 times (20 seconds)
            IncubatorPort.ReadExisting();            
            int cycle = 0;
            while (cycle < 100)
            {
                IncubatorPort.WriteLine("RD 1915");
                string response = IncubatorPort.ReadLine();
                if (response == "0")
                { Thread.Sleep(250); }
                if (response == "1")
                { return true; }
                cycle++;
            }            
            return MakeReady();//should never actually be returned, an error should be thrown instead
        }
        private bool MakeReady()
        {
            //this method is invoked if CheckIfReady can't get a RD1915 respones, it tries to reset the comm and make all okay
            bool fix=CheckCommErrorAndResetAndFixError();
            if (fix)
            {
                return CheckIfReady();
            }
            else
            {
                StatusOK = false;
                throw new InstrumentError("System could not be made ready", false, this);
            }
        }
        [UserCallableMethod()]
        public void ResetIncubator()
        {
            //This command is for resetting the incubator 
            if (IncubatorPort != null)
            {
                if (!IncubatorPort.IsOpen)
                { IncubatorPort.Open(); }
                //IncubatorPort.ReadExisting();
                //IncubatorPort.WriteLine("CR");
                //Thread.Sleep(210);
                //IncubatorPort.ReadLine();//get result back
                PerformCommand("ST 1900");
                Thread.Sleep(200);
                PerformCommand("ST 1801");//reset command
            }
            else
            {
                Initialize();//open port
                ResetIncubator();
            }
        }
        private bool CheckIfPlateClearedTransferStation()
        {
            //this method will check if the incubator has imported a plate off the transfer station
            //this method is essentially a check if the incubator took too long to import the plate,
            //it will either return or throw an error
            
            int cycle = 0;//waits for cycle*thread sleep time ms
            while (cycle < 300)//should be one minute
            {
                //Code below was original code, modified on 7/1/2011 by Nigel when it stopped working
                //on the robots in the teaching lab, not sure what 1815 command was supposed to do, changed it in 
                //block below to read the transfer station sensor instead, it was RD 1815 before this, not sure why at all
                //this is the plate ready flag in the old firmware version
                //IncubatorPort.WriteLine("RD 1815");
                //7/1/2011 note: Not sure why not just using CheckIfSomethingOnTransferOutStation method, 
                //think it is because the ready flag won't be set without it....
                IncubatorPort.WriteLine("RD 1813");
                Thread.Sleep(210);
                string response = IncubatorPort.ReadLine();
                if (response == "1")
                { Thread.Sleep(200); }
                else if (response == "0")
                { return true; }
                else { throw new InstrumentError("Could not check if the plate was lifted of the transfer station during import", false, this); }

            }
            throw new InstrumentError("Plate not lifted off the incubator station in an appropriate amount of time",false,this);

        }
        private bool CheckIfPlateReady()
        {
            //this method is essentially a delay to check if the plate command is finished
            int cycle = 100;
            while (cycle < 10)
            {
                IncubatorPort.WriteLine("RD 1815");
                Thread.Sleep(210);
                string response = IncubatorPort.ReadLine();
                if (response == "1")
                { Thread.Sleep(200); }
                if (response == "0")
                { return true; }
                cycle++;
            }
            return false;
        }
        private void HandleIncubatorError(string IncResponse)
        {
            //this is a generic method designed to go into affect when an error occurs
            
 
        }
        [UserCallableMethod()]
        public bool CheckIfSomethingOnTransferOutStation()
        {
            //this checks the sensor on the 
            return ReadSensor("RD 1813");
        }
        [UserCallableMethod()]
        public bool CheckIfSomethingOnPlateMover()
        {
            return ReadSensor("RD 1812");
        }
        private bool ReadSensor(string Command)
        {
            //This command will read the vairous sensors on the liconic incubator, and return true if they are active (Red light)
           
            string response;
            IncubatorPort.ReadExisting();
            if (StatusOK && CheckIfReady())
            {
                IncubatorPort.WriteLine(Command);
                Thread.Sleep(210);
                response = IncubatorPort.ReadLine();
                if (response == "0")
                {
                    return false;
                }
                else if (response == "1")
                {
                    return true;
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Check sensor command did not return 1 or 0, sensor "+Command, true, this);
                }
            }
            else
            {
                StatusOK = false;
                throw new InstrumentError("Status not okay or instrument could not be made ready for sensor command", false, this);
            }

        }
        [UserCallableMethod()]
        public string PerformCommand(string Command)
        {
            string response="";
            IncubatorPort.ReadExisting();
            if (StatusOK && CheckIfReady())
            {
                IncubatorPort.WriteLine(Command);
                Thread.Sleep(250);//ensures required delay between requests
                response = IncubatorPort.ReadLine();
                if (response != "OK")//this means there was a serious error
                { throw new InstrumentError("Could not perform command " + Command+"\n Received response: "+response, false, this); }
            }
            else
            {
                throw new InstrumentError("Could not perform command " + Command+"\n Received response: "+response, false, this);
            }
            return response;
        }
        /// <summary>
        /// Same as perform command but for those that return 1 or 0.
        /// </summary>
        /// <param name="Command">Command to perform</param>
        /// <returns>"1" or "0"</returns>
        [UserCallableMethod()]
        public string PerformBinaryCommand(string Command)
        {
            //same as above but for commands that return 1 or zero
            string response = "";
            IncubatorPort.ReadExisting();
            if (StatusOK && CheckIfReady())
            {
                IncubatorPort.WriteLine(Command);
                Thread.Sleep(250);//ensures required delay between requests
                response = IncubatorPort.ReadLine();
            }
            else
            {
                throw new InstrumentError("Could not perform command " + Command + "\n Received response: " + response, false, this);
            }
            return response;
        }
        [UserCallableMethod()]
        public string PerformCommandUnsafe(string Command, string password)
        {
            if (password == Password)
            {
                string response = "";
                IncubatorPort.ReadExisting();
                IncubatorPort.WriteLine(Command);
                Thread.Sleep(250);//Make sure no command is sent earlier then this
                response = IncubatorPort.ReadLine();                 
                return response;
            }
            else { return "No Password Entered"; }
        }
        [UserCallableMethod()]
        public void StartShaking()
        {
            PerformCommand("ST 1913");
            //Require a miniumum amount of shaking here, there were big errors without this
            //still seem to be errors at 1500 so I uppped it
            Thread.Sleep(3000);
        }
        [UserCallableMethod()]
        public void ChangeShakingSpeed(int ShakerSpeed)
        {
            try
            {
                if (ShakerSpeed > 1200 | ShakerSpeed < 0)
                {
                    throw new InstrumentError("Your Shaking Speed Was Not Appropriate Using A Default of 600 RPMs", true, this);
                }
               
                Initialize(ShakerSpeed);
                //ShakerSpeed = ShakerSpeed / 10;//RPMS to goofy liconic notation
                //old method below
                //PerformCommand("WR DM39 " + ShakerSpeed.ToString());
                //I think I actually need to reinitialize the incubator to make this work

                //Thread.Sleep(500);
                //StopShaking();
                //Thread.Sleep(500);
                //StartShaking();
            }
            catch
            {
                StatusOK = false;
                throw new InstrumentError("Could Not Change Shaking Speed", false, this);
            }     
            
        }
        [UserCallableMethod()]
        public void StopShaking()
        {
            PerformCommand("RS 1913");
            //I had problems before with the robot arm moving up before shaking had stopped, big problems
            //I am hoping adding the delay below will fix this, so that the command is definitely processed
            Thread.Sleep(1500);
        }
        [UserCallableMethod()]
        public bool CheckCommErrorAndResetAndFixError()
        {
            //this is in case of an error where the thing simply cannot communicate, it is 
            //designed to reset communication, it should not be used otherwise
            //returns true if comm okay, 0 otherwise, should only be called when the 1915 command is failing
            string response;
            IncubatorPort.ReadExisting();
            IncubatorPort.WriteLine("CR");//makes sure the thing is available to talk
            Thread.Sleep(210);
            response=IncubatorPort.ReadLine();
            if(response!="CC")//could not recover from error one day, not sure what the problem was, changed it to full com, but for now setting this as not causing an error
            { StatusOK = false; }//throw new InstrumentError("Error communicating with the incubator, it did not respond to communication request", false, this); }
            IncubatorPort.WriteLine("RD 1814");//checks if there is an error
            Thread.Sleep(210);
            response=IncubatorPort.ReadLine();
            if (response == "1")
            {
                //this means there is an error
                IncubatorPort.WriteLine("RD DM200");
                Thread.Sleep(210);
                string error = IncubatorPort.ReadLine();//this would return the error code
                IncubatorPort.WriteLine("ST 1900");
                Thread.Sleep(210);
                response = IncubatorPort.ReadLine();
                if (response == "OK")
                {
                    StatusOK = true;
                    return true;
                }
                else
                {
                    StatusOK = false;
                    throw new InstrumentError("Error communicating with the machine the error was \n:" + error, false, this);
                }
            }
            else
            {
                //check that the error really exists
                IncubatorPort.ReadExisting();
                IncubatorPort.WriteLine("RD 1915");
                Thread.Sleep(210);
                response = IncubatorPort.ReadLine();
                if (response == "1")
                {
                    StatusOK = true;
                    return true;
                }
                else
                {
                    // this means that there is no "error" but 1915 is busted, this is bad
                    //going to try to reset
                    PerformCommandUnsafe("ST 1900", Password);
                    Thread.Sleep(100);
                    PerformCommandUnsafe("ST 1801", Password);
                    Thread.Sleep(250);
                    int cycle = 0;
                    response = "0";
                    while (cycle < 70)
                    {
                        IncubatorPort.WriteLine("RD 1915");
                        response = IncubatorPort.ReadLine();
                        if (response == "0")
                        { Thread.Sleep(250); }
                        if (response == "1")
                        { break; }
                        cycle++;
                    }
                    if (response == "0")
                    {
                        StatusOK = false;
                        throw new InstrumentError("Error communicating with the machine the error, could not identify problem", false, this);
                    }
                }
            }
            return false;
        }       
        public override bool CloseAndFreeUpResources()
        {
            //free up the port the incubator uses
            StatusOK = false;
            if (IncubatorPort!=null && IncubatorPort.IsOpen)
            {
                try
                {
                    IncubatorPort.ReadExisting();
                    //Do not put to sleep
                    //IncubatorPort.WriteLine("RS 1913");
                    //Thread.Sleep(400);
                    IncubatorPort.WriteLine("CQ");
                    Thread.Sleep(210);
                    IncubatorPort.DiscardOutBuffer();
                    IncubatorPort.DiscardInBuffer();
                    IncubatorPort.Close();
                    //now also dispose
                    IncubatorPort.Dispose();
                    //and sleep a bit to let it to that
                    Thread.Sleep(1000);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        private void PerfromOdd1911Command()
        {
            //this is for a the odd 1911 command, likely only relevent to HT incubators whatever the hell those are
            //CHECK IF ANYTHING IS ON SHOVEL
            PerformCommand("ST 1911");
            Thread.Sleep(200);
            //Read it
            string response;
            IncubatorPort.WriteLine("RD 1812");
            Thread.Sleep(210);
            response = IncubatorPort.ReadLine();
            if (response == "1")
            {
                MessageBox.Show("Something is on the plate");
                return;
            }
        }
        private void CheckCommandReady()
        {
            if (StatusOK && CheckIfReady())
            { return; }
            else
            {
                StatusOK = false;
                //Removed for testing:
                //throw new InstrumentError("Not appropriate to give a command, instrument not initialized or not accepting communications", false, this); }
            }
        }
        [UserCallableMethod()]
        public void LoadPlate(int Position)
        {
            //This method will load a plate in, plates are 0 to 44
            //First to check the plate positions to insure they are free

            CheckCommandReady();
            if (!CheckIfSomethingOnTransferOutStation())
            {
                StatusOK = false;
                throw new InstrumentError("There was no plate on the transfer station before move command", false, this);      }
            if(UseShovelSensor && CheckIfSomethingOnPlateMover())
            {
                StatusOK = false;
                throw new InstrumentError("There was something on the plate mover before move command",false,this);      }

            if (Position > 0 && Position < 45)//make sure position is sensible
            {
                //change to start/stop shaking
                bool Shaking = IsIncubatorShaking();
                if (Shaking) { StopShaking(); }
                //MOVE THE PLATE IN
                PerformCommand("WR DM10 " + Position.ToString());
                //Now inserting the plate ready flag, go
                CheckIfPlateClearedTransferStation();
                //now to give it time to put it in
                int counter = 0;
                while (true)
                {
                    bool PlateOffShovel=false;
                    if (UseShovelSensor)
                    {
                        PlateOffShovel = !CheckIfSomethingOnPlateMover();
                    }
                    else
                    {
                        PlateOffShovel = CheckIfSomethingOnTransferOutStation();
                    }
                    if (!PlateOffShovel && !CheckIfReady())
                    {
                        Thread.Sleep(200);
                    }
                    else { break; }
                    if (counter > 150)
                    {
                        StatusOK = false;
                        throw new InstrumentError("Could not place the plate in the rack in time",false,this); }
                    counter++;
                }
                if (UseShovelSensor)
                {
                    Thread.Sleep(500);
                }
                if (Shaking) { StartShaking(); }
            }
            else
            {
                throw new InstrumentError("You tried to load a plate into a spot that does not exist", true, this);
            }
        }
        [UserCallableMethod()]
        public void UnloadPlate(int Position )
        {
            //This method will load a plate in, plates are 0 to 44
            //First to check the plate positions to insure they are free
            CheckCommandReady();
            if (CheckIfSomethingOnTransferOutStation())
            {
                StatusOK = false;
                throw new InstrumentError("There was a plate on the transfer station before out command", false, this);
            }
            if (UseShovelSensor && CheckIfSomethingOnPlateMover())
            {
                StatusOK = false;
                throw new InstrumentError("There was something on the plate mover before out command", false, this);
            }

            if (Position > 0 && Position < 45)//make sure position is sensible
            {
                //MOVE THE PLATE IN
                bool Shaking = IsIncubatorShaking();
                if (Shaking) { StopShaking(); }
                PerformCommand("WR DM15 " + Position.ToString());
                Thread.Sleep(1500);//give it a bit
                //Now to do two things wait for the sensor to hit the thing, and then wait for nothing to be on it 
                int counter = 0;
                while (true)
                {
                    bool OnPlateShovel = false;
                    if (!UseShovelSensor) { OnPlateShovel = CheckIfSomethingOnPlateMover(); }
                    bool SensorsShowPlateReadyToMove = !OnPlateShovel && CheckIfSomethingOnTransferOutStation();
                    if (!SensorsShowPlateReadyToMove)
                    {
                        Thread.Sleep(200);
                    }
                    else { break; }
                    if (counter > 400)
                    {
                        StatusOK = false;
                        throw new InstrumentError("Could not place the plate on the mover in time", false, this);
                    }
                    counter++;
                }
                //Introduce a delay to give the shovel time to get out of there
                if (!UseShovelSensor)
                {
                    Thread.Sleep(4000);
                }
                ////now to wait for it to get off 
                //counter = 0;
                //while (true)
                //{
                //    if (!CheckIfSomethingOnPlateMover()  && !CheckIfSomethingOnTransferOutStation())
                //    {
                //        Thread.Sleep(200);
                //    }
                //    else { break; }
                //    if (counter > 400)
                //    {
                //        StatusOK = false;
                //        throw new InstrumentError("Could not place the plate on the transfer out in time in time", false, this);
                //    }
                //    counter++;
                //}
                ////now to make sure it made it too the transfer station
                //counter = 0;
                //while (true)
                //{
                //    if (!CheckIfSomethingOnTransferOutStation())
                //    {
                //        Thread.Sleep(200);
                //    }
                //    else { break; }
                //    if (counter > 400)
                //    {
                //        StatusOK = false;
                //        throw new InstrumentError("Plate does not seem too have made it to transfer out station", false, this);
                //    }
                //    counter++;
                //}
                if (Shaking) { StartShaking(); }

            }
            else
            {
                throw new InstrumentError("You tried to unload a plate from a spot that does not exist", true, this);
            }
        }
        private bool IsIncubatorShaking()
        {
            string IsShaking = PerformBinaryCommand("RD 1913");
            return IsShaking == "1" ? true : false;
        }
        [UserCallableMethod()]
        public void StopIncubatorShakingWhenNothingElseWill(string password)
        {
            //for whatever reason it appears that the incubator occasionally will not stop shaking
            //something causes the incubator to think it is not shaking ("RD 1913" comes back "0" despite
            //that it is shaking)  to fix this, I first turn on the shaking to correct, and then turn it off again

            //password not currently implemented
            try
            {
                PerformCommand("RD 1913");//doesn't actually do anything now, but this is how a check should be made
                PerformCommand("ST 1913");
                PerformCommand("RS 1913");
            }
            catch (Exception thrown)
            {
                StatusOK = false;
                throw new InstrumentError("Could not reset the dang thing: "+thrown.Message, false, this);
            }
        }
        public override bool AttemptRecovery()
        {
            StatusOK = false;
            try
            {
                if (IncubatorPort == null)
                { Initialize(); }
                if (!IncubatorPort.IsOpen)
                { IncubatorPort.Open(); }
            }
            catch { return false; }
            //okay, if the initialization worked, then the status okay needs to be set to okay, else,
            //the performcommand option won't work
            StatusOK = true;
            
            try { CheckCommErrorAndResetAndFixError(); }
            catch { }
            try
            {
                ResetIncubator();
                Initialize(this.STARTING_SPEED);
                return true;
            }
            catch { }
            return false;
        }
        public override bool AttemptRecovery(InstrumentError Error)
        {
            //This method will attempt to recover from an error of somesort
            if (Error.CanRecover)
            {
                return false;                
            }
            else { return false; }//could not recover from the error in question
        }
        [UserCallableMethod()]
        public void ReleaseComPort(string PassWord)
        {
            try
            {
                StatusOK = false;
                if (PassWord == "donkey")
                {
                    if (IncubatorPort.IsOpen)
                    {
                        IncubatorPort.Close();
                    }
                    IncubatorPort = null;
                    GC.Collect();
                }
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("Could not close comm port  "+thrown.Message, false, this);
            }
        }
    }
}
