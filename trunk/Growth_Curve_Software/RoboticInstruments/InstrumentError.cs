using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clarity
{
    public class InstrumentError : ApplicationException
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
                this.InstrumentInError = innerException.InstrumentInError;
            this.TimeThrown = innerException.TimeThrown;
            this.CanRecover = innerException.CanRecover;
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
