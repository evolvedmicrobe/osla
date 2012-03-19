using System.IO;
using System;
using System.Linq;

namespace Fit_Growth_Curves
{

    public partial class Form1
    {
        private void ExportData(string FullFileName)
        {
            bool LagData = false;
            //FullFileName = "C:\\FullName.csv";
            StreamWriter SW = new StreamWriter(FullFileName);
            SW.WriteLine("Fitted Data Results");
            SW.WriteLine("Name, Doubling Time(Hrs),Growth Rate, How Determined?,NumPoints,R2,RMSE, Maximum GrowthRate,MaxOD,Notes,Linear-Fit Slope,Reduction in absolute error from ExpFit,LagTime,Reduction in Sum of Squares from Exp Fit,TimeTill_OD_0.02");
            string TitleLine = "Time,";//this will hold the titles for everything below
            foreach (GrowthData GR in lstGrowthCurves.Items)
            {
                if (GR.FirstReadingisODAtTimeZero)
                {
                    LagData = true;
                }
                TitleLine += GR.ToString() + " OD," + "Flag,";                
                string newline=GR.ToString()+",";
                double ActualGrowth = Math.Log(2) / GR.GrowthRate.GrowthRate;
                if (GR.ValidDataSet)
                {
                    newline += ActualGrowth.ToString("n5") + "," + GR.GrowthRate.GrowthRate.ToString() + "," + GR.GrowthRate.FittingUsed + "," + GR.GrowthRate.NumPoints + "," + GR.GrowthRate.R2.ToString("n4") + ","
                    + GR.GrowthRate.RMSE.ToString("n5") + "," + GR.MaxGrowthRate.MaxGrowthRate.ToString("n5") + "," +GR.ODValues.Max().ToString("n4")+","+ GR.GrowthRate.Notes;
                    if (GR.LinearModelFitted && GR.ExpModelFitted)
                    {
                        double dif = GR.LinFit.AbsError - GR.ExpFit.AbsError;
                        double dif2 = GR.LinFit.calculateResidualSumofSquares() - GR.ExpFit.calculateResidualSumofSquares();
                        newline += "," + GR.LinFit.Parameters[1].ToString("n5") + "," + dif.ToString("n5")+","+GR.LagTime.ToString("n5")+","+dif2.ToString("n5")+",";
                    }//+","+RMSEdiff.ToString("n5")+","+GR.LinFit.RMSE.ToString()+","+GR.ExpFit.RMSE.ToString(); }//report the linear fitted slope if possible
                    else { newline += ",No Exp Fit to Compare Against,"; }
                    newline += GR.HoursTillODReached(.02).ToString() + ",";
                }
                else { newline += ",,Weird Data:Blank??,,,,,,,,,,,,"; }
                SW.WriteLine(newline);
            }
            //Below assumes the time is the same for all of them
            string Intermissionline = "Complete Data Listing Below";
            if (LagData) { Intermissionline += "-Initial OD Present"; };
            SW.WriteLine(Intermissionline);
            SW.WriteLine(TitleLine);
            
            foreach (DateTime DT in DateTimesinFile)
            {
                string line=DT.ToString()+",";
                foreach (GrowthData GR in lstGrowthCurves.Items)
                {
                    int indexPos=-1;
                    if (SimpleFunctions.ValueInArray(GR.timeValues, DT,ref indexPos))
                    {
                        //decide if this timepoint was included
                        line += GR.ODValues[indexPos].ToString()+",";
                        double DateX = GR.TimeValuesDecimal[indexPos];
                        if (GR.FittedXValues!=null && SimpleFunctions.ValueInArray(GR.FittedXValues, DateX))//now decide if it made it into the fit
                        {
                            line += "0,";
                        }
                        else { line += "1,"; }
                    }
                    else { line += "-999,-999,"; }
                }
                SW.WriteLine(line);               
            }
            SW.Close();
        }
    }
}