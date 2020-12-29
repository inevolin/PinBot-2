using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using PinBot2.Common;

namespace PinBot2.Algorithms.Scraping.Queue
{
    public class DuplicateChecker
    {
        private static string file = "./dups.txt";
        private static IList<string> list;
        private static readonly object syncLock = new object();
        private static DuplicateChecker THIS;

        private DuplicateChecker()
        {
            list = new List<string>();

            if (!File.Exists(file))
            {
                var fs = File.Create(file);
                fs.Close();
            }
            else
            {
                lock (syncLock)
                {
                    using (StreamReader r = new StreamReader(file))
                    {
                        string line;
                        while ((line = r.ReadLine()) != null)
                        {
                           // string check = (line.Split(' ').ToArray())[0];
                           // if (!check.Contains("@"))
                           //    line = userId + " " + line;
                            list.Add(line);
                        }
                    }
                }
            }
        }
        public static DuplicateChecker init()
        {
            try
            {
                if (THIS != null)
                    return THIS;
                else
                    THIS = new DuplicateChecker();
            }
            catch (Exception ex)
            {
                string msg = "Error DUPC47." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("auto", THIS.GetType().ToString(), msg);
            }
            return THIS;
        }

        public bool IsDuplicate(string userId, string objectId)
        {
            string s = userId + " " + objectId;
            return list.Contains(s);
        }

        public void Add(string userId, string objectId)
        {
            try
            {
                lock (syncLock)
                {
                    string s = userId + " " + objectId;
                    list.Add(s);

                    using (StreamWriter w = File.AppendText(file))
                    {
                        w.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error DUPC76." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("auto", this.GetType().ToString(), msg);
            }
        }

        private static readonly int maxBytes = 5000000; //5MB
        private static void cleanFile()
        {
            try
            {
                if (!File.Exists(file))
                    return;
                var info = new FileInfo(file);
                if (info.Length >= maxBytes)
                {
                    File.Delete(file);
                }
                File.Create(file);
            }
            catch (Exception ex)
            {
                string msg = "Error DUPC97." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("auto", "DuplicateChecker", msg);
            }
        }


    }
}
