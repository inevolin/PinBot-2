using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinBot2.Configurations.PinRepin
{
    public partial class BoardSelector : Form
    {
        public Board SelectedBoard;
        private IList<Board> boards;
        public bool Ok { get; set; }
        public BoardSelector(IList<Board> boards)
        {
            InitializeComponent();
            this.boards = boards;
            foreach (var b in this.boards)
            {
                cboBoards.Items.Add(b);
            }
            cboBoards.DisplayMember = "Boardname";
            cboBoards.ValueMember = "Id";
            cboBoards.SelectedIndex = 0;
            Ok = false;
        }

        private void cboBoards_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedBoard = (Board)cboBoards.SelectedItem;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Ok = true;
            Close();
        }
    }
}
