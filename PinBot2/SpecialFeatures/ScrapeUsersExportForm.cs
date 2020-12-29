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
using PinBot2.Model.Configurations.SpecialFeatures;
using PinBot2.Algorithms;
using PinBot2.Model;
using PinBot2.Presenter.Configurations.Interface;

namespace PinBot2
{
    public partial class ScrapeUsersExportForm : Form, ISpecialFeaturesView
    {
        private IAccount account;
        private ScrapeUsersExportConfiguration _config;
        private ScrapeUsersExportAlgo algo;

        public ScrapeUsersExportForm()
        {
            InitializeComponent();
        }

        public void ShowForm(IAccount account)
        {
            try
            {
                this.account = account;
                panel.Enabled = false;

                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = new ScrapeUsersExportConfiguration(); //or load from DB

                chkLimitScrape.Checked = _config.DoTimeout;

                if (_config.Queries != null) txtQry.Text = String.Join(Environment.NewLine, _config.Queries.Keys);
                if (_config.Timeout != null) txtTimeoutMin.Value = _config.Timeout.Min;
                if (_config.Timeout != null) txtTimeoutMax.Value = _config.Timeout.Max;
                if (_config.CurrentCount != null) txtFollowsMin.Value = _config.CurrentCount.Min;
                if (_config.CurrentCount != null) txtFollowsMax.Value = _config.CurrentCount.Max;

                //criteria
                chkCriteria.Checked = _config.UsersCriteria;
                if (_config.UserFollowers != null) txtFollowersMin.Value = _config.UserFollowers.Min;
                if (_config.UserFollowers != null) txtFollowersMax.Value = _config.UserFollowers.Max;
                if (_config.UserFollowing != null) txtFollowingMin.Value = _config.UserFollowing.Min;
                if (_config.UserFollowing != null) txtFollowingMax.Value = _config.UserFollowing.Max;
                if (_config.UserBoards != null) txtBoardsMin.Value = _config.UserBoards.Min;
                if (_config.UserBoards != null) txtBoardsMax.Value = _config.UserBoards.Max;
                if (_config.UserPins != null) txtPinsMin.Value = _config.UserPins.Min;
                if (_config.UserPins != null) txtPinsMax.Value = _config.UserPins.Max;
                if (_config.HasWebsite.HasValue) chkHasWebsite.Checked = _config.HasWebsite.Value;
                if (_config.HasCustomPic.HasValue) chkHasCustomPic.Checked = _config.HasCustomPic.Value;
                if (_config.HasAboutText.HasValue) chkHasAbout.Checked = _config.HasAboutText.Value;
                if (_config.HasFb.HasValue) chkHasFb.Checked = _config.HasFb.Value;
                if (_config.HasTw.HasValue) chkHasTw.Checked = _config.HasTw.Value;
                if (_config.HasLocation.HasValue) chkHasLocation.Checked = _config.HasLocation.Value;

                //ShowDialog();
                Show();

            }
            catch (Exception ex)
            {
                string msg = "Error SUEF67." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "SUEF", msg);

            }
        }

