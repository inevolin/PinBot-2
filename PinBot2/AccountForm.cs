using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using PinBot2.Algorithms;
using System.Threading.Tasks;
using System.Threading;
using PinBot2.Common;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using PinBot2.Presenter.Helper;

namespace PinBot2
{
    public partial class AccountForm : Form, IAccountView
    {

        /*
            TextWriterTraceListener myListener = new TextWriterTraceListener("TextWriterOutput.log", "myListener");
            myListener.WriteLine("Test message.");
            myListener.Flush(); 
         
         * */

        public event EventHandler AddAccount;
        public event EventHandler EditAccount;
        public event EventHandler DeleteAccount;
        public event EventHandler Run, RunAll;
        public event EventHandler Stop, StopAll;
        //public event EventHandler SaveAcounts;
        public event EventHandler Configure;
        public event EventHandler RefreshAccountInformation;
        public event SpecialFeaturesEventHandler ScrapeUsersExportShow;

        private IList<IAccount> AlreadyStarted;
        public bool IsAlreadyStarted(IAccount a) { return AlreadyStarted.Contains((IAccount)a); }
        public void AccountStarted(IAccount a) { AlreadyStarted.Add(a); }
        public void AccountStopped(IAccount a) { AlreadyStarted.Remove(a); }
        public void AbortAllAccounts() { AlreadyStarted.Clear(); }

        private bool IS_PREMIUM;

        public AccountForm(bool IS_PREMIUM)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IS_PREMIUM = IS_PREMIUM;
            InitializeComponent();
            this.Text = "PinBot" + (IS_PREMIUM ? " PREMIUM" : " TRIAL") + "  " + Application.ProductVersion.ToString();

            AlreadyStarted = new List<IAccount>();

            if (!IS_PREMIUM)
                HideTrialCols();

            btnAccountInformationRefresh.Enabled =
            btnStop.Enabled =
            btnStopAll.Enabled =
            btnRunAll.Enabled =
            btnRunSelected.Enabled =
            btnConfigure.Enabled =
            btnDelete.Enabled =
            btnEdit.Enabled = false;

            Mapper.bgWorker = this.bgWorker;

            
        }
        public void ShowForm()
        {
            Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
            NewVersionHandler();
            startTick();
            Application.Run(this);
        }
        private void HideTrialCols()
        {
            dataGridView1.Columns["Comment"].Visible =
            dataGridView1.Columns["Pin"].Visible =
            dataGridView1.Columns["Unfollow"].Visible =
            dataGridView1.Columns["Invite"].Visible =
            dataGridView1.Columns["Like"].Visible = false;
        }

        private bool DGV_Refresh_Required;
        public void UpdateDGV()
        {
            DGV_Refresh_Required = true;
        }
        
