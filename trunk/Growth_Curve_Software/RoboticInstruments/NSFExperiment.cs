using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace Growth_Curve_Software
{
    public class NSFExperiment : VirtualInstrument
    {
        
        public int pVenusProtocolID = 2000105;
        public int VenusProtocolID { get { return pVenusProtocolID; } set { pVenusProtocolID = value; } }
        private int pOD600ProtocolID = 2000095;
        public int OD600ProtocolID { get { return pOD600ProtocolID; } set { pOD600ProtocolID = value; } }
        private double pMedianODToStartMeasurement=.14;
        public double MedianODToStartMeasurement
        {
            get { return pMedianODToStartMeasurement; }
            set { pMedianODToStartMeasurement = value; }
        }

        private int pMinBetweenReads = 50;
        public int MinBetweenReads
        { get { return pMinBetweenReads; } set { pMinBetweenReads = value; } }

        private int ExpSlot;
        private StaticProtocolItem VenusReadInstruction, ODReadInstruction, toVictorInstruction,awayFromVictorInstruction;
        private StaticProtocolItem ReturnInstruction;
        DelayTime DT;
        private string OD_OutDirec="No Name";
        public override string Name
        {
            get
            {
                return "NSFExperiment";
            }
        }
        public NSFExperiment():base()
        {
            this.ArgumentsTakeProtocol = true;
            this.StatusOK = true;
        }
        
        private void CreateProtocolItems(string ExpName,int Slot)
        {
            ExpSlot = Slot;
            toVictorInstruction = new StaticProtocolItem();
            toVictorInstruction.InstrumentName = "Macros";
            toVictorInstruction.Parameters = new object[1] { Slot };
            toVictorInstruction.MethodName = "MovePlateFromIncubatorToVictor";

            OD_OutDirec=ExpName + "_" + Slot.ToString();

            

            ODReadInstruction = new StaticProtocolItem();
            ODReadInstruction.MethodName = "ReadPlate2";
            ODReadInstruction.InstrumentName = "Plate_Reader";
            ODReadInstruction.Parameters = new object[2] {OD_OutDirec, OD600ProtocolID };

            VenusReadInstruction = ODReadInstruction.Clone();
            //VenusReadInstruction = new StaticProtocolItem();
            //VenusReadInstruction.MethodName = "ReadPlate2";
            //VenusReadInstruction.InstrumentName = "Plate_Reader";
            //VenusReadInstruction.Parameters = new object[2] { "Ve_" + OD_OutDirec, VenusProtocolID };
            VenusReadInstruction.Parameters[1] = VenusProtocolID;

            awayFromVictorInstruction = toVictorInstruction.Clone();
            awayFromVictorInstruction.MethodName = "MovePlateFromVictorToIncubatorWithLidOnTransferStation";

            DT = new DelayTime();
            DT.minutes = MinBetweenReads;

            ReturnInstruction = new StaticProtocolItem();
            ReturnInstruction.MethodName = "ModifyGrowthProtocol";
            ReturnInstruction.InstrumentName = this.Name;
            //Last item is passed in by the parser
            ReturnInstruction.Parameters = new object[3] { ExpName, Slot, null };
            
        }
        [UserCallableMethod(RequiresCurrentProtocol=true)]
        public bool CreateProtocol(string ExpName, int Slot,AdditionalMethodArguments eargs)
        {

            Protocol toModify = eargs.CallingProtocol;
            CreateProtocolItems(ExpName, Slot);
            toModify.ProtocolName = OD_OutDirec;
            toModify.Instructions.Clear();
            toModify.Instructions.Add(toVictorInstruction);
            toModify.Instructions.Add(ODReadInstruction);
            toModify.Instructions.Add(awayFromVictorInstruction);
            toModify.Instructions.Add(ReturnInstruction);
            toModify.Instructions.Add(DT);
            toModify.NextItemToRun = 0;
            return true;
        }
        [UserCallableMethod(RequiresCurrentProtocol = true)]
        public bool ModifyGrowthProtocol(string ExpName, int Slot, AdditionalMethodArguments eargs)
        {
            try
            {
                Protocol toModify = eargs.CallingProtocol;
                CreateProtocolItems(ExpName, Slot);
                toModify.Instructions.Clear();
                toModify.Instructions.Add(DT);
                toModify.Instructions.Add(toVictorInstruction);
                
                string fname;
                try
                {
                    fname = ReturnLastFileName(@"C:\Growth_Curve_Data\\" + OD_OutDirec);
                }
                catch (Exception thrown)
                {
                    throw new Exception("Could not find most recent file. " + thrown.Message);
                }
                double medVal;
                try
                {
                    medVal = FindLastMedianReading(fname);
                }
                catch (Exception thrown)
                {
                    throw new Exception("Could not detemine median reading. " + thrown.Message);
                }
                //move from incubator to platereader           
                if (medVal > pMedianODToStartMeasurement)
                {
                    toModify.Instructions.Add(VenusReadInstruction);
                }
                else { toModify.Instructions.Add(ODReadInstruction); }
                toModify.Instructions.Add(awayFromVictorInstruction);
                toModify.Instructions.Add(ReturnInstruction);
                toModify.NextItemToRun = 0;
                
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("General Problem:" +thrown.Message,true,this);
            }
                return true;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        private string ReturnLastFileName(string DirectoryName)
        {
            DirectoryInfo DI = new DirectoryInfo(DirectoryName);
            FileInfo[] FIs = DI.GetFiles();
            var valid = from x in FIs where x.Extension == ".xls" orderby x.CreationTime select x;
            if (valid.Count() < 1)
            {
                BaseInstrumentClass BI = (BaseInstrumentClass)this;
                throw new InstrumentError("There was no file in the director: "+DirectoryName,true,BI);
            }
            var res = valid.Last();
            return res.FullName;

        }
        private static double FindLastMedianReading(string FileName)
        {
            ApplicationClass app = null;
            double[] absData = new double[48];
            try
            {
                app = new ApplicationClass();
           
                        Workbook workBook = app.Workbooks.Open(FileName,
                            0,
                            true,
                            5,
                            "",
                            "",
                            true,
                            XlPlatform.xlWindows, "", false, false, 0, true, 1, 0);

                        Worksheet workSheet = (Worksheet)workBook.Worksheets[1];
                        Range excelRange = workSheet.UsedRange;
                        object[,] valueArray = (object[,])excelRange.get_Value(
                            XlRangeValueDataType.xlRangeValueDefault);
                        for (int i = 0; i < 48; i++)
                        {
                            var Cell = valueArray[i + 2, 6];
                            double value = (double)Cell;
                            absData[i] = value;
                        }
                        app.Workbooks.Close();
                        
            }
            catch (Exception thrown)
            {
                //Exception ex = new Exception("File " + FI.Name + " is screwed" + error);
                //throw ex;
                throw new InstrumentError(thrown.Message);
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
            }
            Array.Sort(absData);
            double median=(absData[24]+absData[25])/2.0;
            return median;
        }
    }
}