        private void btnScrape_Click(object sender, EventArgs e)
        {
            try
            {

                if (!CheckQueries())
                    return;
                FillStuff();
                if (!chkAppendToFile.Checked && sfdFile.CheckFileExists)
                {//let's overwrite existing content
                    using (System.IO.StreamWriter newTask = new System.IO.StreamWriter(sfdFile.FileName, false))
                    {
                        newTask.WriteLine("");
                    }
                }


                btnStop.Enabled = true;
                btnScrape.Enabled =
                btnSelectFile.Enabled =
                chkAppendToFile.Enabled =
                panel.Enabled = false;

                algo = new ScrapeUsersExportAlgo(this.account, _config, null, sfdFile.FileName, chkAppendToFile.Checked);

                Task t = Task.Factory.StartNew(() =>
                {
                    algo.Run();
                });
                StatusLabel.SetStatus(lblStatus, true, "Started...", Color.Orange);
                t.ContinueWith((tt) =>
                {
                    if (btnStop.InvokeRequired)
                        btnStop.Invoke(new Action(() =>
                        {
                            btnStop.Enabled = false;
                            btnSelectFile.Enabled =
                            chkAppendToFile.Enabled =
                            btnScrape.Enabled =
                            panel.Enabled = true;

                            StatusLabel.SetStatus(lblStatus, true, "Done!", Color.Green);
                        }));
                });
            }
            catch (Exception ex)
            {
                string msg = "Error SUEF136." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "SUEF", msg);

            }
        }
        private bool CheckQueries()
        {
            if (!ValidQueries())
                return false;
            IDictionary<string, PinterestObjectResources> queries = new Dictionary<string, PinterestObjectResources>();
            int i = 0;
            foreach (string s in txtQry.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList())
            {

                if (queries.ContainsKey(s.Trim()))
                    continue;
                else if (queries.ContainsKey(s.Trim()))
                    continue;
                else if (!s.Contains("/"))
                    queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.SearchResource));
                else if (Regex.IsMatch(s, "/[a-zA-Z0-9_\\-\\%]+/followers/"))
                    queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.UserFollowersResource));
                else if (Regex.IsMatch(s, "/[a-zA-Z0-9_\\-\\%]+/following/people/"))
                    queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.UserFollowingResource));
                else if (Regex.IsMatch(s, "/[a-zA-Z0-9_\\-\\%]+/[a-zA-Z0-9_\\-\\%]+/followers/"))
                    queries.Add(new KeyValuePair<string, PinterestObjectResources>(s.Trim(), PinterestObjectResources.BoardFollowersResource));
                else
                {
                    StatusLabel.SetStatus(lblStatus, true, "Invalid query on line " + (i + 1) + ".", Color.Red);
                    return false;
                }
                ++i;
            }

            _config.Queries = queries;
            return true;
        }
        private void FillStuff()
        {
            _config.Timeout = new Range<int>((int)txtTimeoutMin.Value, (int)txtTimeoutMax.Value);
            _config.CurrentCount = new Range<int>((int)txtFollowsMin.Value, (int)txtFollowsMax.Value);
            _config.Autopilot = chkLimitScrape.Checked;

            //users
            _config.UserFollowers = new Range<int>((int)txtFollowersMin.Value, (int)txtFollowersMax.Value);
            _config.UserFollowing = new Range<int>((int)txtFollowingMin.Value, (int)txtFollowingMax.Value);
            _config.UserBoards = new Range<int>((int)txtBoardsMin.Value, (int)txtBoardsMax.Value);
            _config.UserPins = new Range<int>((int)txtPinsMin.Value, (int)txtPinsMax.Value);
            _config.UsersCriteria = chkCriteria.Checked;
        }
        private bool ValidQueries()
        {
            IList<string> list = txtQry.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtQry.Text.Trim() == "" || list.Count <= 0)
            {
                StatusLabel.SetStatus(lblStatus, true, "List of queries is empty.", Color.Red);
                return false;
            }

            string pattern = "/[a-zA-Z0-9_\\-\\%]+/([a-zA-Z0-9_\\-\\%]+|followers|following|)/";
            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i];
                if (!Regex.IsMatch(s, pattern) && s.Contains("/"))
                {
                    StatusLabel.SetStatus(lblStatus, true, "Invalid query on line " + (i + 1) + ".", Color.Red);
                    return false;
                }
            }
            return true;
        }

        #region components

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



        private void chkLimitScrape_CheckedChanged(object sender, EventArgs e)
        {
            grpLimitScrape.Enabled = chkLimitScrape.Checked;
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
            grpCriteria.Enabled = chkCriteria.Checked = chkCriteria.Checked;
        }



        #endregion

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            panel.Enabled = false;
            sfdFile.ShowDialog();
        }

        private void sfdFile_FileOk(object sender, CancelEventArgs e)
        {
            string name = sfdFile.FileName;
            Console.WriteLine(name);

            if (sfdFile.CheckPathExists)
            {
                if (!System.IO.File.Exists(sfdFile.FileName)) System.IO.File.Create(sfdFile.FileName);
                btnScrape.Enabled = panel.Enabled = true;
            }
            else
            {
                StatusLabel.SetStatus(lblStatus, true, "File path does not exist!", Color.Red);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (algo != null)
                algo.Abort();
        }

        private void ScrapeUsersExportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (algo != null)
                algo.Abort();
        }

    }
}
