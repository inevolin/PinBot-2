using PinBot2.Common;
using PinBot2.Helpers;
using PinBot2.Model;
using PinBot2.Model.Configurations;
using PinBot2.Presenter.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinBot2
{
    public partial class SelectForm : Form, ISelectView
    {
        ///////
        public event EventHandler LikeConfig, FollowConfig, UnfollowConfig, RepinConfig, InviteConfig, PinConfig, CommentConfig;
        //...
        public event EventHandler SaveCampaign;
        public event EventHandler RemoveCampaign;
        public event EventHandler ReloadCampaigns;

        public IList<ICampaign> Campaigns { get; set; }
        private IAccount Account { get; set; }
        private bool IS_PREMIUM;

        public ICampaign SelectedCampaign
        {
            get { return (Campaign)cboCampaign.SelectedItem; }
        }

        public SelectForm()
        {
            InitializeComponent();
        }

        public void ShowForm(IAccount account, bool premium)
        {
            try
            {
                IS_PREMIUM = premium;
                if (!IS_PREMIUM)
                {
                    btnComment.Enabled = btnInvite.Enabled = btnLike.Enabled = btnPin.Enabled = btnUnfollow.Enabled = false;
                }

                Account = account;
                //populat dropdown box
                if (Campaigns.Count <= 0)
                {
                    Campaign c = new Campaign((Account)account);
                    c.CampaignName = "(new campaign)";
                    c.ID = 0;
                    Campaigns.Add(c);
                }

                cboCampaign.DataSource = Campaigns;
                cboCampaign.ValueMember = "ID";
                cboCampaign.DisplayMember = "CampaignName";
                cboCampaign.Refresh();

                EnabledBoldIndicator();
                

                ShowDialog();

            }
            catch (Exception ex)
            {
                string msg = "Error SF68." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SelectForm", msg);
                
            }
        }
        private void EnabledBoldIndicator()
        {
            foreach (var c in this.Controls)
            {
                if (c.GetType() == typeof(Button))
                    ((Button)c).Font = SystemFonts.DefaultFont;
            }
                var sc = (Campaign)cboCampaign.SelectedItem;
                if (sc.ID > 0)
                {
                    foreach (var ec in sc.ConfigurationContainer.EnabledConfigurations())
                    {
                        if (ec.GetType().Equals(typeof(LikeConfiguration)) && ec.Enabled)
                            btnLike.Font = new Font(btnLike.Font, FontStyle.Bold);
                        else if (ec.GetType().Equals(typeof(CommentConfiguration)) && ec.Enabled)
                            btnComment.Font = new Font(btnLike.Font, FontStyle.Bold);
                        else if (ec.GetType().Equals(typeof(FollowConfiguration)) && ec.Enabled)
                            btnFollow.Font = new Font(btnLike.Font, FontStyle.Bold);
                        else if (ec.GetType().Equals(typeof(UnfollowConfiguration)) && ec.Enabled)
                            btnUnfollow.Font = new Font(btnLike.Font, FontStyle.Bold);
                        else if (ec.GetType().Equals(typeof(InviteConfiguration)) && ec.Enabled)
                            btnInvite.Font = new Font(btnLike.Font, FontStyle.Bold);
                        else if (ec.GetType().Equals(typeof(PinConfiguration)) && ec.Enabled)
                            btnPin.Font = new Font(btnLike.Font, FontStyle.Bold);
                        else if (ec.GetType().Equals(typeof(RepinConfiguration)) && ec.Enabled)
                            btnRepin.Font = new Font(btnLike.Font, FontStyle.Bold);
                    }
                }
        }

        ////////
        private void btnLike_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            LikeConfig(sender, e);
            EnabledBoldIndicator();
        }
        private void btnFollow_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            FollowConfig(sender, e);
            EnabledBoldIndicator();
        }
        private void btnUnfollow_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            UnfollowConfig(sender, e);
            EnabledBoldIndicator();
        }
        private void btnRepin_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            RepinConfig(sender, e);
            EnabledBoldIndicator();
        }
        private void btnInvite_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            InviteConfig(sender, e);
            EnabledBoldIndicator();
        }
        private void btnComment_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            CommentConfig(sender, e);
            EnabledBoldIndicator();
        }
        private void btnPin_Click(object sender, EventArgs e)
        {
            CheckCampaign();
            PinConfig(sender, e);
            EnabledBoldIndicator();
        }
        //...

        private void CheckCampaign()
        {
            try
            {
                if (SelectedCampaign == null)
                    btnNewCampaign.PerformClick();
            }
            catch (Exception ex)
            {
                string msg = "Error SF121." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SelectForm", msg);
                
            }
        }



        private void btnRenameCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedCampaign == null) btnNewCampaign.PerformClick();
                string newName = "";
                DialogResult result = InputBox.Show("New campaign name", "Give your campaign a name.", ref newName);
                if (result == DialogResult.OK)
                {
                    SelectedCampaign.CampaignName = newName;
                    SaveCampaign(sender, e);
                    ReloadCampaigns(sender, e);

                    int currentIndex = cboCampaign.SelectedIndex;

                    cboCampaign.DataSource = Campaigns;
                    cboCampaign.Refresh();

                    if (cboCampaign.Items.Count > currentIndex)
                        cboCampaign.SelectedIndex = currentIndex;
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SF143." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SelectForm", msg);
                
            }
        }

        private void cboCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            Account.SelectedCampaignId = ((Campaign)cboCampaign.SelectedItem).ID;
            EnabledBoldIndicator();
        }

        private void btnNewCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                Campaign c = new Campaign((Account)Account);
                c.CampaignName = "(new campaign)";
                c.ID = 0;
                Campaigns.Add(c);
                Campaigns = new List<ICampaign>(Campaigns);

                cboCampaign.DataSource = Campaigns;
                cboCampaign.Refresh();

                cboCampaign.SelectedItem = c;
            }
            catch (Exception ex)
            {
                string msg = "Error SF163." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SelectForm", msg);
                
            }
        }

        private void btnDeleteCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if ((Campaign)cboCampaign.SelectedItem == null)
                    return;

                if (MessageBox.Show("You are about to delete this campaign, are you sure?", "Delete Campaign", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes)
                    return;

                Campaigns.Remove((Campaign)cboCampaign.SelectedItem);
                RemoveCampaign(sender, e);

                Campaigns = new List<ICampaign>(Campaigns);

                cboCampaign.DataSource = Campaigns;
                cboCampaign.Refresh();


                EnabledBoldIndicator();
            }
            catch (Exception ex)
            {
                string msg = "Error SF180." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SelectForm", msg);
                
            }
        }

        private void SelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!Account.ValidCredentials)
                    Account.IsConfigured = false;
                else if (cboCampaign.SelectedItem == null || cboCampaign.Items.Count == 0)
                    Account.IsConfigured = false;
                else if (((Campaign)cboCampaign.SelectedItem).ConfigurationContainer.EnabledConfigurations().Count == 0)
                    Account.IsConfigured = false;
                else
                {
                    Account.SelectedCampaignId = ((Campaign)cboCampaign.SelectedItem).ID;
                    Account.IsConfigured = true;
                }

                Account.CheckStatus();

            }
            catch (Exception ex)
            {
                string msg = "Error SF199." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SelectForm", msg);
                
            }
        }













    }
}
