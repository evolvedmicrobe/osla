namespace Robot_Alarm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMonitor = new System.Windows.Forms.TabPage();
            this.lblIDLE = new System.Windows.Forms.Label();
            this.lblLastUpdateTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbIdleTime = new System.Windows.Forms.ComboBox();
            this.chkIdle = new System.Windows.Forms.CheckBox();
            this.chkAlarmDisconnect = new System.Windows.Forms.CheckBox();
            this.chkMonitorVideo = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblcurrentStatus = new System.Windows.Forms.Label();
            this.lblTime2 = new System.Windows.Forms.Label();
            this.lblTime1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnAttemptReconnect = new System.Windows.Forms.Button();
            this.btnSilenceAlarm = new System.Windows.Forms.Button();
            this.tabValidate = new System.Windows.Forms.TabPage();
            this.lstProtocols = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnValidateProtocols = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabMonitor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabValidate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabMonitor);
            this.tabControl1.Controls.Add(this.tabValidate);
            this.tabControl1.Location = new System.Drawing.Point(1, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(716, 503);
            this.tabControl1.TabIndex = 19;
            // 
            // tabMonitor
            // 
            this.tabMonitor.Controls.Add(this.lblIDLE);
            this.tabMonitor.Controls.Add(this.lblLastUpdateTime);
            this.tabMonitor.Controls.Add(this.label2);
            this.tabMonitor.Controls.Add(this.cmbIdleTime);
            this.tabMonitor.Controls.Add(this.chkIdle);
            this.tabMonitor.Controls.Add(this.chkAlarmDisconnect);
            this.tabMonitor.Controls.Add(this.chkMonitorVideo);
            this.tabMonitor.Controls.Add(this.label1);
            this.tabMonitor.Controls.Add(this.lblcurrentStatus);
            this.tabMonitor.Controls.Add(this.lblTime2);
            this.tabMonitor.Controls.Add(this.lblTime1);
            this.tabMonitor.Controls.Add(this.pictureBox2);
            this.tabMonitor.Controls.Add(this.pictureBox1);
            this.tabMonitor.Controls.Add(this.btnAttemptReconnect);
            this.tabMonitor.Controls.Add(this.btnSilenceAlarm);
            this.tabMonitor.Location = new System.Drawing.Point(4, 22);
            this.tabMonitor.Name = "tabMonitor";
            this.tabMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabMonitor.Size = new System.Drawing.Size(708, 477);
            this.tabMonitor.TabIndex = 0;
            this.tabMonitor.Text = "Monitor Robots";
            this.tabMonitor.UseVisualStyleBackColor = true;
            // 
            // lblIDLE
            // 
            this.lblIDLE.AutoSize = true;
            this.lblIDLE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIDLE.ForeColor = System.Drawing.Color.DarkRed;
            this.lblIDLE.Location = new System.Drawing.Point(295, 237);
            this.lblIDLE.Name = "lblIDLE";
            this.lblIDLE.Size = new System.Drawing.Size(0, 16);
            this.lblIDLE.TabIndex = 33;
            // 
            // lblLastUpdateTime
            // 
            this.lblLastUpdateTime.AutoSize = true;
            this.lblLastUpdateTime.Location = new System.Drawing.Point(256, 33);
            this.lblLastUpdateTime.Name = "lblLastUpdateTime";
            this.lblLastUpdateTime.Size = new System.Drawing.Size(0, 13);
            this.lblLastUpdateTime.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(401, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Set Max Idle Time";
            // 
            // cmbIdleTime
            // 
            this.cmbIdleTime.FormattingEnabled = true;
            this.cmbIdleTime.Items.AddRange(new object[] {
            "15 minutes",
            "30 minutes",
            "60 minutes",
            "120 minutes"});
            this.cmbIdleTime.Location = new System.Drawing.Point(547, 56);
            this.cmbIdleTime.Name = "cmbIdleTime";
            this.cmbIdleTime.Size = new System.Drawing.Size(121, 21);
            this.cmbIdleTime.TabIndex = 30;
            // 
            // chkIdle
            // 
            this.chkIdle.AutoSize = true;
            this.chkIdle.Checked = true;
            this.chkIdle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIdle.Location = new System.Drawing.Point(401, 10);
            this.chkIdle.Name = "chkIdle";
            this.chkIdle.Size = new System.Drawing.Size(89, 17);
            this.chkIdle.TabIndex = 29;
            this.chkIdle.Text = "Alarm On Idle";
            this.chkIdle.UseVisualStyleBackColor = true;
            // 
            // chkAlarmDisconnect
            // 
            this.chkAlarmDisconnect.AutoSize = true;
            this.chkAlarmDisconnect.Checked = true;
            this.chkAlarmDisconnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlarmDisconnect.Location = new System.Drawing.Point(506, 33);
            this.chkAlarmDisconnect.Name = "chkAlarmDisconnect";
            this.chkAlarmDisconnect.Size = new System.Drawing.Size(126, 17);
            this.chkAlarmDisconnect.TabIndex = 28;
            this.chkAlarmDisconnect.Text = "Alarm On Disconnect";
            this.chkAlarmDisconnect.UseVisualStyleBackColor = true;
            // 
            // chkMonitorVideo
            // 
            this.chkMonitorVideo.AutoSize = true;
            this.chkMonitorVideo.Checked = true;
            this.chkMonitorVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMonitorVideo.Location = new System.Drawing.Point(506, 10);
            this.chkMonitorVideo.Name = "chkMonitorVideo";
            this.chkMonitorVideo.Size = new System.Drawing.Size(91, 17);
            this.chkMonitorVideo.TabIndex = 27;
            this.chkMonitorVideo.Text = "Monitor Video";
            this.chkMonitorVideo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "The Instruments Current Status is";
            // 
            // lblcurrentStatus
            // 
            this.lblcurrentStatus.AutoSize = true;
            this.lblcurrentStatus.Location = new System.Drawing.Point(11, 49);
            this.lblcurrentStatus.Name = "lblcurrentStatus";
            this.lblcurrentStatus.Size = new System.Drawing.Size(101, 13);
            this.lblcurrentStatus.TabIndex = 25;
            this.lblcurrentStatus.Text = "No Status Reported";
            // 
            // lblTime2
            // 
            this.lblTime2.AutoSize = true;
            this.lblTime2.Location = new System.Drawing.Point(486, 354);
            this.lblTime2.Name = "lblTime2";
            this.lblTime2.Size = new System.Drawing.Size(91, 13);
            this.lblTime2.TabIndex = 24;
            this.lblTime2.Text = "Last Update Time";
            // 
            // lblTime1
            // 
            this.lblTime1.AutoSize = true;
            this.lblTime1.Location = new System.Drawing.Point(109, 354);
            this.lblTime1.Name = "lblTime1";
            this.lblTime1.Size = new System.Drawing.Size(91, 13);
            this.lblTime1.TabIndex = 23;
            this.lblTime1.Text = "Last Update Time";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(388, 106);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(300, 225);
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(14, 106);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 225);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // btnAttemptReconnect
            // 
            this.btnAttemptReconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttemptReconnect.Location = new System.Drawing.Point(388, 405);
            this.btnAttemptReconnect.Name = "btnAttemptReconnect";
            this.btnAttemptReconnect.Size = new System.Drawing.Size(302, 36);
            this.btnAttemptReconnect.TabIndex = 20;
            this.btnAttemptReconnect.Text = "Attempt To Reconnect";
            this.btnAttemptReconnect.UseVisualStyleBackColor = true;
            // 
            // btnSilenceAlarm
            // 
            this.btnSilenceAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSilenceAlarm.Location = new System.Drawing.Point(4, 405);
            this.btnSilenceAlarm.Name = "btnSilenceAlarm";
            this.btnSilenceAlarm.Size = new System.Drawing.Size(302, 36);
            this.btnSilenceAlarm.TabIndex = 3;
            this.btnSilenceAlarm.Text = "Silence Most Recent Alarm";
            this.btnSilenceAlarm.UseVisualStyleBackColor = true;
            // 
            // tabValidate
            // 
            this.tabValidate.Controls.Add(this.btnValidateProtocols);
            this.tabValidate.Controls.Add(this.btnRefresh);
            this.tabValidate.Controls.Add(this.lstProtocols);
            this.tabValidate.Location = new System.Drawing.Point(4, 22);
            this.tabValidate.Name = "tabValidate";
            this.tabValidate.Padding = new System.Windows.Forms.Padding(3);
            this.tabValidate.Size = new System.Drawing.Size(708, 477);
            this.tabValidate.TabIndex = 1;
            this.tabValidate.Text = "Validate Protocol";
            this.tabValidate.UseVisualStyleBackColor = true;
            // 
            // lstProtocols
            // 
            this.lstProtocols.FormattingEnabled = true;
            this.lstProtocols.Location = new System.Drawing.Point(27, 43);
            this.lstProtocols.Name = "lstProtocols";
            this.lstProtocols.Size = new System.Drawing.Size(366, 420);
            this.lstProtocols.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(429, 99);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(206, 41);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh Protocols";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnValidateProtocols
            // 
            this.btnValidateProtocols.Location = new System.Drawing.Point(429, 246);
            this.btnValidateProtocols.Name = "btnValidateProtocols";
            this.btnValidateProtocols.Size = new System.Drawing.Size(206, 41);
            this.btnValidateProtocols.TabIndex = 2;
            this.btnValidateProtocols.Text = "Validate Selected Protocols";
            this.btnValidateProtocols.UseVisualStyleBackColor = true;
            this.btnValidateProtocols.Click += new System.EventHandler(this.btnValidateProtocols_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 537);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Remote Clarity Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabMonitor.ResumeLayout(false);
            this.tabMonitor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabValidate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMonitor;
        private System.Windows.Forms.Label lblIDLE;
        private System.Windows.Forms.Label lblLastUpdateTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbIdleTime;
        private System.Windows.Forms.CheckBox chkIdle;
        private System.Windows.Forms.CheckBox chkAlarmDisconnect;
        private System.Windows.Forms.CheckBox chkMonitorVideo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblcurrentStatus;
        private System.Windows.Forms.Label lblTime2;
        private System.Windows.Forms.Label lblTime1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnAttemptReconnect;
        private System.Windows.Forms.Button btnSilenceAlarm;
        private System.Windows.Forms.TabPage tabValidate;
        private System.Windows.Forms.Button btnValidateProtocols;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListBox lstProtocols;


    }
}

