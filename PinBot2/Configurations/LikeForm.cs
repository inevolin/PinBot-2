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
    public partial class LikeForm : Form, IConfigureView
    {
        public event EventHandler SaveConfig;
        private LikeConfiguration _config;

        public LikeForm(bool IS_PREMIUM)
        {
            InitializeComponent();
            chkAutopilot.Checked = chkAutopilot.Enabled = IS_PREMIUM;
        }

        public void ShowForm(IConfiguration config)
        {
            try
            {
                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = (LikeConfiguration)config;

                if (_config.Queries != null) txtQry.Text = String.Join(Environment.NewLine, _config.Queries.Keys); else txtQry.Text = "";
                if (_config.Timeout != null) txtTimeoutMin.Value = _config.Timeout.Min;
                if (_config.Timeout != null) txtTimeoutMax.Value = _config.Timeout.Max;
                if (_config.CurrentCount != null) txtLikesMin.Value = _config.CurrentCount.Min;
                if (_config.CurrentCount != null) txtLikesMax.Value = _config.CurrentCount.Max;
                if (_config.AutoStart != null) AutoStartTimeMin.Value = DateTime.Today + _config.AutoStart.Min.TimeOfDay;
                if (_config.AutoStart != null) AutoStartTimeMax.Value = DateTime.Today + _config.AutoStart.Max.TimeOfDay;
                else AutoStartTimeMax.Value = DateTime.Now.AddMinutes(15);
                chkEnable.Checked = _config.Enabled;
                chkAutopilot.Checked = _config.Autopilot;

                ShowDialog();

            }
            catch (Exception ex)
            {
                string msg = "Error LF51." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: LikeForm", msg);
                
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (!ValidQueries())
                    return;
                IDictionary<string, PinterestObjectResources> queries = new Dictionary<string, PinterestObjectResources>();
                int i = 0;
                foreach (string s in txtQry.Text.Trim().Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList())
                {
                    if (queries.ContainsKey(s.Trim()))
                        continue;
                    else if (!s.Contains("/"))
                        queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.SearchResource));
                    else if (Regex.IsMatch(s, "/[a-zA-Z0-9_\\-\\%]+/(?!pins)([a-zA-Z0-9_\\-\\%]+)/"))
                        queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.BoardFeedResource));
                    else if (Regex.IsMatch(s, "/[a-zA-Z0-9_\\-\\%]+/(pins)/"))
                        queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.UserPinsResource));
                    else
                    {
                        StatusLabel.SetStatus(lblStatus, true, "Invalid query on line " + (i + 1) + ".", Color.Red);
                        return;
                    }
                    ++i;
                }
                _config.Queries = queries;
                _config.Timeout = new Range<int>((int)txtTimeoutMin.Value, (int)txtTimeoutMax.Value);
                _config.CurrentCount = new Range<int>((int)txtLikesMin.Value, (int)txtLikesMax.Value);
                _config.AutoStart = new Range<DateTime>(AutoStartTimeMin.Value,AutoStartTimeMax.Value);
                _config.Autopilot = chkAutopilot.Checked;
                _config.Enabled = chkEnable.Checked;

                SaveConfig(sender, e);
                StatusLabel.SetStatus(lblStatus, true, "Saved!", Color.Green);

            }
            catch (Exception ex)
            {
                string msg = "Error LF93." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: InviteForm", msg);
                
            }
        }

        private bool ValidQueries()
        {
            IList<string> list = txtQry.Text.Trim().Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtQry.Text.Trim() == "" || list.Count <= 0)
            {
                StatusLabel.SetStatus(lblStatus, true, "List of queries is empty.", Color.Red);
                return false;
            }

            string pattern = "/[a-zA-Z0-9_\\-\\%]+/([a-zA-Z0-9_\\-\\%]+|pins)/";
            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i];
                if (!s.Contains("/")) continue;
                if (!Regex.IsMatch(s, pattern))
                {
                    StatusLabel.SetStatus(lblStatus, true, "Invalid query on line " + (i + 1) + ".", Color.Red);
                    return false;
                }
            }
            return true;
        }

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

        private void txtLikesMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtLikesMin.Value > txtLikesMax.Value)
                txtLikesMin.Value = txtLikesMax.Value;
        }

        private void txtLikesMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtLikesMax.Value < txtLikesMin.Value)
                txtLikesMax.Value = txtLikesMin.Value;
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

    }
}
