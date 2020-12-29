using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Common;
using PinBot2;
using PinBot2.Algorithms.Scraping.Queue;

namespace UnitTestProject1
{
    [TestClass]
    public class CommonTests
    {
        
        [TestMethod]
        public void DupChecker()
        {
            string e = "pinbot@healzer.com";
            string u = DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();

            DuplicateChecker dc = DuplicateChecker.init();

            
            var b = dc.IsDuplicate(e, u);
            Assert.AreEqual(false, b);


            dc.Add(e, u);
            b = dc.IsDuplicate(e, u);
            Assert.AreEqual(true, b);

            
        }
    }
}
