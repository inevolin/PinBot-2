using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Helpers
{
    public static class Licensing
    {
        public static string HID()
        {

            string hardwareID = GenHID();

            try
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PinBotHID", true);
                if (key == null)
                {
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PinBotHID");
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PinBotHID", true);
                    key.SetValue("PinBotHID", hardwareID);
                }
                else
                {
                    hardwareID = key.GetValue("PinBotHID").ToString();
                    key.Close();
                }
            }
            catch { }

            return hardwareID;
        }
        private static string GenHID()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string hardwareID = "";
            string sMacAddress = "";
            string userID = Environment.UserName;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            hardwareID = ("" + sMacAddress + "" + userID).GetHashCode().ToString();
            Console.WriteLine(hardwareID);

            return hardwareID;
        }

        public struct LicenseResponse
        {
            public string Message;
            public Color color;
            public bool IsValid;
        };
        public struct TrialResponse
        {
            public string Message;
            public bool Success;
        };

        public static LicenseResponse CheckLicense(http request, string TID)
        {
            LicenseResponse resp = new LicenseResponse();

            string HID = Licensing.HID();

            string rs = null;
            string url = "https://healzer.com/pinbot/check4.php?tid=" + TID + "&hid=" + HID;
            try
            {
                List<string> headers = new List<string>
                {   
                    "Accept-Language: en-gb,en;q=0.8,de;q=0.5,fr;q=0.3",
                    "Accept-Encoding: gzip, deflate"
                };

                rs = request.GET(
                        url,
                        "",
                        new System.Net.CookieContainer(),
                        headers,
                        null
                        );


            }
            catch (Exception ex)
            {
                string msg = "Error LIC77." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: Licensing", msg);
                

                resp.Message = "Unable to connect to server.";
                resp.color = Color.Red;
                resp.IsValid = false;
                return resp;
            }

            if (rs == null || rs.Length == 0)
            {
                resp.Message = "Unable to connect to server.";
                resp.color = Color.Red;
                resp.IsValid = false;
                return resp;
            }
            else if (rs.Contains("License already in use"))
            {
                resp.Message = "License is already in use.";
                resp.color = Color.Red;
                resp.IsValid = false;
                return resp;
            }
            else if (rs.Contains("Transaction ID not found"))
            {
                resp.Message = "Invalid License ID.";
                resp.color = Color.Red;
                resp.IsValid = false;
                return resp;
            }

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] raw_input = Encoding.UTF32.GetBytes(HID);
            byte[] raw_output = md5.ComputeHash(raw_input);
            string md5_hid = "";
            foreach (byte myByte in raw_output)
                md5_hid += myByte.ToString("X2");

            if (rs.Equals(md5_hid, StringComparison.InvariantCultureIgnoreCase))
            {
                UserProperties.SetProperty("TID", TID);

                try
                {
                    Microsoft.Win32.RegistryKey key;
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PinBotLicense");
                    key.SetValue("tid", TID);
                    key.Close();
                }
                catch { }

                resp.Message = "Success.";
                resp.color = Color.Green;
                resp.IsValid = true;
                return resp;
            }
            else
            {
                UserProperties.SetProperty("TID", "");

                try
                {
                    Microsoft.Win32.RegistryKey key;
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PinBotLicense", true);
                    if (key == null)
                    {
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PinBotLicense");
                        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PinBotLicense", true);
                    }
                    if (key.GetValue("tid") != null)
                    {
                        key.DeleteValue("tid");
                        key.Close();
                    }
                }
                catch { }

                resp.Message = "Invalid License ID.";
                resp.color = Color.Red;
                resp.IsValid = false;
                return resp;
            }
        }

        public static bool AlreadySignedUpTrial()
        {
            bool resp = false;

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(
                "hid="
                + http.HttpEncode(Licensing.HID())
            );
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);
            List<string> headers = new List<string>
                {
                    "Accept-Encoding: gzip,deflate,sdch",
                    "Accept-Language: en-US,en;q=0.8",
                };

            string rs = null;
            http request = new http();
            try
            {
                rs =
                    request.POST(
                        "https://healzer.com/pinbot/trial.php",
                        "",
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        null,
                        null,
                        100000,
                        "application/x-www-form-urlencoded; charset=UTF-8");
            }
            catch (Exception ex)
            {
                string msg = "Error LIC154." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: Licensing", msg);
                
            }

            if (rs != null && rs.Equals("1"))
                resp = true;

            return resp;
        }
        public static TrialResponse SignUpTrial_Send(string email, string name, bool resend)
        {

            TrialResponse resp = new TrialResponse();
            resp.Success = true;

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(
                "name="
                + http.HttpEncode(name)
                + "&email="
                + http.HttpEncode(email)
                + "&hid="
                + http.HttpEncode(Licensing.HID())
                + (resend ? "&resend=1" : "")
            );
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);
            List<string> headers = new List<string>
                {
                    "Accept-Encoding: gzip,deflate,sdch",
                    "Accept-Language: en-US,en;q=0.8",
                };

            string rs = null;
            http request = new http();
            try
            {
                rs =
                    request.POST(
                        "https://healzer.com/pinbot/trial.php",
                        "",
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        null,
                        null,
                        100000,
                        "application/x-www-form-urlencoded; charset=UTF-8");
            }
            catch (Exception ex)
            {
                resp.Success = false;
                string msg = "Error LIC208." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: Licensing", msg);
                
            }

            resp.Message = rs;

            return resp;

        }
        public static TrialResponse SignUpTrial_Validate(string code)
        {

            TrialResponse resp = new TrialResponse();
            resp.Success = true;

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(
                "hid="
                + http.HttpEncode(Licensing.HID())
                + "&code="
                + http.HttpEncode(code)
            );
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);
            List<string> headers = new List<string>
                {
                    "Accept-Encoding: gzip,deflate,sdch",
                    "Accept-Language: en-US,en;q=0.8",
                };

            string rs = null;
            http request = new http();
            try
            {
                rs =
                    request.POST(
                        "https://healzer.com/pinbot/trial.php",
                        "",
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        null,
                        null,
                        100000,
                        "application/x-www-form-urlencoded; charset=UTF-8");
            }
            catch (Exception ex)
            {
                resp.Success = false;
                string msg = "Error LIC208." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: Licensing", msg);
                
            }

            resp.Message = rs;

            return resp;

        }


    }
}
