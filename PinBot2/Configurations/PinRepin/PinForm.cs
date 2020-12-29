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
    public partial class PinForm : Form, IConfigureQueueView
    {

        public event EventHandler Queue;
        public event EventHandler SaveConfig;
        public IPinRepinConfiguration _config { get; set; }
        private IDictionary<Board, IList<string>> allQueries;
        private List<string> SourceUrls;

        public PinForm(bool IS_PREMIUM)
        {
            InitializeComponent();
            chkAutopilot.Checked = chkAutopilot.Enabled = IS_PREMIUM;
        }

        public void ShowForm(IConfiguration config)
        {
            try
            {

                SourceUrls = new List<string>();
                allQueries = new Dictionary<Board, IList<string>>();
                txtQry.Clear();
                txtSourceUrls.Clear();

                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = (PinConfiguration)config;

                lstBoards.Items.Clear();
                if (_config.AllQueries != null)
                {
                    foreach (Board b in _config.AllQueries.Keys)
                    {
                        lstBoards.Items.Add(b);
                        if (_config.AllQueries[b] != null)
                            if (_config.AllQueries[b].Keys.Count > 0)
                                allQueries.Add(b, _config.AllQueries[b].Keys.ToList<string>());
                    }
                    lstBoards.DisplayMember = "Boardname";
                    lstBoards.ValueMember = "Id";
                }

                numSourceUrlRate.Value = ((PinConfiguration)_config).SourceUrlRate;
                if (((PinConfiguration)_config).SourceUrls != null)
                {
                    foreach (var s in ((PinConfiguration)_config).SourceUrls)
                    {
                        txtSourceUrls.Text += s + Environment.NewLine;
                    }
                }

                chkEnable.Checked = _config.Enabled;
                chkAutopilot.Checked = _config.Autopilot;

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
                string msg = "Error PF77." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: PinForm", msg);

            }
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
            ////if (AutoStartTimeMin.Value >= AutoStartTimeMax.Value)
            //    //AutoStartTimeMin.Value = AutoStartTimeMax.Value.AddMinutes(-1);
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

        private bool saved = false;
        private bool PreventNoBoard()
        {
            Board b = ((Board)lstBoards.SelectedItem);
            if (b == null)
            { StatusLabel.SetStatus(lblStatus, true, "Please select a board.", Color.Red); return false; }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                saved = false;
                SaveQueries();
                SaveSourceUrls();
                //we are not validating queries because the user  can import images, so it's optional
                //if (!ValidQueries() || !ValidSourceUrls())
                //    return;
                if (!ValidSourceUrls())
                    return;

                AssignQueries();

                ((PinConfiguration)_config).SourceUrlRate = (int)numSourceUrlRate.Value;
                ((PinConfiguration)_config).SourceUrls = SourceUrls;
                _config.Timeout = new Range<int>((int)txtTimeoutMin.Value, (int)txtTimeoutMax.Value);
                _config.CurrentCount = new Range<int>((int)txtFollowsMin.Value, (int)txtFollowsMax.Value);
                _config.AutoStart = new Range<DateTime>(AutoStartTimeMin.Value, AutoStartTimeMax.Value);
                _config.Autopilot = chkAutopilot.Checked;
                _config.Enabled = chkEnable.Checked;

                SaveConfig(sender, e);
                StatusLabel.SetStatus(lblStatus, true, "Saved!", Color.Green);
                saved = true;

            }
            catch (Exception ex)
            {
                string msg = "Error PF183." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: PinForm", msg);

            }
        }
        private void AssignQueries()
        {
            foreach (var v in _config.AllQueries.Values)
                if (v != null)
                    v.Clear();

            foreach (Board b in allQueries.Keys)
            {
                int i = 0;
                IDictionary<string, PinterestObjectResources> DIC = new Dictionary<string, PinterestObjectResources>();
                foreach (string s in allQueries[b])
                {
                    KeyValuePair<string, PinterestObjectResources>? kv = null;

                    if (DIC.ContainsKey(s))
                        continue;
                    else if (!s.Contains("/"))
                        kv = new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.External);

                    if (kv.HasValue)
                    {
                        DIC.Add(kv.Value);
                    }
                    ++i;
                }

                if (DIC.Count > 0)
                    _config.AllQueries[b] = DIC;
            }
        }

        private void SaveQueries()
        {
            try
            {

                Board b = ((Board)lstBoards.SelectedItem);
                if (b == null) return;

                if (txtQry.Text == "")
                {
                    if (allQueries.ContainsKey(b))
                        allQueries.Remove(b);
                    return;
                }
                List<string> queries = txtQry.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!allQueries.ContainsKey(b))
                    allQueries.Add(b, queries);
                else
                    allQueries[b] = queries;

            }
            catch (Exception ex)
            {
                string msg = "Error PF213." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: PinForm", msg);

            }
        }
        private void SaveSourceUrls()
        {
            try
            {
                SourceUrls = txtSourceUrls.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception ex)
            {
                string msg = "Error PF240." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: PinForm", msg);

            }
        }

        private bool ValidQueries()
        {

            if (!allQueries.Values.Any(x => x != null && x.Count > 0))
            {
                StatusLabel.SetStatus(lblStatus, true, "List of queries is empty.", Color.Red);
                return false;
            }

            return true;
        }
        private bool ValidSourceUrls()
        {
            int i = 1;
            foreach (var s in SourceUrls)
            {
                if (!http.ValidUrl(s))
                {
                    StatusLabel.SetStatus(lblStatus, true, "Invalid URL on line " + i + ".", Color.Red);
                    return false;
                }
                ++i;
            }

            return true;
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            try
            {

                btnSave.PerformClick();
                if (saved)
                    Queue(sender, e);

            }
            catch (Exception ex)
            {
                string msg = "Error PF243." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: PinForm", msg);

            }
        }


        private void lstBoards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBoards.SelectedItem == null)
                return;

            Board b = (Board)lstBoards.SelectedItem;

            if (allQueries.Count == 0 || !allQueries.ContainsKey(b))
            {
                txtQry.Text = "";
                return;
            }

            IList<string> list = allQueries[b];
            if (!(list == null || list.Count == 0))
                txtQry.Text = String.Join(Environment.NewLine, list);
        }

        private void txtQry_TextChanged(object sender, EventArgs e)
        {
            if (!PreventNoBoard()) return;
            SaveQueries();
        }
        private void txtSourceUrls_TextChanged(object sender, EventArgs e)
        {
            SaveSourceUrls();
        }
    }
}
