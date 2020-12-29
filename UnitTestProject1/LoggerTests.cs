using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Common;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Collections.Generic;
 
namespace UnitTestProject1
{ 
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void TestingXMLLogging()
        {
            IList<Task> l = new List<Task>();
            for (int i = 0; i < 1000; i++)
            {
                var f = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(10);
                    method("acc1");

                });
                l.Add(f);

                f = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(12);
                    method("acc2");
                });
                l.Add(f);

                f = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(15);
                    method("acc3");
                });

                l.Add(f);

                f = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(20);
                    method("acc4");
                });

                l.Add(f);

                f = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(12);
                    method("acc5");
                });
            }

            foreach (var b in l)
                b.Wait();

        }
        public void method(string u)
        {
            //Logging.Log(u, "unit_testing");
        }
    }
}
