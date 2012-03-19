namespace Fit_Growth_Curves
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.importPreviousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileWithInitialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryWithRobotDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryWithExcelDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPlateKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAllMaxODValuesBadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAllGrowthRateValuesBadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabBlankRemoval = new System.Windows.Forms.TabPage();
            this.lblDeletePlate = new System.Windows.Forms.Label();
            this.btnDeleteAvg3asBlank = new System.Windows.Forms.Button();
            this.btnDelete2ndPoint = new System.Windows.Forms.Button();
            this.btnDeleteFirstBlank = new System.Windows.Forms.Button();
            this.btnAvgtimePointsDeleteNigel = new System.Windows.Forms.Button();
            this.btnAvgTimeSeries = new System.Windows.Forms.Button();
            this.btnAvgAllRemoveNigelBlanks = new System.Windows.Forms.Button();
            this.btnAvgFirstRemoveNigelBlanks = new System.Windows.Forms.Button();
            this.btnRemoveNigelBlanks = new System.Windows.Forms.Button();
            this.lblBlankInfo = new System.Windows.Forms.Label();
            this.btnAvgAllAndDeleteTony = new System.Windows.Forms.Button();
            this.btnAvgDataPoint1AndDeleteBlanks = new System.Windows.Forms.Button();
            this.btnRemoveTonyCheckerboard = new System.Windows.Forms.Button();
            this.txtDeleteBlanksTab = new System.Windows.Forms.RichTextBox();
            this.toDeletePlateMap = new Fit_Growth_Curves.SelectablePlateMap();
            this.tabRobo = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.lstTimePoints = new System.Windows.Forms.ListBox();
            this.btnTimePoint = new System.Windows.Forms.RadioButton();
            this.rbtnGrRate = new System.Windows.Forms.RadioButton();
            this.rbtnIOD = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.rbtnRS = new System.Windows.Forms.RadioButton();
            this.rbtnGrowthRate = new System.Windows.Forms.RadioButton();
            this.rbtnMaxOD = new System.Windows.Forms.RadioButton();
            this.lblRobo = new System.Windows.Forms.Label();
            this.btnSubtractWrittenBlank = new System.Windows.Forms.Button();
            this.txtBlankValue = new System.Windows.Forms.TextBox();
            this.tabMultPlot = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkLegend = new System.Windows.Forms.CheckBox();
            this.chkMuliLog = new System.Windows.Forms.CheckBox();
            this.chkFillColors = new System.Windows.Forms.CheckBox();
            this.MultiplePlots = new ZedGraph.ZedGraphControl();
            this.lstMultiplePlots = new System.Windows.Forms.ListBox();
            this.tabEnterData = new System.Windows.Forms.TabPage();
            this.txtEnterDataInstructions = new System.Windows.Forms.RichTextBox();
            this.txtDataName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnInsertTime = new System.Windows.Forms.Button();
            this.DataView = new System.Windows.Forms.DataGridView();
            this.PickSampleTime = new System.Windows.Forms.DateTimePicker();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tblRawData = new System.Windows.Forms.DataGridView();
            this.tabPageChangeFitted = new System.Windows.Forms.TabPage();
            this.chkShowFittedPick = new System.Windows.Forms.CheckBox();
            this.ChartPickData = new ZedGraph.ZedGraphControl();
            this.label4 = new System.Windows.Forms.Label();
            this.lstGrowthCurvesMirror = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstDataMirror = new System.Windows.Forms.ListBox();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.bottomrightpanel = new System.Windows.Forms.Panel();
            this.splitContainerbottom = new System.Windows.Forms.SplitContainer();
            this.ChartSlopeN = new ZedGraph.ZedGraphControl();
            this.ChartStandard = new ZedGraph.ZedGraphControl();
            this.topRightPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label7 = new System.Windows.Forms.Label();
            this.ChartN = new ZedGraph.ZedGraphControl();
            this.l = new System.Windows.Forms.Panel();
            this.btnFlagGrowthRate = new System.Windows.Forms.Button();
            this.btnFlagOD = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEndPoint = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSetBounds = new System.Windows.Forms.Button();
            this.txtStartPoint = new System.Windows.Forms.TextBox();
            this.lstData = new System.Windows.Forms.ListBox();
            this.chkShowLin = new System.Windows.Forms.CheckBox();
            this.btnDeleteCurve = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.leftsubbottompanel = new System.Windows.Forms.Panel();
            this.chkODMustIncrease = new System.Windows.Forms.CheckBox();
            this.chkUsePercent = new System.Windows.Forms.CheckBox();
            this.txtMaxODPercent = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnFitODRange = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMaxOD = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMinOD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChangeAxis = new System.Windows.Forms.Button();
            this.txtMaxRange = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMinRange = new System.Windows.Forms.TextBox();
            this.lstGrowthCurves = new System.Windows.Forms.ListBox();
            this.tabMainTab = new System.Windows.Forms.TabControl();
            this.tabPlotGraphic = new System.Windows.Forms.TabPage();
            this.btnMakeEvoGroups = new System.Windows.Forms.Button();
            this.rbtnTimeToOD = new System.Windows.Forms.RadioButton();
            this.chkTreatShowLog = new System.Windows.Forms.CheckBox();
            this.chkTreatLegend = new System.Windows.Forms.CheckBox();
            this.btnClearTreatments = new System.Windows.Forms.Button();
            this.rbtnTreatNumPoints = new System.Windows.Forms.RadioButton();
            this.rbtnTreatTimevOD = new System.Windows.Forms.RadioButton();
            this.rbtnTreatDoublingTime = new System.Windows.Forms.RadioButton();
            this.rbtnTreatInitialOD = new System.Windows.Forms.RadioButton();
            this.label21 = new System.Windows.Forms.Label();
            this.rbtnTreatRSq = new System.Windows.Forms.RadioButton();
            this.rbtnTreatGrowthRate = new System.Windows.Forms.RadioButton();
            this.rbtnTreatMaxOd = new System.Windows.Forms.RadioButton();
            this.label20 = new System.Windows.Forms.Label();
            this.txtTreatment6 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.lstTreatmentSelection = new System.Windows.Forms.ListBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtTreatment5 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTreatment4 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtTreatment3 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTreatment2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTreatment1 = new System.Windows.Forms.TextBox();
            this.plotTreatments = new ZedGraph.ZedGraphControl();
            this.selectablePlateMap1 = new Fit_Growth_Curves.SelectablePlateMap();
            this.venusDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadExcelFileWithODAndVenusDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabBlankRemoval.SuspendLayout();
            this.tabRobo.SuspendLayout();
            this.tabMultPlot.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabEnterData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblRawData)).BeginInit();
            this.tabPageChangeFitted.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.bottomrightpanel.SuspendLayout();
            this.splitContainerbottom.Panel1.SuspendLayout();
            this.splitContainerbottom.Panel2.SuspendLayout();
            this.splitContainerbottom.SuspendLayout();
            this.topRightPanel.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.l.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.leftsubbottompanel.SuspendLayout();
            this.tabMainTab.SuspendLayout();
            this.tabPlotGraphic.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.venusDataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1173, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpenFile,
            this.exportDataToolStripMenuItem1,
            this.importPreviousToolStripMenuItem,
            this.openFileWithInitialToolStripMenuItem,
            this.openDirectoryWithRobotDataToolStripMenuItem,
            this.openDirectoryWithExcelDataToolStripMenuItem,
            this.importPlateKeyToolStripMenuItem,
            this.saveDataToDatabaseToolStripMenuItem,
            this.markAllMaxODValuesBadToolStripMenuItem,
            this.markAllGrowthRateValuesBadToolStripMenuItem});
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.openFileToolStripMenuItem.Text = "File";
            // 
            // MenuOpenFile
            // 
            this.MenuOpenFile.Name = "MenuOpenFile";
            this.MenuOpenFile.Size = new System.Drawing.Size(246, 22);
            this.MenuOpenFile.Text = "Open File";
            this.MenuOpenFile.Click += new System.EventHandler(this.MenuOpenFile_Click);
            // 
            // exportDataToolStripMenuItem1
            // 
            this.exportDataToolStripMenuItem1.Name = "exportDataToolStripMenuItem1";
            this.exportDataToolStripMenuItem1.Size = new System.Drawing.Size(246, 22);
            this.exportDataToolStripMenuItem1.Text = "Export Data";
            this.exportDataToolStripMenuItem1.Click += new System.EventHandler(this.exportDataToolStripMenuItem1_Click);
            // 
            // importPreviousToolStripMenuItem
            // 
            this.importPreviousToolStripMenuItem.Name = "importPreviousToolStripMenuItem";
            this.importPreviousToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.importPreviousToolStripMenuItem.Text = "Import Previous";
            this.importPreviousToolStripMenuItem.Click += new System.EventHandler(this.importPreviousToolStripMenuItem_Click);
            // 
            // openFileWithInitialToolStripMenuItem
            // 
            this.openFileWithInitialToolStripMenuItem.Name = "openFileWithInitialToolStripMenuItem";
            this.openFileWithInitialToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.openFileWithInitialToolStripMenuItem.Text = "Open File with Initial OD";
            this.openFileWithInitialToolStripMenuItem.Click += new System.EventHandler(this.openFileWithInitialToolStripMenuItem_Click);
            // 
            // openDirectoryWithRobotDataToolStripMenuItem
            // 
            this.openDirectoryWithRobotDataToolStripMenuItem.Name = "openDirectoryWithRobotDataToolStripMenuItem";
            this.openDirectoryWithRobotDataToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.openDirectoryWithRobotDataToolStripMenuItem.Text = "Open Directory with Robot Data";
            this.openDirectoryWithRobotDataToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryWithRobotDataToolStripMenuItem_Click);
            // 
            // openDirectoryWithExcelDataToolStripMenuItem
            // 
            this.openDirectoryWithExcelDataToolStripMenuItem.Name = "openDirectoryWithExcelDataToolStripMenuItem";
            this.openDirectoryWithExcelDataToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.openDirectoryWithExcelDataToolStripMenuItem.Text = "Open Directory with Excel Data";
            this.openDirectoryWithExcelDataToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryWithExcelDataToolStripMenuItem_Click);
            // 
            // importPlateKeyToolStripMenuItem
            // 
            this.importPlateKeyToolStripMenuItem.Name = "importPlateKeyToolStripMenuItem";
            this.importPlateKeyToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.importPlateKeyToolStripMenuItem.Text = "Import Plate Key";
            this.importPlateKeyToolStripMenuItem.Click += new System.EventHandler(this.importPlateKeyToolStripMenuItem_Click);
            // 
            // saveDataToDatabaseToolStripMenuItem
            // 
            this.saveDataToDatabaseToolStripMenuItem.Enabled = false;
            this.saveDataToDatabaseToolStripMenuItem.Name = "saveDataToDatabaseToolStripMenuItem";
            this.saveDataToDatabaseToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.saveDataToDatabaseToolStripMenuItem.Text = "Save Data To Database";
            this.saveDataToDatabaseToolStripMenuItem.Click += new System.EventHandler(this.saveDataToDatabaseToolStripMenuItem_Click);
            // 
            // markAllMaxODValuesBadToolStripMenuItem
            // 
            this.markAllMaxODValuesBadToolStripMenuItem.Enabled = false;
            this.markAllMaxODValuesBadToolStripMenuItem.Name = "markAllMaxODValuesBadToolStripMenuItem";
            this.markAllMaxODValuesBadToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.markAllMaxODValuesBadToolStripMenuItem.Text = "Mark All Max OD Values Bad";
            this.markAllMaxODValuesBadToolStripMenuItem.Click += new System.EventHandler(this.markAllMaxODValuesBadToolStripMenuItem_Click);
            // 
            // markAllGrowthRateValuesBadToolStripMenuItem
            // 
            this.markAllGrowthRateValuesBadToolStripMenuItem.Enabled = false;
            this.markAllGrowthRateValuesBadToolStripMenuItem.Name = "markAllGrowthRateValuesBadToolStripMenuItem";
            this.markAllGrowthRateValuesBadToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.markAllGrowthRateValuesBadToolStripMenuItem.Text = "Mark All Growth Rate Values Bad";
            this.markAllGrowthRateValuesBadToolStripMenuItem.Click += new System.EventHandler(this.markAllGrowthRateValuesBadToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "\".txt\",\".csv\",\".gcd\"";
            // 
            // tabBlankRemoval
            // 
            this.tabBlankRemoval.Controls.Add(this.lblDeletePlate);
            this.tabBlankRemoval.Controls.Add(this.btnDeleteAvg3asBlank);
            this.tabBlankRemoval.Controls.Add(this.btnDelete2ndPoint);
            this.tabBlankRemoval.Controls.Add(this.btnDeleteFirstBlank);
            this.tabBlankRemoval.Controls.Add(this.btnAvgtimePointsDeleteNigel);
            this.tabBlankRemoval.Controls.Add(this.btnAvgTimeSeries);
            this.tabBlankRemoval.Controls.Add(this.btnAvgAllRemoveNigelBlanks);
            this.tabBlankRemoval.Controls.Add(this.btnAvgFirstRemoveNigelBlanks);
            this.tabBlankRemoval.Controls.Add(this.btnRemoveNigelBlanks);
            this.tabBlankRemoval.Controls.Add(this.lblBlankInfo);
            this.tabBlankRemoval.Controls.Add(this.btnAvgAllAndDeleteTony);
            this.tabBlankRemoval.Controls.Add(this.btnAvgDataPoint1AndDeleteBlanks);
            this.tabBlankRemoval.Controls.Add(this.btnRemoveTonyCheckerboard);
            this.tabBlankRemoval.Controls.Add(this.txtDeleteBlanksTab);
            this.tabBlankRemoval.Controls.Add(this.toDeletePlateMap);
            this.tabBlankRemoval.Location = new System.Drawing.Point(4, 22);
            this.tabBlankRemoval.Name = "tabBlankRemoval";
            this.tabBlankRemoval.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlankRemoval.Size = new System.Drawing.Size(1165, 664);
            this.tabBlankRemoval.TabIndex = 5;
            this.tabBlankRemoval.Text = "Remove Blanks From Plate Data";
            this.tabBlankRemoval.UseVisualStyleBackColor = true;
            this.tabBlankRemoval.Paint += new System.Windows.Forms.PaintEventHandler(this.tabBlankRemoval_Paint);
            // 
            // lblDeletePlate
            // 
            this.lblDeletePlate.AutoSize = true;
            this.lblDeletePlate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeletePlate.Location = new System.Drawing.Point(572, 239);
            this.lblDeletePlate.Name = "lblDeletePlate";
            this.lblDeletePlate.Size = new System.Drawing.Size(184, 24);
            this.lblDeletePlate.TabIndex = 14;
            this.lblDeletePlate.Text = "Click Well to Delete It";
            // 
            // btnDeleteAvg3asBlank
            // 
            this.btnDeleteAvg3asBlank.Location = new System.Drawing.Point(716, 591);
            this.btnDeleteAvg3asBlank.Name = "btnDeleteAvg3asBlank";
            this.btnDeleteAvg3asBlank.Size = new System.Drawing.Size(298, 37);
            this.btnDeleteAvg3asBlank.TabIndex = 12;
            this.btnDeleteAvg3asBlank.Text = "Delete Average of First Three Points as Blank";
            this.btnDeleteAvg3asBlank.UseVisualStyleBackColor = true;
            this.btnDeleteAvg3asBlank.Click += new System.EventHandler(this.btnDeleteAvg3asBlank_Click);
            // 
            // btnDelete2ndPoint
            // 
            this.btnDelete2ndPoint.Location = new System.Drawing.Point(392, 591);
            this.btnDelete2ndPoint.Name = "btnDelete2ndPoint";
            this.btnDelete2ndPoint.Size = new System.Drawing.Size(298, 37);
            this.btnDelete2ndPoint.TabIndex = 11;
            this.btnDelete2ndPoint.Text = "Delete Second Data Point in Each Well As Blank";
            this.btnDelete2ndPoint.UseVisualStyleBackColor = true;
            this.btnDelete2ndPoint.Click += new System.EventHandler(this.btnDelete2ndPoint_Click);
            // 
            // btnDeleteFirstBlank
            // 
            this.btnDeleteFirstBlank.Location = new System.Drawing.Point(42, 591);
            this.btnDeleteFirstBlank.Name = "btnDeleteFirstBlank";
            this.btnDeleteFirstBlank.Size = new System.Drawing.Size(298, 37);
            this.btnDeleteFirstBlank.TabIndex = 10;
            this.btnDeleteFirstBlank.Text = "Delete First Data Point in Each Well As Blank";
            this.btnDeleteFirstBlank.UseVisualStyleBackColor = true;
            this.btnDeleteFirstBlank.Click += new System.EventHandler(this.btnDeleteFirstBlank_Click);
            // 
            // btnAvgtimePointsDeleteNigel
            // 
            this.btnAvgtimePointsDeleteNigel.Location = new System.Drawing.Point(282, 467);
            this.btnAvgtimePointsDeleteNigel.Name = "btnAvgtimePointsDeleteNigel";
            this.btnAvgtimePointsDeleteNigel.Size = new System.Drawing.Size(209, 80);
            this.btnAvgtimePointsDeleteNigel.TabIndex = 9;
            this.btnAvgtimePointsDeleteNigel.Text = "Average all of the readings  from all of the blank wells at each time point, and " +
    "subtract that from all of the other reads at that time point.  Then delete the b" +
    "lanks based on this layout";
            this.btnAvgtimePointsDeleteNigel.UseVisualStyleBackColor = true;
            this.btnAvgtimePointsDeleteNigel.Click += new System.EventHandler(this.btnAvgtimePointsDeleteNigel_Click);
            // 
            // btnAvgTimeSeries
            // 
            this.btnAvgTimeSeries.Location = new System.Drawing.Point(16, 467);
            this.btnAvgTimeSeries.Name = "btnAvgTimeSeries";
            this.btnAvgTimeSeries.Size = new System.Drawing.Size(209, 80);
            this.btnAvgTimeSeries.TabIndex = 8;
            this.btnAvgTimeSeries.Text = "Average all of the readings  from all of the blank wells at each time point, and " +
    "subtract that from all of the other reads at that time point.  Then delete the b" +
    "lanks based on this layout";
            this.btnAvgTimeSeries.UseVisualStyleBackColor = true;
            this.btnAvgTimeSeries.Click += new System.EventHandler(this.btnAvgTimeSeries_Click);
            // 
            // btnAvgAllRemoveNigelBlanks
            // 
            this.btnAvgAllRemoveNigelBlanks.Location = new System.Drawing.Point(282, 370);
            this.btnAvgAllRemoveNigelBlanks.Name = "btnAvgAllRemoveNigelBlanks";
            this.btnAvgAllRemoveNigelBlanks.Size = new System.Drawing.Size(209, 80);
            this.btnAvgAllRemoveNigelBlanks.TabIndex = 7;
            this.btnAvgAllRemoveNigelBlanks.Text = "Average all of the readings from all of the blank wells, subtract that from all o" +
    "f the other reads, and then delete the blanks based on this layout";
            this.btnAvgAllRemoveNigelBlanks.UseVisualStyleBackColor = true;
            this.btnAvgAllRemoveNigelBlanks.Click += new System.EventHandler(this.btnAvgAllRemoveNigelBlanks_Click);
            // 
            // btnAvgFirstRemoveNigelBlanks
            // 
            this.btnAvgFirstRemoveNigelBlanks.Location = new System.Drawing.Point(282, 275);
            this.btnAvgFirstRemoveNigelBlanks.Name = "btnAvgFirstRemoveNigelBlanks";
            this.btnAvgFirstRemoveNigelBlanks.Size = new System.Drawing.Size(209, 80);
            this.btnAvgFirstRemoveNigelBlanks.TabIndex = 6;
            this.btnAvgFirstRemoveNigelBlanks.Text = "Average the first data point from all of the blanks, subtract that from all of th" +
    "e other reads, and then delete the blanks based on this layout";
            this.btnAvgFirstRemoveNigelBlanks.UseVisualStyleBackColor = true;
            this.btnAvgFirstRemoveNigelBlanks.Click += new System.EventHandler(this.btnAvgFirstRemoveNigelBlanks_Click);
            // 
            // btnRemoveNigelBlanks
            // 
            this.btnRemoveNigelBlanks.Location = new System.Drawing.Point(282, 237);
            this.btnRemoveNigelBlanks.Name = "btnRemoveNigelBlanks";
            this.btnRemoveNigelBlanks.Size = new System.Drawing.Size(209, 23);
            this.btnRemoveNigelBlanks.TabIndex = 5;
            this.btnRemoveNigelBlanks.Text = "Delete blanks based on this layout";
            this.btnRemoveNigelBlanks.UseVisualStyleBackColor = true;
            this.btnRemoveNigelBlanks.Click += new System.EventHandler(this.btnRemoveNigelBlanks_Click);
            // 
            // lblBlankInfo
            // 
            this.lblBlankInfo.AutoSize = true;
            this.lblBlankInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlankInfo.Location = new System.Drawing.Point(802, 457);
            this.lblBlankInfo.Name = "lblBlankInfo";
            this.lblBlankInfo.Size = new System.Drawing.Size(135, 20);
            this.lblBlankInfo.TabIndex = 4;
            this.lblBlankInfo.Text = "Blank Used Was: ";
            // 
            // btnAvgAllAndDeleteTony
            // 
            this.btnAvgAllAndDeleteTony.Location = new System.Drawing.Point(16, 370);
            this.btnAvgAllAndDeleteTony.Name = "btnAvgAllAndDeleteTony";
            this.btnAvgAllAndDeleteTony.Size = new System.Drawing.Size(209, 80);
            this.btnAvgAllAndDeleteTony.TabIndex = 3;
            this.btnAvgAllAndDeleteTony.Text = "Average all of the readings from all of the blank wells, subtract that from all o" +
    "f the other reads, and then delete the blanks based on this layout";
            this.btnAvgAllAndDeleteTony.UseVisualStyleBackColor = true;
            this.btnAvgAllAndDeleteTony.Click += new System.EventHandler(this.btnAvgAllAndDeleteTony_Click);
            // 
            // btnAvgDataPoint1AndDeleteBlanks
            // 
            this.btnAvgDataPoint1AndDeleteBlanks.Location = new System.Drawing.Point(16, 275);
            this.btnAvgDataPoint1AndDeleteBlanks.Name = "btnAvgDataPoint1AndDeleteBlanks";
            this.btnAvgDataPoint1AndDeleteBlanks.Size = new System.Drawing.Size(209, 80);
            this.btnAvgDataPoint1AndDeleteBlanks.TabIndex = 2;
            this.btnAvgDataPoint1AndDeleteBlanks.Text = "Average the first data point from all of the blanks, subtract that from all of th" +
    "e other reads, and then delete the blanks based on this layout";
            this.btnAvgDataPoint1AndDeleteBlanks.UseVisualStyleBackColor = true;
            this.btnAvgDataPoint1AndDeleteBlanks.Click += new System.EventHandler(this.btnAvgDataPoint1AndDeleteBlanks_Click);
            // 
            // btnRemoveTonyCheckerboard
            // 
            this.btnRemoveTonyCheckerboard.Location = new System.Drawing.Point(16, 237);
            this.btnRemoveTonyCheckerboard.Name = "btnRemoveTonyCheckerboard";
            this.btnRemoveTonyCheckerboard.Size = new System.Drawing.Size(209, 23);
            this.btnRemoveTonyCheckerboard.TabIndex = 1;
            this.btnRemoveTonyCheckerboard.Text = "Delete blanks based on this layout";
            this.btnRemoveTonyCheckerboard.UseVisualStyleBackColor = true;
            this.btnRemoveTonyCheckerboard.Click += new System.EventHandler(this.btnRemoveTonyCheckerboard_Click);
            // 
            // txtDeleteBlanksTab
            // 
            this.txtDeleteBlanksTab.Location = new System.Drawing.Point(806, 20);
            this.txtDeleteBlanksTab.Name = "txtDeleteBlanksTab";
            this.txtDeleteBlanksTab.Size = new System.Drawing.Size(310, 408);
            this.txtDeleteBlanksTab.TabIndex = 0;
            this.txtDeleteBlanksTab.Text = "";
            // 
            // toDeletePlateMap
            // 
            this.toDeletePlateMap.CurGroupToSelect = 1;
            this.toDeletePlateMap.Location = new System.Drawing.Point(552, 30);
            this.toDeletePlateMap.Name = "toDeletePlateMap";
            this.toDeletePlateMap.Size = new System.Drawing.Size(239, 189);
            this.toDeletePlateMap.TabIndex = 13;
            // 
            // tabRobo
            // 
            this.tabRobo.Controls.Add(this.label12);
            this.tabRobo.Controls.Add(this.lstTimePoints);
            this.tabRobo.Controls.Add(this.btnTimePoint);
            this.tabRobo.Controls.Add(this.rbtnGrRate);
            this.tabRobo.Controls.Add(this.rbtnIOD);
            this.tabRobo.Controls.Add(this.label9);
            this.tabRobo.Controls.Add(this.rbtnRS);
            this.tabRobo.Controls.Add(this.rbtnGrowthRate);
            this.tabRobo.Controls.Add(this.rbtnMaxOD);
            this.tabRobo.Controls.Add(this.lblRobo);
            this.tabRobo.Controls.Add(this.btnSubtractWrittenBlank);
            this.tabRobo.Controls.Add(this.txtBlankValue);
            this.tabRobo.Location = new System.Drawing.Point(4, 22);
            this.tabRobo.Name = "tabRobo";
            this.tabRobo.Size = new System.Drawing.Size(1165, 664);
            this.tabRobo.TabIndex = 0;
            this.tabRobo.Text = "View 48 Well Plate Data";
            this.tabRobo.UseVisualStyleBackColor = true;
            this.tabRobo.Paint += new System.Windows.Forms.PaintEventHandler(this.tabRobo_Paint);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1009, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Sample Times";
            // 
            // lstTimePoints
            // 
            this.lstTimePoints.FormattingEnabled = true;
            this.lstTimePoints.Location = new System.Drawing.Point(954, 88);
            this.lstTimePoints.Name = "lstTimePoints";
            this.lstTimePoints.Size = new System.Drawing.Size(199, 381);
            this.lstTimePoints.TabIndex = 12;
            this.lstTimePoints.SelectedIndexChanged += new System.EventHandler(this.lstTimePoints_SelectedIndexChanged);
            // 
            // btnTimePoint
            // 
            this.btnTimePoint.AutoSize = true;
            this.btnTimePoint.Location = new System.Drawing.Point(793, 378);
            this.btnTimePoint.Name = "btnTimePoint";
            this.btnTimePoint.Size = new System.Drawing.Size(114, 17);
            this.btnTimePoint.TabIndex = 11;
            this.btnTimePoint.Text = "Plot Selected Time";
            this.btnTimePoint.UseVisualStyleBackColor = true;
            this.btnTimePoint.CheckedChanged += new System.EventHandler(this.btnTimePoint_CheckedChanged);
            // 
            // rbtnGrRate
            // 
            this.rbtnGrRate.AutoSize = true;
            this.rbtnGrRate.Location = new System.Drawing.Point(792, 341);
            this.rbtnGrRate.Name = "rbtnGrRate";
            this.rbtnGrRate.Size = new System.Drawing.Size(114, 17);
            this.rbtnGrRate.TabIndex = 10;
            this.rbtnGrRate.Text = "Plot Doubling Time";
            this.rbtnGrRate.UseVisualStyleBackColor = true;
            this.rbtnGrRate.CheckedChanged += new System.EventHandler(this.rbtnGrRate_CheckedChanged);
            // 
            // rbtnIOD
            // 
            this.rbtnIOD.AutoSize = true;
            this.rbtnIOD.Location = new System.Drawing.Point(792, 309);
            this.rbtnIOD.Name = "rbtnIOD";
            this.rbtnIOD.Size = new System.Drawing.Size(87, 17);
            this.rbtnIOD.TabIndex = 9;
            this.rbtnIOD.Text = "Plot Intial OD";
            this.rbtnIOD.UseVisualStyleBackColor = true;
            this.rbtnIOD.CheckedChanged += new System.EventHandler(this.rbtnIOD_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(789, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(159, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Select Data To Show";
            // 
            // rbtnRS
            // 
            this.rbtnRS.AutoSize = true;
            this.rbtnRS.Location = new System.Drawing.Point(792, 273);
            this.rbtnRS.Name = "rbtnRS";
            this.rbtnRS.Size = new System.Drawing.Size(95, 17);
            this.rbtnRS.TabIndex = 7;
            this.rbtnRS.Text = "Plot R squared";
            this.rbtnRS.UseVisualStyleBackColor = true;
            this.rbtnRS.CheckedChanged += new System.EventHandler(this.rbtnRS_CheckedChanged);
            // 
            // rbtnGrowthRate
            // 
            this.rbtnGrowthRate.AutoSize = true;
            this.rbtnGrowthRate.Checked = true;
            this.rbtnGrowthRate.Location = new System.Drawing.Point(792, 237);
            this.rbtnGrowthRate.Name = "rbtnGrowthRate";
            this.rbtnGrowthRate.Size = new System.Drawing.Size(106, 17);
            this.rbtnGrowthRate.TabIndex = 6;
            this.rbtnGrowthRate.TabStop = true;
            this.rbtnGrowthRate.Text = "Plot Growth Rate";
            this.rbtnGrowthRate.UseVisualStyleBackColor = true;
            this.rbtnGrowthRate.CheckedChanged += new System.EventHandler(this.rbtnGrowthRate_CheckedChanged);
            // 
            // rbtnMaxOD
            // 
            this.rbtnMaxOD.AutoSize = true;
            this.rbtnMaxOD.Location = new System.Drawing.Point(792, 201);
            this.rbtnMaxOD.Name = "rbtnMaxOD";
            this.rbtnMaxOD.Size = new System.Drawing.Size(109, 17);
            this.rbtnMaxOD.TabIndex = 5;
            this.rbtnMaxOD.Text = "Plot Maximum OD";
            this.rbtnMaxOD.UseVisualStyleBackColor = true;
            this.rbtnMaxOD.CheckedChanged += new System.EventHandler(this.rbtnMaxOD_CheckedChanged);
            // 
            // lblRobo
            // 
            this.lblRobo.AutoSize = true;
            this.lblRobo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRobo.Location = new System.Drawing.Point(184, 612);
            this.lblRobo.Name = "lblRobo";
            this.lblRobo.Size = new System.Drawing.Size(0, 24);
            this.lblRobo.TabIndex = 4;
            // 
            // btnSubtractWrittenBlank
            // 
            this.btnSubtractWrittenBlank.Location = new System.Drawing.Point(764, 420);
            this.btnSubtractWrittenBlank.Name = "btnSubtractWrittenBlank";
            this.btnSubtractWrittenBlank.Size = new System.Drawing.Size(167, 23);
            this.btnSubtractWrittenBlank.TabIndex = 2;
            this.btnSubtractWrittenBlank.Text = "Subtract Value Below As Blank";
            this.btnSubtractWrittenBlank.UseVisualStyleBackColor = true;
            this.btnSubtractWrittenBlank.Click += new System.EventHandler(this.btnSubtractWrittenBlank_Click);
            // 
            // txtBlankValue
            // 
            this.txtBlankValue.Location = new System.Drawing.Point(793, 449);
            this.txtBlankValue.Name = "txtBlankValue";
            this.txtBlankValue.Size = new System.Drawing.Size(100, 20);
            this.txtBlankValue.TabIndex = 1;
            this.txtBlankValue.Text = ".034";
            // 
            // tabMultPlot
            // 
            this.tabMultPlot.Controls.Add(this.panel3);
            this.tabMultPlot.Controls.Add(this.MultiplePlots);
            this.tabMultPlot.Controls.Add(this.lstMultiplePlots);
            this.tabMultPlot.Location = new System.Drawing.Point(4, 22);
            this.tabMultPlot.Name = "tabMultPlot";
            this.tabMultPlot.Size = new System.Drawing.Size(1165, 664);
            this.tabMultPlot.TabIndex = 4;
            this.tabMultPlot.Text = "Plot Multiple Curves";
            this.tabMultPlot.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkLegend);
            this.panel3.Controls.Add(this.chkMuliLog);
            this.panel3.Controls.Add(this.chkFillColors);
            this.panel3.Location = new System.Drawing.Point(280, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(311, 36);
            this.panel3.TabIndex = 4;
            // 
            // chkLegend
            // 
            this.chkLegend.AutoSize = true;
            this.chkLegend.Checked = true;
            this.chkLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLegend.Location = new System.Drawing.Point(218, 11);
            this.chkLegend.Name = "chkLegend";
            this.chkLegend.Size = new System.Drawing.Size(92, 17);
            this.chkLegend.TabIndex = 4;
            this.chkLegend.Text = "Show Legend";
            this.chkLegend.UseVisualStyleBackColor = true;
            this.chkLegend.CheckedChanged += new System.EventHandler(this.chkLegend_CheckedChanged);
            // 
            // chkMuliLog
            // 
            this.chkMuliLog.AutoSize = true;
            this.chkMuliLog.Location = new System.Drawing.Point(5, 12);
            this.chkMuliLog.Name = "chkMuliLog";
            this.chkMuliLog.Size = new System.Drawing.Size(109, 17);
            this.chkMuliLog.TabIndex = 2;
            this.chkMuliLog.Text = "Show Log Values";
            this.chkMuliLog.UseVisualStyleBackColor = true;
            this.chkMuliLog.CheckedChanged += new System.EventHandler(this.chkMuliLog_CheckedChanged);
            // 
            // chkFillColors
            // 
            this.chkFillColors.AutoSize = true;
            this.chkFillColors.Location = new System.Drawing.Point(120, 12);
            this.chkFillColors.Name = "chkFillColors";
            this.chkFillColors.Size = new System.Drawing.Size(92, 17);
            this.chkFillColors.TabIndex = 3;
            this.chkFillColors.Text = "Use Fill Colors";
            this.chkFillColors.UseVisualStyleBackColor = true;
            this.chkFillColors.CheckedChanged += new System.EventHandler(this.chkFillColors_CheckedChanged);
            // 
            // MultiplePlots
            // 
            this.MultiplePlots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MultiplePlots.Location = new System.Drawing.Point(260, 0);
            this.MultiplePlots.Name = "MultiplePlots";
            this.MultiplePlots.ScrollGrace = 0D;
            this.MultiplePlots.ScrollMaxX = 0D;
            this.MultiplePlots.ScrollMaxY = 0D;
            this.MultiplePlots.ScrollMaxY2 = 0D;
            this.MultiplePlots.ScrollMinX = 0D;
            this.MultiplePlots.ScrollMinY = 0D;
            this.MultiplePlots.ScrollMinY2 = 0D;
            this.MultiplePlots.Size = new System.Drawing.Size(905, 664);
            this.MultiplePlots.TabIndex = 1;
            // 
            // lstMultiplePlots
            // 
            this.lstMultiplePlots.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstMultiplePlots.FormattingEnabled = true;
            this.lstMultiplePlots.Location = new System.Drawing.Point(0, 0);
            this.lstMultiplePlots.Name = "lstMultiplePlots";
            this.lstMultiplePlots.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMultiplePlots.Size = new System.Drawing.Size(260, 664);
            this.lstMultiplePlots.TabIndex = 0;
            this.lstMultiplePlots.SelectedIndexChanged += new System.EventHandler(this.lstMultiplePlots_SelectedIndexChanged);
            // 
            // tabEnterData
            // 
            this.tabEnterData.Controls.Add(this.txtEnterDataInstructions);
            this.tabEnterData.Controls.Add(this.txtDataName);
            this.tabEnterData.Controls.Add(this.label5);
            this.tabEnterData.Controls.Add(this.btnEnter);
            this.tabEnterData.Controls.Add(this.btnClear);
            this.tabEnterData.Controls.Add(this.btnInsertTime);
            this.tabEnterData.Controls.Add(this.DataView);
            this.tabEnterData.Controls.Add(this.PickSampleTime);
            this.tabEnterData.Location = new System.Drawing.Point(4, 22);
            this.tabEnterData.Name = "tabEnterData";
            this.tabEnterData.Size = new System.Drawing.Size(1165, 664);
            this.tabEnterData.TabIndex = 3;
            this.tabEnterData.Text = "Enter Data";
            this.tabEnterData.UseVisualStyleBackColor = true;
            // 
            // txtEnterDataInstructions
            // 
            this.txtEnterDataInstructions.Location = new System.Drawing.Point(713, 30);
            this.txtEnterDataInstructions.Name = "txtEnterDataInstructions";
            this.txtEnterDataInstructions.ReadOnly = true;
            this.txtEnterDataInstructions.Size = new System.Drawing.Size(400, 421);
            this.txtEnterDataInstructions.TabIndex = 9;
            this.txtEnterDataInstructions.Text = "";
            // 
            // txtDataName
            // 
            this.txtDataName.BackColor = System.Drawing.SystemColors.Info;
            this.txtDataName.Location = new System.Drawing.Point(19, 181);
            this.txtDataName.MaxLength = 256;
            this.txtDataName.Name = "txtDataName";
            this.txtDataName.Size = new System.Drawing.Size(197, 20);
            this.txtDataName.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Name Your Data Below";
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(3, 525);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(229, 23);
            this.btnEnter.TabIndex = 6;
            this.btnEnter.Text = "Add and Fit Data";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(511, 525);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(200, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear Table";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnInsertTime
            // 
            this.btnInsertTime.Location = new System.Drawing.Point(16, 81);
            this.btnInsertTime.Name = "btnInsertTime";
            this.btnInsertTime.Size = new System.Drawing.Size(200, 23);
            this.btnInsertTime.TabIndex = 4;
            this.btnInsertTime.Text = "Create New Data Point at this Time";
            this.btnInsertTime.UseVisualStyleBackColor = true;
            this.btnInsertTime.Click += new System.EventHandler(this.btnInsertTime_Click);
            // 
            // DataView
            // 
            this.DataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataView.DefaultCellStyle = dataGridViewCellStyle2;
            this.DataView.Location = new System.Drawing.Point(260, 12);
            this.DataView.Name = "DataView";
            this.DataView.Size = new System.Drawing.Size(353, 481);
            this.DataView.TabIndex = 3;
            this.DataView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataView_DataError);
            // 
            // PickSampleTime
            // 
            this.PickSampleTime.CustomFormat = "MM/dd/yyyy hh:mm tt";
            this.PickSampleTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PickSampleTime.Location = new System.Drawing.Point(16, 41);
            this.PickSampleTime.MaxDate = new System.DateTime(2209, 12, 27, 0, 0, 0, 0);
            this.PickSampleTime.MinDate = new System.DateTime(2007, 6, 16, 0, 0, 0, 0);
            this.PickSampleTime.Name = "PickSampleTime";
            this.PickSampleTime.Size = new System.Drawing.Size(200, 20);
            this.PickSampleTime.TabIndex = 2;
            this.PickSampleTime.Value = new System.DateTime(2007, 6, 21, 0, 0, 0, 0);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tblRawData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1165, 664);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "View Raw Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tblRawData
            // 
            this.tblRawData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tblRawData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tblRawData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tblRawData.DefaultCellStyle = dataGridViewCellStyle4;
            this.tblRawData.Location = new System.Drawing.Point(27, 40);
            this.tblRawData.Name = "tblRawData";
            this.tblRawData.ReadOnly = true;
            this.tblRawData.Size = new System.Drawing.Size(865, 580);
            this.tblRawData.TabIndex = 0;
            // 
            // tabPageChangeFitted
            // 
            this.tabPageChangeFitted.Controls.Add(this.chkShowFittedPick);
            this.tabPageChangeFitted.Controls.Add(this.ChartPickData);
            this.tabPageChangeFitted.Controls.Add(this.label4);
            this.tabPageChangeFitted.Controls.Add(this.lstGrowthCurvesMirror);
            this.tabPageChangeFitted.Controls.Add(this.label3);
            this.tabPageChangeFitted.Controls.Add(this.lstDataMirror);
            this.tabPageChangeFitted.Location = new System.Drawing.Point(4, 22);
            this.tabPageChangeFitted.Name = "tabPageChangeFitted";
            this.tabPageChangeFitted.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChangeFitted.Size = new System.Drawing.Size(1165, 664);
            this.tabPageChangeFitted.TabIndex = 1;
            this.tabPageChangeFitted.Text = "Change Data To Fit";
            this.tabPageChangeFitted.UseVisualStyleBackColor = true;
            // 
            // chkShowFittedPick
            // 
            this.chkShowFittedPick.AutoSize = true;
            this.chkShowFittedPick.Checked = true;
            this.chkShowFittedPick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowFittedPick.Location = new System.Drawing.Point(15, 608);
            this.chkShowFittedPick.Name = "chkShowFittedPick";
            this.chkShowFittedPick.Size = new System.Drawing.Size(105, 17);
            this.chkShowFittedPick.TabIndex = 8;
            this.chkShowFittedPick.Text = "Show Fitted Line";
            this.chkShowFittedPick.UseVisualStyleBackColor = true;
            this.chkShowFittedPick.CheckedChanged += new System.EventHandler(this.chkShowFittedPick_CheckedChanged);
            // 
            // ChartPickData
            // 
            this.ChartPickData.Location = new System.Drawing.Point(351, 73);
            this.ChartPickData.Name = "ChartPickData";
            this.ChartPickData.ScrollGrace = 0D;
            this.ChartPickData.ScrollMaxX = 0D;
            this.ChartPickData.ScrollMaxY = 0D;
            this.ChartPickData.ScrollMaxY2 = 0D;
            this.ChartPickData.ScrollMinX = 0D;
            this.ChartPickData.ScrollMinY = 0D;
            this.ChartPickData.ScrollMinY2 = 0D;
            this.ChartPickData.Size = new System.Drawing.Size(756, 528);
            this.ChartPickData.TabIndex = 7;
            this.ChartPickData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChartPickData_MouseClick_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(491, 604);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(269, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Two Points Must Always Be Selected";
            // 
            // lstGrowthCurvesMirror
            // 
            this.lstGrowthCurvesMirror.FormattingEnabled = true;
            this.lstGrowthCurvesMirror.Location = new System.Drawing.Point(15, 239);
            this.lstGrowthCurvesMirror.Name = "lstGrowthCurvesMirror";
            this.lstGrowthCurvesMirror.Size = new System.Drawing.Size(282, 355);
            this.lstGrowthCurvesMirror.TabIndex = 5;
            this.lstGrowthCurvesMirror.SelectedIndexChanged += new System.EventHandler(this.lstGrowthCurvesMirror_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(543, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(331, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Click on a point to add or remove it from the fit";
            // 
            // lstDataMirror
            // 
            this.lstDataMirror.FormattingEnabled = true;
            this.lstDataMirror.Location = new System.Drawing.Point(6, 44);
            this.lstDataMirror.Name = "lstDataMirror";
            this.lstDataMirror.Size = new System.Drawing.Size(282, 173);
            this.lstDataMirror.TabIndex = 3;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.bottomrightpanel);
            this.tabPageMain.Controls.Add(this.topRightPanel);
            this.tabPageMain.Controls.Add(this.panel1);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(1165, 664);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "View and Fit Data";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // bottomrightpanel
            // 
            this.bottomrightpanel.Controls.Add(this.splitContainerbottom);
            this.bottomrightpanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomrightpanel.Location = new System.Drawing.Point(238, 329);
            this.bottomrightpanel.Name = "bottomrightpanel";
            this.bottomrightpanel.Size = new System.Drawing.Size(924, 332);
            this.bottomrightpanel.TabIndex = 16;
            // 
            // splitContainerbottom
            // 
            this.splitContainerbottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerbottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerbottom.Name = "splitContainerbottom";
            // 
            // splitContainerbottom.Panel1
            // 
            this.splitContainerbottom.Panel1.Controls.Add(this.ChartSlopeN);
            // 
            // splitContainerbottom.Panel2
            // 
            this.splitContainerbottom.Panel2.Controls.Add(this.ChartStandard);
            this.splitContainerbottom.Size = new System.Drawing.Size(924, 332);
            this.splitContainerbottom.SplitterDistance = 508;
            this.splitContainerbottom.TabIndex = 5;
            // 
            // ChartSlopeN
            // 
            this.ChartSlopeN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartSlopeN.Location = new System.Drawing.Point(0, 0);
            this.ChartSlopeN.Name = "ChartSlopeN";
            this.ChartSlopeN.ScrollGrace = 0D;
            this.ChartSlopeN.ScrollMaxX = 0D;
            this.ChartSlopeN.ScrollMaxY = 0D;
            this.ChartSlopeN.ScrollMaxY2 = 0D;
            this.ChartSlopeN.ScrollMinX = 0D;
            this.ChartSlopeN.ScrollMinY = 0D;
            this.ChartSlopeN.ScrollMinY2 = 0D;
            this.ChartSlopeN.Size = new System.Drawing.Size(508, 332);
            this.ChartSlopeN.TabIndex = 12;
            // 
            // ChartStandard
            // 
            this.ChartStandard.Location = new System.Drawing.Point(2, 3);
            this.ChartStandard.Name = "ChartStandard";
            this.ChartStandard.ScrollGrace = 0D;
            this.ChartStandard.ScrollMaxX = 0D;
            this.ChartStandard.ScrollMaxY = 0D;
            this.ChartStandard.ScrollMaxY2 = 0D;
            this.ChartStandard.ScrollMinX = 0D;
            this.ChartStandard.ScrollMinY = 0D;
            this.ChartStandard.ScrollMinY2 = 0D;
            this.ChartStandard.Size = new System.Drawing.Size(410, 329);
            this.ChartStandard.TabIndex = 0;
            // 
            // topRightPanel
            // 
            this.topRightPanel.Controls.Add(this.splitContainer1);
            this.topRightPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topRightPanel.Location = new System.Drawing.Point(238, 3);
            this.topRightPanel.Name = "topRightPanel";
            this.topRightPanel.Size = new System.Drawing.Size(924, 320);
            this.topRightPanel.TabIndex = 15;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.ChartN);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.l);
            this.splitContainer1.Size = new System.Drawing.Size(924, 320);
            this.splitContainer1.SplitterDistance = 632;
            this.splitContainer1.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(739, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "End Point For Data";
            // 
            // ChartN
            // 
            this.ChartN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartN.Location = new System.Drawing.Point(0, 0);
            this.ChartN.Name = "ChartN";
            this.ChartN.ScrollGrace = 0D;
            this.ChartN.ScrollMaxX = 0D;
            this.ChartN.ScrollMaxY = 0D;
            this.ChartN.ScrollMaxY2 = 0D;
            this.ChartN.ScrollMinX = 0D;
            this.ChartN.ScrollMinY = 0D;
            this.ChartN.ScrollMinY2 = 0D;
            this.ChartN.Size = new System.Drawing.Size(632, 320);
            this.ChartN.TabIndex = 13;
            this.ChartN.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChartN_MouseClick);
            // 
            // l
            // 
            this.l.Controls.Add(this.btnFlagGrowthRate);
            this.l.Controls.Add(this.btnFlagOD);
            this.l.Controls.Add(this.panel2);
            this.l.Controls.Add(this.lstData);
            this.l.Controls.Add(this.chkShowLin);
            this.l.Controls.Add(this.btnDeleteCurve);
            this.l.Dock = System.Windows.Forms.DockStyle.Fill;
            this.l.Location = new System.Drawing.Point(0, 0);
            this.l.Name = "l";
            this.l.Size = new System.Drawing.Size(288, 320);
            this.l.TabIndex = 14;
            // 
            // btnFlagGrowthRate
            // 
            this.btnFlagGrowthRate.Enabled = false;
            this.btnFlagGrowthRate.Location = new System.Drawing.Point(175, 188);
            this.btnFlagGrowthRate.Name = "btnFlagGrowthRate";
            this.btnFlagGrowthRate.Size = new System.Drawing.Size(113, 23);
            this.btnFlagGrowthRate.TabIndex = 19;
            this.btnFlagGrowthRate.Text = "Toggle GR Flag";
            this.btnFlagGrowthRate.UseVisualStyleBackColor = true;
            this.btnFlagGrowthRate.Click += new System.EventHandler(this.btnFlagGrowthRate_Click);
            // 
            // btnFlagOD
            // 
            this.btnFlagOD.Enabled = false;
            this.btnFlagOD.Location = new System.Drawing.Point(175, 159);
            this.btnFlagOD.Name = "btnFlagOD";
            this.btnFlagOD.Size = new System.Drawing.Size(113, 23);
            this.btnFlagOD.TabIndex = 18;
            this.btnFlagOD.Text = "Toggle OD Flag";
            this.btnFlagOD.UseVisualStyleBackColor = true;
            this.btnFlagOD.Click += new System.EventHandler(this.btnFlagOD_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.txtEndPoint);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.btnSetBounds);
            this.panel2.Controls.Add(this.txtStartPoint);
            this.panel2.Location = new System.Drawing.Point(2, 220);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 97);
            this.panel2.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "End Point";
            // 
            // txtEndPoint
            // 
            this.txtEndPoint.Location = new System.Drawing.Point(132, 38);
            this.txtEndPoint.Name = "txtEndPoint";
            this.txtEndPoint.Size = new System.Drawing.Size(66, 20);
            this.txtEndPoint.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Start Point";
            // 
            // btnSetBounds
            // 
            this.btnSetBounds.Location = new System.Drawing.Point(14, 71);
            this.btnSetBounds.Name = "btnSetBounds";
            this.btnSetBounds.Size = new System.Drawing.Size(232, 23);
            this.btnSetBounds.TabIndex = 12;
            this.btnSetBounds.Text = "Fit All Data In Range";
            this.btnSetBounds.UseVisualStyleBackColor = true;
            this.btnSetBounds.Click += new System.EventHandler(this.btnTemp_Click);
            // 
            // txtStartPoint
            // 
            this.txtStartPoint.Location = new System.Drawing.Point(14, 38);
            this.txtStartPoint.Name = "txtStartPoint";
            this.txtStartPoint.Size = new System.Drawing.Size(66, 20);
            this.txtStartPoint.TabIndex = 13;
            // 
            // lstData
            // 
            this.lstData.BackColor = System.Drawing.SystemColors.Control;
            this.lstData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstData.FormattingEnabled = true;
            this.lstData.Location = new System.Drawing.Point(15, 10);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(221, 143);
            this.lstData.TabIndex = 3;
            // 
            // chkShowLin
            // 
            this.chkShowLin.AutoSize = true;
            this.chkShowLin.Checked = true;
            this.chkShowLin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLin.Location = new System.Drawing.Point(15, 197);
            this.chkShowLin.Name = "chkShowLin";
            this.chkShowLin.Size = new System.Drawing.Size(162, 17);
            this.chkShowLin.TabIndex = 11;
            this.chkShowLin.Text = "Show Linear Fit Below Curve";
            this.chkShowLin.UseVisualStyleBackColor = true;
            this.chkShowLin.CheckedChanged += new System.EventHandler(this.chkShowLin_CheckedChanged);
            // 
            // btnDeleteCurve
            // 
            this.btnDeleteCurve.Location = new System.Drawing.Point(15, 168);
            this.btnDeleteCurve.Name = "btnDeleteCurve";
            this.btnDeleteCurve.Size = new System.Drawing.Size(138, 23);
            this.btnDeleteCurve.TabIndex = 10;
            this.btnDeleteCurve.Text = "Delete This Curve";
            this.btnDeleteCurve.UseVisualStyleBackColor = true;
            this.btnDeleteCurve.Click += new System.EventHandler(this.btnDeleteCurve_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.leftsubbottompanel);
            this.panel1.Controls.Add(this.lstGrowthCurves);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 658);
            this.panel1.TabIndex = 14;
            // 
            // leftsubbottompanel
            // 
            this.leftsubbottompanel.Controls.Add(this.chkODMustIncrease);
            this.leftsubbottompanel.Controls.Add(this.chkUsePercent);
            this.leftsubbottompanel.Controls.Add(this.txtMaxODPercent);
            this.leftsubbottompanel.Controls.Add(this.label13);
            this.leftsubbottompanel.Controls.Add(this.btnFitODRange);
            this.leftsubbottompanel.Controls.Add(this.label10);
            this.leftsubbottompanel.Controls.Add(this.txtMaxOD);
            this.leftsubbottompanel.Controls.Add(this.label11);
            this.leftsubbottompanel.Controls.Add(this.txtMinOD);
            this.leftsubbottompanel.Controls.Add(this.label1);
            this.leftsubbottompanel.Controls.Add(this.btnChangeAxis);
            this.leftsubbottompanel.Controls.Add(this.txtMaxRange);
            this.leftsubbottompanel.Controls.Add(this.label2);
            this.leftsubbottompanel.Controls.Add(this.txtMinRange);
            this.leftsubbottompanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.leftsubbottompanel.Location = new System.Drawing.Point(0, 472);
            this.leftsubbottompanel.Name = "leftsubbottompanel";
            this.leftsubbottompanel.Size = new System.Drawing.Size(235, 186);
            this.leftsubbottompanel.TabIndex = 10;
            // 
            // chkODMustIncrease
            // 
            this.chkODMustIncrease.AutoSize = true;
            this.chkODMustIncrease.Location = new System.Drawing.Point(49, 154);
            this.chkODMustIncrease.Name = "chkODMustIncrease";
            this.chkODMustIncrease.Size = new System.Drawing.Size(167, 17);
            this.chkODMustIncrease.TabIndex = 18;
            this.chkODMustIncrease.Text = "Enforce OD Always Increases";
            this.chkODMustIncrease.UseVisualStyleBackColor = true;
            // 
            // chkUsePercent
            // 
            this.chkUsePercent.AutoSize = true;
            this.chkUsePercent.Location = new System.Drawing.Point(49, 131);
            this.chkUsePercent.Name = "chkUsePercent";
            this.chkUsePercent.Size = new System.Drawing.Size(115, 17);
            this.chkUsePercent.TabIndex = 17;
            this.chkUsePercent.Text = "Use Percent Value";
            this.chkUsePercent.UseVisualStyleBackColor = true;
            // 
            // txtMaxODPercent
            // 
            this.txtMaxODPercent.Location = new System.Drawing.Point(164, 76);
            this.txtMaxODPercent.Name = "txtMaxODPercent";
            this.txtMaxODPercent.Size = new System.Drawing.Size(45, 20);
            this.txtMaxODPercent.TabIndex = 15;
            this.txtMaxODPercent.Text = ".9";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(159, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Max OD %";
            // 
            // btnFitODRange
            // 
            this.btnFitODRange.Location = new System.Drawing.Point(28, 102);
            this.btnFitODRange.Name = "btnFitODRange";
            this.btnFitODRange.Size = new System.Drawing.Size(146, 23);
            this.btnFitODRange.TabIndex = 14;
            this.btnFitODRange.Text = "Fit Values in Range";
            this.btnFitODRange.UseVisualStyleBackColor = true;
            this.btnFitODRange.Click += new System.EventHandler(this.btnFitODRange_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Min OD Value";
            // 
            // txtMaxOD
            // 
            this.txtMaxOD.Location = new System.Drawing.Point(89, 76);
            this.txtMaxOD.Name = "txtMaxOD";
            this.txtMaxOD.Size = new System.Drawing.Size(45, 20);
            this.txtMaxOD.TabIndex = 11;
            this.txtMaxOD.Text = ".2";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(84, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Max OD ";
            // 
            // txtMinOD
            // 
            this.txtMinOD.Location = new System.Drawing.Point(8, 76);
            this.txtMinOD.Name = "txtMinOD";
            this.txtMinOD.Size = new System.Drawing.Size(45, 20);
            this.txtMinOD.TabIndex = 10;
            this.txtMinOD.Text = "0.02";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Min Value";
            // 
            // btnChangeAxis
            // 
            this.btnChangeAxis.Location = new System.Drawing.Point(28, 28);
            this.btnChangeAxis.Name = "btnChangeAxis";
            this.btnChangeAxis.Size = new System.Drawing.Size(146, 23);
            this.btnChangeAxis.TabIndex = 9;
            this.btnChangeAxis.Text = "Change Doubling Y Axis";
            this.btnChangeAxis.UseVisualStyleBackColor = true;
            this.btnChangeAxis.Click += new System.EventHandler(this.btnChangeAxis_Click);
            // 
            // txtMaxRange
            // 
            this.txtMaxRange.Location = new System.Drawing.Point(180, 3);
            this.txtMaxRange.Name = "txtMaxRange";
            this.txtMaxRange.Size = new System.Drawing.Size(45, 20);
            this.txtMaxRange.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Max Value";
            // 
            // txtMinRange
            // 
            this.txtMinRange.Location = new System.Drawing.Point(65, 2);
            this.txtMinRange.Name = "txtMinRange";
            this.txtMinRange.Size = new System.Drawing.Size(45, 20);
            this.txtMinRange.TabIndex = 5;
            // 
            // lstGrowthCurves
            // 
            this.lstGrowthCurves.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstGrowthCurves.FormattingEnabled = true;
            this.lstGrowthCurves.Location = new System.Drawing.Point(0, 0);
            this.lstGrowthCurves.Name = "lstGrowthCurves";
            this.lstGrowthCurves.Size = new System.Drawing.Size(235, 472);
            this.lstGrowthCurves.TabIndex = 0;
            this.lstGrowthCurves.SelectedIndexChanged += new System.EventHandler(this.lstGrowthCurves_SelectedIndexChanged);
            this.lstGrowthCurves.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstGrowthCurves_KeyDown);
            // 
            // tabMainTab
            // 
            this.tabMainTab.Controls.Add(this.tabPageMain);
            this.tabMainTab.Controls.Add(this.tabPageChangeFitted);
            this.tabMainTab.Controls.Add(this.tabPage1);
            this.tabMainTab.Controls.Add(this.tabEnterData);
            this.tabMainTab.Controls.Add(this.tabMultPlot);
            this.tabMainTab.Controls.Add(this.tabRobo);
            this.tabMainTab.Controls.Add(this.tabBlankRemoval);
            this.tabMainTab.Controls.Add(this.tabPlotGraphic);
            this.tabMainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMainTab.Location = new System.Drawing.Point(0, 24);
            this.tabMainTab.Name = "tabMainTab";
            this.tabMainTab.SelectedIndex = 0;
            this.tabMainTab.Size = new System.Drawing.Size(1173, 690);
            this.tabMainTab.TabIndex = 9;
            // 
            // tabPlotGraphic
            // 
            this.tabPlotGraphic.Controls.Add(this.btnMakeEvoGroups);
            this.tabPlotGraphic.Controls.Add(this.rbtnTimeToOD);
            this.tabPlotGraphic.Controls.Add(this.chkTreatShowLog);
            this.tabPlotGraphic.Controls.Add(this.chkTreatLegend);
            this.tabPlotGraphic.Controls.Add(this.btnClearTreatments);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatNumPoints);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatTimevOD);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatDoublingTime);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatInitialOD);
            this.tabPlotGraphic.Controls.Add(this.label21);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatRSq);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatGrowthRate);
            this.tabPlotGraphic.Controls.Add(this.rbtnTreatMaxOd);
            this.tabPlotGraphic.Controls.Add(this.label20);
            this.tabPlotGraphic.Controls.Add(this.txtTreatment6);
            this.tabPlotGraphic.Controls.Add(this.label19);
            this.tabPlotGraphic.Controls.Add(this.lstTreatmentSelection);
            this.tabPlotGraphic.Controls.Add(this.label18);
            this.tabPlotGraphic.Controls.Add(this.txtTreatment5);
            this.tabPlotGraphic.Controls.Add(this.label17);
            this.tabPlotGraphic.Controls.Add(this.txtTreatment4);
            this.tabPlotGraphic.Controls.Add(this.label16);
            this.tabPlotGraphic.Controls.Add(this.txtTreatment3);
            this.tabPlotGraphic.Controls.Add(this.label15);
            this.tabPlotGraphic.Controls.Add(this.txtTreatment2);
            this.tabPlotGraphic.Controls.Add(this.label14);
            this.tabPlotGraphic.Controls.Add(this.txtTreatment1);
            this.tabPlotGraphic.Controls.Add(this.plotTreatments);
            this.tabPlotGraphic.Controls.Add(this.selectablePlateMap1);
            this.tabPlotGraphic.Location = new System.Drawing.Point(4, 22);
            this.tabPlotGraphic.Name = "tabPlotGraphic";
            this.tabPlotGraphic.Size = new System.Drawing.Size(1165, 664);
            this.tabPlotGraphic.TabIndex = 6;
            this.tabPlotGraphic.Text = "Plot Different Groups";
            this.tabPlotGraphic.UseVisualStyleBackColor = true;
            // 
            // btnMakeEvoGroups
            // 
            this.btnMakeEvoGroups.Location = new System.Drawing.Point(251, 37);
            this.btnMakeEvoGroups.Name = "btnMakeEvoGroups";
            this.btnMakeEvoGroups.Size = new System.Drawing.Size(111, 45);
            this.btnMakeEvoGroups.TabIndex = 29;
            this.btnMakeEvoGroups.Text = "Make Evolution Groups";
            this.btnMakeEvoGroups.UseVisualStyleBackColor = true;
            this.btnMakeEvoGroups.Click += new System.EventHandler(this.btnMakeEvoGroups_Click);
            // 
            // rbtnTimeToOD
            // 
            this.rbtnTimeToOD.AutoSize = true;
            this.rbtnTimeToOD.Location = new System.Drawing.Point(206, 509);
            this.rbtnTimeToOD.Name = "rbtnTimeToOD";
            this.rbtnTimeToOD.Size = new System.Drawing.Size(109, 17);
            this.rbtnTimeToOD.TabIndex = 28;
            this.rbtnTimeToOD.Text = "Time Until OD .02";
            this.rbtnTimeToOD.UseVisualStyleBackColor = true;
            this.rbtnTimeToOD.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // chkTreatShowLog
            // 
            this.chkTreatShowLog.AutoSize = true;
            this.chkTreatShowLog.Checked = true;
            this.chkTreatShowLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTreatShowLog.Location = new System.Drawing.Point(270, 174);
            this.chkTreatShowLog.Name = "chkTreatShowLog";
            this.chkTreatShowLog.Size = new System.Drawing.Size(109, 17);
            this.chkTreatShowLog.TabIndex = 2;
            this.chkTreatShowLog.Text = "Show Log Values";
            this.chkTreatShowLog.UseVisualStyleBackColor = true;
            this.chkTreatShowLog.CheckedChanged += new System.EventHandler(this.chkTreatShowLog_CheckedChanged);
            // 
            // chkTreatLegend
            // 
            this.chkTreatLegend.AutoSize = true;
            this.chkTreatLegend.Checked = true;
            this.chkTreatLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTreatLegend.Location = new System.Drawing.Point(270, 206);
            this.chkTreatLegend.Name = "chkTreatLegend";
            this.chkTreatLegend.Size = new System.Drawing.Size(92, 17);
            this.chkTreatLegend.TabIndex = 4;
            this.chkTreatLegend.Text = "Show Legend";
            this.chkTreatLegend.UseVisualStyleBackColor = true;
            this.chkTreatLegend.CheckedChanged += new System.EventHandler(this.chkTreatShowLog_CheckedChanged);
            // 
            // btnClearTreatments
            // 
            this.btnClearTreatments.Location = new System.Drawing.Point(251, 108);
            this.btnClearTreatments.Name = "btnClearTreatments";
            this.btnClearTreatments.Size = new System.Drawing.Size(111, 45);
            this.btnClearTreatments.TabIndex = 27;
            this.btnClearTreatments.Text = "Clear All Group Assignments";
            this.btnClearTreatments.UseVisualStyleBackColor = true;
            this.btnClearTreatments.Click += new System.EventHandler(this.btnClearTreatments_Click);
            // 
            // rbtnTreatNumPoints
            // 
            this.rbtnTreatNumPoints.AutoSize = true;
            this.rbtnTreatNumPoints.Location = new System.Drawing.Point(206, 480);
            this.rbtnTreatNumPoints.Name = "rbtnTreatNumPoints";
            this.rbtnTreatNumPoints.Size = new System.Drawing.Size(141, 17);
            this.rbtnTreatNumPoints.TabIndex = 26;
            this.rbtnTreatNumPoints.Text = "Plot Number of Points Fit";
            this.rbtnTreatNumPoints.UseVisualStyleBackColor = true;
            this.rbtnTreatNumPoints.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // rbtnTreatTimevOD
            // 
            this.rbtnTreatTimevOD.AutoSize = true;
            this.rbtnTreatTimevOD.Checked = true;
            this.rbtnTreatTimevOD.Location = new System.Drawing.Point(206, 448);
            this.rbtnTreatTimevOD.Name = "rbtnTreatTimevOD";
            this.rbtnTreatTimevOD.Size = new System.Drawing.Size(102, 17);
            this.rbtnTreatTimevOD.TabIndex = 25;
            this.rbtnTreatTimevOD.TabStop = true;
            this.rbtnTreatTimevOD.Text = "Plot Time vs OD";
            this.rbtnTreatTimevOD.UseVisualStyleBackColor = true;
            this.rbtnTreatTimevOD.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // rbtnTreatDoublingTime
            // 
            this.rbtnTreatDoublingTime.AutoSize = true;
            this.rbtnTreatDoublingTime.Location = new System.Drawing.Point(206, 414);
            this.rbtnTreatDoublingTime.Name = "rbtnTreatDoublingTime";
            this.rbtnTreatDoublingTime.Size = new System.Drawing.Size(114, 17);
            this.rbtnTreatDoublingTime.TabIndex = 24;
            this.rbtnTreatDoublingTime.Text = "Plot Doubling Time";
            this.rbtnTreatDoublingTime.UseVisualStyleBackColor = true;
            // 
            // rbtnTreatInitialOD
            // 
            this.rbtnTreatInitialOD.AutoSize = true;
            this.rbtnTreatInitialOD.Location = new System.Drawing.Point(206, 380);
            this.rbtnTreatInitialOD.Name = "rbtnTreatInitialOD";
            this.rbtnTreatInitialOD.Size = new System.Drawing.Size(87, 17);
            this.rbtnTreatInitialOD.TabIndex = 23;
            this.rbtnTreatInitialOD.Text = "Plot Intial OD";
            this.rbtnTreatInitialOD.UseVisualStyleBackColor = true;
            this.rbtnTreatInitialOD.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(203, 245);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(159, 20);
            this.label21.TabIndex = 22;
            this.label21.Text = "Select Data To Show";
            // 
            // rbtnTreatRSq
            // 
            this.rbtnTreatRSq.AutoSize = true;
            this.rbtnTreatRSq.Location = new System.Drawing.Point(206, 346);
            this.rbtnTreatRSq.Name = "rbtnTreatRSq";
            this.rbtnTreatRSq.Size = new System.Drawing.Size(95, 17);
            this.rbtnTreatRSq.TabIndex = 21;
            this.rbtnTreatRSq.Text = "Plot R squared";
            this.rbtnTreatRSq.UseVisualStyleBackColor = true;
            this.rbtnTreatRSq.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // rbtnTreatGrowthRate
            // 
            this.rbtnTreatGrowthRate.AutoSize = true;
            this.rbtnTreatGrowthRate.Location = new System.Drawing.Point(206, 310);
            this.rbtnTreatGrowthRate.Name = "rbtnTreatGrowthRate";
            this.rbtnTreatGrowthRate.Size = new System.Drawing.Size(106, 17);
            this.rbtnTreatGrowthRate.TabIndex = 20;
            this.rbtnTreatGrowthRate.Text = "Plot Growth Rate";
            this.rbtnTreatGrowthRate.UseVisualStyleBackColor = true;
            this.rbtnTreatGrowthRate.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // rbtnTreatMaxOd
            // 
            this.rbtnTreatMaxOd.AutoSize = true;
            this.rbtnTreatMaxOd.Location = new System.Drawing.Point(206, 274);
            this.rbtnTreatMaxOd.Name = "rbtnTreatMaxOd";
            this.rbtnTreatMaxOd.Size = new System.Drawing.Size(109, 17);
            this.rbtnTreatMaxOd.TabIndex = 19;
            this.rbtnTreatMaxOd.Text = "Plot Maximum OD";
            this.rbtnTreatMaxOd.UseVisualStyleBackColor = true;
            this.rbtnTreatMaxOd.CheckedChanged += new System.EventHandler(this.chkTreat_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(11, 620);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(95, 13);
            this.label20.TabIndex = 18;
            this.label20.Text = "Name Treatment 6";
            // 
            // txtTreatment6
            // 
            this.txtTreatment6.Location = new System.Drawing.Point(11, 639);
            this.txtTreatment6.Name = "txtTreatment6";
            this.txtTreatment6.Size = new System.Drawing.Size(100, 20);
            this.txtTreatment6.TabIndex = 17;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(11, 194);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(132, 26);
            this.label19.TabIndex = 15;
            this.label19.Text = "Select Treatment Group to\r\nAssign by Clicking";
            // 
            // lstTreatmentSelection
            // 
            this.lstTreatmentSelection.FormattingEnabled = true;
            this.lstTreatmentSelection.Items.AddRange(new object[] {
            "Unassigned",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.lstTreatmentSelection.Location = new System.Drawing.Point(11, 223);
            this.lstTreatmentSelection.Name = "lstTreatmentSelection";
            this.lstTreatmentSelection.Size = new System.Drawing.Size(120, 173);
            this.lstTreatmentSelection.TabIndex = 14;
            this.lstTreatmentSelection.SelectedIndexChanged += new System.EventHandler(this.lstTreatmentSelection_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(11, 575);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(95, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Name Treatment 5";
            // 
            // txtTreatment5
            // 
            this.txtTreatment5.Location = new System.Drawing.Point(11, 594);
            this.txtTreatment5.Name = "txtTreatment5";
            this.txtTreatment5.Size = new System.Drawing.Size(100, 20);
            this.txtTreatment5.TabIndex = 12;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 529);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(95, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "Name Treatment 4";
            // 
            // txtTreatment4
            // 
            this.txtTreatment4.Location = new System.Drawing.Point(11, 548);
            this.txtTreatment4.Name = "txtTreatment4";
            this.txtTreatment4.Size = new System.Drawing.Size(100, 20);
            this.txtTreatment4.TabIndex = 10;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 485);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(95, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Name Treatment 3";
            // 
            // txtTreatment3
            // 
            this.txtTreatment3.Location = new System.Drawing.Point(11, 504);
            this.txtTreatment3.Name = "txtTreatment3";
            this.txtTreatment3.Size = new System.Drawing.Size(100, 20);
            this.txtTreatment3.TabIndex = 8;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 441);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(95, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Name Treatment 2";
            // 
            // txtTreatment2
            // 
            this.txtTreatment2.Location = new System.Drawing.Point(11, 460);
            this.txtTreatment2.Name = "txtTreatment2";
            this.txtTreatment2.Size = new System.Drawing.Size(100, 20);
            this.txtTreatment2.TabIndex = 6;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 399);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Name Treatment 1";
            // 
            // txtTreatment1
            // 
            this.txtTreatment1.Location = new System.Drawing.Point(11, 418);
            this.txtTreatment1.Name = "txtTreatment1";
            this.txtTreatment1.Size = new System.Drawing.Size(100, 20);
            this.txtTreatment1.TabIndex = 4;
            // 
            // plotTreatments
            // 
            this.plotTreatments.Location = new System.Drawing.Point(388, 3);
            this.plotTreatments.Name = "plotTreatments";
            this.plotTreatments.ScrollGrace = 0D;
            this.plotTreatments.ScrollMaxX = 0D;
            this.plotTreatments.ScrollMaxY = 0D;
            this.plotTreatments.ScrollMaxY2 = 0D;
            this.plotTreatments.ScrollMinX = 0D;
            this.plotTreatments.ScrollMinY = 0D;
            this.plotTreatments.ScrollMinY2 = 0D;
            this.plotTreatments.Size = new System.Drawing.Size(754, 658);
            this.plotTreatments.TabIndex = 2;
            // 
            // selectablePlateMap1
            // 
            this.selectablePlateMap1.CurGroupToSelect = 1;
            this.selectablePlateMap1.Location = new System.Drawing.Point(8, 3);
            this.selectablePlateMap1.Name = "selectablePlateMap1";
            this.selectablePlateMap1.Size = new System.Drawing.Size(246, 188);
            this.selectablePlateMap1.TabIndex = 3;
            // 
            // venusDataToolStripMenuItem
            // 
            this.venusDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadExcelFileWithODAndVenusDataToolStripMenuItem});
            this.venusDataToolStripMenuItem.Name = "venusDataToolStripMenuItem";
            this.venusDataToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.venusDataToolStripMenuItem.Text = "Venus Data";
            // 
            // loadExcelFileWithODAndVenusDataToolStripMenuItem
            // 
            this.loadExcelFileWithODAndVenusDataToolStripMenuItem.Name = "loadExcelFileWithODAndVenusDataToolStripMenuItem";
            this.loadExcelFileWithODAndVenusDataToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.loadExcelFileWithODAndVenusDataToolStripMenuItem.Text = "Load Excel file with OD and Venus data";
            this.loadExcelFileWithODAndVenusDataToolStripMenuItem.Click += new System.EventHandler(this.loadExcelFileWithODAndVenusDataToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1173, 714);
            this.Controls.Add(this.tabMainTab);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fit Growth Curves";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabBlankRemoval.ResumeLayout(false);
            this.tabBlankRemoval.PerformLayout();
            this.tabRobo.ResumeLayout(false);
            this.tabRobo.PerformLayout();
            this.tabMultPlot.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabEnterData.ResumeLayout(false);
            this.tabEnterData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tblRawData)).EndInit();
            this.tabPageChangeFitted.ResumeLayout(false);
            this.tabPageChangeFitted.PerformLayout();
            this.tabPageMain.ResumeLayout(false);
            this.bottomrightpanel.ResumeLayout(false);
            this.splitContainerbottom.Panel1.ResumeLayout(false);
            this.splitContainerbottom.Panel2.ResumeLayout(false);
            this.splitContainerbottom.ResumeLayout(false);
            this.topRightPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.l.ResumeLayout(false);
            this.l.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.leftsubbottompanel.ResumeLayout(false);
            this.leftsubbottompanel.PerformLayout();
            this.tabMainTab.ResumeLayout(false);
            this.tabPlotGraphic.ResumeLayout(false);
            this.tabPlotGraphic.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuOpenFile;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem importPreviousToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileWithInitialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryWithRobotDataToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        //private C1.Win.C1Chart.C1Chart ChartPickData;
        // private C1.Win.C1Chart.C1Chart ChartStandard;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryWithExcelDataToolStripMenuItem;
        private System.Windows.Forms.TabPage tabBlankRemoval;
        private System.Windows.Forms.Button btnDeleteAvg3asBlank;
        private System.Windows.Forms.Button btnDelete2ndPoint;
        private System.Windows.Forms.Button btnDeleteFirstBlank;
        private System.Windows.Forms.Button btnAvgtimePointsDeleteNigel;
        private System.Windows.Forms.Button btnAvgTimeSeries;
        private System.Windows.Forms.Button btnAvgAllRemoveNigelBlanks;
        private System.Windows.Forms.Button btnAvgFirstRemoveNigelBlanks;
        private System.Windows.Forms.Button btnRemoveNigelBlanks;
        private System.Windows.Forms.Label lblBlankInfo;
        private System.Windows.Forms.Button btnAvgAllAndDeleteTony;
        private System.Windows.Forms.Button btnAvgDataPoint1AndDeleteBlanks;
        private System.Windows.Forms.Button btnRemoveTonyCheckerboard;
        private System.Windows.Forms.RichTextBox txtDeleteBlanksTab;
        private System.Windows.Forms.TabPage tabRobo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListBox lstTimePoints;
        private System.Windows.Forms.RadioButton btnTimePoint;
        private System.Windows.Forms.RadioButton rbtnGrRate;
        private System.Windows.Forms.RadioButton rbtnIOD;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rbtnRS;
        private System.Windows.Forms.RadioButton rbtnGrowthRate;
        private System.Windows.Forms.RadioButton rbtnMaxOD;
        private System.Windows.Forms.Label lblRobo;
        private System.Windows.Forms.Button btnSubtractWrittenBlank;
        private System.Windows.Forms.TextBox txtBlankValue;
        private System.Windows.Forms.TabPage tabMultPlot;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkLegend;
        private System.Windows.Forms.CheckBox chkMuliLog;
        private System.Windows.Forms.CheckBox chkFillColors;
        private ZedGraph.ZedGraphControl MultiplePlots;
        private System.Windows.Forms.ListBox lstMultiplePlots;
        private System.Windows.Forms.TabPage tabEnterData;
        private System.Windows.Forms.RichTextBox txtEnterDataInstructions;
        private System.Windows.Forms.TextBox txtDataName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnInsertTime;
        private System.Windows.Forms.DataGridView DataView;
        private System.Windows.Forms.DateTimePicker PickSampleTime;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView tblRawData;
        private System.Windows.Forms.TabPage tabPageChangeFitted;
        private System.Windows.Forms.CheckBox chkShowFittedPick;
        private ZedGraph.ZedGraphControl ChartPickData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstGrowthCurvesMirror;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstDataMirror;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.Panel bottomrightpanel;
        private System.Windows.Forms.SplitContainer splitContainerbottom;
        private ZedGraph.ZedGraphControl ChartSlopeN;
        private ZedGraph.ZedGraphControl ChartStandard;
        private System.Windows.Forms.Panel topRightPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label7;
        private ZedGraph.ZedGraphControl ChartN;
        private System.Windows.Forms.Panel l;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEndPoint;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSetBounds;
        private System.Windows.Forms.TextBox txtStartPoint;
        public System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.CheckBox chkShowLin;
        private System.Windows.Forms.Button btnDeleteCurve;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel leftsubbottompanel;
        private System.Windows.Forms.CheckBox chkUsePercent;
        private System.Windows.Forms.TextBox txtMaxODPercent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnFitODRange;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMaxOD;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMinOD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChangeAxis;
        private System.Windows.Forms.TextBox txtMaxRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMinRange;
        private System.Windows.Forms.ListBox lstGrowthCurves;
        private System.Windows.Forms.TabControl tabMainTab;
        private System.Windows.Forms.TabPage tabPlotGraphic;
        private ZedGraph.ZedGraphControl plotTreatments;
        private SelectablePlateMap selectablePlateMap1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ListBox lstTreatmentSelection;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtTreatment5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtTreatment4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtTreatment3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtTreatment2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTreatment1;
        private System.Windows.Forms.CheckBox chkTreatLegend;
        private System.Windows.Forms.CheckBox chkTreatShowLog;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtTreatment6;
        private System.Windows.Forms.RadioButton rbtnTreatTimevOD;
        private System.Windows.Forms.RadioButton rbtnTreatDoublingTime;
        private System.Windows.Forms.RadioButton rbtnTreatInitialOD;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.RadioButton rbtnTreatRSq;
        private System.Windows.Forms.RadioButton rbtnTreatGrowthRate;
        private System.Windows.Forms.RadioButton rbtnTreatMaxOd;
        private System.Windows.Forms.RadioButton rbtnTreatNumPoints;
        private System.Windows.Forms.Button btnClearTreatments;
        private System.Windows.Forms.CheckBox chkODMustIncrease;
        private System.Windows.Forms.Label lblDeletePlate;
        private SelectablePlateMap toDeletePlateMap;
        private System.Windows.Forms.RadioButton rbtnTimeToOD;
        private System.Windows.Forms.ToolStripMenuItem importPlateKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDataToDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllMaxODValuesBadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllGrowthRateValuesBadToolStripMenuItem;
        private System.Windows.Forms.Button btnFlagGrowthRate;
        private System.Windows.Forms.Button btnFlagOD;
        private System.Windows.Forms.Button btnMakeEvoGroups;
        private System.Windows.Forms.ToolStripMenuItem venusDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadExcelFileWithODAndVenusDataToolStripMenuItem;

    }
}