        private void UpdateGrid()
        {
            var selected = GetSelectedRow();

            //MapperDataSource.UpdateDataSource();
            //dataGridView1.Refresh();

            if (selected != null)
            {
                if (selected.Index >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.CurrentCell = dataGridView1.Rows[selected.Index].Cells[0];
                    dataGridView1.Rows[selected.Index].Selected = true;
                }
            }
        }
        public void LoadAccounts(IList<IAccount> accounts = null)
        {
            try
            {
                var s_a = GetSelectedRow();
                var selected = s_a == null ? -1 : s_a.Index;

                dataGridView1.AutoGenerateColumns = false;
                MapperDataSource.DataSource(accounts);
                dataGridView1.DataSource = MapperDataSource.DATASOURCE;
                //dataGridView1.Refresh();

                dataGridView1.Columns["Username"].DataPropertyName = "Username";
                dataGridView1.Columns["Email"].DataPropertyName = "Email";
                dataGridView1.Columns["Status"].DataPropertyName = "Status";
                dataGridView1.Columns["Like"].DataPropertyName = "LikeStatus";
                dataGridView1.Columns["Follow"].DataPropertyName = "FollowStatus";
                dataGridView1.Columns["Unfollow"].DataPropertyName = "UnfollowStatus";
                dataGridView1.Columns["Repin"].DataPropertyName = "RepinStatus";
                dataGridView1.Columns["Invite"].DataPropertyName = "InviteStatus";
                dataGridView1.Columns["Comment"].DataPropertyName = "CommentStatus";
                dataGridView1.Columns["Pin"].DataPropertyName = "PinStatus";

                if (selected >= 0)
                {
                    if (selected >= 0)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.CurrentCell = dataGridView1.Rows[selected].Cells[0];
                        dataGridView1.Rows[selected].Selected = true;
                    }
                }

                btnAccountInformationRefresh.Enabled =
                btnStop.Enabled =
                btnStopAll.Enabled =
                btnRunAll.Enabled =
                btnRunSelected.Enabled =
                btnConfigure.Enabled =
                btnDelete.Enabled =
                btnEdit.Enabled = (MapperDataSource.DATASOURCE.Count > 0);

                CheckSelectedAccount();

            }
            catch
            {
                Console.WriteLine("ERROR AF130");
            }

        }
        public IAccount GetAccount()
        {
            return dataGridView1.SelectedRows.Count <= 0 ? null : ((MapperDataSource)GetSelectedRow().DataBoundItem).Account;
        }
        private DataGridViewRow GetSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count > 0)
                return dataGridView1.SelectedRows[0];
            else
                return null;
        }
        public IEnumerable<IAccount> GetSelectedAccounts()
        {
            IList<IAccount> list = new List<IAccount>();
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                list.Add(((MapperDataSource)row.DataBoundItem).Account);
            }
            return list;
        }
        public IEnumerable<IAccount> GetAllAccounts()
        {
            IList<IAccount> list = new List<IAccount>();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                list.Add(((MapperDataSource)row.DataBoundItem).Account);
            }
            return list;
        }

        public bool IsRunningAnyFeature(IAccount account)
        {
            return AlreadyStarted.Contains(account);
        }
        private void CheckSelectedAccount()
        {
            /*
             * Check if the current account is already doing some work, in that case disable some buttons.
             */
            IAccount a = GetAccount();
            if (a != null)
            {
                bool anyF = IsRunningAnyFeature(a);//Mapper.IsRunningAnyFeature(a);
                btnConfigure.Enabled = btnRunSelected.Enabled = !anyF;
                btnStop.Enabled = anyF;
                btnRunSelected.Enabled = !anyF && a.IsConfigured;

                accountInfo.followers = a.AccountInfo.Followers;
                accountInfo.following = a.AccountInfo.Following;
                accountInfo.likes = a.AccountInfo.Likes;
                accountInfo.pins = a.AccountInfo.Pins;
                accountInfo.boards = a.AccountInfo.Boards;
                accountInfo.update();
            }
        }



        #region UI_Actions
        private void RemoveAccountButtonClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to delete this account, are you sure?", "Delete Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                DeleteAccount(sender, e);
        }
        private void RunAccountsButtonClick(object sender, EventArgs e)
        {
            Run(sender, e);
            CheckSelectedAccount();
        }
        private void EditAccountButtonClick(object sender, EventArgs e)
        {
            EditAccount(sender, e);
        }
        private void AddAccountButtonClick(object sender, EventArgs e)
        {
            if (!IS_PREMIUM && dataGridView1.Rows.Count >= 2)
            {
                MessageBox.Show("Limit of accounts reached. Upgrade to Premium to add more accounts.", "Trial limit", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }
            AddAccount(sender, e);
        }
        private void btnConfigure_Click(object sender, EventArgs e)
        {
            Configure(sender, e);
        }
        private void btnStopAll_Click(object sender, EventArgs e)
        {
            StopAll(sender, e);
        }
        private void btnRunAll_Click(object sender, EventArgs e)
        {
            RunAll(sender, e);
            CheckSelectedAccount();
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            CheckSelectedAccount();
        }
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[currentMouseOverRow].Selected = true;
                    ContextMenu m = new ContextMenu();
                    var n = new MenuItem("Scrape usernames and export" + (IS_PREMIUM ? "" : " (Premium only)"));
                    n.Enabled = IS_PREMIUM;
                    n.Click += mnuClickScrapeUsersExport;
                    m.MenuItems.Add(n);
                    m.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));
                }
            }
        }
        private void mnuClickScrapeUsersExport(object sender, EventArgs e)
        {
            var ee = new SpecialFeaturesEventArgs(new ScrapeUsersExportForm());
            ScrapeUsersExportShow(sender, ee);
           
        }
        private void btnAccountInformationRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var context = TaskScheduler.FromCurrentSynchronizationContext();
                btnAccountInformationRefresh.Enabled = false;
                Task t;
                t = Task.Factory.StartNew(() =>
                {
                    RefreshAccountInformation(sender, e);
                })
                .ContinueWith((prev_task) =>
                {
                    btnAccountInformationRefresh.Enabled = true;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, context);
            }
            catch (Exception ex)
            {
                string msg = "Error ACFORM184." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: AccountForm", msg);
                
                btnAccountInformationRefresh.Enabled = true;
            }

        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop(sender, e);
            CheckSelectedAccount();
        }
        private void AccountForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(1);
            /*
            btnStopAll_Click(sender, e);
            Application.Exit();*/
        }
        #endregion

        #region newUpdateCheck
        private void NewVersionHandler()
        {
            statusStrip.Visible = false;
            CheckNewVersion();
            newVersionTmr.Start();
        }
        private void newVersionTmr_Tick(object sender, EventArgs e)
        {
            CheckNewVersion();
        }
        private void CheckNewVersion()
        {
            var request = new http();
            try
            {
                string pv =
                    FileVersionInfo.GetVersionInfo(
                        AppDomain.CurrentDomain.BaseDirectory
                        + "PinBot2.exe"
                    ).ProductVersion;

                string cpv =
                    request.GET(
                    "https://healzer.com/pinbot/update2.txt",
                    "",
                    new System.Net.CookieContainer(),
                    null,
                    null
                );

                if (cpv.Contains("[[[ERROR"))
                    return;


                if (int.Parse(cpv.Replace(".", "")) > int.Parse(pv.Replace(".", "")))
                {
                    statusStrip.Visible = true;

                    return;
                }
            }
            catch
            { }
        }

        private void lblNewVersion_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("PinBot will now close and run the update tool, would you like to continue?", "Update PinBot", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    btnStopAll.PerformClick();
                    Thread.Sleep(1000);
                    Process.Start(Path.Combine(Environment.CurrentDirectory, "Updater2.exe"));
                    Application.Exit();
                }
            }
            catch { }

        }

        private void lblNewVersion_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void lblNewVersion_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        #endregion



        private void startTick()
        {
            Task.Factory.StartNew(() => {
                while (1==1)
                {
                    doTick();
                    Thread.Sleep(2500);
                }
            });
        }
        private void doTick() {
            try
            {
                if (DGV_Refresh_Required && this.InvokeRequired)
                {
                    dataGridView1.Invoke((MethodInvoker)(() => dataGridView1.Refresh()));
                    DGV_Refresh_Required = false;
                }
                FillData();
            }
            catch { }
        }
        private void FillData()
        {
            this.Invoke((MethodInvoker)delegate
            {
                UpdateGrid();
                //SaveAcounts(null, null);
                //CheckSelectedAccount();
            });
        }


        

    }
}
