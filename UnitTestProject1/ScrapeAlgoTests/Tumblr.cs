using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

namespace UnitTestProject1.ScrapeAlgoTests
{
    [TestClass]
    public class Tumblr
    {
        [TestMethod]
        public void TestMethod1()
        {
            string line; string lines = "";
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(".\\fr.txt");
            while ((line = file.ReadLine()) != null)
                lines += line;
            file.Close();

            //lines = Regex.Match(lines,
            Console.WriteLine("");

            lines = HttpUtility.HtmlDecode(lines);

            Console.WriteLine("");

        }
    }
}
