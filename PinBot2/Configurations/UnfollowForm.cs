using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PinBot2.Presenter.Configurations;
using PinBot2.Model.Configurations;
using System.Text.RegularExpressions;
using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using PinBot2.Helpers;

namespace PinBot2
{
    public partial class UnfollowForm : Form, IConfigureView
    {
        public event EventHandler SaveConfig;
        private UnfollowConfiguration _config;

        public UnfollowForm(bool IS_PREMIUM)
        {
            InitializeComponent();
            chkAutopilot.Checked = chkAutopilot.Enabled = IS_PREMIUM;
        }

        public void ShowForm(IConfiguration config)
        {
            try
            {
                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = (UnfollowConfiguration)config;

                chkEnable.Checked = _config.Enabled;
                chkAutopilot.Checked = _config.Autopilot;
                chkNonFollowers.Checked = _config.UnfollowNonFollowers;

                if (_config.Timeout != null) txtTimeoutMin.Value = _config.Timeout.Min;
                if (_config.Timeout != null) txtTimeoutMax.Value = _config.Timeout.Max;
                if (_config.CurrentCount != null) txtFollowsMax.Value = _config.CurrentCount.Max;
                if (_config.CurrentCount != null) txtFollowsMin.Value = _config.CurrentCount.Min;

                if (_config.AutoStart != null) AutoStartTimeMax.Value = DateTime.Today + _config.AutoStart.Max.TimeOfDay;
                else AutoStartTimeMax.Value = DateTime.Now.AddMinutes(15);
                if (_config.AutoStart != null) AutoStartTimeMin.Value = DateTime.Today + _config.AutoStart.Min.TimeOfDay;
                

                //users
                if (_config.UserFollowers != null) txtFollowersMax.Value = _config.UserFollowers.Max;
                if (_config.UserFollowers != null) txtFollowersMin.Value = _config.UserFollowers.Min;
                if (_config.UserFollowing != null) txtFollowingMax.Value = _config.UserFollowing.Max;
                if (_config.UserFollowing != null) txtFollowingMin.Value = _config.UserFollowing.Min;
                if (_config.UserBoards != null) txtBoardsMax.Value = _config.UserBoards.Max;
                if (_config.UserBoards != null) txtBoardsMin.Value = _config.UserBoards.Min;
                if (_config.UserPins != null) txtPinsMax.Value = _config.UserPins.Max;
                if (_config.UserPins != null) txtPinsMin.Value = _config.UserPins.Min;
                
                chkCriteria.Checked = _config.UsersCriteria;
                chkFollowUsers.Checked = _config.UnfollowUsers;

                //boards
                if (_config.BoardPins != null) txtBoardPinsMax.Value = _config.BoardPins.Max;
                if (_config.BoardPins != null) txtBoardPinsMin.Value = _config.BoardPins.Min;                
                if (_config.BoardFollowers != null) txtBoardFollowersMax.Value = _config.BoardFollowers.Max;
                if (_config.BoardFollowers != null) txtBoardFollowersMin.Value = _config.BoardFollowers.Min;                
                chkFollowBoards.Checked = _config.UnfollowBoards;
                chkBoardCriteria.Checked = _config.BoardsCriteria;

                ShowDialog();

            }
            catch (Exception ex)
            {
                string msg = "Error UF73." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: UnfollowForm", msg);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TypeFollowChecked())
                    return;

                _config.Timeout = new Range<int>((int)txtTimeoutMin.Value, (int)txtTimeoutMax.Value);
                _config.CurrentCount = new Range<int>((int)txtFollowsMin.Value, (int)txtFollowsMax.Value);
                _config.AutoStart = new Range<DateTime>(AutoStartTimeMin.Value, AutoStartTimeMax.Value);
                _config.Autopilot = chkAutopilot.Checked;
                _config.Enabled = chkEnable.Checked;
                _config.UnfollowNonFollowers = chkNonFollowers.Checked;

                //users
                _config.UserFollowers = new Range<int>((int)txtFollowersMin.Value, (int)txtFollowersMax.Value);
                _config.UserFollowing = new Range<int>((int)txtFollowingMin.Value, (int)txtFollowingMax.Value);
                _config.UserBoards = new Range<int>((int)txtBoardsMin.Value, (int)txtBoardsMax.Value);
                _config.UserPins = new Range<int>((int)txtPinsMin.Value, (int)txtPinsMax.Value);
                _config.UsersCriteria = chkCriteria.Checked;
                _config.UnfollowUsers = chkFollowUsers.Checked;

                //boards
                _config.BoardPins = new Range<int>((int)txtBoardFollowersMin.Value, (int)txtBoardPinsMax.Value);
                _config.BoardFollowers = new Range<int>((int)txtBoardFollowersMin.Value, (int)txtBoardFollowersMax.Value);
                _config.UnfollowBoards = chkFollowBoards.Checked;
                _config.BoardsCriteria = chkBoardCriteria.Checked;

                SaveConfig(sender, e);
                StatusLabel.SetStatus(lblStatus, true, "Saved!", Color.Green);

            }
            catch (Exception ex)
            {
                string msg = "Error UF111." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: UnfollowForm", msg);

            }
        }

