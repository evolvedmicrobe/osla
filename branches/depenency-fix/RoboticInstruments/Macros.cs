using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Growth_Curve_Software
{
    public class Macros : VirtualInstrument
    {
        //This represents a collection of "Scripts" that calls the other instrument classes 
        //and runs them, there are a few gotchas with this, first, all methods called by these methods
        //should return their own errors, period, at all times, these exceptions can then be
        //passed onto the program that calls the script.  
        //Also, one cannot restart midscript, for example, if the script involves 10 commands, then all 10 must execute
        //finally, none of these methods exist independently of the main form, and all are dependent on it.

        public Macros()
        {
            StatusOK = true;//always going to have this be the case

        }
        public override string Name
        {
            get { return "Macros"; }
        }

        public bool MovePlateFromIncubatorToVictor(int PlateSlot)
        {
            //First to unload the plate
            FakeForm1.Incubator.UnloadPlate(PlateSlot);
            FakeForm1.TransStation.TurnOnVacuumAndLiftLid();
            FakeForm1.TransStation.MoveStationOut();
            FakeForm1.Robot.MovePlateFromTransferStationToPlateReader();
            FakeForm1.TransStation.MoveStationIn();
            FakeForm1.TransStation.TurnOffVacuumAndReturnLid();
            return true;
        }
        public bool AttemptToReseatLid()
        {
            FakeForm1.TransStation.TurnOnVacuumAndLiftLid();
            FakeForm1.TransStation.MoveStationOut();
            FakeForm1.Robot.MovePlateFromTransferStationToPlateReader();
            FakeForm1.TransStation.MoveStationIn();
            FakeForm1.TransStation.TurnOffVacuumAndReturnLid();
            
            FakeForm1.TransStation.TurnOnVacuumAndLiftLid();
            FakeForm1.TransStation.MoveStationOut();
            FakeForm1.Robot.MovePlateFromPlateReaderToIncubator();
            FakeForm1.TransStation.MoveStationIn();
            FakeForm1.TransStation.TurnOffVacuumAndReturnLid();
            return true;
        }
        public bool RackToSciclone(int RackPosition,int SciClonePosition)
        {
            //this method will take a new plate out of the rack, and place its lid in the 3rd 
            //Sciclone spot and the plate in the second
            FakeForm1.Robot.MoveTwisterToSafePosition();
            FakeForm1.Robot.GrabItemFromRack(RackPosition);
            FakeForm1.Robot.PlaceItemInSciclonePosition(SciClonePosition);
            return true;
        }
        public bool IncPlateToSciClone(int IncPosition,int SciClonePos)
        {
            //this method unloads a plate, and moves it to the sciclone shaker, discarding the lid as it goes
            FakeForm1.Incubator.UnloadPlate(IncPosition);
            FakeForm1.TransStation.MoveStationOut();
            FakeForm1.Robot.GrabItemOnTransferStation();
            FakeForm1.Robot.PlaceItemInSciclonePosition(SciClonePos);
            FakeForm1.TransStation.MoveStationIn();
            FakeForm1.Robot.MoveTwisterToSafePosition();
            return true;
        }
        public bool ScicloneToIncubator(int IncPos,int SciPos)
        {
            //This method will move a plate from position 2, with its lid in position 3, to the incubator
            if (FakeForm1.Incubator.CheckIfSomethingOnTransferOutStation())
            {
                throw new InstrumentError("Their is something on the transfer out station", true, this);
            }
            //otherwise move ahead
            FakeForm1.TransStation.MoveStationOut();
            FakeForm1.Robot.GetItemInSciclonePosition(SciPos);
            FakeForm1.Robot.PlaceItemOnTransferStation();
            FakeForm1.TransStation.MoveStationIn();
            FakeForm1.Incubator.LoadPlate(IncPos);
            return true;
        }
        public bool SciCloneToRack(int SciClonePosition, int RackNumber)
        {
            FakeForm1.Robot.GetItemInSciclonePosition(SciClonePosition);
            FakeForm1.Robot.PlaceItemInRack(RackNumber);
            return true;
        }
        public bool PlaceNewTipsInSciclone(int RackWiTips)
        {
            FakeForm1.Robot.HomeAllAxes();
            FakeForm1.Robot.GrabItemFromRack(RackWiTips);
            FakeForm1.TransStation.MoveStationOut();
            if (FakeForm1.Incubator.CheckIfSomethingOnTransferOutStation())
            {
                throw new InstrumentError("Something is already on the transfer out station, cannot place tip lid there", true, this);
            }
            FakeForm1.Robot.PlaceItemOnTransferStation();
            FakeForm1.Robot.GrabItemFromRack(RackWiTips);
            FakeForm1.Robot.PlaceItemInSciclonePosition(1);
            FakeForm1.Robot.GrabItemOnTransferStation();
            FakeForm1.Robot.PlaceItemInRack(RackWiTips);
            FakeForm1.TransStation.MoveStationIn();
            return true;
        }

      
        public bool MovePlateFromVictorToIncubatorWithLidOnTransferStation(int PlateSlot)
        {
            //first to make sure something is on the dang slider thing
            bool HASLID = false;
            if (FakeForm1.Incubator.CheckIfSomethingOnTransferOutStation())
            {
                HASLID = true;
                FakeForm1.TransStation.TurnOnVacuumAndLiftLid();
            }
            FakeForm1.TransStation.MoveStationOut();
            FakeForm1.Robot.MovePlateFromPlateReaderToIncubator();
            FakeForm1.TransStation.MoveStationIn();
            if (HASLID)
            {
                FakeForm1.TransStation.TurnOffVacuumAndReturnLid();
            }
            FakeForm1.Incubator.LoadPlate(PlateSlot);
            return true;
        }
        
        public override bool AttemptRecovery(InstrumentError Error)
        {
            return true;
        }
        public override bool CloseAndFreeUpResources()
        {
            return true;
        }
    }
}
