using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Growth_Curve_Software
{
    /// <summary>
    /// For methods that need arguments available at runtime 
    /// such as other instruments and the calling protocol
    /// this is a class that is passed in to such methods
    /// 
    /// NEEDS TO ALWAY BE LAST ARGUMENT TO A METHOD!!!
    /// </summary>
    public class AdditionalMethodArguments
    {
        public InstrumentManager InstrumentCollection;
        public Protocol CallingProtocol;
       
       
    }
}