        private bool TypeFollowChecked()
        {
            if (!chkFollowBoards.Checked && !chkFollowUsers.Checked && !chkNonFollowers.Checked)
            {
                StatusLabel.SetStatus(lblStatus, true, "Check to follow users and/or boards.", Color.Red);
                return false;
            }
            return true;
        }

        #region components

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            panel.Enabled = chkEnable.Checked;
        }

        private void txtTimeoutMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtTimeoutMin.Value >= txtTimeoutMax.Value)
                txtTimeoutMin.Value = txtTimeoutMax.Value - 1;
        }

        private void txtTimeoutMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtTimeoutMax.Value <= txtTimeoutMin.Value)
                txtTimeoutMax.Value = txtTimeoutMin.Value + 1;
        }

        private void txtFollowsMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtFollowsMin.Value > txtFollowsMax.Value)
                txtFollowsMin.Value = txtFollowsMax.Value;
        }

        private void txtFollowsMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtFollowsMax.Value < txtFollowsMin.Value)
                txtFollowsMax.Value = txtFollowsMin.Value;
        }

        private void AutoStartTimeMin_ValueChanged(object sender, EventArgs e)
        {
            //if (AutoStartTimeMin.Value >= AutoStartTimeMax.Value)
            //AutoStartTimeMin.Value = AutoStartTimeMax.Value.AddMinutes(-1);
        }

        private void AutoStartTimeMax_ValueChanged(object sender, EventArgs e)
        {
            //if (AutoStartTimeMax.Value <= AutoStartTimeMin.Value)
            //AutoStartTimeMax.Value = AutoStartTimeMin.Value.AddMinutes(1);
        }

        private void chkAutopilot_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkAutopilot.Checked && _config != null)
            //MessageBox.Show("If you are experiencing errors/exceptions with PinBot, please disable the Autopilot option", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            grpAutopilot.Enabled = chkAutopilot.Checked;
        }

        private void chkNonFollowers_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This process can take a lot of time and traffic to accomplish, do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                chkNonFollowers.Checked = false;
            }
        }

        #endregion

        #region User components

        private void txtFollowersMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtFollowersMin.Value > txtFollowersMax.Value)
                txtFollowersMin.Value = txtFollowersMax.Value;
        }

        private void txtFollowersMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtFollowersMax.Value < txtFollowersMin.Value)
                txtFollowersMax.Value = txtFollowersMin.Value;

        }

        private void txtFollowingMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtFollowingMin.Value > txtFollowingMax.Value)
                txtFollowingMin.Value = txtFollowingMax.Value;
        }

        private void txtFollowingMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtFollowingMax.Value < txtFollowingMin.Value)
                txtFollowingMax.Value = txtFollowingMin.Value;

        }

        private void txtBoardsMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtBoardsMin.Value > txtBoardsMax.Value)
                txtBoardsMin.Value = txtBoardsMax.Value;

        }

        private void txtBoardsMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtBoardsMax.Value < txtBoardsMin.Value)
                txtBoardsMax.Value = txtBoardsMin.Value;

        }

        private void txtPinsMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtPinsMin.Value > txtPinsMax.Value)
                txtPinsMin.Value = txtPinsMax.Value;

        }

        private void txtPinsMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtPinsMax.Value < txtPinsMin.Value)
                txtPinsMax.Value = txtPinsMin.Value;

        }

        private void chkCriteria_CheckedChanged(object sender, EventArgs e)
        {
            grpCriteria.Enabled = chkCriteria.Checked;
        }

        private void chkFollowUsers_CheckedChanged(object sender, EventArgs e)
        {
            chkCriteria.Enabled = chkFollowUsers.Checked;
        }

        #endregion

        #region Board components

        private void chkBoardCriteria_CheckedChanged(object sender, EventArgs e)
        {
            grpBoardCriteria.Enabled = chkBoardCriteria.Checked;
        }

        private void chkFollowBoards_CheckedChanged(object sender, EventArgs e)
        {
            chkBoardCriteria.Enabled = chkFollowBoards.Checked;
        }

        private void txtBoardFollowersMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtBoardFollowersMin.Value > txtBoardFollowersMax.Value)
                txtBoardFollowersMin.Value = txtBoardFollowersMax.Value;
        }

        private void txtBoardFollowersMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtBoardFollowersMax.Value < txtBoardFollowersMin.Value)
                txtBoardFollowersMax.Value = txtBoardFollowersMin.Value;
        }

        private void txtBoardPinsMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtBoardPinsMin.Value > txtBoardPinsMax.Value)
                txtBoardPinsMin.Value = txtBoardPinsMax.Value;
        }

        private void txtBoardPinsMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtBoardPinsMax.Value < txtBoardPinsMin.Value)
                txtBoardPinsMax.Value = txtBoardPinsMin.Value;
        }

        #endregion





    }
}
