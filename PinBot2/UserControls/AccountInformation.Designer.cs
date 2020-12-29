namespace PinBot2.UserControls
{
    partial class AccountInformation
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

        #region Component Designer generated code

        
        private void InitializeComponent()
        {
            this.lblFollowers = new System.Windows.Forms.Label();
            this.lblFollowing = new System.Windows.Forms.Label();
            this.lblLikes = new System.Windows.Forms.Label();
            this.lblPins = new System.Windows.Forms.Label();
            this.lblBoards = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFollowers
            // 
            this.lblFollowers.AutoSize = true;
            this.lblFollowers.Location = new System.Drawing.Point(3, 0);
            this.lblFollowers.Name = "lblFollowers";
            this.lblFollowers.Size = new System.Drawing.Size(75, 17);
            this.lblFollowers.TabIndex = 0;
            this.lblFollowers.Text = "Followers: ";
            // 
            // lblFollowing
            // 
            this.lblFollowing.AutoSize = true;
            this.lblFollowing.Location = new System.Drawing.Point(3, 17);
            this.lblFollowing.Name = "lblFollowing";
            this.lblFollowing.Size = new System.Drawing.Size(74, 17);
            this.lblFollowing.TabIndex = 1;
            this.lblFollowing.Text = "Following: ";
            // 
            // lblLikes
            // 
            this.lblLikes.AutoSize = true;
            this.lblLikes.Location = new System.Drawing.Point(3, 34);
            this.lblLikes.Name = "lblLikes";
            this.lblLikes.Size = new System.Drawing.Size(49, 17);
            this.lblLikes.TabIndex = 2;
            this.lblLikes.Text = "Likes: ";
            // 
            // lblPins
            // 
            this.lblPins.AutoSize = true;
            this.lblPins.Location = new System.Drawing.Point(3, 51);
            this.lblPins.Name = "lblPins";
            this.lblPins.Size = new System.Drawing.Size(43, 17);
            this.lblPins.TabIndex = 3;
            this.lblPins.Text = "Pins: ";
            // 
            // lblBoards
            // 
            this.lblBoards.AutoSize = true;
            this.lblBoards.Location = new System.Drawing.Point(3, 68);
            this.lblBoards.Name = "lblBoards";
            this.lblBoards.Size = new System.Drawing.Size(61, 17);
            this.lblBoards.TabIndex = 4;
            this.lblBoards.Text = "Boards: ";
            // 
            // AccountInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBoards);
            this.Controls.Add(this.lblPins);
            this.Controls.Add(this.lblLikes);
            this.Controls.Add(this.lblFollowing);
            this.Controls.Add(this.lblFollowers);
            this.Name = "AccountInformation";
            this.Size = new System.Drawing.Size(150, 89);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFollowers;
        private System.Windows.Forms.Label lblFollowing;
        private System.Windows.Forms.Label lblLikes;
        private System.Windows.Forms.Label lblPins;
        private System.Windows.Forms.Label lblBoards;
    }
}
