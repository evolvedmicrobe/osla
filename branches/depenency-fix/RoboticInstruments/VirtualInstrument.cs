using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Growth_Curve_Software
{
    public class VirtualInstrument : BaseInstrumentClass
    {
        /// <summary>
        /// This is a variable to indicate whether the arguments for this virtual class
        /// also obtain the calling protocol, thereby allowing them to call various things.
        /// </summary>
        public bool ArgumentsTakeProtocol;
        public VirtualInstrument() :base()
        {
            this.StatusOK = true;
        }
        public override bool CloseAndFreeUpResources()
        {
            return true;
        }
        public override bool AttemptRecovery()
        {
            return true;
        }
        public override bool AttemptRecovery(InstrumentError Error)
        {
            return true;
        }
    }
}
