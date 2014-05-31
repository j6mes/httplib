using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Redslide.HttpLib.Tests
{
	[TestClass]
	public class Utils
	{
		[TestMethod]
		public void TestSingleKVP()
		{

            string actual = Redslide.HttpLib.Utils.SerializeQueryString(new { key = "value" });
            Assert.AreEqual("key=value", actual);
		}

        [TestMethod]
        public void TestKVPArray()
        {

            string actual = Redslide.HttpLib.Utils.SerializeQueryString(new { key = "value",key2="value2" });
            Assert.AreEqual("key=value&key2=value2", actual);
        }

        [TestMethod]
        public void TestURLEncoding()
        {

            string actual = Redslide.HttpLib.Utils.SerializeQueryString(new { key = "value&" });
            Assert.AreEqual("key=value%26", actual);
        }

       

	}
}
