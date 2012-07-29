using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.ServiceModel;
using System.IO;
namespace Robot_Alarm
{
    enum CurrentAlarmState { AlarmOn, AlarmOff };
    delegate void del_String(string stringarg);
    delegate void del_Button();
   
    delegate void del_BitMap(System.Drawing.Bitmap bmp,string up1, System.Drawing.Bitmap bmp2,string up2);
    /// <summary>
    /// WCF Client
    /// </summary>
    public partial class Form1 : Form
    {
        bool ClientSideAlarmActivation;
        static int POLLING_INTERVAL = 10000;
        bool isConnected;//Lets me know if the monitor is up and running.
        AlarmClient InstrumentMonitor;
        bool PerformPolling;
        Thread PollingThread;
        CurrentAlarmState AlarmOnOrOff;
        SoundPlayer Player;
        AlarmState AlarmToIgnore;
        AlarmState CurAlarm;
        del_String UpdateStatusDel;
        del_BitMap BitmapDel;
        del_String UpdatelblIdleTXT;

        TimeSpan MaxTimeAllowed = new TimeSpan(1, 0, 0);
        //This class we monitor the robots, it works quite simply.  Essentially, every 3 seconds it will poll the server for a change in state, 
        //it will then display that state, and based on that state, perform certain acitions. 
        public Form1()
        {
            InitializeComponent();
            UpdatelblIdleTXT = new del_String(UpdatelblIdel);
        }
        private void UpdateImages(Bitmap Image1,string updateTime1, System.Drawing.Bitmap Image2,string updateTime2)
        {
            pictureBox1.Image = Image1;
            lblTime1.Text = updateTime1;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PerformPolling = true;
            AlarmOnOrOff = CurrentAlarmState.AlarmOff;
            cmbIdleTime.SelectedIndex = 2;
            string curDir = Environment.CurrentDirectory;
            string toAdd;
            if (curDir.Contains('/'))
            {
                toAdd = "/alarm.wav";
            }
            else{toAdd= "\\alarm.wav";}
            Player = new SoundPlayer(new FileStream((curDir+toAdd),FileMode.Open,FileAccess.Read,FileShare.Read));
            UpdateStatusDel=new del_String(UpdateInstrumentStatus);
            BitmapDel=new del_BitMap(UpdateImages);
            //start polling
            BeginMonitoring();
        }
        private bool BeginMonitoring()
        {
            try
            {
                //when an alarm is found, if it equals this, it will not play the alarm
                AlarmToIgnore = new AlarmState();
                AlarmToIgnore.AlarmOn = false;
                //more code to avoid uninitalized exceptions
                CurAlarm = new AlarmState();
                CurAlarm.AlarmOn = false;
                InstrumentMonitor = new AlarmClient();
                InitializePolling();
                return true;
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not initialize the remote client.\n" + thrown.Message);
                isConnected = false;
                ChangeButtonFromOtherThread();
                return false;
            }
        }
        private void ChangebtnState()
        {
            if (btnAttemptReconnect.Enabled)
            {
                btnAttemptReconnect.Enabled = false;
            }
            else
            {
                btnAttemptReconnect.Enabled = true;
                lblcurrentStatus.Text = "Not Connected To Clarity";
            }
        }
        private void ChangeButtonFromOtherThread()
        {
            del_Button SwitchState = ChangebtnState;
            this.Invoke(SwitchState);
        }
        private void InitializePolling()
        {
            try
            {
                PerformPolling = true;
                if (PollingThread != null && PollingThread.ThreadState != ThreadState.Unstarted && PollingThread.ThreadState != ThreadState.Stopped)
                {
                    PollingThread.Abort();
                    PerformPolling = false;
                    PollingThread.Join(10000);
                    PerformPolling = true;
                }
                ThreadStart TS = new ThreadStart(PollInstrumentStatus);
                PollingThread = new Thread(TS);
                PollingThread.Name = "Instrument Monitor Thread";
                PollingThread.IsBackground = true;
                PollingThread.Start();
                isConnected = true;
                ChangeButtonFromOtherThread();
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not initialize instrument checking: "+thrown.Message);
                isConnected = false;
                ChangeButtonFromOtherThread();
            }
        }
        private void PlayAlarmSound()
        {
                AlarmOnOrOff=CurrentAlarmState.AlarmOn;
                Player.PlayLooping();
        }
        private void StopAlarmSound()
        {
            Player.Stop();
            string totalk = "";
            object[] tomake = { totalk };
            this.Invoke(UpdatelblIdleTXT, tomake);
            AlarmOnOrOff=CurrentAlarmState.AlarmOff;
            ClientSideAlarmActivation = false;
        }
        private void UpdateInstrumentStatus(string newStatus)
        {
            lblcurrentStatus.Text=newStatus;
            lblLastUpdateTime.Text="Last updated: "+DateTime.Now.ToString();
        }
        private void UpdatelblIdel(string newText)
        {
            lblIDLE.Text = newText;
        }
        private Image TurnByteArrayToImage(byte[] Data)
        {
            MemoryStream MS = new MemoryStream(Data);
            Image myImage = Image.FromStream(MS);
            return myImage;
        }
        private void CheckForIdle()
        {            
            InstrumentStatus IS = InstrumentMonitor.GetInstrumentStatus();
            TimeSpan TS = DateTime.Now.Subtract(IS.TimeCreated);
            if (TS.CompareTo(MaxTimeAllowed) > 0)
            {
                CreateAndActivateAlarm();
                string totalk="The robot software has not reported anything for a time longer than: " + MaxTimeAllowed.ToString();
                object[] tomake={totalk};
                this.Invoke(UpdatelblIdleTXT, tomake);
                del_Button uncheckidle = new del_Button(UnCheckIdleWatch);
                this.Invoke(uncheckidle);
            }
        }
        private void CreateAndActivateAlarm()
        {
            PlayAlarmSound();
            CurAlarm = new AlarmState();
            CurAlarm.AlarmOn = true;
            CurAlarm.TimeTurnedOn = DateTime.Now;
            ClientSideAlarmActivation = true;            
            AlarmOnOrOff = CurrentAlarmState.AlarmOn;
        }
        private void PollInstrumentStatus()
        {
            try
            {
                while (PerformPolling)
                {
                    AlarmState AS = InstrumentMonitor.GetAlarmStatus();
                    InstrumentStatus IS = InstrumentMonitor.GetInstrumentStatus();
                    if (chkMonitorVideo.Checked)
                    {

                        //update images
                        string updatetime1;
                        string updatetime2;
                        //Bitmap Cam1 = InstrumentMonitor.GetCameraImage1(out updatetime1);
                        //Bitmap Cam2 = InstrumentMonitor.GetCameraImage2(out updatetime2);
                        byte[] myStream = InstrumentMonitor.ReturnJPEGCamera1(out updatetime1);
                        Image Cam1 = TurnByteArrayToImage(myStream);
                        myStream = InstrumentMonitor.ReturnJPEGCamera2(out updatetime2);
                        Image Cam2 = TurnByteArrayToImage(myStream);
                        object[] args = { Cam1, updatetime1, Cam2, updatetime2 };
                        this.Invoke(BitmapDel, args);
                       
                    }
                    if (chkIdle.Checked)
                    { CheckForIdle(); }
                    
                    if (AS.AlarmOn)
                    {
                        if (AlarmOnOrOff == CurrentAlarmState.AlarmOn)
                        {
                            //alarm already running
                            
                            continue;
                        }
                        else if (AS.TimeTurnedOn == AlarmToIgnore.TimeTurnedOn)
                        {
                            //alarm already ignored
                            continue;
                        }
                        else
                        {
                            CurAlarm = new AlarmState();
                            CurAlarm.AlarmOn = AS.AlarmOn;
                            CurAlarm.TimeTurnedOn = AS.TimeTurnedOn;
                            AlarmOnOrOff = CurrentAlarmState.AlarmOn;
                            PlayAlarmSound();
                        }
                    }
                    else if (AlarmOnOrOff== CurrentAlarmState.AlarmOn && !ClientSideAlarmActivation)
                    {
                        StopAlarmSound();
                    }
                    //update the status
                    object[] tmp = new object[1] { IS.Status };
                    this.Invoke(UpdateStatusDel, tmp);
                    //wait 4 seconds
                    Thread.Sleep(POLLING_INTERVAL);
                }
            }
            catch (Exception thrown)
            {
                if (chkAlarmDisconnect.Checked)
                {
                    CreateAndActivateAlarm();
                }
                isConnected = false;
                MessageBox.Show("Could not poll the Clarity software for its status.  This suggests the software is not running.\n\n"+thrown.Message,"Problem Connecting",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ChangeButtonFromOtherThread();
            }
        }
        private void UnCheckIdleWatch()
        {
            chkIdle.Checked = false;
        }
        private void btnSilenceAlarm_Click(object sender, EventArgs e)
        {
            StopAlarmSound();
            AlarmToIgnore = CurAlarm;
        }
        private void btnAttemptReconnect_Click(object sender, EventArgs e)
        {
            BeginMonitoring();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdleTime.SelectedIndex > -1)
            {
                var time = cmbIdleTime.SelectedItem as string;
                int Minutes = Convert.ToInt32(time.Split(' ')[0]);
                MaxTimeAllowed = new TimeSpan(0, Minutes, 0);

            }
        }


       



    }
}
