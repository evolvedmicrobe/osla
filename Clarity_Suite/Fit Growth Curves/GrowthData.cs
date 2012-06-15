using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using System.Data;

namespace Fit_Growth_Curves
{
    /// <summary>
    /// This class represents one set of growth data
    /// </summary>
    public class GrowthData
    {
        //This is some code for database interface
        private int[] pDataBaseRecordIDs;
        public bool DataFromDatabase
        {
            get { return pDataBaseRecordIDs != null; }
        }
        public int[] data_DatabaseIDs
        {
            get
            {
                if (pDataBaseRecordIDs != null)
                { return pDataBaseRecordIDs.ToArray(); }
                else { return null; }
            }
        }
        public bool IsIsolateData;
        //The experiment ID in the database
        private int pdb_ExperimentID;
        public int db_ExperimentID
        { get { return pdb_ExperimentID; } }
        public bool db_MaxOD_Flag;
        public bool db_GrowthRateFLAG;
        public const bool USE_EXPONENTIAL_FITTING = true;
        public const double DEFAULT_MAX_GROWTH_RATE = 10000;
        public bool FirstReadingisODAtTimeZero;//This indicates whether or not I should calculate lag values
        private double lagtime = Double.NaN;
        public double LagTime
        {
            get
            {
                return lagtime;
            }
        }
        public void SetLagTime()
        {
            //This model will calculate the lag time assuming that the 
            //inital od reading represents the time zero.  This reading must be 
            //caluclated from the final od of the old culture and the known dilution ratio.
            if (FirstReadingisODAtTimeZero && USE_EXPONENTIAL_FITTING && ExpModelFitted)
            {
                double initialOD = ODValues[0];//Assume that the zero value is the first;
                double Timezero = timevaluesdecimal[0];
                double GrowthRate = ExpFit.GrowthRate;
                double Constant = ExpFit.InitialPopSize;
                //Now to back calculate
                double TimeGrowthStarts = Math.Log((initialOD / Constant)) / GrowthRate;
                lagtime = TimeGrowthStarts - Timezero;
                //Now we know that 
            }
            else if (FirstReadingisODAtTimeZero && LinearModelFitted)
            {
                double initialOD = ODValues[0];//Assume that the zero value is the first;
                double Timezero = timevaluesdecimal[0];
                double GrowthRate = LinFit.Slope;
                double Constant = Math.Exp(LinFit.Intercept);
                //Now to back calculate
                double TimeGrowthStarts = Math.Log((initialOD / Constant)) / GrowthRate;
                lagtime = TimeGrowthStarts - Timezero;
                //Now we know that 
            }

        }
        public struct GrowthRateInUse
        {
            public double GrowthRate;//=0;
            public string FittingUsed;//="Not Fitted";
            public int NumPoints;// = -999;
            public double R2;
            public double RMSE;
            public string Notes;
            public AbstractFitter FitterUsed;
            public double DoublingTime
            {
                get { return Math.Log(2) / GrowthRate; }
            }
        }
        public struct MaximumGrowthRate
        {
            public double MaxGrowthRate;
            public double[] xvals;
            public double[] yvals;
        }
        private DateTime[] ptimeValues;
        public DateTime[] timeValues
        {
            get { return ptimeValues; }
            set
            {
                ptimeValues = value;
                timevaluesdecimal = new double[value.Length];
                BaseTime = value[0];
                for (int i = 0; i < value.Length; i++)
                {
                    //Now to set the time in minutes
                    timevaluesdecimal[i] = ConvertDateTimeToDouble(value[i]);//store it as a double in ticks               
                }
            }
        }
        public LinearFit LinFit;
        public ExponentialFit ExpFit;
        public MaximumGrowthRate MaxGrowthRate;
        public GrowthRateInUse GrowthRate;
        private DateTime BaseTime;
        /// <summary>
        /// Uses the parameters from the exponential fit to determine when a specific Y value is reached. 
        /// </summary>
        /// <param name="ODValue"></param>
        /// <returns></returns>
        public double? HoursTillODReached(double ODValue)
        {
            if (pODValues.Max() < ODValue || ExpFit == null)
            {
                if (this.DataFromDatabase)
                { return -999.0; }
                else { return null; }
            }
            else
            {
                return ExpFit.GetXValueAtYValue(ODValue);
            }
        }
        private double[] timevaluesdecimal;
        public double[] TimeValuesDecimal//in hours
        {
            get { return timevaluesdecimal; }
        }
        public double[] FittedXValues;//NOTE THAT THESE ARE THE EXPERIMENTAL VALUES
        public double[] FittedLogYValues
        {
            get
            {
                return pFittedLogYValues;
            }
            set
            {
                pFittedLogYValues = value;
                createNonLogFitValues();
            }
        }
        private double[] pFittedLogYValues;
        private double[] pFittedYValues;
        public double[] FittedYValues
        {
            get { return pFittedYValues; }
        }
        private double[] pODValues;
        public double[] ODValues
        {
            get { return pODValues; }
            set
            {
                pODValues = value;
                pLogODValues = new double[value.Length];
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        //first to set the log values
                        if (value[i] > 0)
                        {
                            pLogODValues[i] = Math.Log(value[i]);
                        }
                        else { pLogODValues[i] = double.NaN; }
                    }
                }
            }
        }
        public double HighestRealODValue
        {
            get
            {
                double res = -1;
                foreach (double x in ODValues) if (x > res && SimpleFunctions.IsARealNumber(x)) res = x;
                return res;
            }
        }
        public double[] LogODValues
        {
            get { return pLogODValues; }
        }
        /// <summary>
        /// Should never be set except by OD values
        /// </summary>
        private double[] pLogODValues;

        double MaxPossibleGrowthRate = 1.5;//This says that nothing can double faster then 1.5 hours
        public string DataSetName = "Not initialized";
        //public string FitWarningMessage = "";
        public double[] XvaluesForSlope;
        public double[] SlopeChange;//holds the doubling time between each pair of points, poorly named variable
        public bool ValidDataSet;//determines if the data is valid for fitting or not
        public bool LinearModelFitted
        {
            get
            {
                if (LinFit != null)
                { return LinFit.SuccessfulFit; }
                else { return false; }
            }
        }
        public bool ExpModelFitted
        {
            get
            {
                if (ExpFit != null)
                {
                    return ExpFit.SuccessfulFit;
                }
                else { return false; }
            }
        }
        public override string ToString()
        {
            return DataSetName;
        }
        public GrowthData(string name, DateTime[] timeValues, double[] odvalues, bool HasInitialOD, int[] DatabaseIDs = null, int ExperimentID = 0, bool isIsolateData = false)
        {
            this.pDataBaseRecordIDs=DatabaseIDs;
            this.pdb_ExperimentID = ExperimentID;
            this.IsIsolateData = isIsolateData;
            FirstReadingisODAtTimeZero = HasInitialOD;
            //If these need to be converted to log do it
            if (odvalues.Length < 2 || (timeValues.Length != odvalues.Length)) { throw new Exception("Bad Data in " + name + " Row"); }
            DataSetName = name;
            this.timeValues = timeValues;
            ODValues = odvalues;
            CreateSlopeData();
            calculateMaxGrowthRate();
            DetermineDataToFit(timevaluesdecimal, LogODValues);
            GrowthRate.Notes = "Computer Picked Data";
        }
        public GrowthData(string name, DateTime[] timeValues, double[] odvalues, string Notes, int[] valueFitted, bool LagData)
        {
            FirstReadingisODAtTimeZero = LagData;//If the first reading is an OD reading, we know to calculate lag.
            GrowthRate.Notes = Notes;
            if (odvalues.Length < 2 || (timeValues.Length != odvalues.Length)) { throw new Exception("Bad Data in " + name + " Row"); }
            DataSetName = name;
            this.timeValues = timeValues;
            ODValues = odvalues;
            CreateSlopeData();
            calculateMaxGrowthRate();
            List<int> FittedIndexes = new List<int>();
            for (int i = 0; i < timeValues.Length; i++)
            {
                if (valueFitted[i] != 1)
                {
                    FittedIndexes.Add(i);
                }
            }
            if (FittedIndexes.Count > 1)
            {
                SetFittedRangeFromIndexes(FittedIndexes);
            }
            else
            {
                FittedXValues = null;
                FittedLogYValues = null;
            }
            FitData();
        }
        public GrowthData(string name, DateTime[] timeValues, double[] odvalues, int[] valueFitted, int[] DatabaseIDs,int ExperimentID,bool isIsolateData=false) :
            this(name, timeValues, odvalues, "DatabaseLoaded", valueFitted, false)
        {
            this.pdb_ExperimentID = ExperimentID;
            this.pDataBaseRecordIDs = DatabaseIDs;
            this.IsIsolateData = isIsolateData;
        }
        private double ConvertDateTimeToDouble(DateTime time)
        {
            double timevaluesdecimal = Convert.ToDouble(time.Ticks);//store it as a double in ticks
            timevaluesdecimal = timevaluesdecimal - Convert.ToDouble(BaseTime.Ticks);
            timevaluesdecimal = (timevaluesdecimal / 3600) * (.0000001); //Convert to hours since basetime
            return timevaluesdecimal;
        }
        private void CreateSlopeData()
        {
            SlopeChange = new double[ODValues.Length - 1];
            //this determines the slope of the data at each point
            for (int i = 0; i < ODValues.Length - 1; i++)
            {
                SlopeChange[i] = (LogODValues[i + 1] - LogODValues[i]) / (timevaluesdecimal[i + 1] - timevaluesdecimal[i]);
                SlopeChange[i] = Math.Log((double)2) / SlopeChange[i];
            }
            XvaluesForSlope = new double[ODValues.Length - 1];
            for (int i = 0; i < ODValues.Length - 1; i++)
            {
                XvaluesForSlope[i] = timevaluesdecimal[i];
            }
        }
        private void calculateMaxGrowthRate()
        {
            //This method takes every pair of points ij and calculates the growth rate between them
            //then returns a maximum rate
            MaxGrowthRate.MaxGrowthRate = DEFAULT_MAX_GROWTH_RATE;
            MaxGrowthRate.xvals = new double[2] { FitterForm.BAD_DATA_VALUE, FitterForm.BAD_DATA_VALUE };
            MaxGrowthRate.yvals = new double[2] { 0, 0 };
            int startvalue = 0;
            if (FirstReadingisODAtTimeZero) { startvalue = 1; }
            //if the first value is a lag, the maximum growth rate will be between these this point and another, so we count it out.
            for (int i = startvalue; i < ODValues.Length - 1; i++)
            {
                for (int j = i + 1; j < ODValues.Length; j++)
                {
                    double growthrate = (LogODValues[j] - LogODValues[i]) / (timevaluesdecimal[j] - timevaluesdecimal[i]);
                    growthrate = Math.Log((double)2) / growthrate;
                    if ((MaxGrowthRate.MaxGrowthRate > growthrate) && (growthrate > MaxPossibleGrowthRate))
                    {
                        MaxGrowthRate.MaxGrowthRate = growthrate;
                        MaxGrowthRate.xvals[0] = timevaluesdecimal[i];
                        MaxGrowthRate.xvals[1] = timevaluesdecimal[j];
                        MaxGrowthRate.yvals[0] = LogODValues[i];
                        MaxGrowthRate.yvals[1] = LogODValues[j];
                    }
                }
            }
        }
        private void createNonLogFitValues()
        {
            //since the fittedyvalues will be on the log scale, this method is used to create a second array with
            //non log values of the fitted goodness
            if (pFittedLogYValues != null)
            {
                pFittedYValues = new double[pFittedLogYValues.Length];
                for (int i = 0; i < pFittedLogYValues.Length; i++)
                { FittedYValues[i] = Math.Exp(pFittedLogYValues[i]); }
            }
            else
            {
                pFittedYValues = null;
            }
        }
        private void SetNoFit()
        {
            ValidDataSet = false;
            FittedLogYValues = null;
            ExpFit = null;
            LinFit = null;
            GrowthRate.GrowthRate = Double.NaN;
            GrowthRate.NumPoints = 0;
            GrowthRate.R2 = Double.NaN;
            GrowthRate.RMSE = Double.NaN;
            GrowthRate.FittingUsed = "No Points Available";
            GrowthRate.Notes = "Weird";
        }
        private void FitData()
        {
            if (FittedXValues != null && FittedLogYValues != null && FittedXValues.Length >= 2)
            {
                ValidDataSet = true;
                //if (FittedXValues.Length == 2)
                //{
                //    //Only two points, take the slope and go

                //    GrowthRate.GrowthRate = (FittedLogYValues[1] - FittedLogYValues[0]) / (FittedXValues[1] - FittedXValues[0]);
                //    GrowthRate.FittingUsed = "No Fit";
                //    GrowthRate.NumPoints = 2;
                //    GrowthRate.R2 = 1;
                //    GrowthRate.RMSE = 0;
                //    GrowthRate.FitterUsed = null;
                //}
                //else 
                if (FittedXValues.Length >= 2)
                {
                    //Linear Fit
                    LinFit = new LinearFit(FittedXValues, FittedLogYValues);
                    GrowthRate.FittingUsed = "Linear";
                    GrowthRate.GrowthRate = LinFit.Slope;
                    GrowthRate.R2 = LinFit.R2;
                    GrowthRate.RMSE = LinFit.RMSE;
                    GrowthRate.FitterUsed = LinFit;
                    //Exponential Fit
                    if (USE_EXPONENTIAL_FITTING)
                    {
                        ExpFit = new ExponentialFit(FittedXValues, FittedYValues);
                        if (ExpFit.SuccessfulFit)
                        {
                            GrowthRate.FittingUsed = "Exponential";
                            GrowthRate.GrowthRate = ExpFit.GrowthRate;
                            GrowthRate.R2 = ExpFit.R2;
                            GrowthRate.RMSE = ExpFit.RMSE;
                            GrowthRate.FitterUsed = ExpFit;
                        }
                    }
                    //Now Set the Growth Rate
                    GrowthRate.NumPoints = FittedXValues.Length;
                }
                if (FirstReadingisODAtTimeZero)
                {
                    SetLagTime();
                }
            }
            else
            {
                SetNoFit();
            }
        }
        public void SetFittedRangeFromIndexes(List<int> Indexes)
        {
            SetNoFit();
            List<double> TempX = new List<double>();
            List<double> tempActualY = new List<double>();
            List<double> TempY = new List<double>();
            Indexes.Sort();
            foreach (int i in Indexes)
            {
                double possibleY = LogODValues[i];
                if (SimpleFunctions.IsARealNumber(possibleY))
                {
                    TempX.Add(timevaluesdecimal[i]);
                    tempActualY.Add(pODValues[i]);
                    TempY.Add(possibleY);
                }
            }
            if (TempY.Count == 0)
            {
                FittedXValues = null;
                FittedLogYValues = null;
            }
            else
            {
                FittedXValues = TempX.ToArray();
                FittedLogYValues = TempY.ToArray();
                pFittedYValues = tempActualY.ToArray();
                FitData();
                GrowthRate.Notes = "Range Picked Data";
            }
        }
        public void SetFittedRange(int startindex, int endindex)
        {
            if (ODValues.Length < startindex + 1 || ODValues.Length < endindex + 1)
            {
                throw new Exception("You picked a range to fit outside the range of available values");
            }
            else
            {
                List<int> PossibleValues = new List<int>();
                for (int i = startindex; i <= endindex; i++)
                {
                    PossibleValues.Add(i);
                }
                SetFittedRangeFromIndexes(PossibleValues);
            }
        }
        /// <summary>
        /// Naive implementation, currently just goes from the value above the OD value and then grabs it while it is increasing
        /// </summary>
        /// <param name="startOD"></param>
        /// <param name="minOD"></param>
        public void SetFittedODRange(double startOD, double endOD, bool ODMustIncrease = true)
        {
            List<int> PointsToUse = new List<int>();
            int counter = 0;
            double lastODValue = -999;
            int lastAdded = -9;
            foreach (double ODValue in ODValues)
            {
                if ((ODValue >= startOD || PointsToUse.Capacity > 0) && ODValue <= endOD && ((ODMustIncrease && ODValue > lastODValue) || (ODValue < this.HighestRealODValue)))
                {
                    if (PointsToUse.Count != 0 && counter != (lastAdded + 1))
                    { break; }
                    else
                    {
                        PointsToUse.Add(counter);
                        lastODValue = ODValue;
                        lastAdded = counter;
                    }
                }
                counter++;
            }
            if (PointsToUse.Count >= 2)
            {
                SetFittedRangeFromIndexes(PointsToUse);
            }
        }
        public void SetFittedODRangeFromPercent(double startOD, double PercentOfMaxOD)
        {
            List<int> PointsToUse = new List<int>();
            double MaxAllowed = this.HighestRealODValue * PercentOfMaxOD;
            double MaxOD = this.HighestRealODValue;
            for (int i = 0; i < ODValues.Length; i++)
            {
                double ODValue = ODValues[i];
                if (ODValue == MaxOD)
                { break; }
                else if (ODValue >= startOD && ODValue <= MaxAllowed)
                {
                    PointsToUse.Add(i);
                }
            }
            if (PointsToUse.Count >= 2)
            {
                SetFittedRangeFromIndexes(PointsToUse);
            }
        }
        /// <summary>
        /// Attempts to fit or unfit a point that is selected by it's x,y value
        /// </summary>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <returns></returns>
        public bool ChangePoint(double xval)//, double yval)
        {
            //Completely changed method on 3/4/09 to accomodate clicks if no point has been added to the fit yet
            bool ChangeWorked = false;
            double[] oldX, oldY;
            oldX = FittedXValues.ToArray();
            oldY = FittedLogYValues.ToArray();
            try
            {
                if (timevaluesdecimal.Length > 2 && timevaluesdecimal.Contains(xval))// && LogODValues.Contains(yval))
                {
                    //first if nothing is fit
                    if (FittedXValues == null || FittedLogYValues == null)
                    {
                        int pointPosition = timevaluesdecimal.ToList().IndexOf(xval);
                        List<int> indexToAdd = new List<int>(3);
                        if (pointPosition > 0)
                        { indexToAdd.Add(pointPosition - 1); }
                        indexToAdd.Add(pointPosition);
                        if (pointPosition < (timevaluesdecimal.Length - 1))
                        { indexToAdd.Add(pointPosition + 1); }
                        SetFittedRangeFromIndexes(indexToAdd);
                    }
                    //now if we are removing or adding the point
                    else
                    {
                        List<double> timeList = timevaluesdecimal.ToList();
                        List<int> curIndexes = FittedXValues.Select((z) => timeList.IndexOf(z)).ToList();
                        int pointIndex = timeList.IndexOf(xval);

                        if (curIndexes.Contains(pointIndex))
                            curIndexes.Remove(pointIndex);
                        else
                            curIndexes.Add(pointIndex);
                        if (curIndexes.Count > 1)
                            SetFittedRangeFromIndexes(curIndexes);
                        else
                            return false;

                    }
                    FitData();
                    GrowthRate.Notes = "Hand Picked Data";
                    ChangeWorked = true;
                }
            }

            catch (Exception thrown)
            {
                FittedXValues = oldX;
                FittedLogYValues = oldY;
                FitData();
                System.Windows.Forms.MessageBox.Show("Something went wrong, actual error below:\n" + thrown.Message);
            }
            return ChangeWorked;
        }
        private void DetermineDataToFit(double[] timeValuesDecimal, double[] logODValues)
        {
            //This function will create the list of data to fit, the basic idea here is to calculate the maximum 
            //growth rate, since this is the trait that all growth models share.  values within a range of this will be
            //accepted, the exception is the first and last included points, which will only be accepted if they do
            //not show the greatest lag
            List<int> IndexesToFit = new List<int>();
            if (ODValues.Length == 2)
            {
                IndexesToFit.Add(0);
                IndexesToFit.Add(1);
            }
            if (ODValues.Length > 3)
            {
                //If we have more then three points we will filter the data based on the relationship to the maximum observed growth rate
                //then using the remaining points fit it appropriately
                FittedXValues = new double[logODValues.Length];
                FittedLogYValues = new double[logODValues.Length];
                int PointsInFit = 0;//how many points i am fitting
                int lastFit = -1;
                double WithinValue = .5;//values within this percent range are good
                double CutOffValue = 1.5;//When the slope(Doubling time) is greater then CutOffValue*PreviousSlope we assume lag phase and stop
                //not sure yet it i want to force the data to be fitted to immediately follow one another

                for (int i = 0; i < ODValues.Length - 1; i++)
                {
                    if ((SlopeChange[i] < MaxGrowthRate.MaxGrowthRate * (1 + WithinValue)) && (SlopeChange[i] >= MaxGrowthRate.MaxGrowthRate) && SlopeChange[i] > MaxPossibleGrowthRate)
                    {
                        //if (HighestIncluded < SlopeChange[i]) { HighestIncluded = SlopeChange[i]; }
                        if (lastFit != i)//exclude the point if already included in last grab
                        {
                            IndexesToFit.Add(i);
                            PointsInFit++;
                        }
                        IndexesToFit.Add(i + 1);
                        PointsInFit++;
                        lastFit = i + 1;
                    }
                    if ((PointsInFit > 1) && i > 0 && (SlopeChange[i - 1] > 0) && ((SlopeChange[i - 1] * CutOffValue) < SlopeChange[i])) { break; }
                }
                if (PointsInFit > 3)
                {
                    //Now we have to decide if we want to keep the highest and lowest values, this will happen only if there
                    //is an intermediate value higher then them
                    bool includeFirst = false;//these are changed if they are the lowest values
                    bool includeLast = false;
                    double slopeAtEnd = (logODValues[PointsInFit - 1] - logODValues[PointsInFit - 2]) / (timeValuesDecimal[PointsInFit - 1] - timeValuesDecimal[PointsInFit - 2]);
                    double slopeAtStart = (logODValues[1] - logODValues[0]) / (timeValuesDecimal[1] - timeValuesDecimal[0]);

                    for (int i = 1; i < PointsInFit - 1; i++)
                    {
                        double slopeHere = (FittedLogYValues[i + 1] - FittedLogYValues[i]) / (FittedXValues[i + 1] - FittedXValues[i]);
                        if (slopeHere > slopeAtEnd) { includeLast = true; }
                        if (slopeHere > slopeAtStart) { includeFirst = true; }
                    }
                    if (includeFirst) { IndexesToFit.Remove(IndexesToFit.Min()); }// Reduction--; }
                    if (includeLast) { IndexesToFit.Remove(IndexesToFit.Max()); }// Reduction--; }

                }
                else if (PointsInFit == 2)//just 2 points in fit
                {
                    ValidDataSet = true;
                    double[] FittedXValuesTemp = new double[PointsInFit];
                    double[] FittedYValuesTemp = new double[PointsInFit];
                    for (int i = 0; i < 2; i++)
                    {
                        FittedXValuesTemp[i] = FittedXValues[i];
                        FittedYValuesTemp[i] = FittedLogYValues[i];
                    }

                    FittedXValues = FittedXValuesTemp;
                    FittedLogYValues = FittedYValuesTemp;
                }
                else if (MaxGrowthRate.xvals[0] != FitterForm.BAD_DATA_VALUE)
                {
                    //take the maximum growth rate if their are three, or less then 2 points
                    List<double> timeList = timevaluesdecimal.ToList();
                    IndexesToFit = MaxGrowthRate.xvals.Select((u) => timeList.IndexOf(u)).ToList();
                }

            }
            SetFittedRangeFromIndexes(IndexesToFit);
            GrowthRate.Notes = "Automatically picked";
        }
        public List<string> data_CreateDataTableUpdateString()
        {
            if (IsIsolateData)
                return data_ISOCreateDataTableUpdateString();
            else
            {
                List<string> UpdatesToReturn = new List<string>();
                for (int i = 0; i < pDataBaseRecordIDs.Length; i++)
                {
                    double curX = timevaluesdecimal[i];
                    double curY = ODValues[i];
                    bool isUsed = false;
                    if (FittedXValues != null && SimpleFunctions.ValueInArray(FittedXValues, curX))//now decide if it made it into the fit
                    {
                        isUsed = true;
                    }
                    string cmd = "UPDATE Data SET Data.CorrOD = " + curY.ToString() + ", Data.UsedInFitting = " + isUsed.ToString() + " WHERE Data.ID=" + pDataBaseRecordIDs[i].ToString();
                    UpdatesToReturn.Add(cmd);
                }
                return UpdatesToReturn;
            }
        }
        public List<string> data_CreateResultTableUpdateString()
        {
            if (IsIsolateData)
                return data_ISOCreateResultTableUpdateString();
            else
            {
                List<string> UpdatesToReturn = new List<string>();
                //First a delete event
                string cmd = "DELETE GrowthResults.* FROM GrowthResults WHERE GrowthResults.Well='" + DataSetName + "' AND GrowthResults.ExperimentID=" + db_ExperimentID.ToString();
                UpdatesToReturn.Add(cmd);
                //now add new data

                cmd = "INSERT INTO GrowthResults (Well, MaxOD,GrowthRate, ExperimentID, MaxODFLAG,GrowthRateFLAG,NumPointsFit,R2,TimeTillOD) VALUES ('"
                    + DataSetName + "'," + HighestRealODValue.ToString() + "," + GrowthRate.GrowthRate.ToString() + "," + db_ExperimentID.ToString()
                    + "," + db_MaxOD_Flag.ToString() + "," + db_GrowthRateFLAG.ToString() + "," + GrowthRate.NumPoints.ToString() + "," + GrowthRate.R2.ToString() + "," + HoursTillODReached(.02) + ")";
                cmd = cmd.Replace("NaN", "-999");
                UpdatesToReturn.Add(cmd);
                return UpdatesToReturn;
            }
        }
        public List<string> data_ISOCreateResultTableUpdateString()
        {
            List<string> UpdatesToReturn = new List<string>();
            //First a delete event
            string cmd = "DELETE IsoGrowthResults.* FROM IsoGrowthResults WHERE IsoGrowthResults.Well='" + DataSetName + "' AND IsoGrowthResults.ExperimentID=" + db_ExperimentID.ToString();
            UpdatesToReturn.Add(cmd);
            //now add new data

            cmd = "INSERT INTO IsoGrowthResults (Well, MaxOD,GrowthRate, ExperimentID, MaxODFLAG,GrowthRateFLAG,NumPointsFit,R2,TimeTillOD) VALUES ('"
                + DataSetName + "'," + HighestRealODValue.ToString() + "," + GrowthRate.GrowthRate.ToString() + "," + db_ExperimentID.ToString()
                + "," + db_MaxOD_Flag.ToString() + "," + db_GrowthRateFLAG.ToString() + "," + GrowthRate.NumPoints.ToString() + "," + GrowthRate.R2.ToString() + "," + HoursTillODReached(.02) + ")";
            cmd = cmd.Replace("NaN", "-999");
            UpdatesToReturn.Add(cmd);
            return UpdatesToReturn;
        }
        public List<string> data_ISOCreateDataTableUpdateString()
        {
            List<string> UpdatesToReturn = new List<string>();
            for (int i = 0; i < pDataBaseRecordIDs.Length; i++)
            {
                double curX = timevaluesdecimal[i];
                double curY = ODValues[i];
                bool isUsed = false;
                if (FittedXValues != null && SimpleFunctions.ValueInArray(FittedXValues, curX))//now decide if it made it into the fit
                {
                    isUsed = true;
                }
                string cmd = "UPDATE IsoGrowthData SET IsoGrowthData.CorrOD = " + curY.ToString() + ", IsoGrowthData.UsedInFitting = " + isUsed.ToString() + " WHERE IsoGrowthData.ID=" + pDataBaseRecordIDs[i].ToString();
                UpdatesToReturn.Add(cmd);
            }
            return UpdatesToReturn;
        }
    }
    public class DataBaseToGrowthRateTools
    {
        public static List<GrowthData> CreateGrowthDataFromDataTable(DataTable DT, bool UseCorrectedOD = false,bool IsIsoLateData=false)
        {
            List<GrowthData> toReturn = new List<GrowthData>();
            List<double> OD = new List<double>();
            List<int> Fitted = new List<int>();
            int curExpID = -9;
            string curWell = "";
            List<DateTime> times = new List<DateTime>();
            List<int> DataIDs = new List<int>();
            foreach (DataRow DR in DT.Rows)
            {
                string Well = (string)DR["Well"];
                int ExpID = (int)DR["ExpID"];
                if ((Well != curWell || ExpID != curExpID))
                {
                    if (OD.Count > 0)
                    {
                        GrowthData GD = new GrowthData(curWell, times.ToArray(), OD.ToArray(), Fitted.ToArray(), DataIDs.ToArray(),curExpID,IsIsoLateData);
                        toReturn.Add(GD);
                    }
                    curWell = Well;
                    curExpID = ExpID;
                    OD.Clear();
                    Fitted.Clear();
                    times.Clear();
                    DataIDs.Clear();
                }
                if (UseCorrectedOD) { OD.Add((double)DR["CorrOD"]); }
                else { OD.Add((double)DR["OD"]); }
                times.Add((DateTime)DR["Time"]);
                DataIDs.Add((int)DR["ID"]);
                bool fit = (bool)DR["UsedInFitting"];
                Fitted.Add(fit?0:1);
            }
            if (OD.Count > 0)
            {
                GrowthData GD = new GrowthData(curWell, times.ToArray(), OD.ToArray(), Fitted.ToArray(), DataIDs.ToArray(),curExpID,IsIsoLateData);
                toReturn.Add(GD);
            }
            return toReturn;
        }
        public static List<GrowthData> CreateVenusDataFromDataTable(DataTable DT)
        {
            List<GrowthData> toReturn = new List<GrowthData>();
            List<double> OD = new List<double>();
            List<int> Fitted = new List<int>();
            int curExpID = -9;
            string curWell = "";
            List<DateTime> times = new List<DateTime>();
            List<int> DataIDs = new List<int>();
            foreach (DataRow DR in DT.Rows)
            {
                string Well = (string)DR["Well"];
                int ExpID = (int)DR["ExpID"];
                if ((Well != curWell || ExpID != curExpID))
                {
                    if (OD.Count > 0)
                    {
                        GrowthData GD = new GrowthData(curWell, times.ToArray(), OD.ToArray(), Fitted.ToArray(), DataIDs.ToArray(), curExpID);
                        toReturn.Add(GD);
                    }
                    curWell = Well;
                    curExpID = ExpID;
                    OD.Clear();
                    Fitted.Clear();
                    times.Clear();
                    DataIDs.Clear();
                }
                OD.Add(Convert.ToDouble((int)DR["Venus"])); 
                times.Add((DateTime)DR["Time"]);
                DataIDs.Add((int)DR["ID"]);
               
                Fitted.Add(0);
            }
            if (OD.Count > 0)
            {
                GrowthData GD = new GrowthData(curWell, times.ToArray(), OD.ToArray(), Fitted.ToArray(), DataIDs.ToArray(), curExpID);
                toReturn.Add(GD);
            }
            return toReturn;
        }
    }
}

