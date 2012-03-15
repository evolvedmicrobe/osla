namespace Growth_Curve_Software
{
    partial class MakeProtocols
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MakeProtocols));
            this.MethodsView = new System.Windows.Forms.TreeView();
            this.lstProtocol = new System.Windows.Forms.ListBox();
            this.tblMethodParameterView = new System.Windows.Forms.DataGridView();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lblMethodName = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileOperationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddMethod = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblParametersRequired = new System.Windows.Forms.Label();
            this.lblClickButtonProtocol = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtNumberOfLoops = new System.Windows.Forms.TextBox();
            this.txtLoopStart = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddInstruction = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtDelayMinutes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDelayInstruction = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtEmails = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProtocolName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnDeleteProtocolItem = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.tabOfProtocols = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabVariables = new System.Windows.Forms.TabPage();
            this.pnlVariables = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.btrnAddVariable = new System.Windows.Forms.Button();
            this.dataViewVariable = new System.Windows.Forms.DataGridView();
            this.label14 = new System.Windows.Forms.Label();
            this.lstVariableTypes = new System.Windows.Forms.ListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lstCurrentVariables = new System.Windows.Forms.ListBox();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tblMethodParameterView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabOfProtocols.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabVariables.SuspendLayout();
            this.pnlVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewVariable)).BeginInit();
            this.SuspendLayout();
            // 
            // MethodsView
            // 
            this.MethodsView.BackColor = System.Drawing.Color.DarkBlue;
            this.MethodsView.ForeColor = System.Drawing.Color.White;
            this.MethodsView.Location = new System.Drawing.Point(7, 72);
            this.MethodsView.Margin = new System.Windows.Forms.Padding(4);
            this.MethodsView.Name = "MethodsView";
            this.MethodsView.Size = new System.Drawing.Size(412, 580);
            this.MethodsView.TabIndex = 0;
            this.MethodsView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MethodsView_AfterSelect);
            // 
            // lstProtocol
            // 
            this.lstProtocol.FormattingEnabled = true;
            this.lstProtocol.ItemHeight = 18;
            this.lstProtocol.Location = new System.Drawing.Point(749, 27);
            this.lstProtocol.Margin = new System.Windows.Forms.Padding(4);
            this.lstProtocol.Name = "lstProtocol";
            this.lstProtocol.Size = new System.Drawing.Size(343, 508);
            this.lstProtocol.TabIndex = 2;
            this.lstProtocol.SelectedIndexChanged += new System.EventHandler(this.lstProtocol_SelectedIndexChanged);
            this.lstProtocol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstProtocol_KeyDown);
            // 
            // tblMethodParameterView
            // 
            this.tblMethodParameterView.AllowUserToAddRows = false;
            this.tblMethodParameterView.AllowUserToDeleteRows = false;
            this.tblMethodParameterView.BackgroundColor = System.Drawing.Color.Navy;
            this.tblMethodParameterView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblMethodParameterView.Location = new System.Drawing.Point(10, 98);
            this.tblMethodParameterView.Margin = new System.Windows.Forms.Padding(4);
            this.tblMethodParameterView.Name = "tblMethodParameterView";
            this.tblMethodParameterView.Size = new System.Drawing.Size(284, 55);
            this.tblMethodParameterView.TabIndex = 3;
            this.tblMethodParameterView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.tblMethodParameterView_DataError);
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProtocol.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblProtocol.Location = new System.Drawing.Point(45, 7);
            this.lblProtocol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(182, 36);
            this.lblProtocol.TabIndex = 4;
            this.lblProtocol.Text = "Customize Method and\r\n Add It To Protocol";
            // 
            // lblMethodName
            // 
            this.lblMethodName.AutoSize = true;
            this.lblMethodName.Location = new System.Drawing.Point(530, 187);
            this.lblMethodName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMethodName.Name = "lblMethodName";
            this.lblMethodName.Size = new System.Drawing.Size(0, 18);
            this.lblMethodName.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOperationsToolStripMenuItem,
            this.informationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1149, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileOperationsToolStripMenuItem
            // 
            this.fileOperationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProtocolToolStripMenuItem,
            this.loadProtocolToolStripMenuItem,
            this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem});
            this.fileOperationsToolStripMenuItem.Name = "fileOperationsToolStripMenuItem";
            this.fileOperationsToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.fileOperationsToolStripMenuItem.Text = "File Operations";
            // 
            // saveProtocolToolStripMenuItem
            // 
            this.saveProtocolToolStripMenuItem.Name = "saveProtocolToolStripMenuItem";
            this.saveProtocolToolStripMenuItem.Size = new System.Drawing.Size(306, 22);
            this.saveProtocolToolStripMenuItem.Text = "Save Protocol";
            this.saveProtocolToolStripMenuItem.Click += new System.EventHandler(this.saveProtocolToolStripMenuItem_Click);
            // 
            // loadProtocolToolStripMenuItem
            // 
            this.loadProtocolToolStripMenuItem.Name = "loadProtocolToolStripMenuItem";
            this.loadProtocolToolStripMenuItem.Size = new System.Drawing.Size(306, 22);
            this.loadProtocolToolStripMenuItem.Text = "Load Protocol";
            this.loadProtocolToolStripMenuItem.Click += new System.EventHandler(this.loadProtocolToolStripMenuItem_Click);
            // 
            // insertOtherProtocolAtEndOfThisOneToolStripMenuItem
            // 
            this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem.Name = "insertOtherProtocolAtEndOfThisOneToolStripMenuItem";
            this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem.Size = new System.Drawing.Size(306, 22);
            this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem.Text = "Insert Other Protocol At the End of This One";
            this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem.Click += new System.EventHandler(this.insertOtherProtocolAtEndOfThisOneToolStripMenuItem_Click);
            // 
            // informationToolStripMenuItem
            // 
            this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            this.informationToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.informationToolStripMenuItem.Text = "Information";
            this.informationToolStripMenuItem.Click += new System.EventHandler(this.informationToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(857, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Current Protocol";
            // 
            // btnAddMethod
            // 
            this.btnAddMethod.Location = new System.Drawing.Point(46, 195);
            this.btnAddMethod.Name = "btnAddMethod";
            this.btnAddMethod.Size = new System.Drawing.Size(223, 27);
            this.btnAddMethod.TabIndex = 8;
            this.btnAddMethod.Text = "Add Method To Protocol";
            this.btnAddMethod.UseVisualStyleBackColor = true;
            this.btnAddMethod.Click += new System.EventHandler(this.btnAddMethod_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblParametersRequired);
            this.panel1.Controls.Add(this.lblProtocol);
            this.panel1.Controls.Add(this.btnAddMethod);
            this.panel1.Controls.Add(this.tblMethodParameterView);
            this.panel1.Controls.Add(this.lblClickButtonProtocol);
            this.panel1.Location = new System.Drawing.Point(440, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 235);
            this.panel1.TabIndex = 9;
            // 
            // lblParametersRequired
            // 
            this.lblParametersRequired.AutoSize = true;
            this.lblParametersRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParametersRequired.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblParametersRequired.Location = new System.Drawing.Point(48, 54);
            this.lblParametersRequired.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParametersRequired.Name = "lblParametersRequired";
            this.lblParametersRequired.Size = new System.Drawing.Size(179, 30);
            this.lblParametersRequired.TabIndex = 9;
            this.lblParametersRequired.Text = "Select a method and customize\r\n its input parameters here";
            this.lblParametersRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblClickButtonProtocol
            // 
            this.lblClickButtonProtocol.AutoSize = true;
            this.lblClickButtonProtocol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClickButtonProtocol.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblClickButtonProtocol.Location = new System.Drawing.Point(36, 166);
            this.lblClickButtonProtocol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClickButtonProtocol.Name = "lblClickButtonProtocol";
            this.lblClickButtonProtocol.Size = new System.Drawing.Size(248, 15);
            this.lblClickButtonProtocol.TabIndex = 10;
            this.lblClickButtonProtocol.Text = "After Adding Parameters Click Here To Add It";
            this.lblClickButtonProtocol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(75, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 54);
            this.label2.TabIndex = 10;
            this.label2.Text = "List of Machines and Possible Tasks \r\nSelect one to customize it so you \r\ncan add" +
                " it to your protocol";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.txtNumberOfLoops);
            this.panel2.Controls.Add(this.txtLoopStart);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnAddInstruction);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(440, 266);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(302, 126);
            this.panel2.TabIndex = 11;
            // 
            // txtNumberOfLoops
            // 
            this.txtNumberOfLoops.Location = new System.Drawing.Point(179, 67);
            this.txtNumberOfLoops.Name = "txtNumberOfLoops";
            this.txtNumberOfLoops.Size = new System.Drawing.Size(105, 24);
            this.txtNumberOfLoops.TabIndex = 5;
            // 
            // txtLoopStart
            // 
            this.txtLoopStart.Location = new System.Drawing.Point(26, 67);
            this.txtLoopStart.Name = "txtLoopStart";
            this.txtLoopStart.Size = new System.Drawing.Size(132, 24);
            this.txtLoopStart.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(182, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Number of Loops";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Start Loop At Instruction";
            // 
            // btnAddInstruction
            // 
            this.btnAddInstruction.Location = new System.Drawing.Point(47, 97);
            this.btnAddInstruction.Name = "btnAddInstruction";
            this.btnAddInstruction.Size = new System.Drawing.Size(223, 26);
            this.btnAddInstruction.TabIndex = 1;
            this.btnAddInstruction.Text = "Add Instruction";
            this.btnAddInstruction.UseVisualStyleBackColor = true;
            this.btnAddInstruction.Click += new System.EventHandler(this.btnAddInstruction_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(52, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(218, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Add a Loop Instruction Here";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(210, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(754, 48);
            this.label1.TabIndex = 12;
            this.label1.Text = "To Create A Protocol, Add Methods And Fill Out The Required Information Here\r\nThe" +
                "n Save it to a File And Load That File In Clarity";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.txtDelayMinutes);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.btnDelayInstruction);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Location = new System.Drawing.Point(440, 410);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(302, 104);
            this.panel3.TabIndex = 12;
            // 
            // txtDelayMinutes
            // 
            this.txtDelayMinutes.Location = new System.Drawing.Point(210, 35);
            this.txtDelayMinutes.Name = "txtDelayMinutes";
            this.txtDelayMinutes.Size = new System.Drawing.Size(83, 24);
            this.txtDelayMinutes.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(22, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(177, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "Number of minutes to delay for:";
            // 
            // btnDelayInstruction
            // 
            this.btnDelayInstruction.Location = new System.Drawing.Point(45, 75);
            this.btnDelayInstruction.Name = "btnDelayInstruction";
            this.btnDelayInstruction.Size = new System.Drawing.Size(223, 26);
            this.btnDelayInstruction.TabIndex = 1;
            this.btnDelayInstruction.Text = "Add Delay Instruction";
            this.btnDelayInstruction.UseVisualStyleBackColor = true;
            this.btnDelayInstruction.Click += new System.EventHandler(this.btnDelayInstruction_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(37, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(227, 18);
            this.label9.TabIndex = 0;
            this.label9.Text = "Add a Pause Instruction Here";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel4.Controls.Add(this.txtEmails);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.txtProtocolName);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Location = new System.Drawing.Point(423, 540);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(421, 121);
            this.panel4.TabIndex = 13;
            // 
            // txtEmails
            // 
            this.txtEmails.Location = new System.Drawing.Point(151, 58);
            this.txtEmails.Name = "txtEmails";
            this.txtEmails.Size = new System.Drawing.Size(256, 24);
            this.txtEmails.TabIndex = 6;
            this.txtEmails.Text = "test@robots.com";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(288, 45);
            this.label11.TabIndex = 5;
            this.label11.Text = "Email Address of User(s):\r\n(to have error messages sent to multiple addresses \r\ni" +
                "nsert a semicolon between them)";
            // 
            // txtProtocolName
            // 
            this.txtProtocolName.Location = new System.Drawing.Point(151, 31);
            this.txtProtocolName.Name = "txtProtocolName";
            this.txtProtocolName.Size = new System.Drawing.Size(256, 24);
            this.txtProtocolName.TabIndex = 4;
            this.txtProtocolName.Text = "Write Name Here";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Name of Protocol: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(88, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(284, 18);
            this.label10.TabIndex = 0;
            this.label10.Text = "Fill Out All This Required Information";
            // 
            // btnDeleteProtocolItem
            // 
            this.btnDeleteProtocolItem.Location = new System.Drawing.Point(850, 542);
            this.btnDeleteProtocolItem.Name = "btnDeleteProtocolItem";
            this.btnDeleteProtocolItem.Size = new System.Drawing.Size(242, 30);
            this.btnDeleteProtocolItem.TabIndex = 14;
            this.btnDeleteProtocolItem.Text = "Delete Selected Instruction";
            this.btnDeleteProtocolItem.UseVisualStyleBackColor = true;
            this.btnDeleteProtocolItem.Click += new System.EventHandler(this.btnDeleteProtocolItem_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(850, 578);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(242, 30);
            this.btnMoveUp.TabIndex = 15;
            this.btnMoveUp.Text = "Move Selected Instruction Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(850, 614);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(242, 30);
            this.btnMoveDown.TabIndex = 16;
            this.btnMoveDown.Text = "Move Selected Instruction Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // tabOfProtocols
            // 
            this.tabOfProtocols.Controls.Add(this.tabPage1);
            this.tabOfProtocols.Controls.Add(this.tabVariables);
            this.tabOfProtocols.Location = new System.Drawing.Point(12, 81);
            this.tabOfProtocols.Name = "tabOfProtocols";
            this.tabOfProtocols.SelectedIndex = 0;
            this.tabOfProtocols.Size = new System.Drawing.Size(1137, 709);
            this.tabOfProtocols.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnMoveDown);
            this.tabPage1.Controls.Add(this.MethodsView);
            this.tabPage1.Controls.Add(this.btnMoveUp);
            this.tabPage1.Controls.Add(this.btnDeleteProtocolItem);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel4);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.lstProtocol);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1129, 678);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Method Creation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabVariables
            // 
            this.tabVariables.Controls.Add(this.label15);
            this.tabVariables.Controls.Add(this.lstCurrentVariables);
            this.tabVariables.Controls.Add(this.label12);
            this.tabVariables.Controls.Add(this.lstVariableTypes);
            this.tabVariables.Controls.Add(this.pnlVariables);
            this.tabVariables.Location = new System.Drawing.Point(4, 27);
            this.tabVariables.Name = "tabVariables";
            this.tabVariables.Padding = new System.Windows.Forms.Padding(3);
            this.tabVariables.Size = new System.Drawing.Size(1129, 678);
            this.tabVariables.TabIndex = 1;
            this.tabVariables.Text = "Make Variables";
            this.tabVariables.UseVisualStyleBackColor = true;
            // 
            // pnlVariables
            // 
            this.pnlVariables.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlVariables.Controls.Add(this.label13);
            this.pnlVariables.Controls.Add(this.btrnAddVariable);
            this.pnlVariables.Controls.Add(this.dataViewVariable);
            this.pnlVariables.Controls.Add(this.label14);
            this.pnlVariables.Location = new System.Drawing.Point(348, 91);
            this.pnlVariables.Name = "pnlVariables";
            this.pnlVariables.Size = new System.Drawing.Size(361, 278);
            this.pnlVariables.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(17, 18);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(329, 18);
            this.label13.TabIndex = 4;
            this.label13.Text = "Name Variable and Add It To The  Protocol";
            // 
            // btrnAddVariable
            // 
            this.btrnAddVariable.Location = new System.Drawing.Point(74, 222);
            this.btrnAddVariable.Name = "btrnAddVariable";
            this.btrnAddVariable.Size = new System.Drawing.Size(223, 27);
            this.btrnAddVariable.TabIndex = 8;
            this.btrnAddVariable.Text = "Add Variable To Protocol";
            this.btrnAddVariable.UseVisualStyleBackColor = true;
            this.btrnAddVariable.Click += new System.EventHandler(this.btrnAddVariable_Click);
            // 
            // dataViewVariable
            // 
            this.dataViewVariable.BackgroundColor = System.Drawing.Color.Navy;
            this.dataViewVariable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataViewVariable.Location = new System.Drawing.Point(20, 58);
            this.dataViewVariable.Margin = new System.Windows.Forms.Padding(4);
            this.dataViewVariable.Name = "dataViewVariable";
            this.dataViewVariable.Size = new System.Drawing.Size(326, 119);
            this.dataViewVariable.TabIndex = 3;
            this.dataViewVariable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataViewVariable_DataError);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(40, 195);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(286, 15);
            this.label14.TabIndex = 10;
            this.label14.Text = "After Adding a Name and Value Click Here To Add It";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstVariableTypes
            // 
            this.lstVariableTypes.FormattingEnabled = true;
            this.lstVariableTypes.ItemHeight = 18;
            this.lstVariableTypes.Items.AddRange(new object[] {
            "String",
            "Integer",
            "Floating Point"});
            this.lstVariableTypes.Location = new System.Drawing.Point(24, 115);
            this.lstVariableTypes.Name = "lstVariableTypes";
            this.lstVariableTypes.Size = new System.Drawing.Size(248, 76);
            this.lstVariableTypes.TabIndex = 11;
            this.lstVariableTypes.SelectedIndexChanged += new System.EventHandler(this.lstVariableTypes_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(16, 42);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(256, 18);
            this.label12.TabIndex = 11;
            this.label12.Text = "Select A Variable Type To Create";
            // 
            // lstCurrentVariables
            // 
            this.lstCurrentVariables.FormattingEnabled = true;
            this.lstCurrentVariables.ItemHeight = 18;
            this.lstCurrentVariables.Location = new System.Drawing.Point(795, 79);
            this.lstCurrentVariables.Name = "lstCurrentVariables";
            this.lstCurrentVariables.Size = new System.Drawing.Size(248, 436);
            this.lstCurrentVariables.TabIndex = 12;
            this.lstCurrentVariables.SelectedIndexChanged += new System.EventHandler(this.lstCurrentVariables_SelectedIndexChanged);
            this.lstCurrentVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstCurrentVariables_KeyDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(821, 42);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(138, 18);
            this.label15.TabIndex = 13;
            this.label15.Text = "Current Variables";
            // 
            // MakeProtocols
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(1149, 802);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabOfProtocols);
            this.Controls.Add(this.lblMethodName);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MakeProtocols";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clarity - Protocol Maker";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MakeProtocols_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblMethodParameterView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabOfProtocols.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabVariables.ResumeLayout(false);
            this.tabVariables.PerformLayout();
            this.pnlVariables.ResumeLayout(false);
            this.pnlVariables.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewVariable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView MethodsView;
        private System.Windows.Forms.ListBox lstProtocol;
        private System.Windows.Forms.DataGridView tblMethodParameterView;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.Label lblMethodName;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileOperationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadProtocolToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddMethod;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblClickButtonProtocol;
        private System.Windows.Forms.Label lblParametersRequired;
        private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddInstruction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNumberOfLoops;
        private System.Windows.Forms.TextBox txtLoopStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtDelayMinutes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDelayInstruction;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtProtocolName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEmails;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnDeleteProtocolItem;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.ToolStripMenuItem insertOtherProtocolAtEndOfThisOneToolStripMenuItem;
        private System.Windows.Forms.TabControl tabOfProtocols;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabVariables;
        private System.Windows.Forms.ListBox lstVariableTypes;
        private System.Windows.Forms.Panel pnlVariables;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btrnAddVariable;
        private System.Windows.Forms.DataGridView dataViewVariable;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListBox lstCurrentVariables;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
    }
}