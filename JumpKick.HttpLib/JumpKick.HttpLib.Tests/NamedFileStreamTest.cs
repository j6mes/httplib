using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests
{
    [TestClass]
    public class NamedFileStreamTest
    {
        Stream stream;
        NamedFileStream fs;

        [TestInitialize]
        public void SetUp()
        {
            stream = new MemoryStream();
            fs = new NamedFileStream("n", "fn", "ct", stream);
        }

        [TestMethod]
        public void TestNamedFileStreamSetsNameOnInit()
        {
            Assert.AreEqual("n", fs.Name);
        }

        [TestMethod]
        public void TestNamedFileStreamSetsFileNameOnInit()
        {
            Assert.AreEqual("fn", fs.Filename);
        }

        [TestMethod]
        public void TestNamedFileStreamSetsContentTypeOnInit()
        {
            Assert.AreEqual("ct", fs.ContentType);
        }

        [TestMethod]
        public void TestNamedFileStreamSetsStreamOnInit()
        {
            Assert.AreEqual(stream, fs.Stream);
        }
    }
}
