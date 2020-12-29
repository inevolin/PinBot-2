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
    public partial class InviteForm : Form, IConfigureView
    {
        public event EventHandler SaveConfig;
        private InviteConfiguration _config;

        public InviteForm(bool IS_PREMIUM)
        {
            InitializeComponent();
            chkAutopilot.Checked = chkAutopilot.Enabled = IS_PREMIUM;
        }

        public void ShowForm(IConfiguration config)
        {
            try
            {

                lstBoards.Items.Clear();

                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = (InviteConfiguration)config;

                chkEnable.Checked = _config.Enabled;
                chkAutopilot.Checked = _config.Autopilot;

                foreach (var kv in _config.SelectedBoards)
                {
                    lstBoards.Items.Add(kv.Key);
                    if (kv.Value == true)
                        lstBoards.SetSelected(lstBoards.Items.Count - 1, true);
                }
                lstBoards.DisplayMember = "Boardname";
                lstBoards.ValueMember = "Id";

                if (_config.Timeout != null) txtTimeoutMin.Value = _config.Timeout.Min;
                if (_config.Timeout != null) txtTimeoutMax.Value = _config.Timeout.Max;
                if (_config.CurrentCount != null) txtFollowsMin.Value = _config.CurrentCount.Min;
                if (_config.CurrentCount != null) txtFollowsMax.Value = _config.CurrentCount.Max;

                if (_config.AutoStart != null) AutoStartTimeMin.Value = DateTime.Today + _config.AutoStart.Min.TimeOfDay;
                if (_config.AutoStart != null) AutoStartTimeMax.Value = DateTime.Today + _config.AutoStart.Max.TimeOfDay;
                else AutoStartTimeMax.Value = DateTime.Now.AddMinutes(15);


                if (lstBoards.Items.Count <= 0)
                {
                    btnSave.Enabled = false;
                    StatusLabel.SetStatus(lblStatus, true, "Your account has no boards. Create some boards and click 'refresh'.", Color.Red);
                }
                else
                {
                    btnSave.Enabled = true;
                }

                ShowDialog();

            }
            catch (Exception ex)
            {
                string msg = "Error IF65." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: InviteForm", msg);
                
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidQueries())
                    return;

                int i = 0;
                foreach (var k in lstBoards.Items.Cast<Board>().ToList())
                {
                    _config.SelectedBoards[k] = lstBoards.GetSelected(i++);
                }

                _config.Timeout = new Range<int>((int)txtTimeoutMin.Value, (int)txtTimeoutMax.Value);
                _config.CurrentCount = new Range<int>((int)txtFollowsMin.Value, (int)txtFollowsMax.Value);
                _config.AutoStart = new Range<DateTime>(AutoStartTimeMin.Value,AutoStartTimeMax.Value);
                _config.Autopilot = chkAutopilot.Checked;
                _config.Enabled = chkEnable.Checked;

                SaveConfig(sender, e);
                StatusLabel.SetStatus(lblStatus, true, "Saved!", Color.Green);

            }
            catch (Exception ex)
            {
                string msg = "Error IF93." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: InviteForm", msg);
                
            }
        }

        private bool ValidQueries()
        {

            if (lstBoards.SelectedItems.Count == 0)
            {
                StatusLabel.SetStatus(lblStatus, true, "No boards selected.", Color.Red);
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

        #endregion

        private void lstBoards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBoards.SelectedItem == null)
                return;

        }

    }
}
