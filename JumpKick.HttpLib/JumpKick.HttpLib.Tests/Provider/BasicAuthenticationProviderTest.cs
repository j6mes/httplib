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
    public class BasicAuthenticationProviderTest
    {
        BasicAuthenticationProvider auth;
        [TestInitialize]
        public void SetUp()
        {
            auth = new BasicAuthenticationProvider("test", "test");
            
        }


        [TestMethod]
        public void TestBasicAuthProducesAuthHeader()
        {
            Assert.AreEqual("Authorization", auth.GetAuthHeader().Name);
        }

        [TestMethod]
        public void TestPasswordEncoding()
        {
            Assert.AreEqual("dGVzdDp0ZXN0", BasicAuthenticationProvider.GenerateAuthString("test", "test"));
        }

        [TestMethod]
        public void TestGeneartesCorrectHeaderValue()
        {
            Assert.AreEqual("Basic dGVzdDp0ZXN0",auth.GetAuthHeader().Value);
        }


    }
}
