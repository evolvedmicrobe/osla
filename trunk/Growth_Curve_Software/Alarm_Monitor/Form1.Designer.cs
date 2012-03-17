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
            this.lblcurrentStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSilenceAlarm = new System.Windows.Forms.Button();
            this.lblLastUpdateTime = new System.Windows.Forms.Label();
            this.btnAttemptReconnect = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblTime1 = new System.Windows.Forms.Label();
            this.lblTime2 = new System.Windows.Forms.Label();
            this.chkMonitorVideo = new System.Windows.Forms.CheckBox();
            this.chkAlarmDisconnect = new System.Windows.Forms.CheckBox();
            this.lblIDLE = new System.Windows.Forms.Label();
            this.chkIdle = new System.Windows.Forms.CheckBox();
            this.cmbIdleTime = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblcurrentStatus
            // 
            this.lblcurrentStatus.AutoSize = true;
            this.lblcurrentStatus.Location = new System.Drawing.Point(12, 47);
            this.lblcurrentStatus.Name = "lblcurrentStatus";
            this.lblcurrentStatus.Size = new System.Drawing.Size(101, 13);
            this.lblcurrentStatus.TabIndex = 0;
            this.lblcurrentStatus.Text = "No Status Reported";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "The Instruments Current Status is";
            // 
            // btnSilenceAlarm
            // 
            this.btnSilenceAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSilenceAlarm.Location = new System.Drawing.Point(15, 394);
            this.btnSilenceAlarm.Name = "btnSilenceAlarm";
            this.btnSilenceAlarm.Size = new System.Drawing.Size(302, 36);
            this.btnSilenceAlarm.TabIndex = 2;
            this.btnSilenceAlarm.Text = "Silence Most Recent Alarm";
            this.btnSilenceAlarm.UseVisualStyleBackColor = true;
            this.btnSilenceAlarm.Click += new System.EventHandler(this.btnSilenceAlarm_Click);
            // 
            // lblLastUpdateTime
            // 
            this.lblLastUpdateTime.AutoSize = true;
            this.lblLastUpdateTime.Location = new System.Drawing.Point(30, 161);
            this.lblLastUpdateTime.Name = "lblLastUpdateTime";
            this.lblLastUpdateTime.Size = new System.Drawing.Size(0, 13);
            this.lblLastUpdateTime.TabIndex = 3;
            // 
            // btnAttemptReconnect
            // 
            this.btnAttemptReconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttemptReconnect.Location = new System.Drawing.Point(367, 394);
            this.btnAttemptReconnect.Name = "btnAttemptReconnect";
            this.btnAttemptReconnect.Size = new System.Drawing.Size(302, 36);
            this.btnAttemptReconnect.TabIndex = 6;
            this.btnAttemptReconnect.Text = "Attempt To Reconnect";
            this.btnAttemptReconnect.UseVisualStyleBackColor = true;
            this.btnAttemptReconnect.Click += new System.EventHandler(this.btnAttemptReconnect_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(15, 88);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 225);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(358, 88);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(300, 225);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // lblTime1
            // 
            this.lblTime1.AutoSize = true;
            this.lblTime1.Location = new System.Drawing.Point(84, 334);
            this.lblTime1.Name = "lblTime1";
            this.lblTime1.Size = new System.Drawing.Size(91, 13);
            this.lblTime1.TabIndex = 10;
            this.lblTime1.Text = "Last Update Time";
            // 
            // lblTime2
            // 
            this.lblTime2.AutoSize = true;
            this.lblTime2.Location = new System.Drawing.Point(449, 334);
            this.lblTime2.Name = "lblTime2";
            this.lblTime2.Size = new System.Drawing.Size(91, 13);
            this.lblTime2.TabIndex = 11;
            this.lblTime2.Text = "Last Update Time";
            // 
            // chkMonitorVideo
            // 
            this.chkMonitorVideo.AutoSize = true;
            this.chkMonitorVideo.Checked = true;
            this.chkMonitorVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMonitorVideo.Location = new System.Drawing.Point(507, 8);
            this.chkMonitorVideo.Name = "chkMonitorVideo";
            this.chkMonitorVideo.Size = new System.Drawing.Size(91, 17);
            this.chkMonitorVideo.TabIndex = 12;
            this.chkMonitorVideo.Text = "Monitor Video";
            this.chkMonitorVideo.UseVisualStyleBackColor = true;
            // 
            // chkAlarmDisconnect
            // 
            this.chkAlarmDisconnect.AutoSize = true;
            this.chkAlarmDisconnect.Checked = true;
            this.chkAlarmDisconnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlarmDisconnect.Location = new System.Drawing.Point(507, 31);
            this.chkAlarmDisconnect.Name = "chkAlarmDisconnect";
            this.chkAlarmDisconnect.Size = new System.Drawing.Size(126, 17);
            this.chkAlarmDisconnect.TabIndex = 13;
            this.chkAlarmDisconnect.Text = "Alarm On Disconnect";
            this.chkAlarmDisconnect.UseVisualStyleBackColor = true;
            // 
            // lblIDLE
            // 
            this.lblIDLE.AutoSize = true;
            this.lblIDLE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIDLE.ForeColor = System.Drawing.Color.DarkRed;
            this.lblIDLE.Location = new System.Drawing.Point(69, 365);
            this.lblIDLE.Name = "lblIDLE";
            this.lblIDLE.Size = new System.Drawing.Size(0, 16);
            this.lblIDLE.TabIndex = 14;
            // 
            // chkIdle
            // 
            this.chkIdle.AutoSize = true;
            this.chkIdle.Checked = true;
            this.chkIdle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIdle.Location = new System.Drawing.Point(402, 8);
            this.chkIdle.Name = "chkIdle";
            this.chkIdle.Size = new System.Drawing.Size(89, 17);
            this.chkIdle.TabIndex = 15;
            this.chkIdle.Text = "Alarm On Idle";
            this.chkIdle.UseVisualStyleBackColor = true;
            // 
            // cmbIdleTime
            // 
            this.cmbIdleTime.FormattingEnabled = true;
            this.cmbIdleTime.Items.AddRange(new object[] {
            "15 minutes",
            "30 minutes",
            "60 minutes",
            "120 minutes"});
            this.cmbIdleTime.Location = new System.Drawing.Point(548, 54);
            this.cmbIdleTime.Name = "cmbIdleTime";
            this.cmbIdleTime.Size = new System.Drawing.Size(121, 21);
            this.cmbIdleTime.TabIndex = 17;
            this.cmbIdleTime.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(402, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Set Max Idle Time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 439);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbIdleTime);
            this.Controls.Add(this.chkIdle);
            this.Controls.Add(this.lblIDLE);
            this.Controls.Add(this.chkAlarmDisconnect);
            this.Controls.Add(this.chkMonitorVideo);
            this.Controls.Add(this.lblTime2);
            this.Controls.Add(this.lblTime1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnAttemptReconnect);
            this.Controls.Add(this.lblLastUpdateTime);
            this.Controls.Add(this.btnSilenceAlarm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblcurrentStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Remote Clarity Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblcurrentStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSilenceAlarm;
        private System.Windows.Forms.Label lblLastUpdateTime;
        private System.Windows.Forms.Button btnAttemptReconnect;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblTime1;
        private System.Windows.Forms.Label lblTime2;
        private System.Windows.Forms.CheckBox chkMonitorVideo;
        private System.Windows.Forms.CheckBox chkAlarmDisconnect;
        private System.Windows.Forms.Label lblIDLE;
        private System.Windows.Forms.CheckBox chkIdle;
        private System.Windows.Forms.ComboBox cmbIdleTime;
        private System.Windows.Forms.Label label2;

    }
}

