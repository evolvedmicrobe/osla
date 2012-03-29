//#region NonLinearFitterWithoutDerivativesAddOn;
////abstract class NonLinearFitterWithoutDerivatives : NonLinearFitter
////{
////    const double MACH_PRECISION = 1.19209e-07;
////    double Epsilon;
////    //floating point arithmetic problems, picked 25....
////    public NonLinearFitterWithoutDerivatives()
////    {
////        FuncDel = new Delegates.FunctionToFit(FuncForMarq);
////        Epsilon = Math.Sqrt(MACH_PRECISION);//this value is a heuristic good measure for the difference too use in partial derivatives
////    }
////    public override void FuncForMarq(double x, double[] a, out double y, double[] dyda)
////    {
////        y = FunctiontoFit(x);
////        DetermineNumericPartialDerivatives(x, a, dyda);
////    }
////    //this function is always around, and should be called by the functionformarquadt
////    public virtual void DetermineNumericPartialDerivatives(double x, double[] a, double[] dyda)
////    {
////        //this will evaluate the partial derivatives numerically, 
////        //function could be overloaded/enhanced to reflect some of the options available at 
////        //http://www.physics.wisc.edu/~craigm/idl/fitting.html, going to wait until this doesn't work though as I think this is a good setup

////        //first to create a copy of the old values so I can restore them if need be
////        //this just makes it easy to inherit from this class, but does introduce some computational overhead
////        bool ParamsAlreadySet = false;
////        if (Params == null)
////        {
////            Params = a.ToArray(); //change the global params so FunctionToFit works
////        }
////        else
////        {
////            ParamsAlreadySet = true;
////        }

////        //now to screw with the parameters, for now am assuming all parameters are to be fit, 
////        //though this is not the case for mrqmin, which allows variations, so worst comes to worse, the derivative is evaluated and not used
////        double yHigh, yLow, yAtx, BaseParamValue;
////        double delta, temp;
////        for (int i = 0; i < a.Length; i++)
////        {
////            BaseParamValue = a[i];//get the value              
////            delta = Math.Abs(a[i]) * Epsilon;//usually bout 15-16 digits precision
////            if (delta == 0.0)//if zero change it
////            {
////                delta = Epsilon;
////            }
////            //get original value
////            //yAtx=FunctiontoFit(x);
////            //evaluate slope above and below
////            Params[i] = BaseParamValue + delta;
////            yHigh = FunctiontoFit(x + delta);//one above
////            Params[i] = BaseParamValue - delta;
////            yLow = FunctiontoFit(x - delta);//one below
////            dyda[i] = (yHigh - yLow) / (2.0 * delta);//average to get slope/derivative
////            Params[i] = BaseParamValue;//reset for next partial derivative
////        }
////        //finally, if no parameters have been set yet, keep it so
////        if (!ParamsAlreadySet)
////        { Params = null; };//remove reference to the testing parameters
////    }
////}
//#endregion
////OLD EXPONENTIAL FIT
////#region ExponentialFit
////public class ExponentialFit : NonLinearFitter
////{
////    //the first parameter is the constant, the second is the exponential growth rate
////    public double XOFFSET;//Determines the amount of x to subtract before adding the new stuff

////    public double[] GuessParametersFromLinearFit()
////    {
////        name = "Exp";
////        double[] logdata = y.ToArray();
////        logdata = Array.ConvertAll(logdata, z => Math.Log(z));
////        LinearFit LF = new LinearFit(x, logdata);
////        double[] ParamGuess = new double[2];
////        ParamGuess[1] = LF.Params[1];
////        ParamGuess[0] = Math.Exp(LF.Params[0]);
////        return ParamGuess;
////    }
////    public ExponentialFit(double[] XGOOD, double[] YGOOD)
////    {
////        if ((XGOOD.Length < 2 || YGOOD.Length < 2) || (XGOOD.Length != YGOOD.Length))
////        { throw new Exception("Exponential fit can't work with less then 2 points or unequal matrices"); }
////        //deep copy the data to protect its integrity
////        y = new double[YGOOD.Length];
////        x = new double[XGOOD.Length];
////        YGOOD.CopyTo(y, 0);
////        XGOOD.CopyTo(x, 0);
////        //set initial guesses
////        Params = new double[2];
////        Params = GuessParametersFromLinearFit();

////        //set the function delegate that returns needed information for marquadt minimizer
////        FuncDel = new Delegates.FunctionToFit(FuncForMarq);
////        FitModel();

