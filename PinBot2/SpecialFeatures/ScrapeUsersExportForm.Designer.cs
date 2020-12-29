namespace PinBot2
{
    partial class ScrapeUsersExportForm
    {
        
        private System.ComponentModel.IContainer components = null;

        
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

        
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrapeUsersExportForm));
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimeoutMax = new System.Windows.Forms.NumericUpDown();
            this.txtTimeoutMin = new System.Windows.Forms.NumericUpDown();
            this.grpTimeout = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.grpQueries = new System.Windows.Forms.GroupBox();
            this.txtQry = new System.Windows.Forms.TextBox();
            this.chkLimitScrape = new System.Windows.Forms.CheckBox();
            this.grpLimitScrape = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFollowsMax = new System.Windows.Forms.NumericUpDown();
            this.txtFollowsMin = new System.Windows.Forms.NumericUpDown();
            this.panel = new System.Windows.Forms.Panel();
            this.chkCriteria = new System.Windows.Forms.CheckBox();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkHasWebsite = new System.Windows.Forms.CheckBox();
            this.chkHasTw = new System.Windows.Forms.CheckBox();
            this.chkHasFb = new System.Windows.Forms.CheckBox();
            this.chkHasCustomPic = new System.Windows.Forms.CheckBox();
            this.chkHasLocation = new System.Windows.Forms.CheckBox();
            this.chkHasAbout = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtPinsMax = new System.Windows.Forms.NumericUpDown();
            this.txtPinsMin = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtBoardsMax = new System.Windows.Forms.NumericUpDown();
            this.txtBoardsMin = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtFollowingMax = new System.Windows.Forms.NumericUpDown();
            this.txtFollowingMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFollowersMax = new System.Windows.Forms.NumericUpDown();
            this.txtFollowersMin = new System.Windows.Forms.NumericUpDown();
            this.btnScrape = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.chkAppendToFile = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStop = new System.Windows.Forms.Button();
            this.sfdFile = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMin)).BeginInit();
            this.grpTimeout.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.grpQueries.SuspendLayout();
            this.grpLimitScrape.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMin)).BeginInit();
            this.panel.SuspendLayout();
            this.grpCriteria.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPinsMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPinsMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoardsMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoardsMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowingMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowingMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowersMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowersMin)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "seconds";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "and";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Between";

            this.txtTimeoutMax.Location = new System.Drawing.Point(183, 34);
            this.txtTimeoutMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtTimeoutMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtTimeoutMax.Name = "txtTimeoutMax";
            this.txtTimeoutMax.Size = new System.Drawing.Size(59, 22);
            this.txtTimeoutMax.TabIndex = 4;
            this.txtTimeoutMax.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtTimeoutMax.ValueChanged += new System.EventHandler(this.txtTimeoutMax_ValueChanged);

            this.txtTimeoutMin.Location = new System.Drawing.Point(80, 34);
            this.txtTimeoutMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtTimeoutMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTimeoutMin.Name = "txtTimeoutMin";
            this.txtTimeoutMin.Size = new System.Drawing.Size(59, 22);
            this.txtTimeoutMin.TabIndex = 3;
            this.txtTimeoutMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTimeoutMin.ValueChanged += new System.EventHandler(this.txtTimeoutMin_ValueChanged);
            // 
            // grpTimeout
            // 
            this.grpTimeout.Controls.Add(this.label4);
            this.grpTimeout.Controls.Add(this.label3);
            this.grpTimeout.Controls.Add(this.label2);
            this.grpTimeout.Controls.Add(this.txtTimeoutMax);
            this.grpTimeout.Controls.Add(this.txtTimeoutMin);
            this.grpTimeout.Location = new System.Drawing.Point(10, 12);
            this.grpTimeout.Name = "grpTimeout";
            this.grpTimeout.Size = new System.Drawing.Size(318, 71);
            this.grpTimeout.TabIndex = 5;
            this.grpTimeout.TabStop = false;
            this.grpTimeout.Text = "Timeout per page";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 378);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(665, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // grpQueries
            // 
            this.grpQueries.Controls.Add(this.txtQry);
            this.grpQueries.Location = new System.Drawing.Point(334, 12);
            this.grpQueries.Name = "grpQueries";
            this.grpQueries.Size = new System.Drawing.Size(318, 148);
            this.grpQueries.TabIndex = 10;
            this.grpQueries.TabStop = false;
            this.grpQueries.Text = "Queries (one per line)";

            this.txtQry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtQry.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtQry.Location = new System.Drawing.Point(6, 21);
            this.txtQry.Margin = new System.Windows.Forms.Padding(6);
            this.txtQry.Multiline = true;
            this.txtQry.Name = "txtQry";
            this.txtQry.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQry.Size = new System.Drawing.Size(305, 121);
            this.txtQry.TabIndex = 24;
            // 
            // chkLimitScrape
            // 
            this.chkLimitScrape.AutoSize = true;
            this.chkLimitScrape.Location = new System.Drawing.Point(19, 88);
            this.chkLimitScrape.Name = "chkLimitScrape";
            this.chkLimitScrape.Size = new System.Drawing.Size(103, 21);
            this.chkLimitScrape.TabIndex = 7;
            this.chkLimitScrape.Text = "Scrape limit";
            this.chkLimitScrape.UseVisualStyleBackColor = true;
            this.chkLimitScrape.CheckedChanged += new System.EventHandler(this.chkLimitScrape_CheckedChanged);
            // 
            // grpLimitScrape
            // 
            this.grpLimitScrape.Controls.Add(this.label6);
            this.grpLimitScrape.Controls.Add(this.label7);
            this.grpLimitScrape.Controls.Add(this.txtFollowsMax);
            this.grpLimitScrape.Controls.Add(this.txtFollowsMin);
            this.grpLimitScrape.Enabled = false;
            this.grpLimitScrape.Location = new System.Drawing.Point(10, 89);
            this.grpLimitScrape.Name = "grpLimitScrape";
            this.grpLimitScrape.Size = new System.Drawing.Size(318, 71);
            this.grpLimitScrape.TabIndex = 10;
            this.grpLimitScrape.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(145, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "and";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Between";

            this.txtFollowsMax.Location = new System.Drawing.Point(183, 34);
            this.txtFollowsMax.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtFollowsMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtFollowsMax.Name = "txtFollowsMax";
            this.txtFollowsMax.Size = new System.Drawing.Size(59, 22);
            this.txtFollowsMax.TabIndex = 10;
            this.txtFollowsMax.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});

            this.txtFollowsMin.Location = new System.Drawing.Point(80, 34);
            this.txtFollowsMin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtFollowsMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFollowsMin.Name = "txtFollowsMin";
            this.txtFollowsMin.Size = new System.Drawing.Size(59, 22);
            this.txtFollowsMin.TabIndex = 8;
            this.txtFollowsMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});

            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.chkCriteria);
            this.panel.Controls.Add(this.grpCriteria);
            this.panel.Controls.Add(this.chkLimitScrape);
            this.panel.Controls.Add(this.grpLimitScrape);
            this.panel.Controls.Add(this.grpQueries);
            this.panel.Controls.Add(this.grpTimeout);
            this.panel.Location = new System.Drawing.Point(2, 39);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(660, 336);
            this.panel.TabIndex = 10;
            // 
            // chkCriteria
            // 
            this.chkCriteria.AutoSize = true;
            this.chkCriteria.Checked = true;
            this.chkCriteria.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCriteria.Location = new System.Drawing.Point(19, 164);
            this.chkCriteria.Name = "chkCriteria";
            this.chkCriteria.Size = new System.Drawing.Size(111, 21);
            this.chkCriteria.TabIndex = 10;
            this.chkCriteria.Text = "User criteria:";
            this.chkCriteria.UseVisualStyleBackColor = true;
            this.chkCriteria.CheckedChanged += new System.EventHandler(this.chkCriteria_CheckedChanged);
            // 
            // grpCriteria
            // 
            this.grpCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpCriteria.Controls.Add(this.flowLayoutPanel1);
            this.grpCriteria.Controls.Add(this.label18);
            this.grpCriteria.Controls.Add(this.label19);
            this.grpCriteria.Controls.Add(this.label20);
            this.grpCriteria.Controls.Add(this.txtPinsMax);
            this.grpCriteria.Controls.Add(this.txtPinsMin);
            this.grpCriteria.Controls.Add(this.label15);
            this.grpCriteria.Controls.Add(this.label16);
            this.grpCriteria.Controls.Add(this.label17);
            this.grpCriteria.Controls.Add(this.txtBoardsMax);
            this.grpCriteria.Controls.Add(this.txtBoardsMin);
            this.grpCriteria.Controls.Add(this.label12);
            this.grpCriteria.Controls.Add(this.label13);
            this.grpCriteria.Controls.Add(this.label14);
            this.grpCriteria.Controls.Add(this.txtFollowingMax);
            this.grpCriteria.Controls.Add(this.txtFollowingMin);
            this.grpCriteria.Controls.Add(this.label1);
            this.grpCriteria.Controls.Add(this.label10);
            this.grpCriteria.Controls.Add(this.label11);
            this.grpCriteria.Controls.Add(this.txtFollowersMax);
            this.grpCriteria.Controls.Add(this.txtFollowersMin);
            this.grpCriteria.Location = new System.Drawing.Point(10, 166);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(642, 167);
            this.grpCriteria.TabIndex = 12;
            this.grpCriteria.TabStop = false;

            this.flowLayoutPanel1.Controls.Add(this.chkHasWebsite);
            this.flowLayoutPanel1.Controls.Add(this.chkHasTw);
            this.flowLayoutPanel1.Controls.Add(this.chkHasFb);
            this.flowLayoutPanel1.Controls.Add(this.chkHasCustomPic);
            this.flowLayoutPanel1.Controls.Add(this.chkHasLocation);
            this.flowLayoutPanel1.Controls.Add(this.chkHasAbout);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(350, 37);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(285, 112);
            this.flowLayoutPanel1.TabIndex = 35;
            // 
            // chkHasWebsite
            // 
            this.chkHasWebsite.AutoSize = true;
            this.chkHasWebsite.Location = new System.Drawing.Point(3, 88);
            this.chkHasWebsite.Name = "chkHasWebsite";
            this.chkHasWebsite.Size = new System.Drawing.Size(106, 21);
            this.chkHasWebsite.TabIndex = 29;
            this.chkHasWebsite.Text = "Has website";
            this.chkHasWebsite.UseVisualStyleBackColor = true;
            // 
            // chkHasTw
            // 
            this.chkHasTw.AutoSize = true;
            this.chkHasTw.Location = new System.Drawing.Point(3, 61);
            this.chkHasTw.Name = "chkHasTw";
            this.chkHasTw.Size = new System.Drawing.Size(101, 21);
            this.chkHasTw.TabIndex = 30;
            this.chkHasTw.Text = "Has Twitter";
            this.chkHasTw.UseVisualStyleBackColor = true;
            // 
            // chkHasFb
            // 
            this.chkHasFb.AutoSize = true;
            this.chkHasFb.Location = new System.Drawing.Point(3, 34);
            this.chkHasFb.Name = "chkHasFb";
            this.chkHasFb.Size = new System.Drawing.Size(121, 21);
            this.chkHasFb.TabIndex = 31;
            this.chkHasFb.Text = "Has Facebook";
            this.chkHasFb.UseVisualStyleBackColor = true;
            // 
            // chkHasCustomPic
            // 
            this.chkHasCustomPic.AutoSize = true;
            this.chkHasCustomPic.Location = new System.Drawing.Point(3, 7);
            this.chkHasCustomPic.Name = "chkHasCustomPic";
            this.chkHasCustomPic.Size = new System.Drawing.Size(151, 21);
            this.chkHasCustomPic.TabIndex = 32;
            this.chkHasCustomPic.Text = "Has custom picture";
            this.chkHasCustomPic.UseVisualStyleBackColor = true;
            // 
            // chkHasLocation
            // 
            this.chkHasLocation.AutoSize = true;
            this.chkHasLocation.Location = new System.Drawing.Point(160, 88);
            this.chkHasLocation.Name = "chkHasLocation";
            this.chkHasLocation.Size = new System.Drawing.Size(108, 21);
            this.chkHasLocation.TabIndex = 33;
            this.chkHasLocation.Text = "Has location";
            this.chkHasLocation.UseVisualStyleBackColor = true;
            // 
            // chkHasAbout
            // 
            this.chkHasAbout.AutoSize = true;
            this.chkHasAbout.Location = new System.Drawing.Point(160, 61);
            this.chkHasAbout.Name = "chkHasAbout";
            this.chkHasAbout.Size = new System.Drawing.Size(121, 21);
            this.chkHasAbout.TabIndex = 34;
            this.chkHasAbout.Text = "Has about text";
            this.chkHasAbout.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(248, 132);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 17);
            this.label18.TabIndex = 28;
            this.label18.Text = "pins";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(145, 132);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(32, 17);
            this.label19.TabIndex = 27;
            this.label19.Text = "and";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 132);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(62, 17);
            this.label20.TabIndex = 26;
            this.label20.Text = "Between";
            // 
            // 
            this.txtPinsMax.Location = new System.Drawing.Point(183, 130);
            this.txtPinsMax.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.txtPinsMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtPinsMax.Name = "txtPinsMax";
            this.txtPinsMax.Size = new System.Drawing.Size(59, 22);
            this.txtPinsMax.TabIndex = 18;
            this.txtPinsMax.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtPinsMax.ValueChanged += new System.EventHandler(this.txtPinsMax_ValueChanged);
            // 
            // 
            this.txtPinsMin.Location = new System.Drawing.Point(80, 130);
            this.txtPinsMin.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.txtPinsMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPinsMin.Name = "txtPinsMin";
            this.txtPinsMin.Size = new System.Drawing.Size(59, 22);
            this.txtPinsMin.TabIndex = 17;
            this.txtPinsMin.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtPinsMin.ValueChanged += new System.EventHandler(this.txtPinsMin_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(248, 102);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 17);
            this.label15.TabIndex = 23;
            this.label15.Text = "boards";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(145, 102);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 17);
            this.label16.TabIndex = 22;
            this.label16.Text = "and";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 102);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 17);
            this.label17.TabIndex = 21;
            this.label17.Text = "Between";
            // 
            // 
            this.txtBoardsMax.Location = new System.Drawing.Point(183, 100);
            this.txtBoardsMax.Maximum = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            this.txtBoardsMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtBoardsMax.Name = "txtBoardsMax";
            this.txtBoardsMax.Size = new System.Drawing.Size(59, 22);
            this.txtBoardsMax.TabIndex = 16;
            this.txtBoardsMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtBoardsMax.ValueChanged += new System.EventHandler(this.txtBoardsMax_ValueChanged);
            // 
            // 
            this.txtBoardsMin.Location = new System.Drawing.Point(80, 100);
            this.txtBoardsMin.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtBoardsMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtBoardsMin.Name = "txtBoardsMin";
            this.txtBoardsMin.Size = new System.Drawing.Size(59, 22);
            this.txtBoardsMin.TabIndex = 15;
            this.txtBoardsMin.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.txtBoardsMin.ValueChanged += new System.EventHandler(this.txtBoardsMin_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(248, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 17);
            this.label12.TabIndex = 18;
            this.label12.Text = "following";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(145, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 17);
            this.label13.TabIndex = 17;
            this.label13.Text = "and";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 72);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 17);
            this.label14.TabIndex = 16;
            this.label14.Text = "Between";
            // 

            // 
            this.txtFollowingMax.Location = new System.Drawing.Point(183, 70);
            this.txtFollowingMax.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.txtFollowingMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtFollowingMax.Name = "txtFollowingMax";
            this.txtFollowingMax.Size = new System.Drawing.Size(59, 22);
            this.txtFollowingMax.TabIndex = 14;
            this.txtFollowingMax.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.txtFollowingMax.ValueChanged += new System.EventHandler(this.txtFollowingMax_ValueChanged);
            // 
            // 
            this.txtFollowingMin.Location = new System.Drawing.Point(80, 70);
            this.txtFollowingMin.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.txtFollowingMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFollowingMin.Name = "txtFollowingMin";
            this.txtFollowingMin.Size = new System.Drawing.Size(59, 22);
            this.txtFollowingMin.TabIndex = 13;
            this.txtFollowingMin.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtFollowingMin.ValueChanged += new System.EventHandler(this.txtFollowingMin_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(248, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "followers";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(145, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 17);
            this.label10.TabIndex = 12;
            this.label10.Text = "and";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 17);
            this.label11.TabIndex = 11;
            this.label11.Text = "Between";
            // 
            // 
            this.txtFollowersMax.Location = new System.Drawing.Point(183, 40);
            this.txtFollowersMax.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.txtFollowersMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtFollowersMax.Name = "txtFollowersMax";
            this.txtFollowersMax.Size = new System.Drawing.Size(59, 22);
            this.txtFollowersMax.TabIndex = 12;
            this.txtFollowersMax.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtFollowersMax.ValueChanged += new System.EventHandler(this.txtFollowersMax_ValueChanged);
            // 
           
            // 
            this.txtFollowersMin.Location = new System.Drawing.Point(80, 40);
            this.txtFollowersMin.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.txtFollowersMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFollowersMin.Name = "txtFollowersMin";
            this.txtFollowersMin.Size = new System.Drawing.Size(59, 22);
            this.txtFollowersMin.TabIndex = 11;
            this.txtFollowersMin.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtFollowersMin.ValueChanged += new System.EventHandler(this.txtFollowersMin_ValueChanged);
            // 

            // 
            this.btnScrape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScrape.Enabled = false;
            this.btnScrape.Location = new System.Drawing.Point(495, 3);
            this.btnScrape.Name = "btnScrape";
            this.btnScrape.Size = new System.Drawing.Size(75, 28);
            this.btnScrape.TabIndex = 2;
            this.btnScrape.Text = "Start";
            this.btnScrape.UseVisualStyleBackColor = true;
            this.btnScrape.Click += new System.EventHandler(this.btnScrape_Click);
            // 

            // 
            this.btnSelectFile.Location = new System.Drawing.Point(390, 3);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(99, 28);
            this.btnSelectFile.TabIndex = 12;
            this.btnSelectFile.Text = "Select file...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // chkAppendToFile
            // 
            this.chkAppendToFile.AutoSize = true;
            this.chkAppendToFile.Location = new System.Drawing.Point(267, 3);
            this.chkAppendToFile.Name = "chkAppendToFile";
            this.chkAppendToFile.Size = new System.Drawing.Size(117, 21);
            this.chkAppendToFile.TabIndex = 13;
            this.chkAppendToFile.Text = "Append to file";
            this.chkAppendToFile.UseVisualStyleBackColor = true;
            // 

            // 
            this.flowLayoutPanel2.Controls.Add(this.btnStop);
            this.flowLayoutPanel2.Controls.Add(this.btnScrape);
            this.flowLayoutPanel2.Controls.Add(this.btnSelectFile);
            this.flowLayoutPanel2.Controls.Add(this.chkAppendToFile);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(5, 4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(654, 35);
            this.flowLayoutPanel2.TabIndex = 25;
            // 

            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(576, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 28);
            this.btnStop.TabIndex = 14;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // sfdFile
            // 
            this.sfdFile.OverwritePrompt = false;
            this.sfdFile.FileOk += new System.ComponentModel.CancelEventHandler(this.sfdFile_FileOk);
            // 
            // ScrapeUsersExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 400);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScrapeUsersExportForm";
            this.Text = "Scrape usernames and Export";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScrapeUsersExportForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMin)).EndInit();
            this.grpTimeout.ResumeLayout(false);
            this.grpTimeout.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpQueries.ResumeLayout(false);
            this.grpQueries.PerformLayout();
            this.grpLimitScrape.ResumeLayout(false);
            this.grpLimitScrape.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMin)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPinsMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPinsMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoardsMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoardsMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowingMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowingMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowersMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowersMin)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown txtTimeoutMax;
        private System.Windows.Forms.NumericUpDown txtTimeoutMin;
        private System.Windows.Forms.GroupBox grpTimeout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox grpQueries;
        private System.Windows.Forms.TextBox txtQry;
        private System.Windows.Forms.CheckBox chkLimitScrape;
        private System.Windows.Forms.GroupBox grpLimitScrape;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnScrape;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown txtPinsMax;
        private System.Windows.Forms.NumericUpDown txtPinsMin;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown txtBoardsMax;
        private System.Windows.Forms.NumericUpDown txtBoardsMin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown txtFollowingMax;
        private System.Windows.Forms.NumericUpDown txtFollowingMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown txtFollowersMax;
        private System.Windows.Forms.NumericUpDown txtFollowersMin;
        private System.Windows.Forms.CheckBox chkCriteria;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtFollowsMax;
        private System.Windows.Forms.NumericUpDown txtFollowsMin;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkHasWebsite;
        private System.Windows.Forms.CheckBox chkHasTw;
        private System.Windows.Forms.CheckBox chkHasFb;
        private System.Windows.Forms.CheckBox chkHasCustomPic;
        private System.Windows.Forms.CheckBox chkHasLocation;
        private System.Windows.Forms.CheckBox chkHasAbout;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.CheckBox chkAppendToFile;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.SaveFileDialog sfdFile;


    }
}