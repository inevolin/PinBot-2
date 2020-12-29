namespace PinBot2.Configurations.PinForms
{
    partial class QueueForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.cboBoards = new System.Windows.Forms.ComboBox();
            this.panelManual = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnScrape = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtScrapeMax = new System.Windows.Forms.NumericUpDown();
            this.txtScrapeMin = new System.Windows.Forms.NumericUpDown();
            this.grpQueue = new System.Windows.Forms.FlowLayoutPanel();
            this.txtCurrentPage = new System.Windows.Forms.TextBox();
            this.lblTotalPages = new System.Windows.Forms.Label();
            this.btnPagePrev = new System.Windows.Forms.Button();
            this.btnPageNext = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.files = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            this.panelManual.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtScrapeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScrapeMin)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 348);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(657, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip2";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // cboBoards
            // 
            this.cboBoards.FormattingEnabled = true;
            this.cboBoards.Location = new System.Drawing.Point(6, 51);
            this.cboBoards.Name = "cboBoards";
            this.cboBoards.Size = new System.Drawing.Size(284, 24);
            this.cboBoards.TabIndex = 3;
            // 
            // panelManual
            // 
            this.panelManual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelManual.Controls.Add(this.flowLayoutPanel2);
            this.panelManual.Controls.Add(this.label1);
            this.panelManual.Controls.Add(this.cboBoards);
            this.panelManual.Controls.Add(this.label6);
            this.panelManual.Controls.Add(this.label7);
            this.panelManual.Controls.Add(this.txtScrapeMax);
            this.panelManual.Controls.Add(this.txtScrapeMin);
            this.panelManual.Location = new System.Drawing.Point(11, 3);
            this.panelManual.Name = "panelManual";
            this.panelManual.Size = new System.Drawing.Size(633, 79);
            this.panelManual.TabIndex = 19;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnImport);
            this.flowLayoutPanel2.Controls.Add(this.btnScrape);
            this.flowLayoutPanel2.Controls.Add(this.btnClear);
            this.flowLayoutPanel2.Controls.Add(this.btnSave);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(290, 46);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(337, 32);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 28);
            this.btnImport.TabIndex = 15;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnScrape
            // 
            this.btnScrape.Location = new System.Drawing.Point(84, 3);
            this.btnScrape.Name = "btnScrape";
            this.btnScrape.Size = new System.Drawing.Size(87, 28);
            this.btnScrape.TabIndex = 4;
            this.btnScrape.Text = "Scrape";
            this.btnScrape.UseVisualStyleBackColor = true;
            this.btnScrape.Click += new System.EventHandler(this.btnScrape_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(177, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 28);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear all";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(258, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(287, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "images";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(184, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "and";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Scrape between";
            // 
            // txtScrapeMax
            // 
            this.txtScrapeMax.Location = new System.Drawing.Point(222, 16);
            this.txtScrapeMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtScrapeMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtScrapeMax.Name = "txtScrapeMax";
            this.txtScrapeMax.Size = new System.Drawing.Size(59, 22);
            this.txtScrapeMax.TabIndex = 2;
            this.txtScrapeMax.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.txtScrapeMax.ValueChanged += new System.EventHandler(this.txtScrapeMax_ValueChanged);
            // 
            // txtScrapeMin
            // 
            this.txtScrapeMin.Location = new System.Drawing.Point(119, 16);
            this.txtScrapeMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtScrapeMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtScrapeMin.Name = "txtScrapeMin";
            this.txtScrapeMin.Size = new System.Drawing.Size(59, 22);
            this.txtScrapeMin.TabIndex = 1;
            this.txtScrapeMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtScrapeMin.ValueChanged += new System.EventHandler(this.txtScrapeMin_ValueChanged);
            // 
            // grpQueue
            // 
            this.grpQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpQueue.AutoScroll = true;
            this.grpQueue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpQueue.BackColor = System.Drawing.Color.AliceBlue;
            this.grpQueue.Location = new System.Drawing.Point(11, 88);
            this.grpQueue.Name = "grpQueue";
            this.grpQueue.Size = new System.Drawing.Size(635, 217);
            this.grpQueue.TabIndex = 20;
            // 
            // txtCurrentPage
            // 
            this.txtCurrentPage.Location = new System.Drawing.Point(37, 3);
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.Size = new System.Drawing.Size(34, 22);
            this.txtCurrentPage.TabIndex = 21;
            this.txtCurrentPage.Text = "1";
            this.txtCurrentPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCurrentPage.TextChanged += new System.EventHandler(this.txtCurrentPage_TextChanged);
            // 
            // lblTotalPages
            // 
            this.lblTotalPages.AutoSize = true;
            this.lblTotalPages.Location = new System.Drawing.Point(74, 5);
            this.lblTotalPages.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
            this.lblTotalPages.Name = "lblTotalPages";
            this.lblTotalPages.Size = new System.Drawing.Size(20, 17);
            this.lblTotalPages.TabIndex = 22;
            this.lblTotalPages.Text = "/#";
            // 
            // btnPagePrev
            // 
            this.btnPagePrev.Location = new System.Drawing.Point(3, 3);
            this.btnPagePrev.Name = "btnPagePrev";
            this.btnPagePrev.Size = new System.Drawing.Size(28, 23);
            this.btnPagePrev.TabIndex = 23;
            this.btnPagePrev.Text = "<";
            this.btnPagePrev.UseVisualStyleBackColor = true;
            this.btnPagePrev.Click += new System.EventHandler(this.btnPagePrev_Click);
            // 
            // btnPageNext
            // 
            this.btnPageNext.Location = new System.Drawing.Point(100, 3);
            this.btnPageNext.Name = "btnPageNext";
            this.btnPageNext.Size = new System.Drawing.Size(28, 23);
            this.btnPageNext.TabIndex = 24;
            this.btnPageNext.Text = ">";
            this.btnPageNext.UseVisualStyleBackColor = true;
            this.btnPageNext.Click += new System.EventHandler(this.btnPageNext_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btnPagePrev);
            this.flowLayoutPanel1.Controls.Add(this.txtCurrentPage);
            this.flowLayoutPanel1.Controls.Add(this.lblTotalPages);
            this.flowLayoutPanel1.Controls.Add(this.btnPageNext);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 311);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(635, 27);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // files
            // 
            this.files.Multiselect = true;
            // 
            // QueueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 370);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.grpQueue);
            this.Controls.Add(this.panelManual);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(675, 414);
            this.Name = "QueueForm";
            this.Text = "Queue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueueForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelManual.ResumeLayout(false);
            this.panelManual.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtScrapeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScrapeMin)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ComboBox cboBoards;
        private System.Windows.Forms.Panel panelManual;
        private System.Windows.Forms.Button btnScrape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtScrapeMax;
        private System.Windows.Forms.NumericUpDown txtScrapeMin;
        private System.Windows.Forms.FlowLayoutPanel grpQueue;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCurrentPage;
        private System.Windows.Forms.Label lblTotalPages;
        private System.Windows.Forms.Button btnPagePrev;
        private System.Windows.Forms.Button btnPageNext;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog files;

    }
}