////    }
////    public override void FitModel()
////    {
////        bool[] FitAll = new bool[2];
////        FitAll[0] = true; ;
////        FitAll[1] = true;
////        Fitter = new MarquadtMin(x, y, Params, FitAll, 1, FuncDel);
////        Params = Fitter.Parameters;
////        FittedAlready = true;
////    }
////    public override void GenerateFitLine(double spaceholder, double interval, double HighX, out double[] xvalues, out double[] yvalues)
////    {
////        //THE SPACEHOLDER IS STUCK IN THERE TO OVERRIDE THE LOWX VALUE, SO I DON'T HAVE TO MAKE A NEW BASE CLASS
////        double LowX = XOFFSET;
////        double curVal = LowX;
////        //now to determine the number of values in the interval
////        int ArraySize = Convert.ToInt32(((HighX - LowX) / interval));
////        //NigelMethods.Basics.debug("LOW VAL "+XOFFSET.ToString()+", HIGH VAL "+HighX.ToString());
////        xvalues = new double[ArraySize + 1];
////        yvalues = new double[ArraySize + 1];
////        for (int i = 0; i < ArraySize; i++)
////        {
////            xvalues[i] = curVal;
////            yvalues[i] = FunctiontoFit(curVal - XOFFSET);
////            curVal += interval;
////        }
////        xvalues[ArraySize] = HighX;
////        yvalues[ArraySize] = FunctiontoFit(HighX - XOFFSET);
////        //return base.GenerateFitLine(LowX, interval, HighX, ref xvalues, ref yvalues);
////    }
////    public override void FuncForMarq(double x, double[] a, out double y, double[] dyda)
////    {
////        //f=a[0]*exp(x*a[1])
////        y = 0.0;
////        double ex = Math.Exp(x * a[1]);
////        y = a[0] * ex;
////        dyda[0] = ex;//exp(r*t)
////        dyda[1] = x * y;//r*A*exp(r*t)
////    }
////    public override double FunctiontoFit(double x)
////    {
////        double y = Params[0] * Math.Exp(Params[1] * x);
////        return y;
////    }
////}
////#endregion
////#region ExponentialFitWithoutDerivatives
////class ExponentialFitWithoutDerivatives : NonLinearFitterWithoutDerivatives
////{
////    //the first parameter is the constant, the second is the exponential growth rate
////    public double XOFFSET;//Determines the amount of x to subtract before adding the new stuff

////    public double[] GuessParametersFromLinearFit()
////    {
////        name = "ExpFitNoDerivatives";
////        double[] logdata = y.ToArray();
////        logdata = Array.ConvertAll(logdata, z => Math.Log(z));
////        LinearFit LF = new LinearFit(x, logdata);
////        double[] ParamGuess = new double[2];
////        ParamGuess[1] = LF.Params[1];
////        ParamGuess[0] = Math.Exp(LF.Params[0]);
////        return ParamGuess;
////    }
////    public ExponentialFitWithoutDerivatives(double[] XGOOD, double[] YGOOD)
////    {
////        if ((XGOOD.Length < 2 || YGOOD.Length < 2) || (XGOOD.Length != YGOOD.Length))
////        { throw new Exception("Exponential fit can't work with less then 2 points or unequal matrices"); }
////        //deep copy the data to protect its integrity
////        y = new double[YGOOD.Length];
////        x = new double[XGOOD.Length];
////        YGOOD.CopyTo(y, 0);
////        XGOOD.CopyTo(x, 0);
////        //set initial guesses
////        Params = new double[2];
////        Params = GuessParametersFromLinearFit();

////        //set the function delegate that returns needed information for marquadt minimizer
////        FuncDel = new Delegates.FunctionToFit(FuncForMarq);
////        FitModel();

////    }
////    public override void FitModel()
////    {
////        bool[] FitAll = new bool[2];
////        FitAll[0] = true; ;
////        FitAll[1] = true;
////        Fitter = new MarquadtMin(x, y, Params, FitAll, 1, FuncDel);
////        Params = Fitter.Parameters;
////        FittedAlready = true;
////    }
////    public override void GenerateFitLine(double spaceholder, double interval, double HighX, out double[] xvalues, out double[] yvalues)
////    {
////        //THE SPACEHOLDER IS STUCK IN THERE TO OVERRIDE THE LOWX VALUE, SO I DON'T HAVE TO MAKE A NEW BASE CLASS
////        double LowX = XOFFSET;
////        double curVal = LowX;
////        //now to determine the number of values in the interval
////        int ArraySize = Convert.ToInt32(((HighX - LowX) / interval));
////        //NigelMethods.Basics.debug("LOW VAL "+XOFFSET.ToString()+", HIGH VAL "+HighX.ToString());
////        xvalues = new double[ArraySize + 1];
////        yvalues = new double[ArraySize + 1];
////        for (int i = 0; i < ArraySize; i++)
////        {
////            xvalues[i] = curVal;
////            yvalues[i] = FunctiontoFit(curVal - XOFFSET);
////            curVal += interval;
////        }
////        xvalues[ArraySize] = HighX;
////        yvalues[ArraySize] = FunctiontoFit(HighX - XOFFSET);
////        //return base.GenerateFitLine(LowX, interval, HighX, ref xvalues, ref yvalues);
////    }
////    public override double FunctiontoFit(double x)
////    {
////        double y = Params[0] * Math.Exp(Params[1] * x);
////        return y;
////    }
////}
////#endregion
////LINEAR


