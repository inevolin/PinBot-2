namespace PinBot2.Configurations.PinRepin
{
    partial class BoardSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardSelector));
            this.cboBoards = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboBoards
            // 
            this.cboBoards.FormattingEnabled = true;
            this.cboBoards.Location = new System.Drawing.Point(12, 24);
            this.cboBoards.Name = "cboBoards";
            this.cboBoards.Size = new System.Drawing.Size(264, 24);
            this.cboBoards.TabIndex = 0;
            this.cboBoards.SelectedIndexChanged += new System.EventHandler(this.cboBoards_SelectedIndexChanged);

            this.btnOk.Location = new System.Drawing.Point(12, 54);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 90);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cboBoards);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoardSelector";
            this.Text = "Select a board";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboBoards;
        private System.Windows.Forms.Button btnOk;
    }
}