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

namespace Robot_Alarm
{

    [Serializable]
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.Single)]
    public class ValidationStatus
    {
       public bool Validated; public DateTime TimeValidated;
    }
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class AlarmNotifier : IAlarm

    {
        public const int IMAGE_HEIGHT = 225;
        public const int IMAGE_WIDTH = 300;
        public  static Bitmap Image1;
        public  static Bitmap Image2;
        static Dictionary<string,ValidationStatus> ValidatedProtocols;
        public static List<string> CurrentlyLoadedProtocolNames;
        public string Image1UpdateTime;
        public string Image2UpdateTime;
        public string CurrentOperation="None Set";
        public static AlarmState CurrentAlarmState;
        public static InstrumentStatus CurrentInstrumentStatus;
        public AlarmNotifier()
        {
            CurrentAlarmState = new AlarmState(false);
            CurrentInstrumentStatus = new InstrumentStatus("Monitoring Not Yet Initialized");
            if (ValidatedProtocols == null)
            {
                ValidatedProtocols = new Dictionary<string, ValidationStatus>();
                CurrentlyLoadedProtocolNames = new List<string>();
                CurrentlyLoadedProtocolNames.Add("No Protocols Loaded Yet");
            }
            Image1 = new Bitmap(@"test.bmp");
            Image2 = new Bitmap(@"test.bmp");
            Image1UpdateTime=DateTime.Now.ToString();
            Image2UpdateTime=DateTime.Now.ToString();
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
            CurrentAlarmState=new AlarmState(true);
        }
        public void TurnOffAlarm()
        {
            CurrentAlarmState = new AlarmState(false);
        }
        public void UpdateStatus(string Status)
        {
            CurrentInstrumentStatus = new InstrumentStatus(Status);
        }
       

        public System.Drawing.Bitmap GetCameraImage1(out string updateTime)
        {
            //for some reason, once the object is serilized, it is screwed up locally,
            //so going to have to clone it
            Bitmap BMP = Image1.Clone() as Bitmap;
            updateTime = Image1UpdateTime;
            return BMP;
        }
        public System.Drawing.Bitmap GetCameraImage2(out string UpdateTime)
        {
            Bitmap BMP = Image2.Clone() as Bitmap;
            UpdateTime = Image2UpdateTime;
            return BMP;
        }
        public void SetCameraImage1(System.Drawing.Bitmap Image)
        {
            //same crap with it getting all corrupted, so must be cloned
            Image1 = Image.Clone() as Bitmap;
            
            Image1UpdateTime = DateTime.Now.ToString();
        }
        public byte[] ReturnJPEGCamera1(out string updateTime)
        {
            updateTime=Image1UpdateTime;
            MemoryStream MS = new MemoryStream(15000);
            Bitmap newImage = Image1.Clone() as Bitmap;            
            newImage.Save(MS,ImageFormat.Jpeg);
            return MS.ToArray();
        }
        public byte[] ReturnJPEGCamera2(out string updateTime)
        {
            updateTime=Image2UpdateTime;
            MemoryStream MS = new MemoryStream(15000);
            Bitmap newImage = Image2.Clone() as Bitmap;            
            newImage.Save(MS,ImageFormat.Jpeg);
            return MS.ToArray();
        } 
        public void SetCameraImage2(System.Drawing.Bitmap Image)
        {
            Image2 = Image.Clone() as Bitmap;
            Image2UpdateTime = DateTime.Now.ToString();            
        }
        public int GetImageHeight()
        {
            return IMAGE_HEIGHT;
        }
        public int GetImageWidth()
        {
            return IMAGE_WIDTH;
        }
        public List<string> GetCurrentlyLoadedProtocolNames()
        {
            return CurrentlyLoadedProtocolNames;
        }
        public void SetCurrentlyLoadedProtocolNames(List<string> Names)
        {
            Dictionary<string,ValidationStatus> ValidationStates=new Dictionary<string,ValidationStatus>();
            foreach(string name in Names)
            {
                if (ValidatedProtocols.ContainsKey(name)) { ValidationStates[name] = ValidatedProtocols[name]; }
                else { 
                    ValidationStates[name] = new ValidationStatus();
                    ValidationStates[name].TimeValidated = DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0)); }
            }
            CurrentlyLoadedProtocolNames=Names;
            ValidatedProtocols=ValidationStates;
        }
        public void UpdateOperation(string Operation){CurrentOperation=Operation;}
        public string GetOperation(){return CurrentOperation;}
        public void ValidateProtocol(string Name)
        {
            if(ValidatedProtocols.ContainsKey(Name))
            {ValidatedProtocols[Name].TimeValidated=DateTime.Now;ValidatedProtocols[Name].Validated=true;}
        }
        public DateTime GetValidationTimeOfProtocol(string name)
        {
            if (ValidatedProtocols.ContainsKey(name)) 
            {return ValidatedProtocols[name].TimeValidated; }
            else
            {return DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0)); }
        }

        #region SkypeAlarm
        private static Skype skype = new Skype();
        // This set simply remembers what numbers we've verified and so that we don't
        // waste precious cents re-verifying
        private static HashSet<string> verified = new HashSet<string>();
        private static string number_re = @"^([0-9]{10};? *)+$";
        public bool TestNumbers(string numbers)
        {
            Match m = Regex.Match(numbers, number_re);
            if (!m.Success) { return false; }
            foreach (string n in numbers.Split(';'))
            {
                string number = n.Trim();
                if (verified.Contains(number)) { continue; }
                if (CallConnects(number)) { verified.Add(number); }
                else { return false; }
            }
            return true;
        }
        public bool CallConnects(string number)
        {
            if (!skype.Client.IsRunning)
            {
                skype.Client.Start(true, true);
            }
            Call c;
            try
            {
                c = skype.PlaceCall(number);
            }
            catch { return false; }
            int waits = 1;
            // TODO: Use some kind of call property to avoid waiting for calls that fail instantly
            while (c.Duration == 0)
            {
                System.Threading.Thread.Sleep(500);
                if (waits++ > 50) { return false; }
            }
            try { c.Finish(); }
            catch { return false; }
            return true;

        }
        public bool isVerified(string number)
        {
            return verified.Contains(number);
        }
        public void addToVerified(string number)
        {
            verified.Add(number);
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
        System.Drawing.Bitmap GetCameraImage1(out string UpdateTime);
        [OperationContract]
        //[KnownType(typeof(System.Drawing.Bitmap)]
        System.Drawing.Bitmap GetCameraImage2(out string UpdateTime);
        [OperationContract]
        //[KnownType(typeof(System.Drawing.Bitmap)]
        void SetCameraImage1(System.Drawing.Bitmap Image);
        [OperationContract]
        //[KnownType(typeof(System.Drawing.Bitmap)]
        void SetCameraImage2(System.Drawing.Bitmap Image);
        [OperationContract]
        byte[] ReturnJPEGCamera1(out string updateTime);
        [OperationContract]
        byte[] ReturnJPEGCamera2(out string updateTime);
        [OperationContract]
        int GetImageHeight();
        [OperationContract]
        int GetImageWidth();
        [OperationContract]
        List<string> GetCurrentlyLoadedProtocolNames();
        [OperationContract]
        void SetCurrentlyLoadedProtocolNames(List<string> Names);
        [OperationContract]
        void UpdateOperation(string Operation);
        [OperationContract]
        string GetOperation();
        [OperationContract]
        void ValidateProtocol(string protocolName);
        [OperationContract]
        DateTime GetValidationTimeOfProtocol(string name);

        #region SkypeAlarm
        [OperationContract]
        bool TestNumbers(string numbers);
        [OperationContract]
        bool CallConnects(string number);
        [OperationContract]
        bool isVerified(string number);
        [OperationContract]
        void addToVerified(string number);
        #endregion
    }

}