//#region PowerFitWithoutDerivatives
//class PowerFitWithoutDerivatives : NonLinearFitterWithoutDerivatives
//{
//    //the first parameter is the constant, the second is the exponential growth rate
//    public PowerFitWithoutDerivatives(double[] XGOOD, double[] YGOOD, double guessA, double guessB)
//    {
//        name = "PowerNoDerivatives";
//        if ((XGOOD.Length < 2 || YGOOD.Length < 2) || (XGOOD.Length != YGOOD.Length))
//        { throw new Exception("Power fit can't work with less then 2 points or unequal matrices"); }
//        //deep copy the data to protect its integrity
//        y = new double[YGOOD.Length];
//        x = new double[XGOOD.Length];
//        YGOOD.CopyTo(y, 0);
//        XGOOD.CopyTo(x, 0);
//        //set initial guesses
//        Params = new double[2];
//        //Params[0] = YGOOD[0];
//        Params[0] = guessA;
//        if (Params[0] == 0.0)//Deal with the situation where the blank is a little too good
//        { Params[0] = .1; }
//        Params[1] = guessB;

//        FitModel();

//    }
//    public override void FitModel()
//    {
//        bool[] FitAll = new bool[2];
//        FitAll[0] = true; ;
//        FitAll[1] = true;
//        Fitter = new MarquadtMin(x, y, Params, FitAll, 1, FuncDel);

//        Params = Fitter.Parameters;
//        FittedAlready = true;
//    }
//    public override void GenerateFitLine(double spaceholder, double interval, double HighX, out double[] xvalues, out double[] yvalues)
//    {
//        //THE SPACEHOLDER IS STUCK IN THERE TO OVERRIDE THE LOWX VALUE, SO I DON'T HAVE TO MAKE A NEW BASE CLASS
//        double curVal = x.Min();
//        //now to determine the number of values in the interval
//        int ArraySize = Convert.ToInt32(((HighX - x.Min()) / interval));
//        //NigelMethods.Basics.debug("LOW VAL "+XOFFSET.ToString()+", HIGH VAL "+HighX.ToString());
//        xvalues = new double[ArraySize + 1];
//        yvalues = new double[ArraySize + 1];
//        //for (int i = 0; i < ArraySize; i++)
//        //{
//        //    xvalues[i] = curVal;
//        //    yvalues[i] = FunctiontoFit(curVal - XOFFSET);
//        //    curVal += interval;
//        //}
//        xvalues[ArraySize] = HighX;
//        yvalues[ArraySize] = FunctiontoFit(HighX);
//        //return base.GenerateFitLine(LowX, interval, HighX, ref xvalues, ref yvalues);
//    }
//    public override double FunctiontoFit(double x)
//    {
//        double y = Params[0] * Math.Pow(x, Params[1]);
//        return y;
//    }
//}
//#endregion
//#region PowerFit
//class PowerFit : NonLinearFitter
//{
//    //the first parameter is the constant, the second is the exponential growth rate
//    public double XOFFSET;//Determines the amount of x to subtract before adding the new stuff
//    public PowerFit(double[] XGOOD, double[] YGOOD, double guessA, double guessB)
//    {
//        name = "Power";
//        if ((XGOOD.Length < 2 || YGOOD.Length < 2) || (XGOOD.Length != YGOOD.Length))
//        { throw new Exception("Exponential fit can't work with less then 2 points or unequal matrices"); }
//        //deep copy the data to protect its integrity
//        y = new double[YGOOD.Length];
//        x = new double[XGOOD.Length];
//        YGOOD.CopyTo(y, 0);
//        XGOOD.CopyTo(x, 0);
//        //set initial guesses
//        Params = new double[2];
//        //Params[0] = YGOOD[0];
//        Params[0] = guessA;
//        if (Params[0] == 0.0)//Deal with the situation where the blank is a little too good
//        { Params[0] = .1; }
//        Params[1] = guessB;

//        //set the function delegate that returns needed information for marquadt minimizer
//        FuncDel = new Delegates.FunctionToFit(FuncForMarq);
//        FitModel();

