using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace Fit_Growth_Curves
{
    public class ImportRobotData
    {
        static double[,] absDATA;
        static int plateSize = 96;
        static int ColNumber = 12;
        static int RowNumber = 8;
        static string[] IntToWell;//96 well version of int to name
        static double[] timeValues;//time values as a double
        static string[] IntToName;//this cell will map array integer to cell name;
        static DateTime[] acTimeValues;//time values as a datetime
        static double[,] VenusValues=null;
        static string Directory;
        public static string NameofTempFile = "CondensedForCurveFitter.csv";
        public static string NameofVenusFile = "VenusCondensedForCurveFitter.csv";
        public static void ChangeTo48WellPlates()
        {
            plateSize=48;
            ColNumber=8;
            RowNumber=6;
        }
        public static void ChangeTo96WellPlates()
        {
            plateSize = 96;
            ColNumber = 12;
            RowNumber = 8;
        }
        public static void DecideIf96Or48WellPlate(string FileLocations)
        {
            //takes a directory and attempts to determine if there is 96 or 48 well plate data in it
            DirectoryInfo DI = new DirectoryInfo(FileLocations);
            foreach (FileInfo FI in DI.GetFiles())
            {
                if (FI.Extension == ".txt")
                {
                    StreamReader SR = new StreamReader(FI.FullName);
                    string file = SR.ReadToEnd();
                    if (file.IndexOf("F09") > 0)//plate has F09, is 96 well
                    {
                        ChangeTo96WellPlates();
                    }
                    else
                    {
                        ChangeTo48WellPlates();
                    }
                    break;
                }
            }
        }
        public static void GetData(string FileLocations)
        {
            DecideIf96Or48WellPlate(FileLocations);
            SetIntToWell();
            string error = "";
            //first to create an array of values, I know there will be 48 columns in the second one,
            //and for now I am going to assume we will have 200 datapoints, which we will not!
            Directory = FileLocations;
            timeValues = new double[200];
            acTimeValues = new DateTime[200];
            absDATA = new double[200, plateSize];
            int currentRow = 0;
            DirectoryInfo DI = new DirectoryInfo(FileLocations);
            foreach (FileInfo FI in DI.GetFiles())
            {
                if (FI.Extension == ".txt")
                {
                    try
                    {
                        StreamReader SR = new StreamReader(FI.FullName);
                        SR.ReadLine();//skip the first line
                        string line;
                        for (int i = 0; i < plateSize; i++)
                        {
                            line = SR.ReadLine();
                            string[] splitit = line.Split('\t');
                            absDATA[currentRow, i] = Convert.ToDouble(splitit[5]);
                        }
                        //Now we should be on row 49, but the date/time data is on row 101 in the form below
                        //Measured on ....................... 3/26/2007 6:20:39 PM
                        bool Timefound = false;
                        string timeline = "";
                        while (!Timefound)
                        {
                            line = SR.ReadLine();
                            if (line.StartsWith("Measured on"))
                            {
                                timeline = line.Remove(0, 36);
                                Timefound = true;
                            }
                        }
                        error = timeline;
                        DateTime TIME = Convert.ToDateTime(timeline);
                        error = TIME.ToString();
                        acTimeValues[currentRow] = TIME;
                        currentRow++;
                    }
                    catch (Exception thrown)
                    {
                        Exception ex = new Exception("File " + FI.Name + " is screwed" + error);
                        throw ex;
                    }
                }
            }
            ConvertArrays();
            exportData();
        }
        /// <summary>
        /// This was the old working version of this file, updated to make the import faster recently
        /// </summary>
        /// <param name="FileLocations"></param>
        public static void GetExcelDataOldVersion(string FileLocations)
        {
            ChangeTo48WellPlates();
            //DecideIf96Or48WellPlate(FileLocations);
            SetIntToWell();
            string error = "";
            //first to create an array of values, I know there will be 48 columns in the second one,
            //and for now I am going to assume we will have 200 datapoints, which we will not!
            Directory = FileLocations;
            timeValues = new double[300];
            acTimeValues = new DateTime[300];
            absDATA = new double[300, plateSize];

            int currentRow = 0;
            DirectoryInfo DI = new DirectoryInfo(FileLocations);
            ApplicationClass app=null;
            try
            {
                app = new ApplicationClass();               
                foreach (FileInfo FI in DI.GetFiles())
                {
                    if (FI.Extension == ".xls")
                    {                        
                        Workbook workBook = app.Workbooks.Open(FI.FullName,
                            0,
                            true,
                            5,
                            "",
                            "",
                            true,
                            XlPlatform.xlWindows, "", false, false, 0, true, 1, 0);
                        Worksheet workSheet = (Worksheet)workBook.Worksheets[1];
                        for (int i = 0; i < 48; i++)
                        {
                            var Cell = (Range)workSheet.Cells[i+2, 6];
                            double value = (double)Cell.Value2;
                            absDATA[currentRow, i] = value;
                        }
                        bool Timefound = false;
                        string timeline = "";
                        workSheet = (Worksheet)workBook.Worksheets[3];
                        var Cell2 = (Range)workSheet.Cells[39, 1];
                        timeline = Cell2.Value2 as string;
                        timeline = timeline.Remove(0, 36);
                        DateTime TIME = Convert.ToDateTime(timeline);
                        acTimeValues[currentRow] = TIME;
                        currentRow++;
                        app.Workbooks.Close();
                    }
                }
                ConvertArrays();
                exportData();
            }
            catch (Exception thrown)
            {
                //Exception ex = new Exception("File " + FI.Name + " is screwed" + error);
                //throw ex;
                throw thrown;
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
            }
        }
        public static void GetExcelData(string FileLocations)
        {
            ChangeTo48WellPlates();
            //DecideIf96Or48WellPlate(FileLocations);
            SetIntToWell();
          
            //first to create an array of values, I know there will be 48 columns in the second one,
            //and for now I am going to assume we will have 200 datapoints, which we will not!
            Directory = FileLocations;
            timeValues = new double[300];
            acTimeValues = new DateTime[300];
            absDATA = new double[300, plateSize];
            int currentRow = 0;
            DirectoryInfo DI = new DirectoryInfo(FileLocations);
            ApplicationClass app = null;
            try
            {
                bool hasVenus = false;
                app = new ApplicationClass();
                foreach (FileInfo FI in DI.GetFiles())
                {
                    if (FI.Extension == ".xls")
                    {
                        Workbook workBook = app.Workbooks.Open(FI.FullName,
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
                        int numCols = valueArray.GetUpperBound(1);                        
                        if (currentRow==0 && numCols > 6)
                        {
                            if (VenusValues == null) { VenusValues = new double[300,plateSize]; }
                            hasVenus = true;
                        }                        
                        for (int i = 0; i < 48; i++)
                        {
                            var Cell = valueArray[i + 2, 6];
                            double value = (double)Cell;
                            absDATA[currentRow, i] = value;
                            if (hasVenus)
                            {
                                Cell = valueArray[i + 2, 8];
                                VenusValues[currentRow, i] = Convert.ToDouble(Cell);                               
                                
                            }                            
                        }
                        ///NEW CODE ADDED BELOW
                        string timeline = "";
                        workSheet = (Worksheet)workBook.Worksheets[3];
                        Range excelRange2 = workSheet.UsedRange;
                        object[,] DescriptArray = (object[,])excelRange2.get_Value(XlRangeValueDataType.xlRangeValueDefault);
                        int TotalSize = DescriptArray.GetUpperBound(0);
                        DateTime testTime = DateTime.Now;
                        DateTime TIME = testTime;
                        for (int i = 0; i < TotalSize; i++)
                        {
                            timeline = (string)DescriptArray[i + 1, 1];
                            if (timeline != null && timeline.StartsWith("Measured on ..."))
                            {
                                timeline = timeline.Remove(0, 36);
                                TIME = Convert.ToDateTime(timeline);
                                break;
                            }
                        }
                        if (TIME == testTime) throw new Exception("No time found in file");

                        ///END NEW CODE
                        //bool Timefound = false;
                        //var Cell2 = (Range)workSheet.Cells[39, 1];
                        //timeline = Cell2.Value2 as string;
                        // timeline = timeline.Remove(0, 36);
                        //DateTime TIME = Convert.ToDateTime(timeline);
                        acTimeValues[currentRow] = TIME;
                        currentRow++;
                        app.Workbooks.Close();
                    }
                }
                ConvertArrays();
                exportData();
            }
            catch (Exception thrown)
            {
                //Exception ex = new Exception("File " + FI.Name + " is screwed" ,thrown);
                //throw ex;
                throw thrown;
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
            }
        }
        
        private static void ConvertArrays()
        {
            //This method takes the oversized arrays initialized during data collection and trims them down.
            int EndRow = 0;
            while (true)
            {
                if (absDATA[EndRow, 1] == 0) { break; }
                EndRow++;
            }
            Array.Resize<DateTime>(ref acTimeValues, EndRow);
            Array.Resize(ref timeValues, EndRow);
            //copy data into new smaller arrays, probably not efficent here
            //DateTime[] acTimeTemp = new DateTime[EndRow];
           // double[] timeTemp = new double[EndRow];
            double[,] absDATAtemp = new double[EndRow, plateSize];
            double[,] VenusTemp = null;
            if(VenusValues!=null) VenusTemp= new double[EndRow, plateSize];
            for (int i = 0; i < EndRow; i++)
            {
                //timeTemp[i] = timeValues[i];
               // acTimeTemp[i] = acTimeValues[i];
                for (int r = 0; r < plateSize; r++)
                {
                    absDATAtemp[i, r] = absDATA[i, r];
                    if (VenusValues != null)
                        VenusTemp[i, r] = VenusValues[i, r];
                }
            }
            absDATA = absDATAtemp;
            VenusValues = VenusTemp;
            //acTimeValues = acTimeTemp;
           // timeValues = timeTemp;
        }
        private static void SetIntToWell()
        {
            IntToWell = new string[plateSize];
            string Rows = "ABCDEFGH";
            for (int i = 0; i < plateSize; i++)
            {
                int ColPos = (i % ColNumber) + 1;
                int RowPos = Convert.ToInt32((i / ColNumber));
                string toSet = Rows[RowPos] + ColPos.ToString();
                IntToWell[i] = toSet;

            }
        }
        private static void exportData()
        {
            StreamWriter SW = new StreamWriter(Directory+"\\" + NameofTempFile);
            SW.Write("Time,");
            string nextline = "";
            for (int i = 0; i < plateSize; i++)
            {
                nextline += IntToWell[i] + ",";
            }
            nextline = nextline.Trim(',');
            SW.Write(nextline + "\n");
            for (int i = 0; i < timeValues.Length; i++)
            {
                SW.Write(acTimeValues[i].ToString() + ",");
                string lastline = "";
                for (int j = 0; j < absDATA.GetLength(1); j++)
                {
                    lastline += absDATA[i, j].ToString() + ",";
                }
                lastline = lastline.TrimEnd(',');
                SW.Write(lastline + "\n");
            }
            SW.Close();
            if (VenusValues != null)
            {
                SW = new StreamWriter(Directory + "\\" + NameofVenusFile);
                SW.Write("Time,");
                nextline = "";
                for (int i = 0; i < plateSize; i++)
                {
                    nextline += IntToWell[i] + ",";
                }
                nextline = nextline.Trim(',');
                SW.Write(nextline + "\n");
                for (int i = 0; i < timeValues.Length; i++)
                {
                    SW.Write(acTimeValues[i].ToString() + ",");
                    string lastline = "";
                    for (int j = 0; j < VenusValues.GetLength(1); j++)
                    {
                        lastline += VenusValues[i, j].ToString() + ",";
                    }
                    lastline = lastline.TrimEnd(',');
                    SW.Write(lastline + "\n");
                }
                SW.Close();
            }
            if (VenusValues != null)
            {
                FitterForm newF = new FitterForm();
                newF.Show();
                newF.LoadFile(Directory + "\\" + NameofVenusFile);
            }
            VenusValues = null;
            absDATA = null;
            acTimeValues = null;
            timeValues = null;
            
        }
    }
}
