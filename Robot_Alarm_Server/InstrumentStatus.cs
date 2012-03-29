using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Robot_Alarm
{
    [Serializable]
    public class InstrumentStatus
    {
        public string Status;
        public DateTime TimeCreated;
        public InstrumentStatus(string StatusDescription)
        {
            Status = StatusDescription;
            TimeCreated = DateTime.Now;
        }
    }
    [Serializable]
    public class AlarmState
    {
        public bool AlarmOn;
        public DateTime TimeTurnedOn;
        public AlarmState(bool TurnOn)
        {
            if (TurnOn)
            {
                TimeTurnedOn = DateTime.Now;
                AlarmOn = true;
            }
            else
            {
                AlarmOn = false;
            }
        }
    }

 
}
