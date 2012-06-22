using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clarity
{
    public class ProtcolExcecutionError : Exception
    {
        public string NameOfFailedMethod, NameOfFailedInstrument;

        public ProtcolExcecutionError(string InstrumentName, string MethodName, Exception innerException)
            : base("Could Not Run " + InstrumentName, innerException)
        {
            this.NameOfFailedInstrument = InstrumentName;
            this.NameOfFailedMethod = MethodName;

        }
        public string MakeErrorReport()
        {
            string ErrorReport = "Unhandled error occurred during invocation of method " + NameOfFailedMethod
               + " on instrument " + NameOfFailedInstrument;// +"\nException error is: " + InnerException.Message + "\n\n";
            if (InnerException != null)
            {
                object Test = (object)InnerException;
                if (Test is InstrumentError)
                {
                    InstrumentError IE = (InstrumentError)Test;
                    ErrorReport += "Problem thrown by instrument " + IE.InstrumentInError.ToString() + "\n";
                    ErrorReport += "Problem occured at " + IE.TimeThrown.ToString() + "\n";
                    ErrorReport += "During invokation of method " + NameOfFailedMethod + "\n";
                    ErrorReport += "Exception message is: " + IE.ErrorDescription + "\n";
                    if (IE.Message != null)
                    { ErrorReport += "Exception inner messages is: " + IE.Message + "\n\n\n"; }
                }
                else
                {
                    ErrorReport += InnerException.Message;
                }
                if (InnerException.InnerException != null)
                {
                    ErrorReport += "Further Error Message: " + InnerException.InnerException.Message + "\n";
                }
            }
            return ErrorReport;
        }
    }
    public class  InstrumentError :Exception
    {
        public string ErrorDescription;
        public DateTime TimeThrown;
        public bool CanRecover;
        //public object InstrumentInError;
        public BaseInstrumentClass InstrumentInError;
        
        public InstrumentError(string errorDescription, bool NotActuallyAnInstrumentProblem,BaseInstrumentClass OriginatingInstrument): base(errorDescription)
        {
            //NotActuallyAnInstrumentProblem is to indicate if the command to the machine was bad, or if the machine cannot process the commands;

            this.ErrorDescription = errorDescription;
            this.TimeThrown= System.DateTime.Now;
            this.CanRecover = NotActuallyAnInstrumentProblem;
            this.InstrumentInError = OriginatingInstrument;
        }
        public InstrumentError(string errorDescription, InstrumentError innerException=null):base(errorDescription,innerException)
        {

            this.ErrorDescription = errorDescription;
            if (innerException != null)
            {
                this.InstrumentInError = innerException.InstrumentInError;
                this.CanRecover = innerException.CanRecover;
                this.TimeThrown = innerException.TimeThrown;
            }
            else
            {
                this.CanRecover = false;
                this.TimeThrown = DateTime.Now;
            }
            
        }
        /// <summary>
        /// Generic constructor, assumes error is recoverable
        /// </summary>
        /// <param name="errorDescription"></param>
        /// <param name="innerExcept"></param>
        public InstrumentError(string errorDescription,BaseInstrumentClass OriginatingInstrument, Exception innerExcept):base(errorDescription,innerExcept)
        {
            this.InstrumentInError = OriginatingInstrument;
            this.TimeThrown = System.DateTime.Now;
            this.ErrorDescription = errorDescription;
        }
    }
}
