namespace PinBot2
{
    partial class PinForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PinForm));
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimeoutMax = new System.Windows.Forms.NumericUpDown();
            this.txtTimeoutMin = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnQueue = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFollowsMax = new System.Windows.Forms.NumericUpDown();
            this.txtFollowsMin = new System.Windows.Forms.NumericUpDown();
            this.AutoStartTimeMin = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.AutoStartTimeMax = new System.Windows.Forms.DateTimePicker();
            this.chkAutopilot = new System.Windows.Forms.CheckBox();
            this.grpAutopilot = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtSourceUrls = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstBoards = new System.Windows.Forms.ListBox();
            this.txtQry = new System.Windows.Forms.TextBox();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numSourceUrlRate = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMin)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMin)).BeginInit();
            this.grpAutopilot.SuspendLayout();
            this.panel.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSourceUrlRate)).BeginInit();
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
            // 
            // txtTimeoutMax
            // 
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
            this.txtTimeoutMax.TabIndex = 5;
            this.txtTimeoutMax.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.txtTimeoutMax.ValueChanged += new System.EventHandler(this.txtTimeoutMax_ValueChanged);
            // 
            // txtTimeoutMin
            // 
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
            this.txtTimeoutMin.TabIndex = 4;
            this.txtTimeoutMin.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.txtTimeoutMin.ValueChanged += new System.EventHandler(this.txtTimeoutMin_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTimeoutMax);
            this.groupBox1.Controls.Add(this.txtTimeoutMin);
            this.groupBox1.Location = new System.Drawing.Point(10, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 71);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Timeout";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 473);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(900, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnQueue
            // 
            this.btnQueue.Location = new System.Drawing.Point(686, 5);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Size = new System.Drawing.Size(130, 28);
            this.btnQueue.TabIndex = 2;
            this.btnQueue.Text = "Edit queue";
            this.btnQueue.UseVisualStyleBackColor = true;
            this.btnQueue.Click += new System.EventHandler(this.btnQueue_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtFollowsMax);
            this.groupBox2.Controls.Add(this.txtFollowsMin);
            this.groupBox2.Location = new System.Drawing.Point(10, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 71);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(248, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "pins";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(145, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "and";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "Between";
            // 
            // txtFollowsMax
            // 
            this.txtFollowsMax.Location = new System.Drawing.Point(183, 34);
            this.txtFollowsMax.Maximum = new decimal(new int[] {
            1000,
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
            this.txtFollowsMax.TabIndex = 7;
            this.txtFollowsMax.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.txtFollowsMax.ValueChanged += new System.EventHandler(this.txtFollowsMax_ValueChanged);
            // 
            // txtFollowsMin
            // 
            this.txtFollowsMin.Location = new System.Drawing.Point(80, 34);
            this.txtFollowsMin.Maximum = new decimal(new int[] {
            1000,
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
            this.txtFollowsMin.TabIndex = 6;
            this.txtFollowsMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtFollowsMin.ValueChanged += new System.EventHandler(this.txtFollowsMin_ValueChanged);
            // 
            // AutoStartTimeMin
            // 
            this.AutoStartTimeMin.CustomFormat = "HH:mm";
            this.AutoStartTimeMin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AutoStartTimeMin.Location = new System.Drawing.Point(80, 33);
            this.AutoStartTimeMin.Name = "AutoStartTimeMin";
            this.AutoStartTimeMin.ShowUpDown = true;
            this.AutoStartTimeMin.Size = new System.Drawing.Size(74, 22);
            this.AutoStartTimeMin.TabIndex = 9;
            this.AutoStartTimeMin.ValueChanged += new System.EventHandler(this.AutoStartTimeMin_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(160, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "and";
            // 
            // AutoStartTimeMax
            // 
            this.AutoStartTimeMax.CustomFormat = "HH:mm";
            this.AutoStartTimeMax.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AutoStartTimeMax.Location = new System.Drawing.Point(198, 33);
            this.AutoStartTimeMax.Name = "AutoStartTimeMax";
            this.AutoStartTimeMax.ShowUpDown = true;
            this.AutoStartTimeMax.Size = new System.Drawing.Size(74, 22);
            this.AutoStartTimeMax.TabIndex = 10;
            this.AutoStartTimeMax.ValueChanged += new System.EventHandler(this.AutoStartTimeMax_ValueChanged);
            // 
            // chkAutopilot
            // 
            this.chkAutopilot.AutoSize = true;
            this.chkAutopilot.Location = new System.Drawing.Point(19, 167);
            this.chkAutopilot.Name = "chkAutopilot";
            this.chkAutopilot.Size = new System.Drawing.Size(167, 21);
            this.chkAutopilot.TabIndex = 8;
            this.chkAutopilot.Text = "Autopilot (24h format)";
            this.chkAutopilot.UseVisualStyleBackColor = true;
            this.chkAutopilot.CheckedChanged += new System.EventHandler(this.chkAutopilot_CheckedChanged);
            // 
            // grpAutopilot
            // 
            this.grpAutopilot.Controls.Add(this.AutoStartTimeMax);
            this.grpAutopilot.Controls.Add(this.AutoStartTimeMin);
            this.grpAutopilot.Controls.Add(this.label8);
            this.grpAutopilot.Controls.Add(this.label9);
            this.grpAutopilot.Enabled = false;
            this.grpAutopilot.Location = new System.Drawing.Point(10, 168);
            this.grpAutopilot.Name = "grpAutopilot";
            this.grpAutopilot.Size = new System.Drawing.Size(318, 71);
            this.grpAutopilot.TabIndex = 10;
            this.grpAutopilot.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 17);
            this.label9.TabIndex = 6;
            this.label9.Text = "Between";
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.groupBox4);
            this.panel.Controls.Add(this.groupBox3);
            this.panel.Controls.Add(this.chkAutopilot);
            this.panel.Controls.Add(this.grpAutopilot);
            this.panel.Controls.Add(this.groupBox2);
            this.panel.Controls.Add(this.groupBox1);
            this.panel.Enabled = false;
            this.panel.Location = new System.Drawing.Point(2, 39);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(895, 431);
            this.panel.TabIndex = 10;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtSourceUrls);
            this.groupBox4.Controls.Add(this.numSourceUrlRate);
            this.groupBox4.Location = new System.Drawing.Point(10, 245);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(318, 175);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Source Url\'s (one per line)";
            // 
            // txtSourceUrls
            // 
            this.txtSourceUrls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSourceUrls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSourceUrls.Location = new System.Drawing.Point(9, 68);
            this.txtSourceUrls.Margin = new System.Windows.Forms.Padding(6);
            this.txtSourceUrls.Multiline = true;
            this.txtSourceUrls.Name = "txtSourceUrls";
            this.txtSourceUrls.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSourceUrls.Size = new System.Drawing.Size(300, 101);
            this.txtSourceUrls.TabIndex = 13;
            this.txtSourceUrls.TextChanged += new System.EventHandler(this.txtSourceUrls_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.lstBoards);
            this.groupBox3.Controls.Add(this.txtQry);
            this.groupBox3.Location = new System.Drawing.Point(334, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(552, 408);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Queries per board (one query per line)";
            // 
            // lstBoards
            // 
            this.lstBoards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstBoards.FormattingEnabled = true;
            this.lstBoards.ItemHeight = 16;
            this.lstBoards.Location = new System.Drawing.Point(6, 25);
            this.lstBoards.Name = "lstBoards";
            this.lstBoards.ScrollAlwaysVisible = true;
            this.lstBoards.Size = new System.Drawing.Size(229, 372);
            this.lstBoards.TabIndex = 11;
            this.lstBoards.SelectedIndexChanged += new System.EventHandler(this.lstBoards_SelectedIndexChanged);
            // 
            // txtQry
            // 
            this.txtQry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtQry.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtQry.Location = new System.Drawing.Point(244, 25);
            this.txtQry.Margin = new System.Windows.Forms.Padding(6);
            this.txtQry.Multiline = true;
            this.txtQry.Name = "txtQry";
            this.txtQry.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQry.Size = new System.Drawing.Size(305, 377);
            this.txtQry.TabIndex = 12;
            this.txtQry.TextChanged += new System.EventHandler(this.txtQry_TextChanged);
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Location = new System.Drawing.Point(12, 12);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(82, 21);
            this.chkEnable.TabIndex = 1;
            this.chkEnable.Text = "Enabled";
            this.chkEnable.UseVisualStyleBackColor = true;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(822, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Add random URL for";
            // 
            // numSourceUrlRate
            // 
            this.numSourceUrlRate.Location = new System.Drawing.Point(156, 37);
            this.numSourceUrlRate.Name = "numSourceUrlRate";
            this.numSourceUrlRate.Size = new System.Drawing.Size(46, 22);
            this.numSourceUrlRate.TabIndex = 10;
            this.numSourceUrlRate.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(212, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 17);
            this.label10.TabIndex = 14;
            this.label10.Text = "% of pins.";
            // 
            // PinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 495);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PinForm";
            this.Text = "Pin";
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeoutMin)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowsMin)).EndInit();
            this.grpAutopilot.ResumeLayout(false);
            this.grpAutopilot.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSourceUrlRate)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtFollowsMax;
        private System.Windows.Forms.NumericUpDown txtFollowsMin;
        private System.Windows.Forms.DateTimePicker AutoStartTimeMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker AutoStartTimeMax;
        private System.Windows.Forms.CheckBox chkAutopilot;
        private System.Windows.Forms.GroupBox grpAutopilot;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQueue;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstBoards;
        private System.Windows.Forms.TextBox txtQry;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtSourceUrls;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numSourceUrlRate;


    }
}