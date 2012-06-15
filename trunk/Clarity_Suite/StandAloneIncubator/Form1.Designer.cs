namespace StandAloneIncubator
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
            this.btnInitialize = new System.Windows.Forms.Button();
            this.btnStartShaking = new System.Windows.Forms.Button();
            this.btnStopShaking = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(23, 18);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(138, 23);
            this.btnInitialize.TabIndex = 0;
            this.btnInitialize.Text = "Initialize Incubator";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // btnStartShaking
            // 
            this.btnStartShaking.Location = new System.Drawing.Point(23, 63);
            this.btnStartShaking.Name = "btnStartShaking";
            this.btnStartShaking.Size = new System.Drawing.Size(138, 23);
            this.btnStartShaking.TabIndex = 1;
            this.btnStartShaking.Text = "Start Shaking";
            this.btnStartShaking.UseVisualStyleBackColor = true;
            // 
            // btnStopShaking
            // 
            this.btnStopShaking.Location = new System.Drawing.Point(23, 107);
            this.btnStopShaking.Name = "btnStopShaking";
            this.btnStopShaking.Size = new System.Drawing.Size(138, 23);
            this.btnStopShaking.TabIndex = 2;
            this.btnStopShaking.Text = "Stop Shaking";
            this.btnStopShaking.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 149);
            this.Controls.Add(this.btnStopShaking);
            this.Controls.Add(this.btnStartShaking);
            this.Controls.Add(this.btnInitialize);
            this.Name = "Form1";
            this.Text = "Stand Alone Incubator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Button btnStartShaking;
        private System.Windows.Forms.Button btnStopShaking;
    }
}

