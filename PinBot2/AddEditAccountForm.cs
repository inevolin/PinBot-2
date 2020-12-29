using System;
using System.Windows.Forms;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.Drawing;
using PinBot2.Common;
using System.Threading;

namespace PinBot2
{
    public partial class AddEditAccountForm : Form, IAddEditAccountView
    {
        private http request; //for making calls, and to abort httpwebrequest when form closes.
        private bool IS_PREMIUM;
        public IAccount _Account { get; set; }
        public event PinBot2.Presenter.Helpers.CustomEventHandler SaveAccount;

        public AddEditAccountForm()
        {
            InitializeComponent();
        }

        public void ShowForm(IAccount account, bool IS_PREMIUM)
        {
            lblStatus.Text = "";
            btnCancel.Visible = false;
            btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;

            this.IS_PREMIUM = IS_PREMIUM;
            txtProxyUser.Text = txtProxyPass.Text = txtProxy.Text = txtProxyPass.Text = txtProxyUser.Text = "";
            if (account == null)
            {
                MessageBox.Show("Please select an account to edit.", "No account selected", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            _Account = account;
            txtEmail.Text = account.Email;
            txtPass.Text = account.Password;
            lblStatus.Text = "";

            if (IS_PREMIUM)
            {
                if (account.WebProxy != null)
                {
                    txtProxy.Text = account.WebProxy.Ip + ":" + account.WebProxy.Port;
                    txtProxyUser.Text = account.WebProxy.User;
                    txtProxyPass.Text = account.WebProxy.Pass;

                }
            }
            else
            {
                grpProxy.Enabled = false;
            }
            ShowDialog();
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            btnCancel.Visible = true;
            btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = false;

            if (request == null)
                request = new http();

            _Account.Email = txtEmail.Text;
            _Account.Password = txtPass.Text;
            _Account.ValidCredentials = false;

            if (_Account.Password.Length <= 1)
            {
                _Account.Status = Account.STATUS.WRONG_EMAIL_OR_PASS;
                SetStatus(true, "Enter your password.", Color.Red);

                btnCancel.Visible = false;
                btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;


                return;
            }
            else if (!Regex.IsMatch(_Account.Email, "[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\\.[a-zA-Z]{2,4}"))
            {
                _Account.Status = Account.STATUS.WRONG_EMAIL_OR_PASS;
                SetStatus(true, "Invalid email format.", Color.Red);

                btnCancel.Visible = false;
                btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;

                return;
            }

            string proxy = txtProxy.Text.Trim();

            Task t;
            t = Task.Factory.StartNew(() =>
            {
                try
                {
                    if (IS_PREMIUM && proxy.Length > 0)
                    {
                        SetStatus(true, "Testing proxy...", Color.Orange);
                        if (request == null)
                            return;
                        
                        bool workingProxy = TestProxy(proxy);
                        if (request == null)
                            return;
                    }
                    else
                    {
                        _Account.ValidProxy = true;
                        _Account.WebProxy = null;
                    }

                    
                    SetStatus(true, "Testing account, please wait...", Color.Orange);


                    PinBot2.Presenter.Helpers.CustomEventArgs ce = new PinBot2.Presenter.Helpers.CustomEventArgs(true);
                    SaveAccount(sender, ce);
                    if (ce.Var == false)
                    {
                        SetStatus(true, "Account already exists!", Color.Red);
                        this.Invoke((MethodInvoker)delegate
                        {
                            btnCancel.Visible = false;
                            btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;
                        });
                        return;
                    }

                    if (request == null)
                        return;
                    _Account.LoginSync(true, request);
                    if (request == null)
                        return;

                    if (!_Account.IsLoggedIn)
                    {
                        SetStatus(true, "Account does not work!", Color.Red);
                        this.Invoke((MethodInvoker)delegate
                        {
                            btnCancel.Visible = false;
                            btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;
                        });
                    }
                    else
                    {
                        _Account.ValidCredentials = true;
                        _Account.CheckStatus();

                        SaveAccount(sender, ce);
                        SetStatus(true, "Account saved...", Color.Green);

                        Thread.Sleep(1000);
                        btnSave.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error AEAF139." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log("user", "account action: addEditAccountForm", msg);
                    

                    this.Invoke((MethodInvoker)delegate
                    {
                        btnCancel.Visible = false;
                        btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;
                    });
                }

            });



        }

        private bool TestProxy(string strProxy)
        {

            string strIPPattern = "((\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3}))";
            string strPortPattern = ":(\\d{2,5})";
            string strProxyPattern = strIPPattern + strPortPattern;

            if (!Regex.IsMatch(strProxy, strProxyPattern))
            {
                SetStatus(true, "Invalid proxy format.", Color.Red);
                _Account.ValidProxy = false;
                return false;
            }

            string strIP = Regex.Match(strProxy, strIPPattern).Groups[1].Value;
            string strPort = Regex.Match(strProxy, strPortPattern).Groups[1].Value;

            string strUser = txtProxyUser.Text.Trim();
            string strPass = txtProxyPass.Text.Trim();

            Proxy proxy = new Proxy(strIP, int.Parse(strPort), strUser.Trim(), strPass.Trim());
            _Account.WebProxy = proxy;

            var workingProxy = request.TestProxy(proxy);
            if (request == null)
                return false;

            if (!workingProxy)
            {
                _Account.Status = Account.STATUS.INVALID_PROXY;
                SetStatus(true, "Proxy doesn't work.", Color.Red);
                _Account.ValidProxy = false;
                return false;
            }
            else
            {
                _Account.ValidProxy = true;
                return true;
            }



        }

        public void SetStatus(bool visible, string s = "", Color? g = null)
        {
            btnSave.Invoke((MethodInvoker)delegate
            {
                if (g.HasValue) lblStatus.ForeColor = g.Value;
                lblStatus.Text = s;
                lblStatus.Visible = visible;
            });
        }


        private void AddEditAccountForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (request != null)
                request.Abort();
            request = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            request.Abort();
            request = null;

            this.Invoke((MethodInvoker)delegate
            {
                btnCancel.Visible = false;
                btnSave.Enabled = grpLogin.Enabled = grpProxy.Enabled = true;
            });
            SetStatus(true, "Aborted.", Color.Red);
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
                txtPass.PasswordChar = '\0';
            else
                txtPass.PasswordChar = '*';
        }

        private void chkProxyPassShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProxyPassShow.Checked)
                txtProxyPass.PasswordChar = '\0';
            else
                txtProxyPass.PasswordChar = '*';

        }
    }
}