//    }
//    public override void FitModel()
//    {
//        bool[] FitAll = new bool[2];
//        FitAll[0] = true; ;
//        FitAll[1] = true;
//        MarquadtMin Fitter = new MarquadtMin(x, y, Params, FitAll, 1, FuncDel);
//        Params = Fitter.Parameters;
//        FittedAlready = true;
//    }
//    public override void GenerateFitLine(double spaceholder, double interval, double HighX, out double[] xvalues, out double[] yvalues)
//    {
//        //THE SPACEHOLDER IS STUCK IN THERE TO OVERRIDE THE LOWX VALUE, SO I DON'T HAVE TO MAKE A NEW BASE CLASS
//        double LowX = XOFFSET;
//        double curVal = LowX;
//        //now to determine the number of values in the interval
//        int ArraySize = Convert.ToInt32(((HighX - LowX) / interval));
//        //NigelMethods.Basics.debug("LOW VAL "+XOFFSET.ToString()+", HIGH VAL "+HighX.ToString());
//        xvalues = new double[ArraySize + 1];
//        yvalues = new double[ArraySize + 1];
//        for (int i = 0; i < ArraySize; i++)
//        {
//            xvalues[i] = curVal;
//            yvalues[i] = FunctiontoFit(curVal - XOFFSET);
//            curVal += interval;
//        }
//        xvalues[ArraySize] = HighX;
//        yvalues[ArraySize] = FunctiontoFit(HighX - XOFFSET);
//        //return base.GenerateFitLine(LowX, interval, HighX, ref xvalues, ref yvalues);
//    }
//    public override void FuncForMarq(double x, double[] a, out double y, double[] dyda)
//    {
//        //f=a[0]*pow(x*a[1])
//        double ex = Math.Pow(x, a[1]);
//        y = a[0] * ex;
//        dyda[0] = ex;
//        dyda[1] = a[0] * ex * Math.Log(x);

//    }
//    public override double FunctiontoFit(double x)
//    {
//        //double y = Params[0] * Math.Exp(Params[1] * x);
//        double y = Params[0] * Math.Pow(x, Params[1]);
//        return y;
//    }
//}
//#endregion
////Exponential with Constant
////class ExponentialFitWithConstant : NonLinearFitter
////{
////    ///A+B*exp(-Ct)
////    //the first parameter is the constant, the second is the exponential growth rate
////    public double XOFFSET;//Determines the amount of x to subtract before adding the new stuff
////    public double GuessBlank=.03;//Best Guess from all our other planks 
////    public double[] GuessParameters()
////    {

////        double[] logdata = y.ToArray();
////        logdata = Array.ConvertAll(logdata, z => Math.Log(z-GuessBlank));
////        LinearFit LF = new LinearFit(x, logdata);
////        double[] ParamGuess = new double[3];
////        ParamGuess[0] = GuessBlank;
////        ParamGuess[1] = 1e-7;
////        ParamGuess[2] = .218;
////        return ParamGuess;
////    }
////    public override void FuncForMarq(double x, double[] a, out double y, double[] dyda)
////    {
////        //f=a[0]*exp(x*a[1])
////        y = 0.0;
////        double ex = Math.Exp(x * a[2]);
////        y = a[0]+a[1] * ex;
////        dyda[0] = 1;
////        dyda[1] = ex;//exp(r*t)
////        dyda[2] = x * y;//r*A*exp(r*t)
////    }
////    public ExponentialFitWithConstant(double[] XGOOD, double[] YGOOD)
////    {
////        name = "Exponential Fit with Constant";
////        if ((XGOOD.Length < 3 || YGOOD.Length < 3) || (XGOOD.Length != YGOOD.Length))
////        { throw new Exception("Exponential fit can't work with less then 3 points or unequal matrices"); }
////        //deep copy the data to protect its integrity
////        y = new double[YGOOD.Length];
////        x = new double[XGOOD.Length];
////        YGOOD.CopyTo(y, 0);
////        XGOOD.CopyTo(x, 0);
////        //set initial guesses
////        Params = new double[3];
////        Params = GuessParameters();

////        //set the function delegate that returns needed information for marquadt minimizer
////        FuncDel = new Delegates.FunctionToFit(FuncForMarq);
////        FitModel();

