using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Growth_Curve_Software;

namespace StandAloneIncubator
{
    public partial class Form1 : Form
    {
        public IncubatorServ incubator;
        public string COMPORT = "COM11";
        public int SHAKINGSPEED = 650;
        public Form1()
        {
            InitializeComponent();
            incubator = new IncubatorServ();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            incubator.Initialize();
            incubator.COM_PORT = COMPORT;
            incubator.InitCommands = "";
        }
    }
}
