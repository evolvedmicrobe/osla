using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Clarity
{
    public partial class WelcomeForm : Form
    {
        public bool StayAlive;
        public WelcomeForm()
        {
            InitializeComponent();
        }
        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            StayAlive = true;
            string curdirec = System.Environment.CurrentDirectory;
            Uri address = new Uri(curdirec + @"\WelcomeInstructions.htm",UriKind.Absolute);
            webBrowser1.Url = address;
            StayAlive = true;
            this.Refresh();
            this.Show();
            RunCheckForFinished();

        }
        private void RunCheckForFinished()
        {
            int counter=0;
            while (StayAlive & counter<150)
            {
                Application.DoEvents();
                Thread.Sleep(1000);
                counter++;
            }
            Application.ExitThread();

        }

        private void WelcomeForm_Shown(object sender, EventArgs e)
        {
            RunCheckForFinished();
            Application.ExitThread();
        }

    }
}

