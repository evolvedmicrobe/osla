using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Growth_Curve_Software
{
    public class Alarm
    {
        //Class for turning on and off the remote alarm
        AlarmClient AC;
        public enum AlarmState{On,Off,Disconnected};
        public bool Connected;
        public Alarm()
        {
                try
                {
                    AC = new AlarmClient();
                    Connected = true;
                }
                catch { Connected = false; }
        }
        public void TurnOnAlarm(string StatusMessage)
        {
            if (Connected)
            {
                try
                {
                    AC.TurnOnAlarm();
                    AC.UpdateStatus(StatusMessage);
                }
                catch { Connected = false; }
            }
        }
        public void TurnOnAlarm()
        {
            TurnOnAlarm("Alarm On");
        }
        public void TurnOffAlarm(string StatusUpdate)
        {
            try { AC.TurnOffAlarm(); AC.UpdateStatus(StatusUpdate); }
            catch { Connected = false; }
        }
        public void TurnOffAlarm()
        {
            TurnOffAlarm("Alarm off");
        }
        public void ChangeStatus(string StatusMessage)
        {
            try { AC.UpdateStatus(StatusMessage); }
            catch { Connected = false; }
        }
        public AlarmState IsAlarmOn()
        {
            try
            {
                bool status= AC.GetAlarmStatus().AlarmOn;
                if (status) { return AlarmState.On; }
                else { return AlarmState.Off; }
            }
            catch
            {
                return AlarmState.Disconnected;
            }
        }
    }
}
