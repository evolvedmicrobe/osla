namespace Clarity
{
    partial class ClarityForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClarityForm));
            this.MainTab = new System.Windows.Forms.TabControl();
            this.tabSubMain = new System.Windows.Forms.TabPage();
            this.btnInstrumentRefresh = new System.Windows.Forms.Button();
            this.pnlFailure = new System.Windows.Forms.Panel();
            this.lblFailureInstructionName = new System.Windows.Forms.Label();
            this.lblFailure = new System.Windows.Forms.Label();
            this.btnRetryLastInstruction = new System.Windows.Forms.Button();
            this.lblCurrentRunningProtocol = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteProtocol = new System.Windows.Forms.Button();
            this.btnChangeCurrentProtPosition = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lstLoadedProtocols = new System.Windows.Forms.ListBox();
            this.lstSelectedProtocol = new System.Windows.Forms.ListBox();
            this.btnCancelProtocolExecution = new System.Windows.Forms.Button();
            this.btnExecuteProtocols = new System.Windows.Forms.Button();
            this.lstCurrentProtocol = new System.Windows.Forms.ListBox();
            this.btnMakeProtocols = new System.Windows.Forms.Button();
            this.tabRecovery = new System.Windows.Forms.TabPage();
            this.btnEmailOkay = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.wBrowRecovInstructions = new System.Windows.Forms.WebBrowser();
            this.IncubatorTab = new System.Windows.Forms.TabPage();
            this.btnStopIncubatorShaking = new System.Windows.Forms.Button();
            this.btnReinitializeIncubator = new System.Windows.Forms.Button();
            this.btnResetIncubator = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstIncubatorSlots = new System.Windows.Forms.ListBox();
            this.btnChangeShakingSpeed = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbShakeSpeed = new System.Windows.Forms.ComboBox();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.btnPerformCommand = new System.Windows.Forms.Button();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.btnUnloadPlate = new System.Windows.Forms.Button();
            this.btnLoadPlate = new System.Windows.Forms.Button();
            this.btnStopShaking = new System.Windows.Forms.Button();
            this.btnStartShaking = new System.Windows.Forms.Button();
            this.tabMakeGrowthInstructions = new System.Windows.Forms.TabPage();
            this.textbox_number = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.chkGBO = new System.Windows.Forms.CheckBox();
            this.chk48WellPlate = new System.Windows.Forms.CheckBox();
            this.txtGrowthRateMinutes = new System.Windows.Forms.TextBox();
            this.txtGrowthRateTimesToMeasure = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnStartGrowthRate = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.txtGrowthRateEmail = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtGrowthRateExperimentName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lstGrowthRatesProtocol = new System.Windows.Forms.ListBox();
            this.wBrowGrowthRate = new System.Windows.Forms.WebBrowser();
            this.tabNSFExperiment = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtNSFName = new System.Windows.Forms.TextBox();
            this.txtNSFTransferNumber = new System.Windows.Forms.TextBox();
            this.btnGenerateNSFData = new System.Windows.Forms.Button();
            this.lstNSFPlates = new System.Windows.Forms.ListBox();
            this.lstInstrumentStatus = new System.Windows.Forms.ListBox();
            this.lblErrorLog = new System.Windows.Forms.Label();
            this.btnViewAdvancedControls = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCurrentProtocolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPreviousSystemStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recoverLastProtcolInstructionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alarmStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOnAlarmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffAlarmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAlarmStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reconnectToAlarmServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtErrorLog = new System.Windows.Forms.RichTextBox();
            this.TimeToGo = new Clarity.CountdownTimer();
            this.MainTab.SuspendLayout();
            this.tabSubMain.SuspendLayout();
            this.pnlFailure.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabRecovery.SuspendLayout();
            this.IncubatorTab.SuspendLayout();
            this.tabMakeGrowthInstructions.SuspendLayout();
            this.tabNSFExperiment.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.tabSubMain);
            this.MainTab.Controls.Add(this.tabRecovery);
            this.MainTab.Controls.Add(this.IncubatorTab);
            this.MainTab.Controls.Add(this.tabMakeGrowthInstructions);
            this.MainTab.Controls.Add(this.tabNSFExperiment);
            this.MainTab.Location = new System.Drawing.Point(14, 30);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(796, 677);
            this.MainTab.TabIndex = 0;
            // 
            // tabSubMain
            // 
            this.tabSubMain.Controls.Add(this.btnInstrumentRefresh);
            this.tabSubMain.Controls.Add(this.pnlFailure);
            this.tabSubMain.Controls.Add(this.lblCurrentRunningProtocol);
            this.tabSubMain.Controls.Add(this.panel1);
            this.tabSubMain.Controls.Add(this.btnCancelProtocolExecution);
            this.tabSubMain.Controls.Add(this.btnExecuteProtocols);
            this.tabSubMain.Controls.Add(this.lstCurrentProtocol);
            this.tabSubMain.Controls.Add(this.btnMakeProtocols);
            this.tabSubMain.Location = new System.Drawing.Point(4, 24);
            this.tabSubMain.Name = "tabSubMain";
            this.tabSubMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabSubMain.Size = new System.Drawing.Size(788, 649);
            this.tabSubMain.TabIndex = 0;
            this.tabSubMain.Text = "Run Protocols";
            this.tabSubMain.UseVisualStyleBackColor = true;
            // 
            // btnInstrumentRefresh
            // 
            this.btnInstrumentRefresh.Location = new System.Drawing.Point(7, 186);
            this.btnInstrumentRefresh.Name = "btnInstrumentRefresh";
            this.btnInstrumentRefresh.Size = new System.Drawing.Size(184, 23);
            this.btnInstrumentRefresh.TabIndex = 10;
            this.btnInstrumentRefresh.Text = "Refresh Instrument Status";
            this.btnInstrumentRefresh.UseVisualStyleBackColor = true;
            this.btnInstrumentRefresh.Click += new System.EventHandler(this.btnInstrumentRefresh_Click);
            // 
            // pnlFailure
            // 
            this.pnlFailure.Controls.Add(this.lblFailureInstructionName);
            this.pnlFailure.Controls.Add(this.lblFailure);
            this.pnlFailure.Controls.Add(this.btnRetryLastInstruction);
            this.pnlFailure.Location = new System.Drawing.Point(13, 299);
            this.pnlFailure.Name = "pnlFailure";
            this.pnlFailure.Size = new System.Drawing.Size(746, 91);
            this.pnlFailure.TabIndex = 9;
            this.pnlFailure.Visible = false;
            // 
            // lblFailureInstructionName
            // 
            this.lblFailureInstructionName.AutoSize = true;
            this.lblFailureInstructionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFailureInstructionName.Location = new System.Drawing.Point(33, 32);
            this.lblFailureInstructionName.Name = "lblFailureInstructionName";
            this.lblFailureInstructionName.Size = new System.Drawing.Size(226, 17);
            this.lblFailureInstructionName.TabIndex = 10;
            this.lblFailureInstructionName.Text = "The Failed Instruction Will Be Here";
            // 
            // lblFailure
            // 
            this.lblFailure.AutoSize = true;
            this.lblFailure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFailure.Location = new System.Drawing.Point(4, 4);
            this.lblFailure.Name = "lblFailure";
            this.lblFailure.Size = new System.Drawing.Size(322, 13);
            this.lblFailure.TabIndex = 9;
            this.lblFailure.Text = "I will show an option to retry an instruction if it fails during a protocol";
            // 
            // btnRetryLastInstruction
            // 
            this.btnRetryLastInstruction.Location = new System.Drawing.Point(503, 56);
            this.btnRetryLastInstruction.Name = "btnRetryLastInstruction";
            this.btnRetryLastInstruction.Size = new System.Drawing.Size(184, 27);
            this.btnRetryLastInstruction.TabIndex = 8;
            this.btnRetryLastInstruction.Text = "Retry Last Failed Instruction";
            this.btnRetryLastInstruction.UseVisualStyleBackColor = true;
            this.btnRetryLastInstruction.Click += new System.EventHandler(this.btnRetryLastInstruction_Click);
            // 
            // lblCurrentRunningProtocol
            // 
            this.lblCurrentRunningProtocol.AutoSize = true;
            this.lblCurrentRunningProtocol.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRunningProtocol.Location = new System.Drawing.Point(195, 19);
            this.lblCurrentRunningProtocol.Name = "lblCurrentRunningProtocol";
            this.lblCurrentRunningProtocol.Size = new System.Drawing.Size(192, 17);
            this.lblCurrentRunningProtocol.TabIndex = 7;
            this.lblCurrentRunningProtocol.Text = "Current Running Protocol";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.btnDeleteProtocol);
            this.panel1.Controls.Add(this.btnChangeCurrentProtPosition);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lstLoadedProtocols);
            this.panel1.Controls.Add(this.lstSelectedProtocol);
            this.panel1.Location = new System.Drawing.Point(6, 396);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 247);
            this.panel1.TabIndex = 6;
            // 
            // btnDeleteProtocol
            // 
            this.btnDeleteProtocol.Location = new System.Drawing.Point(7, 212);
            this.btnDeleteProtocol.Name = "btnDeleteProtocol";
            this.btnDeleteProtocol.Size = new System.Drawing.Size(247, 23);
            this.btnDeleteProtocol.TabIndex = 7;
            this.btnDeleteProtocol.Text = "Delete Currently Selected Protocol";
            this.btnDeleteProtocol.UseVisualStyleBackColor = true;
            this.btnDeleteProtocol.Click += new System.EventHandler(this.btnDeleteProtocol_Click);
            // 
            // btnChangeCurrentProtPosition
            // 
            this.btnChangeCurrentProtPosition.Location = new System.Drawing.Point(500, 10);
            this.btnChangeCurrentProtPosition.Name = "btnChangeCurrentProtPosition";
            this.btnChangeCurrentProtPosition.Size = new System.Drawing.Size(253, 23);
            this.btnChangeCurrentProtPosition.TabIndex = 6;
            this.btnChangeCurrentProtPosition.Text = "Change Current Protocol Position";
            this.btnChangeCurrentProtPosition.UseVisualStyleBackColor = true;
            this.btnChangeCurrentProtPosition.Click += new System.EventHandler(this.btnChangeCurrentProtPosition_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(181, 15);
            this.label9.TabIndex = 5;
            this.label9.Text = "Protocol Name - Next Run Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(279, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Protocol Description";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "Loaded Protocols";
            // 
            // lstLoadedProtocols
            // 
            this.lstLoadedProtocols.FormattingEnabled = true;
            this.lstLoadedProtocols.HorizontalScrollbar = true;
            this.lstLoadedProtocols.ItemHeight = 15;
            this.lstLoadedProtocols.Location = new System.Drawing.Point(7, 64);
            this.lstLoadedProtocols.Name = "lstLoadedProtocols";
            this.lstLoadedProtocols.Size = new System.Drawing.Size(247, 139);
            this.lstLoadedProtocols.TabIndex = 1;
            this.lstLoadedProtocols.SelectedIndexChanged += new System.EventHandler(this.lstLoadedProtocols_SelectedIndexChanged);
            this.lstLoadedProtocols.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstLoadedProtocols_KeyDown);
            // 
            // lstSelectedProtocol
            // 
            this.lstSelectedProtocol.FormattingEnabled = true;
            this.lstSelectedProtocol.ItemHeight = 15;
            this.lstSelectedProtocol.Location = new System.Drawing.Point(275, 64);
            this.lstSelectedProtocol.Name = "lstSelectedProtocol";
            this.lstSelectedProtocol.Size = new System.Drawing.Size(484, 169);
            this.lstSelectedProtocol.TabIndex = 2;
            // 
            // btnCancelProtocolExecution
            // 
            this.btnCancelProtocolExecution.Location = new System.Drawing.Point(6, 107);
            this.btnCancelProtocolExecution.Name = "btnCancelProtocolExecution";
            this.btnCancelProtocolExecution.Size = new System.Drawing.Size(184, 23);
            this.btnCancelProtocolExecution.TabIndex = 5;
            this.btnCancelProtocolExecution.Text = "Cancel Protocol Execution";
            this.btnCancelProtocolExecution.UseVisualStyleBackColor = true;
            this.btnCancelProtocolExecution.Click += new System.EventHandler(this.btnCancelProtocolExecution_Click);
            // 
            // btnExecuteProtocols
            // 
            this.btnExecuteProtocols.Location = new System.Drawing.Point(6, 71);
            this.btnExecuteProtocols.Name = "btnExecuteProtocols";
            this.btnExecuteProtocols.Size = new System.Drawing.Size(184, 23);
            this.btnExecuteProtocols.TabIndex = 4;
            this.btnExecuteProtocols.Text = "Start Protocol Execution";
            this.btnExecuteProtocols.UseVisualStyleBackColor = true;
            this.btnExecuteProtocols.Click += new System.EventHandler(this.btnExecuteProtocols_Click);
            // 
            // lstCurrentProtocol
            // 
            this.lstCurrentProtocol.FormattingEnabled = true;
            this.lstCurrentProtocol.ItemHeight = 15;
            this.lstCurrentProtocol.Location = new System.Drawing.Point(198, 45);
            this.lstCurrentProtocol.Name = "lstCurrentProtocol";
            this.lstCurrentProtocol.Size = new System.Drawing.Size(567, 244);
            this.lstCurrentProtocol.TabIndex = 3;
            // 
            // btnMakeProtocols
            // 
            this.btnMakeProtocols.Location = new System.Drawing.Point(6, 146);
            this.btnMakeProtocols.Name = "btnMakeProtocols";
            this.btnMakeProtocols.Size = new System.Drawing.Size(184, 23);
            this.btnMakeProtocols.TabIndex = 0;
            this.btnMakeProtocols.Text = "Create New Protocols";
            this.btnMakeProtocols.UseVisualStyleBackColor = true;
            this.btnMakeProtocols.Click += new System.EventHandler(this.btnMakeProtocols_Click);
            // 
            // tabRecovery
            // 
            this.tabRecovery.Controls.Add(this.btnEmailOkay);
            this.tabRecovery.Controls.Add(this.label8);
            this.tabRecovery.Controls.Add(this.wBrowRecovInstructions);
            this.tabRecovery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabRecovery.Location = new System.Drawing.Point(4, 24);
            this.tabRecovery.Name = "tabRecovery";
            this.tabRecovery.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecovery.Size = new System.Drawing.Size(788, 649);
            this.tabRecovery.TabIndex = 2;
            this.tabRecovery.Text = "Recover From Errors";
            this.tabRecovery.UseVisualStyleBackColor = true;
            // 
            // btnEmailOkay
            // 
            this.btnEmailOkay.BackColor = System.Drawing.Color.Snow;
            this.btnEmailOkay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnEmailOkay.Location = new System.Drawing.Point(19, 580);
            this.btnEmailOkay.Name = "btnEmailOkay";
            this.btnEmailOkay.Size = new System.Drawing.Size(233, 41);
            this.btnEmailOkay.TabIndex = 11;
            this.btnEmailOkay.Text = "Email System OK to Users";
            this.btnEmailOkay.UseVisualStyleBackColor = false;
            this.btnEmailOkay.Click += new System.EventHandler(this.btnEmailOkay_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 245);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(204, 68);
            this.label8.TabIndex = 1;
            this.label8.Text = "Wait 5 mins after attempting\r\na recovery\r\n\r\nMore Advanced Controls Below";
            // 
            // wBrowRecovInstructions
            // 
            this.wBrowRecovInstructions.AllowNavigation = false;
            this.wBrowRecovInstructions.Dock = System.Windows.Forms.DockStyle.Right;
            this.wBrowRecovInstructions.Location = new System.Drawing.Point(267, 3);
            this.wBrowRecovInstructions.MinimumSize = new System.Drawing.Size(20, 20);
            this.wBrowRecovInstructions.Name = "wBrowRecovInstructions";
            this.wBrowRecovInstructions.ScriptErrorsSuppressed = true;
            this.wBrowRecovInstructions.Size = new System.Drawing.Size(518, 643);
            this.wBrowRecovInstructions.TabIndex = 0;
            this.wBrowRecovInstructions.WebBrowserShortcutsEnabled = false;
            // 
            // IncubatorTab
            // 
            this.IncubatorTab.Controls.Add(this.btnStopIncubatorShaking);
            this.IncubatorTab.Controls.Add(this.btnReinitializeIncubator);
            this.IncubatorTab.Controls.Add(this.btnResetIncubator);
            this.IncubatorTab.Controls.Add(this.label4);
            this.IncubatorTab.Controls.Add(this.label3);
            this.IncubatorTab.Controls.Add(this.label2);
            this.IncubatorTab.Controls.Add(this.lstIncubatorSlots);
            this.IncubatorTab.Controls.Add(this.btnChangeShakingSpeed);
            this.IncubatorTab.Controls.Add(this.label1);
            this.IncubatorTab.Controls.Add(this.cmbShakeSpeed);
            this.IncubatorTab.Controls.Add(this.txtResponse);
            this.IncubatorTab.Controls.Add(this.btnPerformCommand);
            this.IncubatorTab.Controls.Add(this.txtCommand);
            this.IncubatorTab.Controls.Add(this.btnUnloadPlate);
            this.IncubatorTab.Controls.Add(this.btnLoadPlate);
            this.IncubatorTab.Controls.Add(this.btnStopShaking);
            this.IncubatorTab.Controls.Add(this.btnStartShaking);
            this.IncubatorTab.Location = new System.Drawing.Point(4, 24);
            this.IncubatorTab.Name = "IncubatorTab";
            this.IncubatorTab.Padding = new System.Windows.Forms.Padding(3);
            this.IncubatorTab.Size = new System.Drawing.Size(788, 649);
            this.IncubatorTab.TabIndex = 1;
            this.IncubatorTab.Text = "Incubator Controls";
            this.IncubatorTab.UseVisualStyleBackColor = true;
            // 
            // btnStopIncubatorShaking
            // 
            this.btnStopIncubatorShaking.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopIncubatorShaking.Location = new System.Drawing.Point(477, 496);
            this.btnStopIncubatorShaking.Name = "btnStopIncubatorShaking";
            this.btnStopIncubatorShaking.Size = new System.Drawing.Size(305, 27);
            this.btnStopIncubatorShaking.TabIndex = 19;
            this.btnStopIncubatorShaking.Text = "Stop Shaking When Nothing Else Will";
            this.btnStopIncubatorShaking.UseVisualStyleBackColor = true;
            this.btnStopIncubatorShaking.Click += new System.EventHandler(this.btnStopIncubatorShaking_Click);
            // 
            // btnReinitializeIncubator
            // 
            this.btnReinitializeIncubator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReinitializeIncubator.Location = new System.Drawing.Point(477, 442);
            this.btnReinitializeIncubator.Name = "btnReinitializeIncubator";
            this.btnReinitializeIncubator.Size = new System.Drawing.Size(305, 27);
            this.btnReinitializeIncubator.TabIndex = 15;
            this.btnReinitializeIncubator.Text = "Reinitialize Incubator";
            this.btnReinitializeIncubator.UseVisualStyleBackColor = true;
            this.btnReinitializeIncubator.Click += new System.EventHandler(this.btnReinitializeIncubator_Click);
            // 
            // btnResetIncubator
            // 
            this.btnResetIncubator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetIncubator.Location = new System.Drawing.Point(477, 386);
            this.btnResetIncubator.Name = "btnResetIncubator";
            this.btnResetIncubator.Size = new System.Drawing.Size(305, 27);
            this.btnResetIncubator.TabIndex = 14;
            this.btnResetIncubator.Text = "Reset Incubator";
            this.btnResetIncubator.UseVisualStyleBackColor = true;
            this.btnResetIncubator.Click += new System.EventHandler(this.btnResetIncubator_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(343, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Select Plate Slot";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 592);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Command Response (Error if not \"OK\")";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 531);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Type Manual Command To Enter Here";
            // 
            // lstIncubatorSlots
            // 
            this.lstIncubatorSlots.FormattingEnabled = true;
            this.lstIncubatorSlots.ItemHeight = 15;
            this.lstIncubatorSlots.Location = new System.Drawing.Point(322, 38);
            this.lstIncubatorSlots.Name = "lstIncubatorSlots";
            this.lstIncubatorSlots.Size = new System.Drawing.Size(139, 259);
            this.lstIncubatorSlots.TabIndex = 10;
            // 
            // btnChangeShakingSpeed
            // 
            this.btnChangeShakingSpeed.Location = new System.Drawing.Point(34, 87);
            this.btnChangeShakingSpeed.Name = "btnChangeShakingSpeed";
            this.btnChangeShakingSpeed.Size = new System.Drawing.Size(163, 27);
            this.btnChangeShakingSpeed.TabIndex = 9;
            this.btnChangeShakingSpeed.Text = "Change Shaking Speed";
            this.btnChangeShakingSpeed.UseVisualStyleBackColor = true;
            this.btnChangeShakingSpeed.Click += new System.EventHandler(this.btnChangeShakingSpeed_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose Shaking Speed (RPMs)";
            // 
            // cmbShakeSpeed
            // 
            this.cmbShakeSpeed.FormattingEnabled = true;
            this.cmbShakeSpeed.Items.AddRange(new object[] {
            "100",
            "200",
            "300",
            "400",
            "500",
            "600",
            "650",
            "700",
            "750",
            "800",
            "850",
            "900",
            "950",
            "1000",
            "1100",
            "1200"});
            this.cmbShakeSpeed.Location = new System.Drawing.Point(34, 48);
            this.cmbShakeSpeed.Name = "cmbShakeSpeed";
            this.cmbShakeSpeed.Size = new System.Drawing.Size(174, 23);
            this.cmbShakeSpeed.TabIndex = 7;
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(18, 611);
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(306, 21);
            this.txtResponse.TabIndex = 6;
            // 
            // btnPerformCommand
            // 
            this.btnPerformCommand.Location = new System.Drawing.Point(346, 562);
            this.btnPerformCommand.Name = "btnPerformCommand";
            this.btnPerformCommand.Size = new System.Drawing.Size(163, 27);
            this.btnPerformCommand.TabIndex = 5;
            this.btnPerformCommand.Text = "Perform Command";
            this.btnPerformCommand.UseVisualStyleBackColor = true;
            this.btnPerformCommand.Click += new System.EventHandler(this.btnPerformCommand_Click);
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(18, 562);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(306, 21);
            this.txtCommand.TabIndex = 4;
            // 
            // btnUnloadPlate
            // 
            this.btnUnloadPlate.Location = new System.Drawing.Point(477, 137);
            this.btnUnloadPlate.Name = "btnUnloadPlate";
            this.btnUnloadPlate.Size = new System.Drawing.Size(163, 27);
            this.btnUnloadPlate.TabIndex = 3;
            this.btnUnloadPlate.Text = "Unload Plate";
            this.btnUnloadPlate.UseVisualStyleBackColor = true;
            this.btnUnloadPlate.Click += new System.EventHandler(this.btnUnloadPlate_Click);
            // 
            // btnLoadPlate
            // 
            this.btnLoadPlate.Location = new System.Drawing.Point(477, 104);
            this.btnLoadPlate.Name = "btnLoadPlate";
            this.btnLoadPlate.Size = new System.Drawing.Size(163, 27);
            this.btnLoadPlate.TabIndex = 2;
            this.btnLoadPlate.Text = "Load Plate";
            this.btnLoadPlate.UseVisualStyleBackColor = true;
            this.btnLoadPlate.Click += new System.EventHandler(this.btnLoadPlate_Click);
            // 
            // btnStopShaking
            // 
            this.btnStopShaking.Location = new System.Drawing.Point(34, 177);
            this.btnStopShaking.Name = "btnStopShaking";
            this.btnStopShaking.Size = new System.Drawing.Size(163, 27);
            this.btnStopShaking.TabIndex = 1;
            this.btnStopShaking.Text = "Stop Shaking";
            this.btnStopShaking.UseVisualStyleBackColor = true;
            this.btnStopShaking.Click += new System.EventHandler(this.btnStopShaking_Click);
            // 
            // btnStartShaking
            // 
            this.btnStartShaking.Location = new System.Drawing.Point(34, 133);
            this.btnStartShaking.Name = "btnStartShaking";
            this.btnStartShaking.Size = new System.Drawing.Size(163, 27);
            this.btnStartShaking.TabIndex = 0;
            this.btnStartShaking.Text = "Start Shaking";
            this.btnStartShaking.UseVisualStyleBackColor = true;
            this.btnStartShaking.Click += new System.EventHandler(this.btnStartShaking_Click);
            // 
            // tabMakeGrowthInstructions
            // 
            this.tabMakeGrowthInstructions.Controls.Add(this.textbox_number);
            this.tabMakeGrowthInstructions.Controls.Add(this.label18);
            this.tabMakeGrowthInstructions.Controls.Add(this.chkGBO);
            this.tabMakeGrowthInstructions.Controls.Add(this.chk48WellPlate);
            this.tabMakeGrowthInstructions.Controls.Add(this.txtGrowthRateMinutes);
            this.tabMakeGrowthInstructions.Controls.Add(this.txtGrowthRateTimesToMeasure);
            this.tabMakeGrowthInstructions.Controls.Add(this.label15);
            this.tabMakeGrowthInstructions.Controls.Add(this.btnStartGrowthRate);
            this.tabMakeGrowthInstructions.Controls.Add(this.label14);
            this.tabMakeGrowthInstructions.Controls.Add(this.txtGrowthRateEmail);
            this.tabMakeGrowthInstructions.Controls.Add(this.label13);
            this.tabMakeGrowthInstructions.Controls.Add(this.label12);
            this.tabMakeGrowthInstructions.Controls.Add(this.txtGrowthRateExperimentName);
            this.tabMakeGrowthInstructions.Controls.Add(this.label11);
            this.tabMakeGrowthInstructions.Controls.Add(this.label10);
            this.tabMakeGrowthInstructions.Controls.Add(this.lstGrowthRatesProtocol);
            this.tabMakeGrowthInstructions.Controls.Add(this.wBrowGrowthRate);
            this.tabMakeGrowthInstructions.Location = new System.Drawing.Point(4, 24);
            this.tabMakeGrowthInstructions.Name = "tabMakeGrowthInstructions";
            this.tabMakeGrowthInstructions.Padding = new System.Windows.Forms.Padding(3);
            this.tabMakeGrowthInstructions.Size = new System.Drawing.Size(788, 649);
            this.tabMakeGrowthInstructions.TabIndex = 3;
            this.tabMakeGrowthInstructions.Text = "Make A Growth Rate Experiment Protocol";
            this.tabMakeGrowthInstructions.UseVisualStyleBackColor = true;
            // 
            // textbox_number
            // 
            this.textbox_number.Location = new System.Drawing.Point(442, 536);
            this.textbox_number.Name = "textbox_number";
            this.textbox_number.Size = new System.Drawing.Size(240, 21);
            this.textbox_number.TabIndex = 20;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(373, 513);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(274, 20);
            this.label18.TabIndex = 19;
            this.label18.Text = "F - Type your phone number here";
            // 
            // chkGBO
            // 
            this.chkGBO.AutoSize = true;
            this.chkGBO.Checked = true;
            this.chkGBO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGBO.Location = new System.Drawing.Point(645, 93);
            this.chkGBO.Name = "chkGBO";
            this.chkGBO.Size = new System.Drawing.Size(117, 17);
            this.chkGBO.TabIndex = 18;
            this.chkGBO.Text = "48 Well Excel Data";
            this.chkGBO.UseVisualStyleBackColor = true;
            this.chkGBO.CheckedChanged += new System.EventHandler(this.chkGBO_CheckedChanged);
            // 
            // chk48WellPlate
            // 
            this.chk48WellPlate.AutoSize = true;
            this.chk48WellPlate.Location = new System.Drawing.Point(645, 71);
            this.chk48WellPlate.Name = "chk48WellPlate";
            this.chk48WellPlate.Size = new System.Drawing.Size(89, 17);
            this.chk48WellPlate.TabIndex = 14;
            this.chk48WellPlate.Text = "48 Well Plate";
            this.chk48WellPlate.UseVisualStyleBackColor = true;
            this.chk48WellPlate.CheckedChanged += new System.EventHandler(this.chk48WellPlate_CheckedChanged);
            // 
            // txtGrowthRateMinutes
            // 
            this.txtGrowthRateMinutes.Location = new System.Drawing.Point(442, 288);
            this.txtGrowthRateMinutes.Name = "txtGrowthRateMinutes";
            this.txtGrowthRateMinutes.Size = new System.Drawing.Size(240, 21);
            this.txtGrowthRateMinutes.TabIndex = 13;
            // 
            // txtGrowthRateTimesToMeasure
            // 
            this.txtGrowthRateTimesToMeasure.Location = new System.Drawing.Point(442, 350);
            this.txtGrowthRateTimesToMeasure.Name = "txtGrowthRateTimesToMeasure";
            this.txtGrowthRateTimesToMeasure.Size = new System.Drawing.Size(240, 21);
            this.txtGrowthRateTimesToMeasure.TabIndex = 14;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(373, 327);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(377, 20);
            this.label15.TabIndex = 11;
            this.label15.Text = "C-Specify the number of times to measure OD";
            // 
            // btnStartGrowthRate
            // 
            this.btnStartGrowthRate.Location = new System.Drawing.Point(442, 614);
            this.btnStartGrowthRate.Name = "btnStartGrowthRate";
            this.btnStartGrowthRate.Size = new System.Drawing.Size(240, 28);
            this.btnStartGrowthRate.TabIndex = 17;
            this.btnStartGrowthRate.Text = "Begin Protocol";
            this.btnStartGrowthRate.UseVisualStyleBackColor = true;
            this.btnStartGrowthRate.Click += new System.EventHandler(this.btnStartGrowthRate_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(373, 575);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(348, 20);
            this.label14.TabIndex = 9;
            this.label14.Text = "G - Double Check Everything and Hit Start";
            // 
            // txtGrowthRateEmail
            // 
            this.txtGrowthRateEmail.Location = new System.Drawing.Point(442, 474);
            this.txtGrowthRateEmail.Name = "txtGrowthRateEmail";
            this.txtGrowthRateEmail.Size = new System.Drawing.Size(240, 21);
            this.txtGrowthRateEmail.TabIndex = 16;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(373, 451);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(271, 20);
            this.label13.TabIndex = 7;
            this.label13.Text = "E - Type your email address here";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(373, 265);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(413, 20);
            this.label12.TabIndex = 5;
            this.label12.Text = "B - Specify the delay, in minutes, in between reads";
            // 
            // txtGrowthRateExperimentName
            // 
            this.txtGrowthRateExperimentName.Location = new System.Drawing.Point(442, 412);
            this.txtGrowthRateExperimentName.Name = "txtGrowthRateExperimentName";
            this.txtGrowthRateExperimentName.Size = new System.Drawing.Size(240, 21);
            this.txtGrowthRateExperimentName.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(373, 389);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(266, 20);
            this.label11.TabIndex = 3;
            this.label11.Text = "D - Name Your Experiment Here";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(373, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(393, 20);
            this.label10.TabIndex = 2;
            this.label10.Text = "A- Select The Slots That Have Your Plates Here";
            // 
            // lstGrowthRatesProtocol
            // 
            this.lstGrowthRatesProtocol.FormattingEnabled = true;
            this.lstGrowthRatesProtocol.ItemHeight = 15;
            this.lstGrowthRatesProtocol.Location = new System.Drawing.Point(403, 36);
            this.lstGrowthRatesProtocol.Name = "lstGrowthRatesProtocol";
            this.lstGrowthRatesProtocol.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstGrowthRatesProtocol.Size = new System.Drawing.Size(240, 214);
            this.lstGrowthRatesProtocol.TabIndex = 1;
            // 
            // wBrowGrowthRate
            // 
            this.wBrowGrowthRate.Dock = System.Windows.Forms.DockStyle.Left;
            this.wBrowGrowthRate.Location = new System.Drawing.Point(3, 3);
            this.wBrowGrowthRate.MinimumSize = new System.Drawing.Size(20, 20);
            this.wBrowGrowthRate.Name = "wBrowGrowthRate";
            this.wBrowGrowthRate.Size = new System.Drawing.Size(364, 643);
            this.wBrowGrowthRate.TabIndex = 0;
            // 
            // tabNSFExperiment
            // 
            this.tabNSFExperiment.Controls.Add(this.label17);
            this.tabNSFExperiment.Controls.Add(this.label16);
            this.tabNSFExperiment.Controls.Add(this.txtNSFName);
            this.tabNSFExperiment.Controls.Add(this.txtNSFTransferNumber);
            this.tabNSFExperiment.Controls.Add(this.btnGenerateNSFData);
            this.tabNSFExperiment.Controls.Add(this.lstNSFPlates);
            this.tabNSFExperiment.Location = new System.Drawing.Point(4, 24);
            this.tabNSFExperiment.Name = "tabNSFExperiment";
            this.tabNSFExperiment.Padding = new System.Windows.Forms.Padding(3);
            this.tabNSFExperiment.Size = new System.Drawing.Size(788, 649);
            this.tabNSFExperiment.TabIndex = 4;
            this.tabNSFExperiment.Text = "NSF Experiment";
            this.tabNSFExperiment.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(317, 460);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(33, 15);
            this.label17.TabIndex = 5;
            this.label17.Text = "Date";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(320, 503);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 15);
            this.label16.TabIndex = 4;
            this.label16.Text = "Transfer Number";
            // 
            // txtNSFName
            // 
            this.txtNSFName.Location = new System.Drawing.Point(429, 460);
            this.txtNSFName.Name = "txtNSFName";
            this.txtNSFName.Size = new System.Drawing.Size(135, 21);
            this.txtNSFName.TabIndex = 3;
            // 
            // txtNSFTransferNumber
            // 
            this.txtNSFTransferNumber.Location = new System.Drawing.Point(429, 496);
            this.txtNSFTransferNumber.Name = "txtNSFTransferNumber";
            this.txtNSFTransferNumber.Size = new System.Drawing.Size(135, 21);
            this.txtNSFTransferNumber.TabIndex = 2;
            // 
            // btnGenerateNSFData
            // 
            this.btnGenerateNSFData.Location = new System.Drawing.Point(320, 559);
            this.btnGenerateNSFData.Name = "btnGenerateNSFData";
            this.btnGenerateNSFData.Size = new System.Drawing.Size(274, 23);
            this.btnGenerateNSFData.TabIndex = 1;
            this.btnGenerateNSFData.Text = "Make NSF Data";
            this.btnGenerateNSFData.UseVisualStyleBackColor = true;
            this.btnGenerateNSFData.Click += new System.EventHandler(this.btnGenerateNSFData_Click);
            // 
            // lstNSFPlates
            // 
            this.lstNSFPlates.FormattingEnabled = true;
            this.lstNSFPlates.ItemHeight = 15;
            this.lstNSFPlates.Location = new System.Drawing.Point(320, 17);
            this.lstNSFPlates.Name = "lstNSFPlates";
            this.lstNSFPlates.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstNSFPlates.Size = new System.Drawing.Size(274, 424);
            this.lstNSFPlates.TabIndex = 0;
            // 
            // lstInstrumentStatus
            // 
            this.lstInstrumentStatus.FormattingEnabled = true;
            this.lstInstrumentStatus.ItemHeight = 15;
            this.lstInstrumentStatus.Location = new System.Drawing.Point(812, 53);
            this.lstInstrumentStatus.Name = "lstInstrumentStatus";
            this.lstInstrumentStatus.Size = new System.Drawing.Size(214, 94);
            this.lstInstrumentStatus.TabIndex = 3;
            // 
            // lblErrorLog
            // 
            this.lblErrorLog.AutoSize = true;
            this.lblErrorLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorLog.Location = new System.Drawing.Point(872, 229);
            this.lblErrorLog.Name = "lblErrorLog";
            this.lblErrorLog.Size = new System.Drawing.Size(84, 20);
            this.lblErrorLog.TabIndex = 2;
            this.lblErrorLog.Text = "Error Log";
            // 
            // btnViewAdvancedControls
            // 
            this.btnViewAdvancedControls.Location = new System.Drawing.Point(842, 669);
            this.btnViewAdvancedControls.Name = "btnViewAdvancedControls";
            this.btnViewAdvancedControls.Size = new System.Drawing.Size(168, 27);
            this.btnViewAdvancedControls.TabIndex = 0;
            this.btnViewAdvancedControls.Text = "Create Advanced Controls";
            this.btnViewAdvancedControls.UseVisualStyleBackColor = true;
            this.btnViewAdvancedControls.Click += new System.EventHandler(this.btnViewAdvancedControls_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(838, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Instrument Status";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.versionToolStripMenuItem,
            this.alarmStateToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1052, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadProtocolToolStripMenuItem,
            this.saveCurrentProtocolsToolStripMenuItem,
            this.loadPreviousSystemStateToolStripMenuItem,
            this.recoverLastProtcolInstructionToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadProtocolToolStripMenuItem
            // 
            this.loadProtocolToolStripMenuItem.Name = "loadProtocolToolStripMenuItem";
            this.loadProtocolToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.loadProtocolToolStripMenuItem.Text = "Load Protocol";
            this.loadProtocolToolStripMenuItem.Click += new System.EventHandler(this.loadProtocolToolStripMenuItem_Click);
            // 
            // saveCurrentProtocolsToolStripMenuItem
            // 
            this.saveCurrentProtocolsToolStripMenuItem.Name = "saveCurrentProtocolsToolStripMenuItem";
            this.saveCurrentProtocolsToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.saveCurrentProtocolsToolStripMenuItem.Text = "Save Current System State";
            this.saveCurrentProtocolsToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentProtocolsToolStripMenuItem_Click);
            // 
            // loadPreviousSystemStateToolStripMenuItem
            // 
            this.loadPreviousSystemStateToolStripMenuItem.Name = "loadPreviousSystemStateToolStripMenuItem";
            this.loadPreviousSystemStateToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.loadPreviousSystemStateToolStripMenuItem.Text = "Load Previous System State";
            this.loadPreviousSystemStateToolStripMenuItem.Click += new System.EventHandler(this.loadPreviousSystemStateToolStripMenuItem_Click);
            // 
            // recoverLastProtcolInstructionToolStripMenuItem
            // 
            this.recoverLastProtcolInstructionToolStripMenuItem.Name = "recoverLastProtcolInstructionToolStripMenuItem";
            this.recoverLastProtcolInstructionToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.recoverLastProtcolInstructionToolStripMenuItem.Text = "Recover From System Failure, Load Last System State";
            this.recoverLastProtcolInstructionToolStripMenuItem.Click += new System.EventHandler(this.recoverLastProtcolInstructionToolStripMenuItem_Click);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.versionToolStripMenuItem.Text = "Version";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // alarmStateToolStripMenuItem
            // 
            this.alarmStateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.turnOnAlarmToolStripMenuItem,
            this.turnOffAlarmToolStripMenuItem,
            this.getAlarmStateToolStripMenuItem,
            this.reconnectToAlarmServerToolStripMenuItem});
            this.alarmStateToolStripMenuItem.Name = "alarmStateToolStripMenuItem";
            this.alarmStateToolStripMenuItem.Size = new System.Drawing.Size(125, 20);
            this.alarmStateToolStripMenuItem.Text = "Alarm Management";
            // 
            // turnOnAlarmToolStripMenuItem
            // 
            this.turnOnAlarmToolStripMenuItem.Name = "turnOnAlarmToolStripMenuItem";
            this.turnOnAlarmToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.turnOnAlarmToolStripMenuItem.Text = "Turn On Alarm";
            this.turnOnAlarmToolStripMenuItem.Click += new System.EventHandler(this.turnOnAlarmToolStripMenuItem_Click);
            // 
            // turnOffAlarmToolStripMenuItem
            // 
            this.turnOffAlarmToolStripMenuItem.Name = "turnOffAlarmToolStripMenuItem";
            this.turnOffAlarmToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.turnOffAlarmToolStripMenuItem.Text = "Turn Off Alarm";
            this.turnOffAlarmToolStripMenuItem.Click += new System.EventHandler(this.turnOffAlarmToolStripMenuItem_Click);
            // 
            // getAlarmStateToolStripMenuItem
            // 
            this.getAlarmStateToolStripMenuItem.Name = "getAlarmStateToolStripMenuItem";
            this.getAlarmStateToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.getAlarmStateToolStripMenuItem.Text = "Get Alarm State";
            this.getAlarmStateToolStripMenuItem.Click += new System.EventHandler(this.getAlarmStateToolStripMenuItem_Click);
            // 
            // reconnectToAlarmServerToolStripMenuItem
            // 
            this.reconnectToAlarmServerToolStripMenuItem.Name = "reconnectToAlarmServerToolStripMenuItem";
            this.reconnectToAlarmServerToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.reconnectToAlarmServerToolStripMenuItem.Text = "Reconnect To Alarm Server";
            this.reconnectToAlarmServerToolStripMenuItem.Click += new System.EventHandler(this.reconnectToAlarmServerToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 705);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1052, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(115, 17);
            this.StatusLabel.Text = "System Status: None";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(842, 632);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(153, 21);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.Text = "Type Password Here";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtErrorLog
            // 
            this.txtErrorLog.Location = new System.Drawing.Point(816, 252);
            this.txtErrorLog.Name = "txtErrorLog";
            this.txtErrorLog.Size = new System.Drawing.Size(210, 369);
            this.txtErrorLog.TabIndex = 10;
            this.txtErrorLog.Text = "";
            // 
            // TimeToGo
            // 
            this.TimeToGo.AutoSize = true;
            this.TimeToGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeToGo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TimeToGo.Location = new System.Drawing.Point(812, 162);
            this.TimeToGo.Name = "TimeToGo";
            this.TimeToGo.Size = new System.Drawing.Size(153, 40);
            this.TimeToGo.TabIndex = 0;
            this.TimeToGo.Text = "No Processes \r\nCurrently Running";
            // 
            // ClarityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 727);
            this.Controls.Add(this.txtErrorLog);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.TimeToGo);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnViewAdvancedControls);
            this.Controls.Add(this.lblErrorLog);
            this.Controls.Add(this.lstInstrumentStatus);
            this.Controls.Add(this.MainTab);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ClarityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clarity - Template -  Robot Management Program";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.MainTab.ResumeLayout(false);
            this.tabSubMain.ResumeLayout(false);
            this.tabSubMain.PerformLayout();
            this.pnlFailure.ResumeLayout(false);
            this.pnlFailure.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabRecovery.ResumeLayout(false);
            this.tabRecovery.PerformLayout();
            this.IncubatorTab.ResumeLayout(false);
            this.IncubatorTab.PerformLayout();
            this.tabMakeGrowthInstructions.ResumeLayout(false);
            this.tabMakeGrowthInstructions.PerformLayout();
            this.tabNSFExperiment.ResumeLayout(false);
            this.tabNSFExperiment.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage tabSubMain;
        private System.Windows.Forms.TabPage IncubatorTab;
        private System.Windows.Forms.Button btnStopShaking;
        private System.Windows.Forms.Button btnStartShaking;
        private System.Windows.Forms.Button btnLoadPlate;
        private System.Windows.Forms.Button btnUnloadPlate;
        private System.Windows.Forms.Button btnPerformCommand;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbShakeSpeed;
        private System.Windows.Forms.Button btnChangeShakingSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstIncubatorSlots;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnResetIncubator;
        private System.Windows.Forms.Button btnReinitializeIncubator;
        private System.Windows.Forms.Button btnViewAdvancedControls;
        private System.Windows.Forms.Label lblErrorLog;
        private System.Windows.Forms.ListBox lstInstrumentStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadProtocolToolStripMenuItem;

        private CountdownTimer TimeToGo;
        private System.Windows.Forms.Button btnMakeProtocols;
        private System.Windows.Forms.ListBox lstSelectedProtocol;
        private System.Windows.Forms.ListBox lstLoadedProtocols;
        private System.Windows.Forms.ListBox lstCurrentProtocol;
        private System.Windows.Forms.Button btnExecuteProtocols;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Button btnCancelProtocolExecution;
        private System.Windows.Forms.Label lblCurrentRunningProtocol;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabRecovery;
        private System.Windows.Forms.Button btnRetryLastInstruction;
        private System.Windows.Forms.WebBrowser wBrowRecovInstructions;
        private System.Windows.Forms.Panel pnlFailure;
        private System.Windows.Forms.Label lblFailure;
        private System.Windows.Forms.Label lblFailureInstructionName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TabPage tabMakeGrowthInstructions;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox lstGrowthRatesProtocol;
        private System.Windows.Forms.WebBrowser wBrowGrowthRate;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtGrowthRateExperimentName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtGrowthRateEmail;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnStartGrowthRate;
        private System.Windows.Forms.TextBox txtGrowthRateTimesToMeasure;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RichTextBox txtErrorLog;
        private System.Windows.Forms.TextBox txtGrowthRateMinutes;
        private System.Windows.Forms.Button btnStopIncubatorShaking;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnEmailOkay;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentProtocolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPreviousSystemStateToolStripMenuItem;
        private System.Windows.Forms.Button btnChangeCurrentProtPosition;
        private System.Windows.Forms.ToolStripMenuItem recoverLastProtcolInstructionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alarmStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOnAlarmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffAlarmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getAlarmStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reconnectToAlarmServerToolStripMenuItem;
        private System.Windows.Forms.CheckBox chk48WellPlate;
        private System.Windows.Forms.Button btnDeleteProtocol;
        private System.Windows.Forms.CheckBox chkGBO;
        private System.Windows.Forms.Button btnInstrumentRefresh;
        private System.Windows.Forms.TextBox textbox_number;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabNSFExperiment;
        private System.Windows.Forms.ListBox lstNSFPlates;
        private System.Windows.Forms.Button btnGenerateNSFData;
        private System.Windows.Forms.TextBox txtNSFName;
        private System.Windows.Forms.TextBox txtNSFTransferNumber;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;

    }
}

