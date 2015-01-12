using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using SKYPE4COMLib;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace Robot_Alarm
{
    public struct ProtocolData
    {
        public string name, emails, phones;
        public int maxdelay;
        public ProtocolData(Tuple<string, string, string, int> t)
        {
            this.name = t.Item1;
            this.emails = t.Item2;
            this.phones = t.Item3;
            this.maxdelay = t.Item4;
        }
    }
    [Serializable]
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.Single)]
    public class AlarmNotifier : IAlarm
    {
        public const int IMAGE_HEIGHT = 225;
        public const int IMAGE_WIDTH = 300;
        public const int MAX_CALLS = 3;
        public static Bitmap Image1;
        public static Bitmap Image2;
        public string Image1UpdateTime;
        public string Image2UpdateTime;
        public string CurrentOperation = "None Set";
        public static AlarmState CurrentAlarmState;
        public static InstrumentStatus CurrentInstrumentStatus;
        public AlarmNotifier()
        {
            CurrentAlarmState = new AlarmState(false);
            CurrentInstrumentStatus = new InstrumentStatus("Monitoring Not Yet Initialized");
            Image1 = new Bitmap(@"test.bmp");
            Image2 = new Bitmap(@"test.bmp");
            Image1UpdateTime = DateTime.Now.ToString();
            Image2UpdateTime = DateTime.Now.ToString();
            try
            {
                Skype skype = createSkypeClient();
                if (!skype.Client.IsRunning)
                {
                    skype.Client.Start(true, true);
                }
            }
            catch (Exception thrown)
            {
                Console.WriteLine(DateTime.Now.ToString() + " Could not start skype or create DTMF event. " + thrown.Message);
            }
        }

        public InstrumentStatus GetInstrumentStatus()
        {
            return CurrentInstrumentStatus;
        }
        public AlarmState GetAlarmStatus()
        {
            return CurrentAlarmState;
        }
        public void TurnOnAlarm()
        {
            CurrentAlarmState = new AlarmState(true);
            ReportToAllUsers("The robot alarm has been activated!");
            Console.WriteLine(DateTime.Now.ToString() + " Alarm Turned On");
        }
        public void TurnOffAlarm()
        {
            CurrentAlarmState = new AlarmState(false);
            Console.WriteLine(DateTime.Now.ToString() + " Alarm Turned Off");
        }
        public void UpdateStatus(string Status)
        {
            CurrentInstrumentStatus = new InstrumentStatus(Status);
            Console.WriteLine(DateTime.Now.ToString() + " Updated Instrument Status");
        }


        public void UpdateOperation(string Operation) { CurrentOperation = Operation; }
        public string GetOperation() { return CurrentOperation; }

        // Each protocol is associated with a name, email list, phone number, and max delay time list
        public static List<ProtocolData> CurrentlyLoadedProtocolData = new List<ProtocolData>();
        public List<string> GetCurrentlyLoadedProtocolNames()
        {
            return CurrentlyLoadedProtocolData.Select(x => x.name).ToList();
        }
        public void SetCurrentlyLoadedProtocolData(List<Tuple<string, string, string, int>> Data)
        {
            CurrentlyLoadedProtocolData = Data.Select(x => new ProtocolData(x)).ToList();
            Console.WriteLine(DateTime.Now.ToString() + " Set Protocol Data");
        }

        #region SkypeAlarm
        public SmtpClient createSmtpClient()
        {
            //IF THIS FAILS, IT IS LIKELY DUE TO THE MCAFEE VIRUS SCANNER
            //CHANGE THE ACCESS PROTECTION TO ALLOW AN EXCEPTION FOR THE PROGRAM
            SmtpClient ToSend = new SmtpClient("smtp.gmail.com");//need to fill this in
            ToSend.UseDefaultCredentials = false;
            ToSend.Port = 587;
            ToSend.EnableSsl = true;
            ToSend.Credentials = new NetworkCredential("clarity.lab.automation@gmail.com", "icontrolrobots");
            return ToSend;

        }
        public Skype createSkypeClient()
        {
            Skype skype = new Skype();
            try
            {
                if (!skype.Client.IsRunning)
                {
                    skype.Client.Start(true, true);
                }
            }
            catch (Exception thrown)
            {
                Console.WriteLine(DateTime.Now.ToString() + " Could not start skype or create DTMF event. " + thrown.Message);
            }
            return skype;
        }
        static string number_re = @"^([0-9]{10};? *)+$";
        public bool ValidNumbers(string numbers)
        {
            Match m = Regex.Match(numbers, number_re);
            if (!m.Success) { return false; }
            return true;
        }
        public bool CallConnects(string number)
        {
            bool callSuccessful = false;
            try
            {
                Skype skype = createSkypeClient();
                Call c;
                c = skype.PlaceCall(number);
                // TODO: Use some kind of call property to avoid waiting for calls that fail instantly
                while (c.Status != TCallStatus.clsInProgress)
                {
                    TCallStatus curStatus = c.Status;
                    if (curStatus == TCallStatus.clsFailed | curStatus == TCallStatus.clsRefused
                        || curStatus == TCallStatus.clsCancelled || curStatus == TCallStatus.clsBusy ||
                        curStatus == TCallStatus.clsBusy)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(500);
                }
                if (c.Status == TCallStatus.clsInProgress)
                {
                    c.Finish();
                }
            }
            catch (Exception thrown)
            {
                Console.WriteLine("Could not access skype!\n\n" + thrown.Message);
            }
            return callSuccessful;
        }
        public void EmailUser(string address, string message)
        {
            //IF THIS FAILS, IT IS LIKELY DUE TO THE MCAFEE VIRUS SCANNER
            //CHANGE THE ACCESS PROTECTION TO ALLOW AN EXCEPTION FOR THE PROGRAM
            String senderAddress = "clarity.lab.automation@gmail.com";
            try
            {
                MailMessage email = new MailMessage(senderAddress, address, "Robot Alert", message);
                SmtpClient ToSend = createSmtpClient();
                ToSend.Send(email);
            }
            catch (Exception thrown)
            {
                Console.WriteLine(thrown.Message);

            }
        }
        public void CallAllUsers()
        {
            HashSet<string> unique_numbers = new HashSet<string>();
            foreach (ProtocolData p in AlarmNotifier.CurrentlyLoadedProtocolData)
            {
                if (!String.IsNullOrEmpty(p.phones))
                {
                    foreach (string number in p.phones.Split(';'))
                    {
                        unique_numbers.Add(number);
                    }
                }
            }
            foreach (string n in unique_numbers)
            {
                int tries = 0;
                while (!CallConnects(n))
                {
                    tries += 1;
                    if (tries >= MAX_CALLS)
                    {
                        break;
                    }

                }
            }
        }
        public void EmailAllUsers(string ErrorMessage = "The robot has an error, and has stopped working")
        {
            HashSet<string> unique_emails = new HashSet<string>();
            foreach (ProtocolData p in AlarmNotifier.CurrentlyLoadedProtocolData)
            {
                if (!String.IsNullOrEmpty(p.emails))
                {
                    string[] emails = p.emails.Split(';');
                    foreach (string emailaddress in emails)
                    {
                        unique_emails.Add(emailaddress);
                    }
                }
            }
            foreach (string e in unique_emails) EmailUser(e, ErrorMessage);
        }
        public void ReportToAllUsers(string message = "The robot has an error, and has stopped working")
        {
            Console.WriteLine(DateTime.Now.ToString() + " Reported to all users: " + message);
            EmailAllUsers(message);
            CallAllUsers(); 
        }
        #endregion
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    //[ServiceKnownType(typeof(System.Drawing.Bitmap))]
    [ServiceKnownType(typeof(byte[]))]
    [ServiceKnownType(typeof(System.Drawing.Imaging.Metafile))]
    //[ServiceKnownType(typeof(System.Collections.Generic.List<string>))]
    public interface IAlarm
    {
        [OperationContract]
        InstrumentStatus GetInstrumentStatus();
        [OperationContract]
        AlarmState GetAlarmStatus();
        [OperationContract]
        void TurnOnAlarm();
        [OperationContract]
        void TurnOffAlarm();
        [OperationContract]
        void UpdateStatus(string Status);
        [OperationContract]
        List<string> GetCurrentlyLoadedProtocolNames();
        [OperationContract]
        void SetCurrentlyLoadedProtocolData(List<Tuple<string, string, string, int>> Data);
        [OperationContract]
        void UpdateOperation(string Operation);
        [OperationContract]
        string GetOperation();

        #region SkypeAlarm
        [OperationContract]
        bool ValidNumbers(string numbers);
        [OperationContract]
        bool CallConnects(string number);
        #endregion
    }

}
