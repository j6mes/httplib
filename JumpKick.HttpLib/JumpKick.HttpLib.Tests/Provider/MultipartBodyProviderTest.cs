using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JumpKick.HttpLib.Provider;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class MultipartBodyProviderTest
    {

        MultipartBodyProvider provider;

        [TestInitialize]
        public void SetUp()
        {
            provider = new MultipartBodyProvider();
        }

        [TestMethod]
        public void TestBoundaryStringIsInitialized()
        {
            Assert.IsNotNull(provider.GetBoundary());
        }

        [TestMethod]
        public void TestContentTypeIsMultipart()
        {
            Assert.IsTrue(provider.GetContentType().Contains("multipart/form-data"));
        }

        [TestMethod]
        public void TestContentTypeContainsBoundaryString()
        {
            Assert.IsTrue(provider.GetContentType().Contains(provider.GetBoundary()));
        }

        [TestMethod]
        public void TestContentType()
        {
            Assert.AreEqual("multipart/form-data, boundary=" + provider.GetBoundary(), provider.GetContentType());
        }
    }
}