////    }
////    public override void FitModel()
////    {
////        bool[] FitAll = new bool[3];
////        Array.ConvertAll(FitAll,x => true);
////        Fitter = new MarquadtMin(x, y, Params, FitAll, 1, FuncDel);
////        Params = Fitter.Parameters;
////        FittedAlready = true;
////    }
////    public override void GenerateFitLine(double spaceholder, double interval, double HighX, out double[] xvalues, out double[] yvalues)
////    {
////        //THE SPACEHOLDER IS STUCK IN THERE TO OVERRIDE THE LOWX VALUE, SO I DON'T HAVE TO MAKE A NEW BASE CLASS
////        double LowX = XOFFSET;
////        double curVal = LowX;
////        //now to determine the number of values in the interval
////        int ArraySize = Convert.ToInt32(((HighX - LowX) / interval));
////        //NigelMethods.Basics.debug("LOW VAL "+XOFFSET.ToString()+", HIGH VAL "+HighX.ToString());
////        xvalues = new double[ArraySize + 1];
////        yvalues = new double[ArraySize + 1];
////        for (int i = 0; i < ArraySize; i++)
////        {
////            xvalues[i] = curVal;
////            yvalues[i] = FunctiontoFit(curVal - XOFFSET);
////            curVal += interval;
////        }
////        xvalues[ArraySize] = HighX;
////        yvalues[ArraySize] = FunctiontoFit(HighX - XOFFSET);
////        //return base.GenerateFitLine(LowX, interval, HighX, ref xvalues, ref yvalues);
////    }
////    public override double FunctiontoFit(double x)
////    {
////        double y = Params[0] +Params[1]* Math.Exp(Params[2] * x);
////        return y;
////    }


////}
//////NEEDED FUNCTION DELEGATE

//////MARQUADT MINIMUM
////#region MarquadtMin
////public class MarquadtMin
////{
////    //This function excepts a list of values, and a list of
////    const int DEFAULT_MAX_ITERATIONS = 1000;
////    const double DEFAULT_TOLERANCE = 1e-9;//percent change in chi square after which I cut it off
////    double SmallestErrorOfConcern = 0.0;
////    int NumParametersToFit, maxIterations;
////    bool CallConvergenceAfterUnchanged = true;
////    double chisq, tolerance;
////    double totalError = Double.MaxValue;
////    double[,] oneda, alpha, covar;
////    double[] atry, beta, da, Sigmas;
////    public double[] Parameters
////    { get { return parameters; } }//should probably return a deep copy of the array instead
////    //the internal parameters
////    double[] parameters;
////    //ia is an array of booleans indicating if the parameters should be fit
////    bool[] FitParameterAtIndex;
////    double alamda;
////    int NumDataPoints, NumOfParameters;
////    double[] x, y;
////    private Delegates.FunctionToFit Func;

////    private static double EstimateSigma(double ExpectedR2, double[] YVals, int parameterCount)
////    {
////        //static to make calling it from constructor easier

////        //This code is here to solve a problem I had,
////        //the file originally wanted the user to specify the sigma 
////        //but for any given dataset this is not always known,
////        //my impression is that the only point of sigma is to standardize the chi squared function, and thus
////        //in some sense to scale the delta factor for the parameters, however, I think it should be fine if I "guess" at the sigma by assuming
////        //that the data should have a error somewhere in the range of the expectedR2 inputed here, this could speed up convergence, but I do not believe it is critical.
////        //In fact, I am reasonably sure this is totally unnecessary, but want to be safe. (the book says minor or major fiddling with the alpha matrix is fine)
////        if (ExpectedR2 > 1 || ExpectedR2 < 0)
////        { throw new Exception("Expected R2 value is invalid"); }
////        double meany = YVals.Average();
////        //get total sum of squares
////        double SST = 0.0;
////        foreach (double val in YVals)
////        { SST += Math.Pow((val - meany), 2.0); }
////        //now estimate residual sum of squares
////        double SSE = SST * (1 - ExpectedR2);
////        //correct by degrees of freedom
////        SSE = SSE / (double)(YVals.Length - parameterCount);
////        double sig = Math.Sqrt(SSE);
////        return sig;
////    }
////    public MarquadtMin(double[] XVALS, double[] YVALS, double[] ParamGuess, bool[] toFit,
////        double EstimatedSigma, Delegates.FunctionToFit function)
////        : this(XVALS, YVALS, ParamGuess,
////            toFit, EstimatedSigma, function, DEFAULT_MAX_ITERATIONS, DEFAULT_TOLERANCE)
////    {
////        //DEFAULT PARAMETERS PASSED ON
////    }
////    public MarquadtMin(double[] XVALS, double[] YVALS, double[] ParamGuess, bool[] toFit, double EstimatedSigma, Delegates.FunctionToFit function, int MaxIterations, double Tolerance)
////    {
////        //THIS IS THE MAIN CONSTRUCTOR
////        //XVALS=X DATA
////        //YVALS = Y DATA
////        //PARAM GUESS =THIS IS AN ARRAY OF YOUR BEST GUESS OF THE PARAMETERS, WHERE THEY ARE ORDERED THE SAME AS THEY ARE RETURNED BY FUNCTION
////        //toFit =A BOOLEAN ARRAY WHOSE ELEMENT IS TRUE IF THE PARAMETER AT THAT POSITION SHOULD BE FIXED
////        //sigVal = AN ESTIMATE OF THE STANDARD ERROR IN THIS DATA (THE RMSE)
////        //function =A FUNCTION THAT CALCULATES A Y FROM X, AS WELL AS PARTIAL DERIVATIVES, SEE EXAMPLE IN EXPONENTIAL FITTER
////        //maxIterations = HOW MANY ITERATIONS OCCUR BEFORE AN ERROR IS THROWN
////        //tolerance = HOW LITTLE THE CHISQUARE CHANGES IN ORDER FOR CONVERGENCE TO BE DECLARED, EXITING THE FITTING ROUTINE
////        this.maxIterations = MaxIterations;
////        this.tolerance = Tolerance;
////        int TotalParameters = toFit.Length;
////        if (XVALS.Length != YVALS.Length)
////        { throw new Exception("X,Y data to be fit is of unequal length"); }

