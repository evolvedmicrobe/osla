using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Clarity
{
    public class CountdownTimer : Label
    {
        public int MilliSecondsToGo;
        public DateTime NextEventTime;
        Timer CountDown;
        public CountdownTimer(): base()
        {
            CountDown = new Timer();
            CountDown.Tick += new EventHandler(CountDown_Tick);
            CountDown.Interval = 1000;
            NextEventTime = DateTime.Now;
        }
        public void Start()
        {
            CountDown.Start();
        }
        public void Start(DateTime NextInstructionTime)
        {
            NextEventTime = NextInstructionTime;
            WriteString();
            CountDown.Start();
        }
        public void Start(int MilliSecondsToGo)
        {
            DateTime NextTime = DateTime.Now.AddMilliseconds(MilliSecondsToGo);
            Start(NextTime);
        }
        public void Stop()
        {
            CountDown.Stop();
            this.Text = "";
        }
        public void WriteString()
        {
            TimeSpan TS = DateTime.Now.Subtract(NextEventTime);
            //TimeSpan TS = NextEventTime.Subtract(DateTime.Now);
            if (TS.TotalMinutes < 10)
            { this.ForeColor = Color.Red; }
            else { this.ForeColor = Color.Black; }
            this.Text = "Next Event In -- " + TS.Hours.ToString().Replace("-", "") + " : " + TS.Minutes.ToString().Replace("-", "") + " : " + TS.Seconds.ToString().Replace("-", "");
        }
        void CountDown_Tick(object sender, EventArgs e)
        {
            WriteString();           
        }
    }
}
