using PinBot2.Common;
using PinBot2.Dal;
using PinBot2.Helpers;
using PinBot2.Presenter.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinBot2
{
    public partial class LicensingForm : Form, ILicensingView
    {
        private bool PREMIUM;
        public bool IS_PREMIUM { get { return PREMIUM; } }

        private bool mayContinue;
        public bool MayContinue { get { return mayContinue; } }

        http request;

        public LicensingForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.Text = "PinBot" + "  " + Application.ProductVersion.ToString();
            request = new http();
            mayContinue = false;
            this.TopMost = true;
        }

        public void ShowForm()
        {
            Application.Run(this);
        }

        private void btnTrial_Click(object sender, EventArgs e)
        {
            PREMIUM = false;
            mayContinue = true;
            Close();
        }
        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            string TID = txtLicense.Text.Trim().Replace("'", "").Replace("?", "").Replace("&", "").Replace(":", "");
            if (TID.Length == 0)
            {
                SetMessage("License ID is empty.", Color.Red);
                txtLicense.Focus();
                return;
            }

            try
            {
                btnTrial.Enabled = btnSubmit.Enabled = false;
                SetMessage("Validating License...", Color.Red);
                Licensing.LicenseResponse resp = Licensing.CheckLicense(request, TID);
                if (resp.IsValid)
                {
                    PREMIUM = true;
                    mayContinue = true;
                    SetMessage("Starting PinBot...", Color.Green);
                    await Task.Factory.StartNew(() => { Thread.Sleep(5000); });
                    { Close(); return; }
                }
                else
                {
                    SetMessage(resp.Message, resp.color);
                }
            }
            catch { }
            finally { btnTrial.Enabled = btnSubmit.Enabled = true; }
        }

        private void SetMessage(string msg, Color color)
        {
            lblStatus.Text = msg;
            lblStatus.ForeColor = color;
        }

        private void linkBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://healzer.com/pinbot/");
        }
    }
}
