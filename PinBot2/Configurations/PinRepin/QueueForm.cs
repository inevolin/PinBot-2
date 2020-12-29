using PinBot2.Algorithms;
using PinBot2.Algorithms.Helpers;
using PinBot2.Common;
using PinBot2.Configurations.PinRepin;
using PinBot2.Helpers;
using PinBot2.Model;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using PinBot2.Presenter.Configurations;
using PinBot2.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinBot2.Configurations.PinForms
{


    public partial class QueueForm : Form, IConfigureQueueScrapeView
    {
        public event EventHandler SaveConfig;
        public event EventHandler<ScrapingEventArgs> Scrape;
        private bool saved = false;

        private IPinRepinConfiguration _config;


        public QueueForm()
        {
            InitializeComponent();
        }
        public void ShowForm(IConfiguration config)
        {
            try
            {
                CurrentPage = 1;
                StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
                _config = (IPinRepinConfiguration)config;

                cboBoards.Items.Clear();
                cboBoards.Items.Add("Scrape for a random mapped board");
                if (_config.AllQueries != null)
                {
                    foreach (var kv in _config.AllQueries)
                    {
                        if (kv.Value != null && kv.Value.Count > 0)
                            cboBoards.Items.Add(kv.Key);
                    }
                    cboBoards.DisplayMember = "Boardname";
                    cboBoards.ValueMember = "Id";
                }
                cboBoards.SelectedIndex = 0;

                //Pin: because mapping is optional
                txtScrapeMax.Enabled =
                txtScrapeMin.Enabled =
                cboBoards.Enabled =
                btnScrape.Enabled = (_config.AllQueries.Count(x => x.Value != null && x.Value.Count > 0) > 0);


                grpQueue.Controls.Clear();

                if (_config.Scrape == null)
                {
                    _config.Scrape = new Range<int>((int)txtScrapeMin.Value, (int)txtScrapeMax.Value);
                }
                else
                {
                    txtScrapeMin.Value = _config.Scrape.Min;
                    txtScrapeMax.Value = _config.Scrape.Max;
                }
                if (_config.Queue == null)
                    _config.Queue = new Dictionary<PinterestObject, Board>();

                btnImport.Visible = btnImport.Enabled = config.GetType().Equals(typeof(PinConfiguration));
                SetPageCountMaxLabel();
                LoadPins();
                ButtonHandling();


                ShowDialog();
                _config.Scrape = new Range<int>((int)txtScrapeMin.Value, (int)txtScrapeMax.Value);
                SaveConfig(null, null);

            }
            catch (Exception ex)
            {
                string msg = "Error QF82." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: QueueForm", msg);

            }
        }

        private int CurrentPage = 1;
        private const int MAX_PER_PAGE = 9;
        private void LoadPins()
        {
            try
            {
                grpQueue.Controls.Clear();
                if (_config.Queue == null || _config.Queue.Count == 0)
                    return;
                for (int i = 0; i < MAX_PER_PAGE; i++)
                {
                    var index = (CurrentPage - 1) * MAX_PER_PAGE + i;
                    if (index >= _config.Queue.Count)
                        break;
                    var kv = _config.Queue.ElementAt(index);
                    Console.WriteLine("index: " + index);

                    QueueItem itm = new QueueItem(_config, kv.Value, kv.Key, this);
                    if (!grpQueue.Controls.Cast<QueueItem>().Any(x => x.UID == itm.UID))
                    {
                        grpQueue.Controls.Add(itm);
                        grpQueue.Controls.SetChildIndex(itm, 0);
                    }
                }
                SetPageCountMaxLabel();
                ButtonHandling();
            }
            catch (Exception ex)
            {
                string msg = "Error QF108." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: QueueForm", msg);

            }
        }
        private void SetPageCountMaxLabel()
        {
            lblTotalPages.Text = "/" + PageCount().ToString();
        }
        private void ButtonHandling()
        {
            btnPageNext.Enabled = btnPagePrev.Enabled = txtCurrentPage.Enabled = !(PageCount() == 1);
            btnPageNext.Enabled = !(CurrentPage == PageCount());
            btnPagePrev.Enabled = !(CurrentPage == 1);
        }

        private bool SavePins()
        {

            if (_config.Queue == null || _config.Queue.Count == 0)
                return true;

            var backup = _config.Queue;

            try
            {
                var newdic = new Dictionary<PinterestObject, Board>();
                foreach (Control c in grpQueue.Controls)
                {
                    if (c.GetType() != typeof(QueueItem))
                        continue;

                    QueueItem q = (QueueItem)c;
                    var temp = new KeyValuePair<PinterestObject, Board>(q.GetPinterestObject, q.GetBoard);
                    newdic.Add(temp.Key, temp.Value);

                    _config.Queue[temp.Key] = temp.Value;
                }
                //_config.Queue = newdic;
            }
            catch (Exception ex)
            {
                SetMessage("Something went wrong saving.", "red");
                _config.Queue = backup;
                string msg = "Error QF160." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: QueueForm", msg);

                return false;
            }
            return true;
        }

        private void btnScrape_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            grpQueue.Enabled = cboBoards.Enabled = txtScrapeMax.Enabled = txtScrapeMin.Enabled = btnClear.Enabled = btnSave.Enabled = btnScrape.Enabled = false;

            ScrapingEventArgs ee = new ScrapingEventArgs();
            ee.InQueue = _config.Queue.Keys.Cast<PinterestObject>().ToList();
            if (cboBoards.SelectedIndex == 0)
                ee.SelectedBoard = null; //scrape foreach board
            else
                ee.SelectedBoard = (Board)cboBoards.SelectedItem;

            if (txtScrapeMin.Value > 0 && txtScrapeMax.Value >= txtScrapeMin.Value)
            {
                _config.Scrape.Min = (int)txtScrapeMin.Value;
                _config.Scrape.Max = (int)txtScrapeMax.Value;
            }
            else
            {
                txtScrapeMin.Value = _config.Scrape.Min = 1;
                txtScrapeMax.Value = _config.Scrape.Max = 10;
            }

            Logging.Log("user", "queue action", "scrape clicked");
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        Scrape(sender, ee);
                        LoadPins();
                        saved = false;
                    });

                }
                catch (Exception ex)
                {
                    string msg = "Error QF119." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log("user", "account action: QueueForm", msg);

                }
                finally
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Cursor = Cursors.Arrow;
                        grpQueue.Enabled = cboBoards.Enabled = txtScrapeMax.Enabled = txtScrapeMin.Enabled = btnClear.Enabled = btnSave.Enabled = btnScrape.Enabled = true;
                    });
                }
            });
        }

        private void txtScrapeMin_ValueChanged(object sender, EventArgs e)
        {
            if (txtScrapeMin.Value >= txtScrapeMax.Value)
                txtScrapeMin.Value = txtScrapeMax.Value - 1;
        }

        private void txtScrapeMax_ValueChanged(object sender, EventArgs e)
        {
            if (txtScrapeMax.Value <= txtScrapeMin.Value)
                txtScrapeMax.Value = txtScrapeMin.Value + 1;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure?", "Clear queue", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                    return;

                grpQueue.Controls.Clear();
                _config.Queue.Clear();
                saved = false;
                ButtonHandling();
                SetPageCountMaxLabel();
            }
            catch (Exception ex)
            {
                string msg = "Error QF170." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: QueueForm", msg);

            }
        }

        public void SetMessage(string msg, string color)
        {
            lblStatus.Text = msg;
            switch (color)
            {
                case "red":
                    lblStatus.ForeColor = Color.Red;
                    break;

                case "orange":
                    lblStatus.ForeColor = Color.Orange;
                    break;

                case "green":
                    lblStatus.ForeColor = Color.Green;
                    break;
            }

        }

        public void DeleteQueueItem(QueueItem itm)
        {
            grpQueue.Controls.Remove(itm);
            saved = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SavePins())
            {
                SaveConfig(sender, e);
                saved = true;
                SetMessage("Saved!", "green");
            }
        }

        private void QueueForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved && grpQueue.Controls.Count > 0)
            {
                if (MessageBox.Show("Would you like to save before closing?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (SavePins())
                    {
                        SaveConfig(sender, e);
                    }
                    else
                    {
                        SetMessage("Something went wrong saving.", "red");
                        e.Cancel = true;
                    }
                }
            }
        }

        private int PageCount()
        {
            if (_config == null || _config.Queue == null || _config.Queue.Count == 0)
                return 1;
            else
                return (int)(Math.Ceiling((decimal)_config.Queue.Count / (decimal)MAX_PER_PAGE));
        }
        private void txtCurrentPage_TextChanged(object sender, EventArgs e)
        {
            if (txtCurrentPage.Text == "") return;
            StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
            txtCurrentPage.TextChanged -= txtCurrentPage_TextChanged;
            int n;
            bool isNumeric = int.TryParse(txtCurrentPage.Text, out n);
            if (isNumeric)
            {
                int totalPages = PageCount();
                if (totalPages < n)
                    n = totalPages;
                CurrentPage = n;
            }
            else
            {
                CurrentPage = 1;
                txtCurrentPage.Text = CurrentPage.ToString();
            }
            txtCurrentPage.TextChanged += txtCurrentPage_TextChanged;
            SavePins();
            LoadPins();
        }
        private void btnPagePrev_Click(object sender, EventArgs e)
        {
            StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
            txtCurrentPage.TextChanged -= txtCurrentPage_TextChanged;
            if (CurrentPage > 1)
            {
                CurrentPage--;
                txtCurrentPage.Text = CurrentPage.ToString();
            }
            else
            {
                CurrentPage = 1;
                txtCurrentPage.Text = CurrentPage.ToString();
            }
            txtCurrentPage.TextChanged += txtCurrentPage_TextChanged;
            SavePins();
            LoadPins();
        }
        private void btnPageNext_Click(object sender, EventArgs e)
        {
            StatusLabel.SetStatus(lblStatus, true, "", Color.Black);
            txtCurrentPage.TextChanged -= txtCurrentPage_TextChanged;
            if (CurrentPage < PageCount())
            {
                CurrentPage++;
                txtCurrentPage.Text = CurrentPage.ToString();
            }
            else
            {
                CurrentPage = PageCount();
                txtCurrentPage.Text = CurrentPage.ToString();
            }
            txtCurrentPage.TextChanged += txtCurrentPage_TextChanged;
            SavePins();
            LoadPins();

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                BoardSelector bs = new BoardSelector(_config.AllQueries.Keys.Cast<Board>().ToList());
                bs.ShowDialog();
                if (bs.Ok)
                {
                    files.Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.gif, *.png) | *.jpg; *.jpeg; *.bmp; *.gif; *.png";
                    files.ShowDialog();

                    foreach (string path in files.FileNames)
                    {
                        ExternalPin p = new ExternalPin("", path, null, PinterestObjectResources.External);
                        if (_config.GetType().Equals(typeof(PinConfiguration))) {
                        var url = QueueHelper.GetRandomSourceUrl((PinConfiguration)_config);
                        if (url != null && url != "")
                            p.Link = url;
                        }
                        if (_config.Queue == null)
                            _config.Queue = new Dictionary<PinterestObject, Board>();
                        _config.Queue.Add(p, bs.SelectedBoard);//(Board)_config.AllQueries.Keys.ElementAt(0));
                    }
                    LoadPins();
                }
            }
            catch (Exception ex)
            {
                string msg = "Error QFRM404." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", this.GetType().ToString(), msg);
                MessageBox.Show("Error occured: #ImportImages", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
