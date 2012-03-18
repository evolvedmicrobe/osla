﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace Robot_Alarm
{

    [Serializable]
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.Single)]
    public class ValidationStatus
    {
       public bool Validated; public DateTime TimeValidated;
    }
    public class AlarmNotifier : IAlarm

    {
        public const int IMAGE_HEIGHT = 225;
        public const int IMAGE_WIDTH = 300;
        public  Bitmap Image1;
        public  Bitmap Image2;
        Dictionary<string,ValidationStatus> ValidatedProtocols;
        public List<string> CurrentlyLoadedProtocolNames;
        public string Image1UpdateTime;
        public string Image2UpdateTime;
        public string CurrentOperation="None Set";
        public static AlarmState CurrentAlarmState;
        public static InstrumentStatus CurrentInstrumentStatus;
        public AlarmNotifier()
        {
            CurrentAlarmState = new AlarmState(false);
            CurrentInstrumentStatus = new InstrumentStatus("Monitoring Not Yet Initialized");
            ValidatedProtocols=new Dictionary<string,ValidationStatus>();
            CurrentlyLoadedProtocolNames=new List<string>();
            CurrentlyLoadedProtocolNames.Add("No Protocols Loaded Yet");
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
       
        #region WebCamMonitor Members
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
    }

}
