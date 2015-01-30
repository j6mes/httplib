using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JumpKick.HttpLib.Provider;
using System.IO;
using System.Collections.Generic;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class DictionaryHeaderProviderTest
    {

     
    
        [TestMethod]
        public void TestEmptyDictionaryOfHeaders()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(headers);
            Assert.AreEqual(0, provider.GetHeaders().Length);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToDictionaryKeysSingle()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add("a", "b");
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(headers);
            Assert.AreEqual("a", provider.GetHeaders()[0].Name);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToDictionaryValuesSingle()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add("a", "b");
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(headers);
            Assert.AreEqual("b", provider.GetHeaders()[0].Value);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToDictionaryKeysDouble()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add("a", "b");
            headers.Add("c", "d");
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(headers);
            Assert.AreEqual("c", provider.GetHeaders()[1].Name);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToDictionaryValuesDouble()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add("a", "b");
            headers.Add("c", "d");
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(headers);
            Assert.AreEqual("d", provider.GetHeaders()[1].Value);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToObjectKeysSingle()
        {
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(new { a = "b" });
            Assert.AreEqual("a", provider.GetHeaders()[0].Name);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToObjectValuesSingle()
        {
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(new { a = "b" });
            Assert.AreEqual("b", provider.GetHeaders()[0].Value);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToObjectKeysDouble()
        {
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(new { a = "b", c = "d" });
            Assert.AreEqual("c", provider.GetHeaders()[1].Name);
        }

        [TestMethod]
        public void TestHeaderValuesCorrespondToObjectValuesDouble()
        {
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider(new { a = "b", c = "d" });
            Assert.AreEqual("d", provider.GetHeaders()[1].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCatchesNullObject()
        {
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider((IDictionary<string, string>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCatchesNullObject2()
        {
            DictionaryHeaderProvider provider = new DictionaryHeaderProvider((object)null);
        }


    }
}
