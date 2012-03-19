using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using alglib;

namespace Fit_Growth_Curves
{

    //ABSTRACT CLASSES TO BE INHERITED FROM
    #region AbstractFitter

    public abstract class AbstractFitter
    {
        public string name = "";
        protected double[] x;
        public double[] X
        {
            get { return x.ToArray(); }
        }
        public double[] Y
        {
            get {return y.ToArray();}
        }
        protected double[] y;
        protected double[] ypred;
        public double[] PredictedValues
        {
            get { return ypred.ToArray(); }

        }
        protected double[] pParameters;//parameters in the model
        public double[] Parameters { get { return pParameters.ToArray(); } }
        public bool SuccessfulFit;//determines if the model is fitted and has parameters
        public double RMSE
        {
            get
            {
                if (SuccessfulFit)
                {
                    return calculateRMSE();
                }
                else
                {
                    return -999;
                }

            }
        }
        public double R2
        {
            get
            {
                if (SuccessfulFit)
                {
                    return calculateR2();
                }
                else
                {
                    return -999;
                }
            }
        }
        public double AbsError
        {
            //This calculates teh absolute error |yhat-y|
            get
            {
                if (SuccessfulFit)
                {
                    return calculateAbsError();
                }
                else
                {
                    return -999;
                }
            }
        }
        public AbstractFitter()
        {

        }
        public void makeYHAT()
        {
            ypred = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                ypred[i] = FunctiontoFit(x[i]);
            }
        }
        private double calculateRMSE()
        {
            if (!SuccessfulFit)
                return double.NaN;
            double MSE = calculateResidualSumofSquares();
            double DataPoints = x.Length;
            double NumParmeters = pParameters.Length;
            double df = DataPoints - NumParmeters;
            MSE = MSE / df;
            double RMSE = Math.Sqrt(MSE);
            return RMSE;
        }
        private double calculateR2()
        {
            if (!SuccessfulFit)
                return double.NaN;
            double SST, SSR;
            SSR = calculateResidualSumofSquares();
            double Ymean = y.Average();
            SST = 0;
            for (int i = 0; i < x.Length; i++)
            {
                SST += Math.Pow((y[i] - Ymean), 2.0);
            }
            return (1.0 - (SSR / SST));
        }
        /// <summary>
        /// Only should be called by external classes following a fit
        /// </summary>
        /// <returns>NaN if not successful fit, otherwise the sum of squares</returns>
        public double calculateResidualSumofSquares()
        {
            if (!SuccessfulFit)
                return double.NaN;
            if (ypred == null)
                makeYHAT();
            double SSR = 0;
            for (int i = 0; i < y.Length; i++)
            {SSR += Math.Pow((ypred[i]- y[i]), 2.0);}
            return SSR;
        }
        public virtual double calculateAbsError()
        {
            if (!SuccessfulFit)
                return double.NaN;
            if(ypred==null)
                makeYHAT();
            double Error = 0;
            for (int i = 0; i < ypred.Length; i++)
            {
                Error += Math.Abs(ypred[i] - y[i]);
            }
            return Error;
        }
        public abstract double FunctiontoFit(double x);
        protected abstract void FitModel();
        public virtual void GenerateFitLine(double LowX, double interval, double HighX, out double[] xvalues, out double[] yvalues)
        {
            double curVal = LowX;
            //now to determine the number of values in the interval
            int ArraySize = (int)((HighX - LowX) / interval);
            xvalues = new double[ArraySize + 1];
            yvalues = new double[ArraySize + 1];
            for (int i = 0; i < ArraySize; i++)
            {
                xvalues[i] = curVal;
                yvalues[i] = FunctiontoFit(curVal);
                curVal += interval;
            }
            xvalues[ArraySize] = HighX;
            yvalues[ArraySize] = FunctiontoFit(HighX);
        }
    }
    #endregion
    #region NonLinearFitter
    /// <summary>
    /// A wrapper class for the alglib functions, used to fit non-linear models
    /// </summary>
    public abstract class NonLinearFitterWithGradientAndHessian : AbstractFitter
    {
        public enum TerminationType:int{NotYetSet=-2,WrongParams=-1,SumOfSquaresChangeConverge=1,ParameterConverge=2,MaxIterations=5};
        protected Delegates.DerivativePortionCalculator[,] HessianFunctions;
        /// <summary>
        /// stopping criterion. Algorithm stops if
        /// |X(k+1)-X(k)| <= EpsX*(1+|X(k)|)
        /// </summary>
        protected double EPSX;
        /// <summary>
        /// Fill in the Hessian matrix for the sum of squares function
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="Hessian"></param>
        private void EvaluateHessian(double[] Params, double[,] Hessian)
        {
            int n = Params.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    Delegates.DerivativePortionCalculator calcDeriv = HessianFunctions[i, j];
                    Hessian[i, j] = EvaluateDerivative(Params, calcDeriv);
                    if (i != j)
                    { Hessian[j, i] = Hessian[i, j]; }
                }
            }
        }
        /// <summary>
        /// Since for sum of squares the method is simply a sum over each of the components, this general method performs the loop
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="calc"></param>
        /// <returns></returns>
        private double EvaluateDerivative(double[] Params, Delegates.DerivativePortionCalculator calc)
        {
            double sum = 0;
            for (int i = 0; i < Params.Length; i++)
            {
                sum += calc(Params, x[i], y[i]);
            }
            return sum;
        }
        /// <summary>
        /// Used by the fitting routine to evaluate possible values,
        /// meant to stay compatible with alg lig
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        protected abstract double EvaluateSumofSquaresFunction(double[] Params);
        protected abstract void EvaluateSumOfSquaresGradient(double[] Params, double[] GradientVec);
        protected abstract void CreateHessianDelegates();
        protected abstract double[] CreateInitialParameterGuess();
        /// <summary>
        /// See the example in alglib on levenberg marquadt to understand this code.
        /// Based entirely on their example, somethings kept around for compatibility
        /// </summary>
        /// <returns></returns>
        protected  minlm.lmreport GeneralLMFitting()
        {
            minlm.lmstate state = new minlm.lmstate();
            minlm.lmreport rep = new minlm.lmreport();
            double[] InitValues= CreateInitialParameterGuess();
            minlm.minlmfgh(pParameters.Length, ref InitValues, 0.0, EPSX, 0, ref state);
            while (minlm.minlmiteration(ref state))
            {
                //needs to evaluate function
                if (state.needf || state.needfg || state.needfgh)
                {
                    state.f = EvaluateSumofSquaresFunction(state.x);
                    if (state.needfg || state.needfgh)
                    {
                        EvaluateSumOfSquaresGradient(state.x, state.g);
                        if (state.needfgh)
                        {
                            EvaluateHessian(state.x, state.h);
                        }
                    }
                }
            }
            //have functions change em
            minlm.minlmresults(ref state, ref pParameters, ref rep);
            return rep;
           
        }
    }
    #endregion
    #region ExponentialFit
    public class ExponentialFit : NonLinearFitterWithGradientAndHessian
    {
        //the first parameter is the constant, the second is the exponential growth rate
        enum ParametersIndex : int { P0Index = 0, rIndex = 1 };
        public TerminationType ReasonMarquadtEnded = TerminationType.NotYetSet;
        public double GrowthRate
        {
            get { return pParameters[(int)ParametersIndex.rIndex]; }
        }
        public double InitialPopSize
        {
            get { return pParameters[(int)ParametersIndex.P0Index]; }
        }
        public ExponentialFit(double[] XDATA, double[] YDATA)
        {
            this.name = "Exp";
            EPSX = .000001;
            if ((XDATA.Length < 2 || YDATA.Length < 2) || (XDATA.Length != YDATA.Length))
            { throw new ArgumentOutOfRangeException("Exponential fit can't work with less then 2 points or unequal matrices"); }
            if (XDATA.Length != YDATA.Length)
                throw new ArgumentException("Arrays of unequal size were passed to the non-linear fitter");
            //deep copy the data to protect its integrity
            y = YDATA.ToArray();
            x = XDATA.ToArray();
            pParameters = new double[2] { Form1.BAD_DATA_VALUE, Form1.BAD_DATA_VALUE };
            CreateHessianDelegates();
            FitModel();
        }
        protected override void FitModel()
        {
            minlm.lmreport mla = GeneralLMFitting();
            TerminationType term =  (TerminationType) Enum.ToObject(typeof(TerminationType),mla.terminationtype);
            ReasonMarquadtEnded = term;
            //bad fit
            if (term == TerminationType.MaxIterations || term == TerminationType.WrongParams)
            {
                SuccessfulFit = false;
                pParameters = null;
            }
            else
            {
                SuccessfulFit = true;
                makeYHAT();
            }
        }
        public override double FunctiontoFit(double x)
        {
            double y = pParameters[(int)ParametersIndex.P0Index] * Math.Exp(pParameters[(int)ParametersIndex.rIndex] * x);
            return y;
        }
        protected override double EvaluateSumofSquaresFunction(double[] Params)
        {
            //Calculate the sum  of squares for each point
            double SS = 0;
            double r = Params[(int)ParametersIndex.rIndex];
            double a = Params[(int)ParametersIndex.P0Index];
            for (int i = 0; i < x.Length & i < y.Length; i++)
            {
                double yPred = a * Math.Exp(r * x[i]);
                double yDif = Math.Pow((yPred - y[i]), 2.0);
                SS += yDif;
            }
            return SS;
        }
        protected override void EvaluateSumOfSquaresGradient(double[] Params, double[] GradientVec)
        {
            double A, b;
            double sum = 0;

            A = Params[(int)ParametersIndex.P0Index];
            b = Params[(int)ParametersIndex.rIndex];
            //the A gradient
            for (int i = 0; i < x.Length; i++)
            {
                sum += -0.2e1 * (y[i] - A * Math.Exp(b * x[i])) * Math.Exp(b * x[i]);
            }
            GradientVec[(int)ParametersIndex.P0Index] = sum;
            //the B gradient 
            sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += -2.0 * (y[i] - A * Math.Exp(b * x[i])) * A * x[i] * Math.Exp(b * x[i]);
            }
            GradientVec[(int)ParametersIndex.rIndex] = sum;
        }
        /// <summary>
        /// Creates an upper triangular array of delegates to calculate portions of each element of the hessian matrix
        /// </summary>
        protected override void CreateHessianDelegates()
        {
            HessianFunctions = new Delegates.DerivativePortionCalculator[pParameters.Length, pParameters.Length];
            HessianFunctions[0, 0] = new Delegates.DerivativePortionCalculator(CalculateAADerivateComponent);
            HessianFunctions[0, 1] = new Delegates.DerivativePortionCalculator(CalculateABDerivativeComponent);
            HessianFunctions[1, 1] = new Delegates.DerivativePortionCalculator(CalculateBBDerivativeComponent);
        }
        /// <summary>
        /// Makes an initial guess based on linear regreesion
        /// </summary>
        /// <returns></returns>
        protected override double[] CreateInitialParameterGuess()
        {
            double[] logdata = y.ToArray();
            logdata = Array.ConvertAll(logdata, z => Math.Log(z));
            LinearFit LF = new LinearFit(x, logdata);
            double[] ParamGuess = new double[2];
            ParamGuess[(int)ParametersIndex.rIndex] = LF.Slope;;
            ParamGuess[(int)ParametersIndex.P0Index] = Math.Exp(LF.Intercept);
            return ParamGuess;
        }

        #region DoubleDerivative Functions
        /// <summary>
        /// dy^2/dA^2
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double CalculateAADerivateComponent(double[] Params, double x, double y)
        {
            double b = Params[(int)ParametersIndex.rIndex];
            double value = 2.0 * Math.Pow(Math.Exp(b * x), 2.0);
            return value;
        }
        /// <summary>
        /// dy^2/dA,dB
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double CalculateABDerivativeComponent(double[] Params, double x, double y)
        {
            double b = Params[(int)ParametersIndex.rIndex];
            double A = Params[(int)ParametersIndex.P0Index];
            double value = -0.2e1 * (y - A * Math.Exp(b * x)) * A * x * Math.Exp(b * x);
            return value;
        }
        /// <summary>
        /// Calculates one portion of dy^2/dBdB
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double CalculateBBDerivativeComponent(double[] Params, double x, double y)
        {
            double b = Params[(int)ParametersIndex.rIndex];
            double A = Params[(int)ParametersIndex.P0Index];
            double value = 0.2e1 * A * A * Math.Pow(x, 0.2e1) * Math.Pow(Math.Exp(b * x), 0.2e1) - 0.2e1 * (y - A * Math.Exp(b * x)) * A * Math.Pow(x, 0.2e1) * Math.Exp(b * x);
            return value;
        } 
        #endregion
        public double GetXValueAtYValue(double yValue)
        {
            if (!SuccessfulFit)
            { return Form1.BAD_DATA_VALUE; }
            else
            {
               double t= Math.Log(yValue/ Parameters[(int)ParametersIndex.P0Index])/Parameters[(int)ParametersIndex.rIndex];
               return t;
            }
        }
 
    }
    #endregion

    
    #region LinearFit

    public class LinearFit : AbstractFitter
    {
        public double XOFFSET;
        public LinearFit(double[] XGOOD, double[] YGOOD)
        {
            name = "Linear";
            if ((XGOOD.Length < 2 || YGOOD.Length < 2) || (XGOOD.Length != YGOOD.Length))
            { throw new Exception("Linear fit can't work with less then 2 points or unequal matrices"); }
            if(XGOOD.Count(n=>!SimpleFunctions.IsARealNumber(n))>0 || YGOOD.Count(n=>!SimpleFunctions.IsARealNumber(n))>0)
            {throw new Exception("Linear fit can't work on data that contains non real numbers");}
            x = XGOOD;
            y = YGOOD;
            pParameters = new double[2];//linear model
            FitModel();

        }
        public override double calculateAbsError()
        {
            //WARNING DOES NOT CALCULATE THE ABSERROR OF A LINEAR FIT!!!!!
            //TO DO THAT SIMPLY DELETE THIS METHOD!!!!
            //I am using this because before this would calculate the 
            //ABSERROR IN SEMI LOG SPACE for the growth curve fitter, but I wanted it in linear space
            makeYHAT();
            double Error = 0;
            for (int i = 0; i < ypred.Length; i++)
            {
                Error += Math.Abs(Math.Exp(ypred[i]) - Math.Exp(y[i]));
            }
            return Error;
        }

        protected override void FitModel()
        {
            //First to calculate m
            double n = x.Length;
            double SumofSQX = 0, SumofSQY = 0, SumofX = 0, SumofY = 0, SumofXY = 0;
            for (int i = 0; i < n; i++)
            {
                SumofSQX += Math.Pow(x[i], (double)2);
                SumofSQY += Math.Pow(y[i], (double)2);
                SumofX += x[i];
                SumofY += y[i];
                SumofXY += x[i] * y[i];
            }
            //Calculate the slope
            pParameters[1] = (n * SumofXY - (SumofX * SumofY)) / ((n * SumofSQX) - Math.Pow(SumofX, 2));//the slope
            pParameters[0] = (SumofY - pParameters[1] * SumofX) / n;//the constant
            SuccessfulFit = true;
            makeYHAT();
        }
        public override double FunctiontoFit(double x)
        {
            double y = pParameters[0] + pParameters[1] * x;
            return y;
        }
        enum ParameterIndexes { Intercept = 0, Slope = 1 };
        public double Slope
        {
            get { return pParameters[(int)ParameterIndexes.Slope]; }
        }
        public double Intercept
        {
            get { return pParameters[(int)ParameterIndexes.Intercept]; }
        }


    }
    #endregion
   
    #region Delegates
    public class Delegates
    {
        public delegate void FunctionToFit(double x, double[] a, out double y, double[] dyda);
        public delegate double DerivativePortionCalculator(double[] Params,double  x, double y);

    }
    #endregion   

    #region SimpleFunctions
    public class SimpleFunctions
    {
        public static void CleanNonRealNumbersFromYvaluesInXYPair(ref double[] x, ref double[] y)
        {
            if ((x.Length != y.Length) | x.Rank != 1 | y.Rank != 1) { throw new Exception("This XY pair is sized wrong"); }
            ArrayList NewXValues = new ArrayList(x.Length);
            ArrayList NewYValues = new ArrayList(y.Length);
            //int toRemoveIndex=new int[x.Length];
            int countToRemove = 0;
            for (int i = 0; i < x.Length; i++)
            {
                if (Double.IsPositiveInfinity(x[i]) || Double.IsNegativeInfinity(x[i]) || Double.IsNaN(x[i]) || Double.IsPositiveInfinity(y[i]) || Double.IsNegativeInfinity(y[i]) || Double.IsNaN(y[i]))
                {

                    countToRemove++;
                    //toRemoveIndex[countToRemove] = i;
                }
                else
                {
                    NewXValues.Add(x[i]);
                    NewYValues.Add(y[i]);
                }
            }
            x = new double[x.Length - countToRemove];
            y = new double[x.Length - countToRemove];
            NewYValues.CopyTo(y);
            NewXValues.CopyTo(x);
        }
        public static bool IsARealNumber(double value)
        {
            bool toReturn = true;
            if (Double.IsNaN(value) || Double.IsNegativeInfinity(value) || Double.IsPositiveInfinity(value))
            {
                toReturn = false;
            }
            return toReturn;
        }
        public static double Max(double[] values)
        {
            double res = values[0];
            foreach (double x in values) if (x > res) res = x;
            return res;
        }
        public static double Min(double[] values)
        {
            double res = values[0];
            foreach (double x in values) if (x < res) res = x;
            return res;
        }
        public static double MinNotZero(double[] values)
        {
            double res = Max(values);
            foreach (double x in values) if (x < res && x != 0) res = x;
            return res;
        }
        public static bool ValueInArray(double[] array, double val)
        {
            bool IsIT = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == val) { IsIT = true; break; }
            }
            return IsIT;
        }
        public static bool ValueInArray(DateTime[] array, DateTime val)
        {
            bool IsIT = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == val) { IsIT = true; break; }
            }
            return IsIT;
        }
        public static bool ValueInArray(DateTime[] array, DateTime val, ref int Index)
        {
            bool IsIT = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == val) { IsIT = true; Index = i; break; }
            }
            return IsIT;
        }


    }
    #endregion
}
