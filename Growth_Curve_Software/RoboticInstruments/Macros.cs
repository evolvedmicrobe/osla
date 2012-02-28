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
            Form1.Incubator.UnloadPlate(PlateSlot);
            Form1.TransStation.TurnOnVacuumAndLiftLid();
            Form1.TransStation.MoveStationOut();
            Form1.Robot.MovePlateFromTransferStationToPlateReader();
            Form1.TransStation.MoveStationIn();
            Form1.TransStation.TurnOffVacuumAndReturnLid();
            return true;
        }
        public bool AttemptToReseatLid()
        {
            Form1.TransStation.TurnOnVacuumAndLiftLid();
            Form1.TransStation.MoveStationOut();
            Form1.Robot.MovePlateFromTransferStationToPlateReader();
            Form1.TransStation.MoveStationIn();
            Form1.TransStation.TurnOffVacuumAndReturnLid();
            
            Form1.TransStation.TurnOnVacuumAndLiftLid();
            Form1.TransStation.MoveStationOut();
            Form1.Robot.MovePlateFromPlateReaderToIncubator();
            Form1.TransStation.MoveStationIn();
            Form1.TransStation.TurnOffVacuumAndReturnLid();
            return true;
        }
        public bool RackToSciclone(int RackPosition,int SciClonePosition)
        {
            //this method will take a new plate out of the rack, and place its lid in the 3rd 
            //Sciclone spot and the plate in the second
            Form1.Robot.MoveTwisterToSafePosition();
            Form1.Robot.GrabItemFromRack(RackPosition);
            Form1.Robot.PlaceItemInSciclonePosition(SciClonePosition);
            return true;
        }
        public bool IncPlateToSciClone(int IncPosition,int SciClonePos)
        {
            //this method unloads a plate, and moves it to the sciclone shaker, discarding the lid as it goes
            Form1.Incubator.UnloadPlate(IncPosition);
            Form1.TransStation.MoveStationOut();
            Form1.Robot.GrabItemOnTransferStation();
            Form1.Robot.PlaceItemInSciclonePosition(SciClonePos);
            Form1.TransStation.MoveStationIn();
            Form1.Robot.MoveTwisterToSafePosition();
            return true;
        }
        public bool ScicloneToIncubator(int IncPos,int SciPos)
        {
            //This method will move a plate from position 2, with its lid in position 3, to the incubator
            if (Form1.Incubator.CheckIfSomethingOnTransferOutStation())
            {
                throw new InstrumentError("Their is something on the transfer out station", true, this);
            }
            //otherwise move ahead
            Form1.TransStation.MoveStationOut();
            Form1.Robot.GetItemInSciclonePosition(SciPos);
            Form1.Robot.PlaceItemOnTransferStation();
            Form1.TransStation.MoveStationIn();
            Form1.Incubator.LoadPlate(IncPos);
            return true;
        }
        public bool SciCloneToRack(int SciClonePosition, int RackNumber)
        {
            Form1.Robot.GetItemInSciclonePosition(SciClonePosition);
            Form1.Robot.PlaceItemInRack(RackNumber);
            return true;
        }
        public bool PlaceNewTipsInSciclone(int RackWiTips)
        {
            Form1.Robot.HomeAllAxes();
            Form1.Robot.GrabItemFromRack(RackWiTips);
            Form1.TransStation.MoveStationOut();
            if (Form1.Incubator.CheckIfSomethingOnTransferOutStation())
            {
                throw new InstrumentError("Something is already on the transfer out station, cannot place tip lid there", true, this);
            }
            Form1.Robot.PlaceItemOnTransferStation();
            Form1.Robot.GrabItemFromRack(RackWiTips);
            Form1.Robot.PlaceItemInSciclonePosition(1);
            Form1.Robot.GrabItemOnTransferStation();
            Form1.Robot.PlaceItemInRack(RackWiTips);
            Form1.TransStation.MoveStationIn();
            return true;
        }
        public bool PlaceTwoSetsOfNewTipsInSciclone(int RackWiTips)
        {
            PlaceNewTipsInSciclone(RackWiTips);
            Form1.LiquidHandler.RunMethod("MoveTipsToBox2", "none");
            PlaceNewTipsInSciclone(RackWiTips);
            return true;
        }
        public bool RemoveTwoSetsOfTipLidsFromSciclone(int DumpRack)
        {
            Form1.Robot.GetItemInSciclonePosition(1);
            Form1.Robot.PlaceItemInRack(DumpRack);
            Form1.LiquidHandler.RunMethod("MoveTipLid2ToBox1", "none");
            Form1.Robot.GetItemInSciclonePosition(1);
            Form1.Robot.PlaceItemInRack(DumpRack);
            return true;

        }
        public bool MovePlateFromVictorToIncubatorWithLidOnTransferStation(int PlateSlot)
        {
            //first to make sure something is on the dang slider thing
            bool HASLID = false;
            if (Form1.Incubator.CheckIfSomethingOnTransferOutStation())
            {
                HASLID = true;
                Form1.TransStation.TurnOnVacuumAndLiftLid();
            }
            Form1.TransStation.MoveStationOut();
            Form1.Robot.MovePlateFromPlateReaderToIncubator();
            Form1.TransStation.MoveStationIn();
            if (HASLID)
            {
                Form1.TransStation.TurnOffVacuumAndReturnLid();
            }
            Form1.Incubator.LoadPlate(PlateSlot);
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
