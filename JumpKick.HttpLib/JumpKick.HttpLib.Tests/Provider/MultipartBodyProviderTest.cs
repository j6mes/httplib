using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JumpKick.HttpLib.Provider;
using System.IO;
using System.Text.RegularExpressions;
using Moq;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class MultipartBodyProviderTest
    {
        Stream dummyFileStream;
        MultipartBodyProvider provider;
        Stream dummyFileStream2;
        StreamWriter w;
        [TestInitialize]
        public void SetUp()
        {
            provider = new MultipartBodyProvider();
            dummyFileStream = new MemoryStream();
            dummyFileStream2 = new MemoryStream();
            w = new StreamWriter(dummyFileStream);
            w.Write("file");
            w.Flush();

            w = new StreamWriter(dummyFileStream2);
            w.Write("file");
            w.Flush();


            dummyFileStream.Seek(0, SeekOrigin.Begin);

            dummyFileStream2.Seek(0, SeekOrigin.Begin);
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

        [TestMethod]
        public void TestBodyIsEmptyWithSingleItemQueryString()
        {
            provider.SetParameters(new { });
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.AreEqual("", content);
        }

        [TestMethod]
        public void TestBodyStartsWithNewLineWithParams()
        {
            provider.SetParameters(new { a="b"});
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            Assert.IsTrue(content.StartsWith("\n"));
        }


        [TestMethod]
        public void TestBodyContainsOneSeparatorPerFormItem()
        {
            provider.SetParameters(new { a = "b", c = "d", e = "f" });
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

    
            Assert.AreEqual(3, Regex.Matches(content, provider.GetBoundary()).Count);
        }


        [TestMethod]
        public void TestPostBodyContainsFileContents()
        {
            provider.AddFile(new NamedFileStream("a","b","c",dummyFileStream));
            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();
            Assert.IsTrue(content.Contains("file"));
        }


        [TestMethod]
        public void TestPostBodyContainsOneSeparatorPerFile()
        {
            provider.AddFile(new NamedFileStream("a", "b", "c", dummyFileStream));
            provider.AddFile(new NamedFileStream("a", "b", "c", dummyFileStream2));

            StreamReader r = new StreamReader(provider.GetBody());
            String content = r.ReadToEnd();

            /*
             * 2 between each item in array
             */
            Assert.AreEqual(4, Regex.Matches(content, provider.GetBoundary()).Count);

        }

    }
}
