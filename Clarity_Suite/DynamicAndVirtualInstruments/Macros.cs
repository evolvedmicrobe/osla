using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Clarity
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
        [UserCallableMethod(RequiresInstrumentManager = true)]
        public bool MovePlateFromIncubatorToVictor(int PlateSlot, AdditionalMethodArguments eargs)
        {

            IncubatorServ Incubator = eargs.InstrumentCollection.ReturnInstrumentType<IncubatorServ>();
            TransferStation TransStation = eargs.InstrumentCollection.ReturnInstrumentType<TransferStation>();
            Twister Robot = eargs.InstrumentCollection.ReturnInstrumentType<Twister>();
            //First to unload the plate
            Incubator.UnloadPlate(PlateSlot);
            TransStation.TurnOnVacuumAndLiftLid();
            TransStation.MoveStationOut();
            Robot.MovePlateFromTransferStationToPlateReader(eargs);
            TransStation.MoveStationIn();
            TransStation.TurnOffVacuumAndReturnLid();
            return true;
        }
        /// <summary>
        /// Temporary method required to remotely fix the occasional error., do not regularly call
        /// </summary>
        /// <param name="toModify"></param>
        /// <param name="eargs"></param>
        /// <returns></returns>
        [UserCallableMethod(RequiresInstrumentManager=true)]
        public bool AttemptToReseatLid(Protocol toModify, AdditionalMethodArguments eargs)
        {
            IncubatorServ Incubator = eargs.InstrumentCollection.ReturnInstrumentType<IncubatorServ>();
            TransferStation TransStation = eargs.InstrumentCollection.ReturnInstrumentType<TransferStation>();
            Twister Robot = eargs.InstrumentCollection.ReturnInstrumentType<Twister>();

            TransStation.TurnOnVacuumAndLiftLid();
            TransStation.MoveStationOut();
            Robot.MovePlateFromTransferStationToPlateReader(eargs);
            TransStation.MoveStationIn();
            TransStation.TurnOffVacuumAndReturnLid();
            
            TransStation.TurnOnVacuumAndLiftLid();
            TransStation.MoveStationOut();
            Robot.MovePlateFromPlateReaderToIncubator();
            TransStation.MoveStationIn();
            TransStation.TurnOffVacuumAndReturnLid();
            return true;
        }
        [UserCallableMethod(RequiresInstrumentManager = true)]
        public bool MovePlateFromVictorToIncubatorWithLidOnTransferStation(int PlateSlot, AdditionalMethodArguments eargs)
        {

            IncubatorServ Incubator = eargs.InstrumentCollection.ReturnInstrumentType<IncubatorServ>();
            TransferStation TransStation = eargs.InstrumentCollection.ReturnInstrumentType<TransferStation>();
            Twister Robot = eargs.InstrumentCollection.ReturnInstrumentType<Twister>();
            //First to unload the plate
            //first to make sure something is on the dang slider thing
            bool HASLID = false;
            if (Incubator.CheckIfSomethingOnTransferOutStation())
            {
                HASLID = true;
                TransStation.TurnOnVacuumAndLiftLid();
            }
            TransStation.MoveStationOut();
            Robot.MovePlateFromPlateReaderToIncubator();
            TransStation.MoveStationIn();
            if (HASLID)
            {
                TransStation.TurnOffVacuumAndReturnLid();
            }
            Incubator.LoadPlate(PlateSlot);
            return true;
        }
        [UserCallableMethod()]
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
