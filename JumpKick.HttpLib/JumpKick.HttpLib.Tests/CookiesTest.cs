using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests
{
    [TestClass]
    public class CookiesTest
    {
        [TestMethod]
        public void TestInitHasCookieContainer()
        {
            Assert.IsNotNull(Cookies.Container);
        }

        [TestMethod]
        public void TestClearCookiesReturnsNewContainer()
        {
            CookieContainer c = Cookies.Container;

            Cookies.ClearCookies();
            Assert.AreNotEqual(c,Cookies.Container);
        }

        [TestMethod]
        public void TestSetCookieContainer()
        {
            CookieContainer c =  new CookieContainer();

            Cookies.SetCookies(c);
            Assert.AreEqual(c,Cookies.Container);
        }
    }
}
