using PinBot2;
using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class Updater : Form
    {
        private readonly string mainprogram = "PinBot2.exe";
        private http request = new http();

        public Updater()
        {
            InitializeComponent();
            lblStatus.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Task t;
            t = Task.Factory.StartNew(() =>
            {
                update();
                if (checkv())
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        Thread.Sleep(1000);                        
                        try {
                            lblStatus.Text = "Starting PinBot, please wait...";
                            lblStatus.ForeColor = Color.Green;
                            Process.Start(AppDomain.CurrentDomain.BaseDirectory + mainprogram);
                            Thread.Sleep(1500);
                        }
                        catch { }
                        this.Close();
                    });
                }
            });
        }

            private void Updater_Load(object sender, EventArgs e)
            {
                Task t;
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + mainprogram))
                {
                    btnUpdate.Enabled = false;
                    lblStatus.Text = "Downloading PinBot...";
                    t = Task.Factory.StartNew(() =>
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile("https://healzer.com/pinbot/PinBot2.exe", "./" + mainprogram);
                        }
                    })
                    .ContinueWith((prev_task) =>
                    {
                        if (prev_task.Exception != null)
                        {

                            btnUpdate.Invoke(new MethodInvoker(delegate
                            {
                                btnUpdate.Enabled = true;
                            }));
                            lblStatus.Invoke(new MethodInvoker(delegate
                            {
                                lblStatus.Text = "Download failed \n Try again or contact support.";
                                lblStatus.ForeColor = Color.Red;
                            }));
                        }
                        else
                        {
                            btnUpdate.Invoke(new MethodInvoker(delegate
                            {
                                btnUpdate.Enabled = false;
                            }));
                            lblStatus.Invoke(new MethodInvoker(delegate
                            {
                                lblStatus.Text = "Please wait...";
                            }));
                        }
                    })
                    .ContinueWith((prev_task) =>
                    {
                        t = Task.Factory.StartNew(() =>
                        {
                            checkv();
                        });
                    });
                }
                else
                {
                    t = Task.Factory.StartNew(() => { checkv(); });
                }

            }

        private void update()
        {
            try
            {
                btnUpdate.Invoke(new MethodInvoker(delegate
                {
                    btnUpdate.Enabled = false;
                }));
                lblStatus.Invoke(new MethodInvoker(delegate
                {
                    lblStatus.Text = "Please wait...";
                }));

                int attempts = 0;
                while (File.Exists(AppDomain.CurrentDomain.BaseDirectory + mainprogram))
                {
                    try
                    {
                        foreach (var process in Process.GetProcessesByName(mainprogram.Replace(".exe", "")))
                        {
                            process.Kill();
                        }
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + mainprogram);
                        ++attempts;
                    }
                    catch (Exception ex)
                    {
                        string msg = "Error U117." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                        Logging.Log("user", "updater", msg);
                    }
                    if (attempts > 5) throw new Exception();
                }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://healzer.com/pinbot/PinBot2.exe", "./" + mainprogram);
                }

            }
            catch (Exception ex)
            {
                string msg = "Error U131." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "updater", msg);
                MessageBox.Show("Something went wrong. Please try again or contact support.", "ERROR"); Environment.Exit(Environment.ExitCode);
            }
        }
        private bool checkv()
        {
            bool equal = false;
            try
            {
                btnUpdate.Invoke(new MethodInvoker(delegate
                {
                    btnUpdate.Enabled = false;
                }));

                string pv = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + mainprogram).ProductVersion;
                string cpv = request.GET("https://healzer.com/pinbot/update2.txt", "", new System.Net.CookieContainer(), null, null);

                equal = pv.Equals(cpv);

                lblStatus.Invoke(new MethodInvoker(delegate
                {
                    lblStatus.Text = "Your version: " + pv + "\nLatest version: " + cpv; ;
                    lblStatus.ForeColor = Color.Green;
                }));
                btnUpdate.Invoke(new MethodInvoker(delegate
                {
                    btnUpdate.Enabled = false;
                }));
                if (int.Parse(cpv.Replace(".", "")) > int.Parse(pv.Replace(".", "")))
                {
                    lblStatus.Invoke(new MethodInvoker(delegate
                    {
                        lblStatus.ForeColor = Color.Red;
                    }));
                    btnUpdate.Invoke(new MethodInvoker(delegate
                    {
                        btnUpdate.Enabled = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SF164." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                        Logging.Log("user", "updater", msg);
                MessageBox.Show("Something went wrong. Please try again or contact support.", "ERROR"); Environment.Exit(Environment.ExitCode);
                equal = false;
            }

            return equal;

        }

    }
}
