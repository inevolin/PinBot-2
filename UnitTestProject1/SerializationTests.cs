using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Dal;
using PinBot2.Model;
using System.Collections.Generic;
using PinBot2.Model.PinterestObjects;

namespace UnitTestProject1
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Test_Add_Serialized_Account()
        {
            InMemoryAccountRepository rp = new InMemoryAccountRepository();
            Account acc = new Account();
            acc.Email = "test@gmail.com";
            acc.Password = "test";
            acc.WebProxy = new PinBot2.Proxy("127.0.0.1", 8080);
            
            var info = new AccountInformation();
            info.Boards = 5;
            acc.AccountInfo = info;

            acc.AppVersion = "fz";
            var col = new HashSet<Board>();
            var b = new Board("1", "", "", "", PinterestObjectResources.BoardFeedResource);
            col.Add(b);
            acc.Boards = col;

            acc.CookieContainer = new System.Net.CookieContainer();
            acc.CsrfToken = "te";
            acc.IsConfigured = true;
            acc.IsLoggedIn = true;
            var _x = acc.Request;
            acc.SelectedCampaignId = 5;
            acc.Status = Account.STATUS.LOGGED_IN;
            acc.Username = "taza";
            acc.ValidCredentials = true;
            acc.ValidProxy = true;
            
            
            rp.AddAccount(acc);

            rp.SaveAccount(acc);
            var ac = rp.GetAccount(acc.Id);
            var acs = rp.GetAccounts(true);
            rp.DeleteAccount(acc.Id);

            Assert.AreNotEqual(0, acs.Count);

            
        }

        [TestMethod]
        public void Test_GetAccount()
        {
            InMemoryAccountRepository rp = new InMemoryAccountRepository();
            var acs = rp.GetAccounts(true);
            Console.WriteLine(acs.Count);
        }
    }
}
