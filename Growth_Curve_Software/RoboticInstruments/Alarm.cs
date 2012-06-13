using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Clarity.AlarmServer;
namespace Clarity
{
    /// <summary>
    /// To make a client:
    /// 
    /// Allow the program to use an address:
    ///     netsh http add urlacl url=http://+:8001/AlarmNotifier user=DOMAIN\user
    ///     
    /// Now follow: http://msdn.microsoft.com/en-us/library/ms733133.aspx
    /// </summary>
    public class Alarm
    {
        //Class for turning on and off the remote alarm
        AlarmServer.AlarmClient AC;
        public enum AlarmState { On, Off, Disconnected };
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
        public void SetProtocolData(List<Tuple<string, string, string, int>> Data)
        {
            AC.SetCurrentlyLoadedProtocolData(Data);
        }
        public AlarmState IsAlarmOn()
        {
            try
            {
                bool status = AC.GetAlarmStatus().AlarmOn;
                if (status) { return AlarmState.On; }
                else { return AlarmState.Off; }
            }
            catch
            {
                return AlarmState.Disconnected;
            }
        }
        public bool ValidNumbers(string numbers)
        {
            try
            {
                if (AC != null && Connected)
                {
                    return AC.ValidNumbers(numbers);
                }
                else { return false; }
            }
            catch
            { return false; }
        }
        public bool CallConnects(string number)
        {
            try
            {
                if (AC != null && Connected)
                {
                    return AC.CallConnects(number);
                }
                else { return false; }
            }
            catch
            { return false; }
        }
    }
}
