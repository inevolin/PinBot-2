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
    public partial class CommentForm : Form, IConfigureView
    {
        public event EventHandler SaveConfig;
        private CommentConfiguration _config;

        public CommentForm(bool IS_PREMIUM)
        {
            InitializeComponent();
            chkAutopilot.Checked = chkAutopilot.Enabled = IS_PREMIUM;
        }

        public void ShowForm(IConfiguration config)
        {
            try
            {


                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = (CommentConfiguration)config;

                if (_config.Queries != null) txtQry.Text = String.Join(Environment.NewLine, _config.Queries.Keys);
                if (_config.Comments != null) txtComments.Text = String.Join(Environment.NewLine, _config.Comments);
                if (_config.Timeout != null) txtTimeoutMin.Value = _config.Timeout.Min;
                if (_config.Timeout != null) txtTimeoutMax.Value = _config.Timeout.Max;
                if (_config.CurrentCount != null) txtCommentsMin.Value = _config.CurrentCount.Min;
                if (_config.CurrentCount != null) txtCommentsMax.Value = _config.CurrentCount.Max;
                if (_config.AutoStart != null) AutoStartTimeMin.Value = DateTime.Today + _config.AutoStart.Min.TimeOfDay;
                if (_config.AutoStart != null) AutoStartTimeMax.Value = DateTime.Today + _config.AutoStart.Max.TimeOfDay;
                else AutoStartTimeMax.Value = DateTime.Now.AddMinutes(15);
                chkEnable.Checked = _config.Enabled;
                chkAutopilot.Checked = _config.Autopilot;

                ShowDialog();

            }
            catch (Exception ex)
            {
                string msg = "Error CF54." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: comment form", msg);

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
                foreach (string s in txtQry.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList())
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

                List<string> comments = new List<string>();
                i = 0;
                foreach (string s in txtComments.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList())
                {
                    if (s.Trim() == "")
                    {
                        StatusLabel.SetStatus(lblStatus, true, "Invalid comment on line " + (i + 1) + ".", Color.Red);
                        return;
                    }
                    else if (s.Length <= 2)
                    {
                        StatusLabel.SetStatus(lblStatus, true, "Comment too short on line " + (i + 1) + ".", Color.Red);
                        return;
                    }
                    else
                    {
                        comments.Add(s.Trim());
                    }
                    ++i;
                }

                _config.Queries = queries;
                _config.Comments = comments;
                _config.Timeout = new Range<int>((int)txtTimeoutMin.Value, (int)txtTimeoutMax.Value);
                _config.CurrentCount = new Range<int>((int)txtCommentsMin.Value, (int)txtCommentsMax.Value);
                _config.AutoStart = new Range<DateTime>(AutoStartTimeMin.Value, AutoStartTimeMax.Value);
                _config.Autopilot = chkAutopilot.Checked;
                _config.Enabled = chkEnable.Checked;

                SaveConfig(sender, e);
                StatusLabel.SetStatus(lblStatus, true, "Saved!", Color.Green);

            }
            catch (Exception ex)
            {
                string msg = "Error RF120." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: CommentForm", msg);

            }
        }

        private bool ValidQueries()
        {
            IList<string> list = txtQry.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
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


            list = txtComments.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtComments.Text.Trim() == "" || list.Count <= 0)
            {
                StatusLabel.SetStatus(lblStatus, true, "List of comments is empty.", Color.Red);
                return false;
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

        private void txtCommentsMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtCommentsMin.Value > txtCommentsMax.Value)
                txtCommentsMin.Value = txtCommentsMax.Value;
        }

        private void txtCommentsMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtCommentsMax.Value < txtCommentsMin.Value)
                txtCommentsMax.Value = txtCommentsMin.Value;
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
            // MessageBox.Show("If you are experiencing errors/exceptions with PinBot, please disable the Autopilot option", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            grpAutopilot.Enabled = chkAutopilot.Checked;
        }

    }
}
