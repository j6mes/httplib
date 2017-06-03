using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JumpKick.HttpLib.Provider;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class FormBodyProviderTest
    {

        public class TestObject
        {
            public String a { get { throw new NullReferenceException("test");} }
            public int b { get { return 0;  } }
        }

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
        [ExpectedException(typeof(SerializationException))]

        public void TestSerializeObjectWithNullValue()
        {
            FormBodyProvider.SerializeQueryString(new TestObject());
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
