using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Growth_Curve_Software
{
    public class EvoInstructs :BaseInstrumentClass
    {
        int MaxZ8Vol = 100;
        static int[] Big48WellColumnTo96WellColumn = {-999,1,3,4,6,7,9,10,12};//give the corresponding position of  96 well plate as a 48 well plate
        static int CurNewTipsIndex=1;//1,5,9 or 
        static int RackToDumpOldBoxes = 2;
        static int RackWithNewTips = 5;
        enum TipsToLoad {All_For_48,Only_2_5_8,Only_1_4_7};
        bool TipsLoaded = true;
        public EvoInstructs()
        {
            StatusOK = true;
        }
        public override string Name
        {
            get { return "Evolution_Protocols"; }
        }

        private int[] ConvertTotalVolumeToSubVolumes(int Volume)
        {
            if (Volume <= MaxZ8Vol)
            {
                return new int[] { Volume };
            }
            else
            {
                int totalTransfers = (int)Math.Ceiling(Volume / (double)MaxZ8Vol);
                int[] VolumesInTransfers = new int[totalTransfers];
                for(int i=0;i<totalTransfers;i++)
                {VolumesInTransfers[i]=MaxZ8Vol;}
                int Remainder = Volume % 100;
                if (Remainder > 0) 
                    VolumesInTransfers[totalTransfers]=Remainder;
                //make sure a decent amount is moved
                if (Remainder < 30 && Remainder>0)
                {
                    int newVolume = Convert.ToInt32((MaxZ8Vol + Remainder) / (double)MaxZ8Vol);
                    VolumesInTransfers[totalTransfers] = newVolume;
                    VolumesInTransfers[totalTransfers - 1] = newVolume;
                }
                return VolumesInTransfers;
            }
        }
        public void SetRackWithNewTips(int RackNumber)
        {
            if (RackNumber > 6 || RackNumber < 0)
                throw new InstrumentError(RackNumber.ToString() + " is not a valid rack.",true,this);
            RackWithNewTips = RackNumber;
        }
        public void DoNothing()
        {
            
        }
        public void SetTipsNotInSciClone()
        {
            CurNewTipsIndex = 13;
            TipsLoaded = false;
        }
        public void SetNewTipsIndex(int Column)
        {
            if (Column % 4 != 0)
            {
                throw new InstrumentError("The column to grab tips from must be divisible by 4!!",true,this);

            }
            CurNewTipsIndex = Column;
            TipsLoaded = true;
        }
        private void LoadZ8Tips(TipsToLoad loadOption)
        {
            string LoadSetting="";
            if(loadOption==TipsToLoad.All_For_48)
            {
                LoadSetting="0";
            }
            else if(loadOption==TipsToLoad.Only_1_4_7)
            {
                LoadSetting="1";
            }
            else if (loadOption==TipsToLoad.Only_2_5_8)
            {
                LoadSetting="2";
            }
            //now combine with the column argument
            LoadSetting = CurNewTipsIndex.ToString() + "," + LoadSetting;
            CurNewTipsIndex++;
            Form1.LiquidHandler.RunMethod("LoadZ8_Tips", LoadSetting);
            
        }
        public void SetDumpRack(int RackNumber)
        {
            if (RackNumber > 6 || RackNumber < 0)
                throw new InstrumentError(RackNumber.ToString() + " is not a valid rack.",true,this);
            RackToDumpOldBoxes = RackNumber;
        }
        private void loadTipsIfNecessary()
        {
            if (CurNewTipsIndex > 12)
            {
                if (TipsLoaded)
                {
                    Form1.ScriptsLibrary.SciCloneToRack(1, RackToDumpOldBoxes);
                    TipsLoaded = false;
                }
                Form1.ScriptsLibrary.PlaceNewTipsInSciclone(RackWithNewTips);
                CurNewTipsIndex = 1;
                TipsLoaded = true;
            }
        }

        public void PlaceFreshMediaIn48WellPlate()
        {
            int Volume = 500;
            ////this method moves new media to a 48 well plate in position 3 on the sciclone, it
            ////assumes the lid is off the old media
            ////remove 48 well plate lids
            //loadTipsIfNecessary();
            //Form1.LiquidHandler.RunMethod("Remove48wellLid", "True");
            //Form1.LiquidHandler.RunMethod("Remove48wellLid", "False");
            //remove reservoir lids
            Form1.LiquidHandler.RunMethod("RemoveReservoirLid","none");
            //load z8 tips for 6 positions


            LoadZ8Tips(TipsToLoad.All_For_48);
            //now to stir the media
            Form1.LiquidHandler.RunMethod("Scrape_Media_With_Z8","none");

            //z8 can only move 100 ul at a time, so to compensate, going to split the amount of 
            int[] VolumesToMove = ConvertTotalVolumeToSubVolumes(Volume);
            for (int col48 = 1; col48 <= 8; col48++)
            {
                //get the 96 well column that corresponds to this one
                int ColPos = Big48WellColumnTo96WellColumn[col48];
                foreach (int volToTransfer in VolumesToMove)
                {
                    Form1.LiquidHandler.RunMethod("Fill48WellMedia", volToTransfer.ToString() + "," + ColPos.ToString());
                }
            }
            Form1.LiquidHandler.RunMethod("Dump_Z8_Tips","none");
  
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