////        //create variables
////        NumDataPoints = XVALS.Length;
////        NumOfParameters = ParamGuess.Length;
////        covar = new Double[TotalParameters, TotalParameters];
////        parameters = ParamGuess;
////        FitParameterAtIndex = toFit;
////        alamda = -1;//a value that indicates to the fitter to undergo initialization
////        alpha = new Double[TotalParameters, TotalParameters];
////        //double[,] oneda = new Double[ma, ma];
////        Sigmas = new double[XVALS.Length];//Assuming constant error across all data for now 
////        for (int i = 0; i < Sigmas.Length; i++)
////        { Sigmas[i] = EstimatedSigma; }
////        //set the data smallest error to check for early convergence
////        SmallestErrorOfConcern = XVALS.Length * EstimateSigma(1 - 1e-5, YVALS, ParamGuess.Length);


////        //copy data to guarantee integrity
////        x = XVALS.ToArray();
////        y = YVALS.ToArray();
////        Func = function;
////        RunMinimization();
////    }
////    public MarquadtMin(double[] XVALS, double[] YVALS, double[] ParamGuess, bool[] toFit,
////        Delegates.FunctionToFit function, int MaxIterations, double Tolerance, double EstimatedR2)
////        : this(XVALS, YVALS, ParamGuess,
////            toFit, EstimateSigma(EstimatedR2, YVALS, ParamGuess.Length), function, DEFAULT_MAX_ITERATIONS, DEFAULT_TOLERANCE)
////    {
////        //THIS CONSTRUCTOR HAS NO SIGMA, SO IS TAKEN FROM ESTIMATE

////    }
////    public void RunMinimization()
////    {
////        int totalRuns;
////        double improvement, oldValue;
////        mrqmin();
////        if (totalError < SmallestErrorOfConcern)
////        { return; }
////        int term = 0;
////        int maxterm = 3;
////        int timesUnchanged = 0;
////        oldValue = chisq;
////        for (totalRuns = 0; totalRuns < maxIterations; totalRuns++)
////        {
////            mrqmin();
////            if (totalError < SmallestErrorOfConcern)
////            { return; }

////            if (chisq != oldValue)//if the chisquare is unchanged, means last attempt didn't improve anything, so
////            //the mrqmin() should be rerun with a new set of parameters to look for improvement
////            {
////                //otherwise, chisq is better than it was
////                improvement = (1 - chisq / oldValue);
////                if (improvement < tolerance)
////                {
////                    term++;
////                    if (term > maxterm)
////                    {
////                        return;
////                    }
////                }
////            }
////            else
////            { timesUnchanged++; }
////            oldValue = chisq;
////        }
////        if (timesUnchanged > 10)
////        {
////            return;
////        }
////        throw new Exception("Marquadt algorithm failed to converge after " + totalRuns.ToString() + " runs.");
////    }
////    //public static void mrqmin(double[] x, double[] y, double[] sig, ref double[] a, bool[] ia, ref double[,] covar, ref double[,] alpha, ref double chisq, ref double alamda)
////    public void mrqmin()
////    {
////        //summary below is out of date, lots of variables promoted to class level here, applies to old NR version 
////        /// <summary>
////        /// Levenberg-Marquardt method, attempting to reduce the value \chi^2 of a fit between a set of data
////        /// points x[0..ndata-1], y[0..ndata-1] with individual standard deviations sig[0..ndata-1],
////        /// and a nonlinear function dependent on ma coefficients a[0..ma-1]. The input array ia[0..ma-1]
////        /// indicates by true entries those components of a that should be fitted for, and by false 
////        /// entries those components that should be held fixed at their input values. The program returns
////        /// current best-fit values for the parameters a[0..ma-1], and \chi^2 = chisq. The arrays
////        /// covar[0..ma-1,0..ma-1], alpha[0..ma-1,0..ma-1] are used as working space during most
////        /// iterations. Supply a routine funcs(x,a) that evaluates the fitting function
////        /// yfit=dyda[ma], and its derivatives dyda[0..ma-1] with respect to the fitting parameters a at x. On
////        /// the first call provide an initial guess for the parameters a, and set alamda less than 0 for initialization
////        /// (which then sets alamda=0.001). If a step succeeds chisq becomes smaller and alamda decreases
////        /// by a factor of 10. If a step fails alamda grows by a factor of 10. You must call this
////        /// routine repeatedly until convergence is achieved. Then, make one final call with alamda=0, so
////        /// that covar[0..ma-1,0..ma-1] returns the covariance matrix, and alpha the curvature matrix.
////        /// (Parameters held fixed will return zero covariances.)
////        /// </summary>

