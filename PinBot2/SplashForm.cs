using PinBot2.Common;
using PinBot2.Dal;
using PinBot2.Helpers;
using PinBot2.Presenter.Interface;
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

namespace PinBot2
{
    public partial class SplashForm : Form, ISplashView
    {
        http request;
        PinBot2.Helpers.Licensing.LicenseResponse resp;


        public bool IsValid { get; private set; }
        public bool MayContinue { get; private set; }
        public bool IsTrialMember { get; private set; }

        public SplashForm()
        {

            this.StartPosition = FormStartPosition.Manual;
            Point p = new Point();
            p.X = p.Y = 0;
            this.Location = p;

            InitializeComponent();
            lblStatus.Text = "";
            this.Text = "PinBot" + "  " + Application.ProductVersion.ToString();
            request = new http();
            this.TopMost = true;
        }
        public void ShowForm()
        {
            Application.Run(this);
        }
        private async void SplashForm_Load(object sender, EventArgs e)
        {
            CleanUp();

            //=================================
            MayContinue = CheckDB();
            if (!MayContinue) { Close(); return; }
            await Task.Factory.StartNew(() => { Thread.Sleep(500); });
            //=================================
            MayContinue = LoadCheckUpdater();
            if (!MayContinue) { Close(); return; }
            await Task.Factory.StartNew(() => { Thread.Sleep(500); });
            //=================================
            MayContinue = LoadCheckUpdate();
            if (!MayContinue) { Close(); return; }
            await Task.Factory.StartNew(() => { Thread.Sleep(500); });
            //=================================
            MayContinue = LoadCheckLicense();
            if (!MayContinue) { Close(); return; }
            await Task.Factory.StartNew(() => { Thread.Sleep(500); });
            //=================================
            IsTrialMember = Licensing.AlreadySignedUpTrial();
            //=================================

            Close();
        }

        private void CleanUp()
        {
            try {
                if (File.Exists("./debug.txt"))
                    File.Delete("./debug.txt");

                if (!File.Exists("./debug.xml"))
                    File.Create("./debug.xml");

            }
            catch { }
        }
        private bool CheckDB()
        {
            lblStatus.Text = "Preparing PinBot...";
            /*if (!DatabaseManager.Create())
            {
                MessageBox.Show("Unable to create Database.\nPlease run PinBot as administrator.\nPinBot needs rights to write and create files in the working directory.");
                return false;
            }

            if (!DatabaseManager.Test())
            {
                MessageBox.Show("Unable to use Database.\nPlease run PinBot as administrator.\nPinBot needs rights to read, write and create files in the working directory.");
                return false;
            }*/
            return true;
        }
        private bool LoadCheckLicense()
        {
            try
            {
                UserProperties.OnLoad(); //call only once !!!

                string LocalTID = (string)UserProperties.GetProperty("TID");
                try
                {
                    Microsoft.Win32.RegistryKey key;
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PinBotLicense", true);
                    if (key == null)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PinBotLicense");
                    else
                        LocalTID = key.GetValue("tid") == null ? "" : key.GetValue("tid").ToString();
                    key.Close();
                }
                catch { }

                if (LocalTID.Length > 0)
                {
                    lblStatus.Text = "";
                    Task.Factory.StartNew(() => { Thread.Sleep(100); });
                    lblStatus.Text = "Verifying license...";

                    resp = Licensing.CheckLicense(request, LocalTID);
                }

                IsValid = resp.IsValid;
            }
            catch (Exception ex)
            {
                string msg = "Error SF100." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SplashForm", msg);
                

                MessageBox.Show("Error SF100. Please contact support.");
                return false;
            }
            return true;
        }
        private bool LoadCheckUpdater()
        {
            try
            {
                int attempts = 0;
                try
                {
                    while (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Updater2.exe"))
                    {
                        if (attempts >= 5)
                            throw new Exception();
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile("https://healzer.com/pinbot/Updater2.exe", AppDomain.CurrentDomain.BaseDirectory + "Updater2.exe");
                        }
                        attempts++;
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error SF125." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log("user", "account action: SplashForm", msg);
                    

                    MessageBox.Show("Unable to download an update. Please contact support.");
                    return false;
                }

                attempts = 0;
                string pv = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "Updater2.exe").ProductVersion;
                string cpv = request.GET("https://healzer.com/pinbot/updater2.txt", "", new System.Net.CookieContainer(), null, null);

                if (cpv.Contains("[[[ERROR"))
                {
                    MessageBox.Show("Error while connecting to server. Please contact support.");
                    return false;
                }


                if (int.Parse(cpv.Replace(".", "")) > int.Parse(pv.Replace(".", "")))
                {
                    lblStatus.Invoke(new MethodInvoker(delegate
                    {
                        lblStatus.Text = "Downloading update...";
                    }));


                    while (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Updater2.exe"))
                    {
                        try
                        {
                            foreach (var process in Process.GetProcessesByName("Updater2"))
                            {
                                process.Kill();
                            }
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Updater2.exe");
                            ++attempts;
                        }
                        catch (Exception ex)
                        {
                            string msg = "Error SF160." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                            Logging.Log("user", "account action: SplashForm", msg);
                            
                        }
                        if (attempts > 5) throw new Exception();
                    }
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile("https://healzer.com/pinbot/Updater2.exe", AppDomain.CurrentDomain.BaseDirectory + "Updater2.exe");
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SF164." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SplashForm", msg);
                

                MessageBox.Show("Error SF164. Please contact support.");
                return false;
            }
            return true;
        }
        private bool LoadCheckUpdate()
        {
            try
            {
                string pv = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "PinBot2.exe").ProductVersion;
                string cpv = request.GET("https://healzer.com/pinbot/update2.txt", "", new System.Net.CookieContainer(), null, null);

                if (cpv.Contains("[[[ERROR"))
                {
                    MessageBox.Show("Error while connecting to server. Please contact support.");
                    return false;
                }

                if (int.Parse(cpv.Replace(".", "")) > int.Parse(pv.Replace(".", "")))
                {
                    Process.Start(Path.Combine(Environment.CurrentDirectory, "Updater2.exe"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SF188." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: SplashForm", msg);
                

                MessageBox.Show("Error SF188. Please contact support.");
                return false;

            }
            return true;

        }
    }
}
