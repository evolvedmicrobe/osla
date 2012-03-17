using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Growth_Curve_Software
{
    /// <summary>
    /// This is an interface that defines types that can be passed to virtual methods, does stuff like get
    /// insturments, etc.
    /// </summary>
    public interface InstrumentManager
    {
        BaseInstrumentClass ReturnInstrument(string InstrumentName);
        T ReturnInstrumentType<T>() where T : BaseInstrumentClass ; 
        /// <summary>
        /// Run the method of an instrument
        /// </summary>
        /// <param name="instrumentName">name of instrument</param>
        /// <param name="methodName">name of method</param>
        /// <param name="args">arguments to method, default is none</param>
        void RunMethod(string instrumentName, string methodName, object[] args = null);
        
    }
}
