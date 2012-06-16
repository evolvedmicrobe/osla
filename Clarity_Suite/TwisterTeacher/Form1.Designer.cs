namespace Clarity
{
    partial class TwisterTeacher
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
            this.btnGetPosition = new System.Windows.Forms.Button();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lstPositions = new System.Windows.Forms.ListBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.txtVertical = new System.Windows.Forms.TextBox();
            this.txtReach = new System.Windows.Forms.TextBox();
            this.txtRotary = new System.Windows.Forms.TextBox();
            this.txtWrist = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChangePosition = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAllPositionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGripMaterial = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetPosition
            // 
            this.btnGetPosition.Location = new System.Drawing.Point(59, 81);
            this.btnGetPosition.Name = "btnGetPosition";
            this.btnGetPosition.Size = new System.Drawing.Size(203, 23);
            this.btnGetPosition.TabIndex = 0;
            this.btnGetPosition.Text = "Get Position";
            this.btnGetPosition.UseVisualStyleBackColor = true;
            this.btnGetPosition.Click += new System.EventHandler(this.btnGetPosition_Click);
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(59, 492);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(401, 20);
            this.txtPosition.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(59, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Open Grip";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(59, 121);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(203, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Home Grip Axis";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lstPositions
            // 
            this.lstPositions.FormattingEnabled = true;
            this.lstPositions.Location = new System.Drawing.Point(347, 27);
            this.lstPositions.Name = "lstPositions";
            this.lstPositions.Size = new System.Drawing.Size(304, 394);
            this.lstPositions.TabIndex = 4;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(59, 166);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(203, 23);
            this.btnMove.TabIndex = 5;
            this.btnMove.Text = "Move To Selected Position";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // txtVertical
            // 
            this.txtVertical.Location = new System.Drawing.Point(674, 102);
            this.txtVertical.Name = "txtVertical";
            this.txtVertical.Size = new System.Drawing.Size(116, 20);
            this.txtVertical.TabIndex = 6;
            // 
            // txtReach
            // 
            this.txtReach.Location = new System.Drawing.Point(674, 157);
            this.txtReach.Name = "txtReach";
            this.txtReach.Size = new System.Drawing.Size(116, 20);
            this.txtReach.TabIndex = 7;
            // 
            // txtRotary
            // 
            this.txtRotary.Location = new System.Drawing.Point(674, 210);
            this.txtRotary.Name = "txtRotary";
            this.txtRotary.Size = new System.Drawing.Size(116, 20);
            this.txtRotary.TabIndex = 8;
            // 
            // txtWrist
            // 
            this.txtWrist.Location = new System.Drawing.Point(674, 272);
            this.txtWrist.Name = "txtWrist";
            this.txtWrist.Size = new System.Drawing.Size(116, 20);
            this.txtWrist.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(674, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Vertical";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(671, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Reach";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(671, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Rotary";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(674, 246);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Wrist";
            // 
            // btnChangePosition
            // 
            this.btnChangePosition.Location = new System.Drawing.Point(59, 207);
            this.btnChangePosition.Name = "btnChangePosition";
            this.btnChangePosition.Size = new System.Drawing.Size(203, 23);
            this.btnChangePosition.TabIndex = 15;
            this.btnChangePosition.Text = "Save Current Position To Selected Position";
            this.btnChangePosition.UseVisualStyleBackColor = true;
            this.btnChangePosition.Click += new System.EventHandler(this.btnChangePosition_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAllPositionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(890, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAllPositionsToolStripMenuItem
            // 
            this.saveAllPositionsToolStripMenuItem.Name = "saveAllPositionsToolStripMenuItem";
            this.saveAllPositionsToolStripMenuItem.Size = new System.Drawing.Size(111, 20);
            this.saveAllPositionsToolStripMenuItem.Text = "Save All Positions";
            this.saveAllPositionsToolStripMenuItem.Click += new System.EventHandler(this.saveAllPositionsToolStripMenuItem_Click);
            // 
            // btnGripMaterial
            // 
            this.btnGripMaterial.Location = new System.Drawing.Point(59, 294);
            this.btnGripMaterial.Name = "btnGripMaterial";
            this.btnGripMaterial.Size = new System.Drawing.Size(203, 23);
            this.btnGripMaterial.TabIndex = 17;
            this.btnGripMaterial.Text = "Grip Material";
            this.btnGripMaterial.UseVisualStyleBackColor = true;
            this.btnGripMaterial.Click += new System.EventHandler(this.btnGripMaterial_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(317, 429);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(388, 40);
            this.label5.TabIndex = 18;
            this.label5.Text = "Do not set clearance positions at the very top!!!\r\nIt will burn out the motor if " +
    "it can\'t get there.";
            // 
            // TwisterTeacher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 544);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnGripMaterial);
            this.Controls.Add(this.btnChangePosition);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWrist);
            this.Controls.Add(this.txtRotary);
            this.Controls.Add(this.txtReach);
            this.Controls.Add(this.txtVertical);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.lstPositions);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.btnGetPosition);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TwisterTeacher";
            this.Text = "Get Twister Position";
            this.Load += new System.EventHandler(this.TwisterTeacher_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetPosition;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lstPositions;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.TextBox txtVertical;
        private System.Windows.Forms.TextBox txtReach;
        private System.Windows.Forms.TextBox txtRotary;
        private System.Windows.Forms.TextBox txtWrist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChangePosition;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveAllPositionsToolStripMenuItem;
        private System.Windows.Forms.Button btnGripMaterial;
        private System.Windows.Forms.Label label5;
    }
}

