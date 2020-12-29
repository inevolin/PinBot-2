using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinBot2.UserControls
{
    public partial class AccountInformation : UserControl
    {
        public int followers { get; set; }
        public int following { get; set; }
        public int likes { get; set; }
        public int boards { get; set; }
        public int pins { get; set; }
        public void update()
        {
            lblFollowers.Text = "Followers: " + followers;
            lblFollowing.Text = "Following: " + following;
            lblLikes.Text = "Likes: " + likes;
            lblBoards.Text = "Boards: " + boards;
            lblPins.Text = "Pins: " + pins;
        }

        public AccountInformation(/*int followers, int following, int likes, int boards, int pins*/)
        {
            InitializeComponent();
            /*
            this.followers = followers;
            this.following = following;
            this.likes = likes;
            this.boards = boards;
            this.pins = pins;
             */
        }
    }
}