////        int j, k, l;
////        double newchisq;
////        if (alamda < 0.0)//initialize if first run
////        {
////            atry = new double[NumOfParameters];
////            beta = new double[NumOfParameters];
////            da = new double[NumOfParameters];
////            //decide how many I am fitting
////            NumParametersToFit = 0;
////            for (j = 0; j < NumOfParameters; j++)
////                if (FitParameterAtIndex[j]) NumParametersToFit++;
////            oneda = new double[NumParametersToFit, 1];
////            alamda = 0.00001;
////            chisq = mrqcof(Sigmas, parameters, alpha);
////            for (j = 0; j < NumOfParameters; j++) atry[j] = parameters[j];
////        }

////        for (j = 0; j < NumParametersToFit; j++)
////        {
////            for (k = 0; k < NumParametersToFit; k++)
////            { covar[j, k] = alpha[j, k]; }
////            covar[j, j] = alpha[j, j] * (1.0 + alamda);
////            oneda[j, 0] = beta[j];
////        }
////        gaussj(covar, oneda);//get matrix solution 
////        for (j = 0; j < NumParametersToFit; j++)
////        {
////            da[j] = oneda[j, 0];//update 
////        }
////        if (alamda == 0.0)
////        {
////            //If rerun with alambda as 0, this turns alpha into the curvature matrix, and covar into the covariance matrix
////            //in practice I never use these, and this is never called
////            covsrt(covar, FitParameterAtIndex, NumParametersToFit);
////            covsrt(alpha, FitParameterAtIndex, NumParametersToFit);
////            return;
////        }
////        //if not rerun as zero, check the result for success!
////        for (j = 0, l = 0; l < NumOfParameters; l++)
////        {
////            if (FitParameterAtIndex[l]) { atry[l] = parameters[l] + da[j++]; }//update parameters to give it a try
////        }
////        newchisq = mrqcof(Sigmas, atry, covar);//retry it all 
////        if (newchisq < chisq)//improvement in chisquare
////        {
////            alamda *= 0.1;//drop the step down
////            chisq = newchisq;
////            for (j = 0; j < NumParametersToFit; j++)
////            {
////                for (k = 0; k < NumParametersToFit; k++)
////                { alpha[j, k] = covar[j, k]; }
////                beta[j] = da[j];
////            }
////            for (l = 0; l < NumOfParameters; l++) parameters[l] = atry[l];//success move on!
////        }
////        else//no improvment, back up and give it a go again
////        {
////            alamda *= 10.0;
////        }
////    }
////    private double mrqcof(double[] sig, double[] ParamsToTry, double[,] LocalAlpha)
////    {
////        //returns chi square
////        double LocalChiSquare;
////        double LocalTotalError;

////        int i, j, k, l, m;
////        double ymod;
////        double wt, sig2i, dy;
////        double[] dyda = new double[NumOfParameters];//is function of 
////        //set them all to zero, I suppose this is better then allocating new memory

