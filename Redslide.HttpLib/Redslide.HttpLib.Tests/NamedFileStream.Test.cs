using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Redslide.HttpLib.Tests
{
    
    [TestClass()]
    public class NamedFileStreamTest
    {


        [TestMethod()]
        public void NamedFileStreamConstructorTest()
        {
            string name = "name";
            string filename = "test.jpg";
            string contentType = "image/jpeg";
            Stream stream = new MemoryStream();
            NamedFileStream target = new NamedFileStream(name, filename, contentType, stream);


            Assert.AreEqual(name, target.Name);
            Assert.AreEqual(filename, target.Filename);
            Assert.AreEqual(stream, target.Stream);
            Assert.AreEqual(contentType, target.ContentType);
        }
    }
}
