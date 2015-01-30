using JumpKick.HttpLib.Builder;
using JumpKick.HttpLib.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests.Builder
{
    [TestClass]
    public class RequestBuilderTest
    {
        [TestMethod]
        public void TestGetUrlFromRequestBuilder()
        {
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            Assert.AreEqual("url", rb.Url);
        }

        [TestMethod]
        public void TestGetVerbFromRequestBuilder()
        {
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Head);
            Assert.AreEqual(HttpVerb.Head, rb.Method);
        }

        [TestMethod]
        public void TestSetsActionProvider()
        {
            Mock<ActionProvider> ap = new Mock<ActionProvider>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            rb.Action(ap.Object);
            Assert.AreEqual(ap.Object,rb.GetAction());
        }

        [TestMethod]
        public void TestSetsActionProviderReturnsThis()
        {
            Mock<ActionProvider> ap = new Mock<ActionProvider>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            Assert.AreEqual(rb, rb.Action(ap.Object));
        }



        [TestMethod]
        [Ignore]
        public void TestSetsActionSingleString()
        {
            Mock<Action<WebHeaderCollection,Stream>> succ = new Mock<Action<WebHeaderCollection,Stream>>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            rb.OnSuccess(succ.Object);

            
            Assert.AreEqual(succ.Object,  rb.GetOnSuccess());
        }

       


        [TestMethod]
        public void TestSetsActionSingleStringReturnsThis()
        {
            Mock<Action<String>> succ = new Mock<Action<String>>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            Assert.AreEqual(rb, rb.OnSuccess(succ.Object));
        }

        [TestMethod]
        [Ignore]
        public void TestSetsActionStringHeaderCollection()
        {
            Mock<Action<WebHeaderCollection, String>> succ = new Mock<Action<WebHeaderCollection,String>>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            rb.OnSuccess(succ.Object);
            Assert.AreEqual(succ.Object, rb.GetOnSuccess());
        }

        [TestMethod]
        public void TestSetsActionStringHeaderCollectionReturnsThis()
        {
            Mock<Action<WebHeaderCollection,String>> succ = new Mock<Action<WebHeaderCollection, String>>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            Assert.AreEqual(rb, rb.OnSuccess(succ.Object));
        }

        [TestMethod]
        public void TestSetActionStream()
        {
            Mock<Action<WebHeaderCollection, Stream>> succ = new Mock<Action<WebHeaderCollection, Stream>>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            rb.OnSuccess(succ.Object);
            Assert.AreEqual(succ.Object, rb.GetOnSuccess());
        }

        [TestMethod]
        public void TestSetActionStreamReturnsThis()
        {
            Mock<Action<WebHeaderCollection, Stream>> succ = new Mock<Action<WebHeaderCollection, Stream>>();
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            Assert.AreEqual(rb, rb.OnSuccess(succ.Object));
        }

        [TestMethod]
        public void TestAppendToReturnsThis()
        {
            RequestBuilder rb = new RequestBuilder("url", HttpVerb.Get);
            Assert.AreEqual(rb, rb.AppendTo("f"));
        }

       
        //Download To

        //Fail




        //Auth 1
        //Auth 2

        //Auth 3

        //Upload

        //Upload
        
        //Form

        //Form

        //Body

        //Body

        //Body

        //Body

        //Body

        //Headers

        //Headers

        //Headers

        //Go
    }
}
