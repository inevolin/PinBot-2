using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PinBot2.Common
{

    public static class Logging
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private static ReaderWriterLockSlim _readWriteLock2 = new ReaderWriterLockSlim();
        private static string file = "./debug.xml";
        private static StringBuilder sb = new StringBuilder(); //write only every 5 sec
        private static Timer tmr = null;

        private static void ProcessTick(object state)
        {
            _readWriteLock2.EnterWriteLock();
            try
            {
                using (StreamWriter sw = new StreamWriter(file, true))
                {
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sb.Clear();
                }
            }
            catch { }
            finally { _readWriteLock2.ExitWriteLock(); }
        }


        private static int HowManyFieldsDoesLoggingObjectHave = 5;
        public class LoggingObject
        {
            public string Time { get; set; }
            public string Who { get; set; }
            public string Feature { get; set; }
            public string FunctionName { get; set; }
            public string Info { get; set; }
        }

        public static void Log(string who, string feature, string info)
        {
            _readWriteLock.EnterWriteLock();
            if (tmr == null)
                new Timer(new TimerCallback(ProcessTick), null, 0, 5 * 1000);
            try
            {                
                Write(CreateEntry(who, feature, info, new StackFrame(1, true)));
                TruncFile();
            }
            catch { }
            finally { _readWriteLock.ExitWriteLock(); }
        }
        private static LoggingObject CreateEntry(string who, string feature, string info, StackFrame sf)
        {
            LoggingObject o = new LoggingObject();
            o.Time = DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString();
            o.FunctionName = sf.GetMethod().ReflectedType.FullName + " : " + sf.GetMethod().Name;
            o.Feature = feature;
            o.Who = who;
            o.Info = info + Environment.NewLine;
            return o;
        }
        private static void Write(LoggingObject o)
        {
            var writer = new XmlSerializer(typeof(LoggingObject));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    writer.Serialize(xmlWriter, o);
                }
                sb.AppendLine(textWriter.ToString()); //This is the output as a string
            }

        }
        private static void TruncFile()
        {
            FileInfo txtfile = new FileInfo(file);
            if (txtfile.Length > (10 * 1024 * 1024))
            {
                int skip = (HowManyFieldsDoesLoggingObjectHave + 2) * 100;
                var lines = File.ReadAllLines(file).Skip(skip).ToArray();  // skip lines from top
                File.WriteAllLines(file, lines);
            }
        }

        internal static Object CreateObjectFromXML(string XMLString, Object YourClassObject)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(YourClassObject.GetType());
            YourClassObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
            return YourClassObject;
        }

    }
}
