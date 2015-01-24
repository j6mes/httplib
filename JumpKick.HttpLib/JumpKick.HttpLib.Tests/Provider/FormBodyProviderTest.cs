using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JumpKick.HttpLib.Provider;
using System.IO;
using System.Collections.Generic;

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
        public void TestBodyIsContainsSingleItemQueryString()
        {
            provider.AddParameters(new { a = "b" });
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.AreEqual("a=b", content);
        }

        [TestMethod]
        public void TestBodyIsContainsSingleItemQueryStringDictionary()
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("a", "b");

            provider.AddParameters(p);
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.AreEqual("a=b", content);
        }

        [TestMethod]
        public void TestBodyIsContainsSingleItemQueryTwoItemDictionary()
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("a", "b");
            p.Add("c", "d");
            provider.AddParameters(p);
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.AreEqual("a=b&c=d", content);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullParameterThrowsException()
        {
            provider.AddParameters(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSerializingNullStringFails()
        {
            FormBodyProvider.SerializeQueryString(null);
        }

    }
}
