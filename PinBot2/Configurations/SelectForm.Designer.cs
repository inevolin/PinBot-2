namespace PinBot2
{
    partial class SelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectForm));
            this.btnLike = new System.Windows.Forms.Button();
            this.btnFollow = new System.Windows.Forms.Button();
            this.btnRepin = new System.Windows.Forms.Button();
            this.btnPin = new System.Windows.Forms.Button();
            this.btnUnfollow = new System.Windows.Forms.Button();
            this.btnComment = new System.Windows.Forms.Button();
            this.cboCampaign = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRenameCampaign = new System.Windows.Forms.Button();
            this.btnNewCampaign = new System.Windows.Forms.Button();
            this.btnDeleteCampaign = new System.Windows.Forms.Button();
            this.btnInvite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLike
            // 
            this.btnLike.Location = new System.Drawing.Point(191, 73);
            this.btnLike.Name = "btnLike";
            this.btnLike.Size = new System.Drawing.Size(85, 72);
            this.btnLike.TabIndex = 7;
            this.btnLike.Text = "Like";
            this.btnLike.UseVisualStyleBackColor = true;
            this.btnLike.Click += new System.EventHandler(this.btnLike_Click);
            // 
            // btnFollow
            // 
            this.btnFollow.Location = new System.Drawing.Point(99, 73);
            this.btnFollow.Name = "btnFollow";
            this.btnFollow.Size = new System.Drawing.Size(85, 72);
            this.btnFollow.TabIndex = 6;
            this.btnFollow.Text = "Follow";
            this.btnFollow.UseVisualStyleBackColor = true;
            this.btnFollow.Click += new System.EventHandler(this.btnFollow_Click);
            // 
            // btnRepin
            // 
            this.btnRepin.Location = new System.Drawing.Point(8, 73);
            this.btnRepin.Name = "btnRepin";
            this.btnRepin.Size = new System.Drawing.Size(85, 72);
            this.btnRepin.TabIndex = 5;
            this.btnRepin.Text = "Repin";
            this.btnRepin.UseVisualStyleBackColor = true;
            this.btnRepin.Click += new System.EventHandler(this.btnRepin_Click);
            // 
            // btnPin
            // 
            this.btnPin.Location = new System.Drawing.Point(8, 151);
            this.btnPin.Name = "btnPin";
            this.btnPin.Size = new System.Drawing.Size(85, 72);
            this.btnPin.TabIndex = 8;
            this.btnPin.Text = "Pin";
            this.btnPin.UseVisualStyleBackColor = true;
            this.btnPin.Click += new System.EventHandler(this.btnPin_Click);
            // 
            // btnUnfollow
            // 
            this.btnUnfollow.Location = new System.Drawing.Point(99, 151);
            this.btnUnfollow.Name = "btnUnfollow";
            this.btnUnfollow.Size = new System.Drawing.Size(85, 72);
            this.btnUnfollow.TabIndex = 9;
            this.btnUnfollow.Text = "Unfollow";
            this.btnUnfollow.UseVisualStyleBackColor = true;
            this.btnUnfollow.Click += new System.EventHandler(this.btnUnfollow_Click);
            // 
            // btnComment
            // 
            this.btnComment.Location = new System.Drawing.Point(191, 151);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(85, 72);
            this.btnComment.TabIndex = 10;
            this.btnComment.Text = "Comment";
            this.btnComment.UseVisualStyleBackColor = true;
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // cboCampaign
            // 
            this.cboCampaign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCampaign.FormattingEnabled = true;
            this.cboCampaign.Location = new System.Drawing.Point(86, 22);
            this.cboCampaign.Name = "cboCampaign";
            this.cboCampaign.Size = new System.Drawing.Size(170, 24);
            this.cboCampaign.TabIndex = 1;
            this.cboCampaign.SelectedIndexChanged += new System.EventHandler(this.cboCampaign_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Campaign:";
            // 
            // btnRenameCampaign
            // 
            this.btnRenameCampaign.Location = new System.Drawing.Point(262, 21);
            this.btnRenameCampaign.Name = "btnRenameCampaign";
            this.btnRenameCampaign.Size = new System.Drawing.Size(67, 28);
            this.btnRenameCampaign.TabIndex = 2;
            this.btnRenameCampaign.Text = "rename";
            this.btnRenameCampaign.UseVisualStyleBackColor = true;
            this.btnRenameCampaign.Click += new System.EventHandler(this.btnRenameCampaign_Click);
            // 
            // btnNewCampaign
            // 
            this.btnNewCampaign.Location = new System.Drawing.Point(335, 21);
            this.btnNewCampaign.Name = "btnNewCampaign";
            this.btnNewCampaign.Size = new System.Drawing.Size(49, 28);
            this.btnNewCampaign.TabIndex = 3;
            this.btnNewCampaign.Text = "add";
            this.btnNewCampaign.UseVisualStyleBackColor = true;
            this.btnNewCampaign.Click += new System.EventHandler(this.btnNewCampaign_Click);
            // 
            // btnDeleteCampaign
            // 
            this.btnDeleteCampaign.Location = new System.Drawing.Point(390, 21);
            this.btnDeleteCampaign.Name = "btnDeleteCampaign";
            this.btnDeleteCampaign.Size = new System.Drawing.Size(49, 28);
            this.btnDeleteCampaign.TabIndex = 4;
            this.btnDeleteCampaign.Text = "del";
            this.btnDeleteCampaign.UseVisualStyleBackColor = true;
            this.btnDeleteCampaign.Click += new System.EventHandler(this.btnDeleteCampaign_Click);
            // 
            // btnInvite
            // 
            this.btnInvite.Location = new System.Drawing.Point(282, 151);
            this.btnInvite.Name = "btnInvite";
            this.btnInvite.Size = new System.Drawing.Size(85, 72);
            this.btnInvite.TabIndex = 11;
            this.btnInvite.Text = "Invite";
            this.btnInvite.UseVisualStyleBackColor = true;
            this.btnInvite.Click += new System.EventHandler(this.btnInvite_Click);
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 238);
            this.Controls.Add(this.btnInvite);
            this.Controls.Add(this.btnDeleteCampaign);
            this.Controls.Add(this.btnNewCampaign);
            this.Controls.Add(this.btnRenameCampaign);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboCampaign);
            this.Controls.Add(this.btnComment);
            this.Controls.Add(this.btnUnfollow);
            this.Controls.Add(this.btnPin);
            this.Controls.Add(this.btnRepin);
            this.Controls.Add(this.btnFollow);
            this.Controls.Add(this.btnLike);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectForm";
            this.Text = "Feature configurations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLike;
        private System.Windows.Forms.Button btnFollow;
        private System.Windows.Forms.Button btnRepin;
        private System.Windows.Forms.Button btnPin;
        private System.Windows.Forms.Button btnUnfollow;
        private System.Windows.Forms.Button btnComment;
        private System.Windows.Forms.ComboBox cboCampaign;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRenameCampaign;
        private System.Windows.Forms.Button btnNewCampaign;
        private System.Windows.Forms.Button btnDeleteCampaign;
        private System.Windows.Forms.Button btnInvite;
    }
}