////        for (j = 0; j < NumParametersToFit; j++)
////        {
////            for (k = 0; k <= j; k++) LocalAlpha[j, k] = 0.0;
////            beta[j] = 0.0;
////        }
////        LocalChiSquare = LocalTotalError = 0.0;
////        //now to recreate the alpha matrix, essentially the second derivative matrix first
////        //but for mathematical reasons, the element at position k,l is the partial derivative of the function
////        //with respect to k times the partial derivative with respect to l, all over sig squared.
////        for (i = 0; i < NumDataPoints; i++)//loop over data
////        {
////            Func(x[i], ParamsToTry, out ymod, dyda); //calculate y and dyda, could use return type
////            sig2i = 1.0 / (sig[i] * sig[i]);//this seems rather wasteful, as it is always the same value one initalized,
////            //leaving it now in case I decide to have a "dynamically" estimate sig later
////            dy = y[i] - ymod;
////            LocalTotalError += Math.Abs(dy);
////            for (j = 0, l = 0; l < NumOfParameters; l++)//loop over parameters
////            {
////                if (FitParameterAtIndex[l])
////                {
////                    wt = dyda[l] * sig2i;//weight factor for this guy times its partial derivative
////                    for (k = 0, m = 0; m < l + 1; m++)
////                    {
////                        if (FitParameterAtIndex[m])
////                        {
////                            LocalAlpha[j, k++] += wt * dyda[m]; //mutiply it by every other to fill in the approximate hessian/alpha matrix
////                        }
////                    }
////                    beta[j++] += dy * wt;
////                }
////            }
////            LocalChiSquare += dy * dy * sig2i;//add the squared error divided by the expected deviation to the chisquare variance
////        }
////        for (j = 1; j < NumParametersToFit; j++)//fill in the symmetric triangle of the matrix
////            for (k = 0; k < j; k++) LocalAlpha[k, j] = LocalAlpha[j, k];
////        if (LocalTotalError < totalError)
////        {
////            //should really assign this in another way for consistency, assuming parameters are updated
////            //outside of this funciton to match this error following an improvement
////            totalError = LocalTotalError;
////        }
////        return LocalChiSquare;
////    }
////    public static void gaussj(double[,] a, double[,] b)
////    {
////        //I understand this routine uses gauss jordan elimination to solve
////        //a ins a square input matrix, and b is returned as the solution, 
////        //while a is returned as its inverse
////        int i, irow, icol, j, k, l, ll;
////        i = irow = j = k = icol = l = ll = 0;
////        double big, dum, pivinv;
////        int n = a.GetUpperBound(0) + 1;
////        int m = b.GetLowerBound(0) + 1;
////        int[] indxc = new int[n];
////        int[] indxr = new int[n];
////        int[] ipiv = new int[n];
////        for (j = 0; j < n; j++) { ipiv[j] = 0; }
////        for (i = 0; i < n; i++)
////        {
////            big = 0.0;
////            for (j = 0; j < n; j++)
////            {
////                if (ipiv[j] != 1)
////                {
////                    for (k = 0; k < n; k++)
////                    {
////                        if (ipiv[k] == 0)
////                        {
////                            if (Math.Abs(a[j, k]) >= big)
////                            {
////                                big = Math.Abs(a[j, k]);
////                                irow = j;
////                                icol = k;
////                            }
////                        }
////                        else if (ipiv[k] > 1)
////                        {
////                            throw new Exception("Singular Matrix, can't be solved with Gauss Jordan");
////                        }
////                    }
////                }
////            }
////            ++(ipiv[icol]);
////            if (irow != icol)
////            {
////                for (l = 0; l < n; l++) { bSwap(ref a[irow, l], ref a[icol, l]); }
////                for (l = 0; l < m; l++) { bSwap(ref b[irow, l], ref b[icol, l]); }
////            }
////            indxr[i] = irow;
////            indxc[i] = icol;
////            if (a[icol, icol] == 0.0) throw new System.Exception("gaussj: Singular Matrix");
////            pivinv = 1.0 / a[icol, icol];
////            a[icol, icol] = 1.0;
////            for (l = 0; l < n; l++) { a[icol, l] *= pivinv; }
////            for (l = 0; l < m; l++) { b[icol, l] *= pivinv; }
////            for (ll = 0; ll < n; ll++)
////                if (ll != icol)
////                {
////                    dum = a[ll, icol];
////                    a[ll, icol] = 0.0;
////                    for (l = 0; l < n; l++) { a[ll, l] -= a[icol, l] * dum; }
////                    for (l = 0; l < m; l++) { b[ll, l] -= b[icol, l] * dum; }
////                }
////        }
////        for (l = n - 1; l >= 0; l--)
////        {
////            if (indxr[l] != indxc[l])
////                for (k = 0; k < n; k++)
////                    bSwap(ref a[k, indxr[l]], ref a[k, indxc[l]]);
////        }
////    }
////    private static void covsrt(double[,] covar, bool[] ia, int mfit)
////    {
////        int i, j, k;
////        int ma = ia.Length;
////        for (i = mfit; i < ma; i++)
////            for (j = 0; j < i + 1; j++) covar[i, j] = covar[j, i] = 0.0;
////        k = mfit - 1;
////        for (j = ma - 1; j >= 0; j--)
////        {
////            if (ia[j])
////            {
////                for (i = 0; i < ma; i++) bSwap(ref covar[i, k], ref covar[i, j]);
////                for (i = 0; i < ma; i++) bSwap(ref covar[k, i], ref covar[j, i]);
////                k--;
////            }
////        }
////    }
////    private static void bSwap(ref double a, ref double b)
////    {
////        // swaps a with b
////        double temp = a;
////        a = b;
////        b = temp;
////    }
////}
////#endregion
////SIMPLE RECIPES, MOSTLY OUTDATED BY C# 3.0
