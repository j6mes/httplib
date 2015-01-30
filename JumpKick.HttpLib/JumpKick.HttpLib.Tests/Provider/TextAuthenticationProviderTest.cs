using JumpKick.HttpLib.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class TextAuthenticationProviderTest
    {
        TextAuthenticationProvider auth;
        [TestInitialize]
        public void SetUp()
        {
            auth = new TextAuthenticationProvider("test");
            
        }


        [TestMethod]
        public void TestBasicAuthProducesAuthHeader()
        {
            Assert.AreEqual("Authorization", auth.GetAuthHeader().Name);
        }


        [TestMethod]
        public void TestGeneartesCorrectHeaderValue()
        {
            Assert.AreEqual("test",auth.GetAuthHeader().Value);
        }


    }
}
