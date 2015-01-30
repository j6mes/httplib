using JumpKick.HttpLib.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class StreamBodyProviderTest
    {
        StreamBodyProvider provider;
        StreamBodyProvider ctprovider;
        Stream stream;
        Mock<Stream> strmock;
        [TestInitialize]
        public void SetUp()
        {
            strmock = new Mock<Stream>();
            stream = strmock.Object;
            provider = new StreamBodyProvider(stream);
            ctprovider = new StreamBodyProvider("test", stream);
        }

        [TestMethod]
        public void TestDefaultContentType()
        {
            Assert.AreEqual("application/octet-stream", provider.GetContentType());
        }

        [TestMethod]
        public void TestOverrideContentType()
        {
            Assert.AreEqual("test", ctprovider.GetContentType());
        }


    

        [TestMethod]
        public void TestStreamOutIsSame()
        {
            Assert.AreEqual(stream,provider.GetBody());
        }


    }
}
