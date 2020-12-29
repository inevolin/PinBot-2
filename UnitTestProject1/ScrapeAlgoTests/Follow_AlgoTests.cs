using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Algorithms.Scraping;
using PinBot2.Common;
using PinBot2.Model;
using PinBot2.Algorithms;
using PinBot2.Dal.Interface;
using PinBot2.Dal;
using PinBot2.Model.Configurations;
using System.Collections.Generic;
using PinBot2.Model.PinterestObjects;

namespace UnitTestProject1.ScrapeAlgoTests
{
    [TestClass]
    public class Follow_AlgoTests
    {
        private http request = new http();
        private IAccount acc;
        private IList<PinterestObject> list;
        private FollowAlgo followAlgo;

        private FollowConfiguration GetConfig()
        {
            FollowConfiguration config = new FollowConfiguration();
            config.Enabled = true;
            config.Timeout = new Range<int>(5, 10);
            config.CurrentCount = new Range<int>(2, 2);
            config.Queries = new Dictionary<string, PinterestObjectResources>();
            config.Queries.Add(new KeyValuePair<string, PinterestObjectResources>("fashion", PinterestObjectResources.SearchResource));
            return config;
        }

        [TestMethod]
        public void ScrapeUsersByKeyword()
        {
            list = null;
            acc = new AccountTests.AccountTest()._Login();

            var config = GetConfig();
            config.FollowUsers = true;
            config.FollowBoards = false;

            followAlgo = new FollowAlgo(acc, config, new InMemoryAccountRepository());
            ScrapeSessionManager sm = new ScrapeSessionManager(followAlgo, config, request, acc);
            list = sm.Scrape(this.GetType().ToString());

            Assert.IsTrue(list != null && list.Count > 0);
        }
        [TestMethod]
        public void FollowScrapedUsers()
        {
            ScrapeUsersByKeyword();
            followAlgo.Run();
            Assert.AreEqual(followAlgo.Config.CurrentCount.Max, followAlgo.CurrentCount.Min);
        }

        [TestMethod]
        public void ScrapeBoardsByKeyword()
        {
            list = null;
            acc = new AccountTests.AccountTest()._Login();

            var config = GetConfig();
            config.FollowBoards = true;
            config.FollowUsers = false;

            followAlgo = new FollowAlgo(acc, config, new InMemoryAccountRepository());
            ScrapeSessionManager sm = new ScrapeSessionManager(followAlgo, config, request, acc);
            list = sm.Scrape(this.GetType().ToString());

            Assert.IsTrue(list != null && list.Count > 0);
        }
        [TestMethod]
        public void FollowScrapedBoards()
        {
            ScrapeBoardsByKeyword();
            followAlgo.Run();
            Assert.AreEqual(followAlgo.Config.CurrentCount.Max, followAlgo.CurrentCount.Min);
        }
    }
}
