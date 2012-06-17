using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarity
{
    /// <summary>
    /// This is an interface that defines types that can be passed to methods, required for a clarity engine
    /// namely the events it must allow one to subscribe to and the methods to start/stop, etc.  
    /// 
    /// The InstrumentManagerClass implements this interface, though in the future it might be on a remote computer
    /// or in any other class.
    /// </summary>
    public interface InstrumentManager
    {
        event ProtocolPauseEventHandler OnProtocolPaused;
        event InstrumentManagerEventHandler OnAllRunningProtocolsEnded;
        event InstrumentManagerErrorHandler OnGenericError;
        event InstrumentManagerErrorHandler OnErrorDuringProtocolExecution;
        event InstrumentManagerEventHandler OnProtocolSuccessfullyCancelled;
        event InstrumentManagerEventHandler OnProtocolExecutionUpdates;
        event InstrumentManagerEventHandler OnInstrumentStatusUpdate;
        event InstrumentManagerEventHandler OnProtocolsStarted;
        
        void StartProtocolExecution();
        void RequestProtocolCancellation();
        void ShutdownEngine();
        void AddProtocol(Protocol toAdd);
        ProtocolRemoveResult RemoveProtocol(Protocol toRemove);

        BaseInstrumentClass ReturnInstrument(string InstrumentName);
        T ReturnInstrumentType<T>() where T : BaseInstrumentClass ;
        Alarm GiveAlarmReference();
        
        StaticProtocolItem GetLastFailedInstruction();
        ProtocolManager LoadedProtocols
        { get; }
        Alarm Clarity_Alarm { get; }
    }
}
