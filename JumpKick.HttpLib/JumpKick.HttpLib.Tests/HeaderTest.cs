using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests
{
    [TestClass]
    public class HeaderTest
    {
        Header header;

        [TestInitialize]
        public void SetUp()
        {
            header = new Header("a","b");
        }


        [TestMethod]
        public void TestNameIsSetOnInit()
        {
            Assert.AreEqual("a", header.Name);
        }

        [TestMethod]
        public void TestValueIsSetOnInit()
        {
            Assert.AreEqual("b", header.Value);
        }

    }
}
