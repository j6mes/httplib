using JumpKick.HttpLib.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class TextBodyProviderTest
    {
        TextBodyProvider provider;
        TextBodyProvider ctprovider;

        [TestInitialize]
        public void SetUp()
        {
            provider = new TextBodyProvider("test");
            ctprovider = new TextBodyProvider("test","test");
        }

        [TestMethod]
        public void TestDefaultContentType()
        {
            Assert.AreEqual("application/text", provider.GetContentType());
        }

        [TestMethod]
        public void TestOverrideContentType()
        {
            Assert.AreEqual("test", ctprovider.GetContentType());
        }

        [TestMethod]
        public void TestStreamIsOutput()
        {
            Assert.IsInstanceOfType(ctprovider.GetBody(), typeof(Stream));
        }

        [TestMethod]
        public void TestStreamIsNotNull()
        {
            Assert.IsNotNull(ctprovider.GetBody());
        }

        [TestMethod]
        public void TestTextStreamContainsInputString()
        {
            StreamReader reader = new StreamReader(provider.GetBody());
            String text = reader.ReadToEnd();
            Assert.AreEqual("test", text);
        }
    }
}
