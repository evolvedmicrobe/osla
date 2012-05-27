using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Growth_Curve_Software.AlarmServer;
namespace Growth_Curve_Software
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
        public DateTime getValidationTimeForProtocol(string ProtocolName)
        {
            if (Connected && AC != null)
            {
                return AC.GetValidationTimeOfProtocol(ProtocolName);
            }
            else { return DateTime.Now; }//not the best here, assuming is working
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
        public void SetProtocolNames(List<string> Names)
        {
            AC.SetCurrentlyLoadedProtocolNames(Names);
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
        public DateTime GetProtocolTime(string ProtocolName)
        {
            try
            {
                if (AC != null && Connected)
                {
                    return AC.GetValidationTimeOfProtocol(ProtocolName);
                }
                else { return DateTime.Now; }
            }
            catch
            { return DateTime.Now; }
        }
        public bool ValidNumbers(string numbers)
        {
            try
            {
                if (AC != null && Connected)
                {
                    return AC.TestNumbers(numbers);
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
