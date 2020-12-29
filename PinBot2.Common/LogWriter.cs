
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PinBot2.Common
{
    public static class __LogWriter
    {
        private static bool writing;
        private static string file = "./debug.txt";

        public static void writeLog(string s)
        {
            while (writing)
                Thread.Sleep(200);

            writing = true;

            try
            {
                cleanFile();
                string dt = System.DateTime.Now.ToLongTimeString() + "  " + System.DateTime.Now.ToShortDateString();
                File.AppendAllText(file, dt + "        " + s);
                File.AppendAllText(file, "\r\n\r\n======================\r\n\r\n");
            }
            catch { }

            writing = false;
        }

        private static readonly int maxBytes = 15000000; //15MB
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
            }
            catch { }
        }
    }
}
