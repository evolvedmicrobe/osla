using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarity
{
    public delegate void delVoidVoid();
    public delegate void del_InstrumentError(BaseInstrumentClass BEC, string Error);
    /// <summary>
    /// These are methods which are exposed and can be 
    /// called on dynamically, all methods that can run
    /// during protocol execution should have this attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UserCallableMethod : System.Attribute
    {
        public bool RequiresInstrumentManager=false;
        public bool RequiresCurrentProtocol=false;
    }
    
}
