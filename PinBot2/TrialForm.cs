using PinBot2.Helpers;
using PinBot2.Presenter.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace PinBot2
{
    public partial class TrialForm : Form, ITrialView
    {
        private string code;

        public bool MayContinue { get; private set; }

        public TrialForm()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Application.Run(this);
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Length <= 0)
            {
                MessageBox.Show("Email address missing", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtName.Text.Length <= 0)
            {
                MessageBox.Show("Email address missing", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid email address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            btnSubmit.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            string name = txtName.Text;
            string email = txtEmail.Text;

            Licensing.TrialResponse resp = new Licensing.TrialResponse();
            Task<Licensing.TrialResponse> t = Task<Licensing.TrialResponse>.Factory.StartNew(() =>
            {
                return (resp = Licensing.SignUpTrial_Send(email, name, false));
            })
            .ContinueWith((prev_task) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Cursor = Cursors.Default;
                });
                    if (resp.Success && resp.Message == "1") //user already in DB
                    {
                        MayContinue = true;
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    }
                    else if (Regex.IsMatch(resp.Message, "^\\d\\d\\d\\d\\d\\d$"))
                    {
                        code = resp.Message;
                        this.Invoke((MethodInvoker)delegate
                        {
                            p1.Visible = p1.Enabled = false;
                            p2.Visible = true;
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            btnSubmit.Enabled = true;
                        });
                    }
               
                return resp;
            });

        }
        void resend()
        {
            string name = txtName.Text;
            string email = txtEmail.Text;

            Licensing.TrialResponse resp = new Licensing.TrialResponse();
            Task<Licensing.TrialResponse> t = Task<Licensing.TrialResponse>.Factory.StartNew(() =>
            {
                return (resp = Licensing.SignUpTrial_Send(email, name, true));
            })
            .ContinueWith((prev_task) =>
            {
                if (!resp.Success)
                {
                    MessageBox.Show("Something went wrong. Please try again or contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (resp.Success && resp.Message.Equals("1")) //user already in DB
                {
                    MayContinue = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Close();
                    });
                }
                else
                {
                    code = resp.Message;
                    MessageBox.Show("Code has been sent to your mailbox", "Code sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return resp;
            });



        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnVerifyCode_Click(object sender, EventArgs e)
        {
            code = txtCode.Text;

            if (!Regex.IsMatch(code, "\\d\\d\\d\\d\\d\\d"))
            {
                MessageBox.Show("Code must be 6 digits", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            btnVerifyCode.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            


            Licensing.TrialResponse resp = new Licensing.TrialResponse();
            Task<Licensing.TrialResponse> t = Task<Licensing.TrialResponse>.Factory.StartNew(() =>
            {
                return (resp = Licensing.SignUpTrial_Validate(code));
            })
            .ContinueWith((prev_task) =>
            {

                this.Invoke((MethodInvoker)delegate
                {
                    this.Cursor = Cursors.Default;
                });
                

                if (!resp.Success)
                {
                    MessageBox.Show("Something went wrong. Please try again or contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (resp.Success && resp.Message.Equals("invalid"))
                {
                    MessageBox.Show("Invalid code.", "Invalid code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Invoke((MethodInvoker)delegate
                    {
                        btnVerifyCode.Enabled = true;
                    });
                }
                else if (resp.Success && resp.Message.Equals("1"))
                {
                    MessageBox.Show("Success! Enjoy using PinBot Trial", "Success", MessageBoxButtons.OK);
                    MayContinue = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Close();
                    });
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please try again or contact support", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Invoke((MethodInvoker)delegate
                    {
                        btnVerifyCode.Enabled = true;
                    });
                }

                return resp;
            });

        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            resend();
            this.Cursor = Cursors.Default;
        }
        private void btnGoBack_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = true;
            p1.Visible = p1.Enabled = true;
            p2.Visible = false;
        }
        private void btnPrivacy_Click(object sender, EventArgs e)
        {
            Process.Start("https://healzer.com/privacy.html");
        }



    }
}
