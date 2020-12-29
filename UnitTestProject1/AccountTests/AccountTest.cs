using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Model;

namespace UnitTestProject1.AccountTests
{
    [TestClass]
    public class AccountTest
    {
        private IAccount acc = new Account();

        public IAccount _Login()
        {
            string email = "test@gmail.com";
            string pass = "test";
            acc.Email = email;
            acc.Password = pass;
            acc.IsLoggedIn = false;
            acc.LoginSync(true);
            return acc;
        }

        [TestMethod]
        public void Login()
        {
            _Login();
            Assert.AreEqual(true, acc.IsLoggedIn);
        }
        
        [TestMethod]
        public void Boards()
        {
            Login();
            Assert.IsTrue(acc.Boards.Count > 0);
        }
    }
}
