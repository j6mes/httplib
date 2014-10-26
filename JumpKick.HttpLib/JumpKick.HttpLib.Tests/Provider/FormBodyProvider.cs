using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JumpKick.HttpLib.Provider;
using System.IO;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class FormBodyProviderTest
    {

        FormBodyProvider provider;

        [TestInitialize]
        public void SetUp()
        {
            provider = new FormBodyProvider();
        }



        [TestMethod]
        public void TestContentTypeIsForm()
        {
            Assert.AreEqual("application/x-www-form-urlencoded",provider.GetContentType());
        }


        [TestMethod]
        public void TestBodyIsEmptyWithNoContent()
        {
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.AreEqual(0, content.Length);
        }

        [TestMethod]
        public void TestObjectSerializerCreatesTwoItemQueryString()
        {
            Assert.AreEqual("a=b&c=d", FormBodyProvider.SerializeQueryString(new { a = "b", c = "d" }));
        }

        [TestMethod]
        public void TestObjectSerializerCreatesSingleItemQueryString()
        {
            Assert.AreEqual("a=b", FormBodyProvider.SerializeQueryString(new { a = "b" }));
        }

        [TestMethod]
        public void TestBodyIsEmptyWithSingleItemQueryString()
        {
            provider.AddParameters(new { a = "b" });
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.AreEqual("a=b", content);
        }
    }
}
