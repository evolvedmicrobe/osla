using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarity
{
    /// <summary>
    /// This is an interface that defines types that can be passed to virtual methods, does stuff like get
    /// insturments, etc.
    /// </summary>
    public interface InstrumentManager
    {
        BaseInstrumentClass ReturnInstrument(string InstrumentName);
        T ReturnInstrumentType<T>() where T : BaseInstrumentClass ;
        Alarm GiveAlarmReference();
    }
}
