using JumpKick.HttpLib.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests
{
    [TestClass]
    public class HttpTest
    {
        [TestMethod]
        public void TestGetMethodReturnsRequestWithGetType()
        {
            RequestBuilder b = Http.Get("a");
            Assert.AreEqual(HttpVerb.Get, b.Method);
        }

        [TestMethod]
        public void TestPostMethodReturnsRequestWithPostType()
        {
            RequestBuilder b = Http.Post("a");
            Assert.AreEqual(HttpVerb.Post, b.Method);
        }

        [TestMethod]
        public void TestPutMethodReturnsRequestWithPutType()
        {
            RequestBuilder b = Http.Put("a");
            Assert.AreEqual(HttpVerb.Put, b.Method);
        }

        [TestMethod]
        public void TestHeadMethodReturnsRequestWithHeadType()
        {
            RequestBuilder b = Http.Head("a");
            Assert.AreEqual(HttpVerb.Head, b.Method);
        }

        [TestMethod]
        public void TestPatchMethodReturnsRequestWithPatchType()
        {
            RequestBuilder b = Http.Patch("a");
            Assert.AreEqual(HttpVerb.Patch, b.Method);
        }

        [TestMethod]
        public void TestDeleteMethodReturnsRequestWithDeleteType()
        {
            RequestBuilder b = Http.Delete("a");
            Assert.AreEqual(HttpVerb.Delete, b.Method);
        }

        [TestMethod]
        public void TestGetMethodReturnsRequestWithCorrectUrl()
        {
            RequestBuilder b = Http.Get("a");
            Assert.AreEqual("a", b.Url);
        }

        [TestMethod]
        public void TestPostMethodReturnsRequestWithCorrectUrl()
        {
            RequestBuilder b = Http.Post("a");
            Assert.AreEqual("a", b.Url);
        }

        [TestMethod]
        public void TestPutMethodReturnsRequestWithCorrectUrl()
        {
            RequestBuilder b = Http.Put("a");
            Assert.AreEqual("a", b.Url);
        }

        [TestMethod]
        public void TestHeadMethodReturnsRequestWithCorrectUrl()
        {
            RequestBuilder b = Http.Head("a");
            Assert.AreEqual("a", b.Url);
        }

        [TestMethod]
        public void TestPatchMethodReturnsRequestWithCorrecturl()
        {
            RequestBuilder b = Http.Delete("a");
            Assert.AreEqual("a", b.Url);
        }

        [TestMethod]
        public void TestDeleteMethodReturnsRequestWithCorrectUrl()
        {
            RequestBuilder b = Http.Delete("a");
            Assert.AreEqual("a", b.Url);
        }


    }
}
