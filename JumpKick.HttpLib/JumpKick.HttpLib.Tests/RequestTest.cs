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

namespace JumpKick.HttpLib.Tests
{
    [TestClass]
    public class RequestTest
    {
        Mock<IAsyncResult> asyncResultMock;
        Mock<HttpWebRequest> webRequestMock;
        Mock<HttpWebResponse> webResponseMock;

        IAsyncResult result;
        [TestInitialize]
        public void SetUp()
        {
            webResponseMock = new Mock<HttpWebResponse>();
            webRequestMock = new Mock<HttpWebRequest>();
            webRequestMock.Setup((w) => w.EndGetResponse(It.IsAny<IAsyncResult>())).Returns(webResponseMock.Object);

            asyncResultMock = new Mock<IAsyncResult>();
            asyncResultMock.Setup((r) => r.AsyncState).Returns(webRequestMock.Object);

            result = asyncResultMock.Object;
        }


        public class RequestWrapper : Request
        {
            private HttpWebRequest webReq;

            public RequestWrapper()
            {

            }


            public RequestWrapper(HttpWebRequest webReq)
            {
                this.webReq = webReq;
            }


            public new AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
            {
                return base.ProcessCallback(success, fail);
            }

            public void MakeRequest(HttpVerb method, string url, HeaderProvider header, AuthenticationProvider auth, BodyProvider body)
            {
                this.method = method;
                this.url = url;
                this.headers = header;
                this.auth = auth;
                this.body = body;

                this.MakeRequest();
            }

            protected override HttpWebRequest GetWebRequest(string url)
            {
                if(this.webReq==null)
                {
                    this.webReq = base.GetWebRequest(url);
                }
                return this.webReq;
            }

            protected override void ExecuteRequestWithBody(HttpWebRequest req)
            {

            }


            protected override void ExecuteRequestWithoutBody(HttpWebRequest req)
            {

            }


            public HttpWebRequest WebRequest
            {
                get { return this.webReq; }
            }
        }

        public class RequestWrapper2 : Request
        {
            public void ExWithBody(HttpWebRequest req) 
            {
                this.ExecuteRequestWithBody(req);
            }

            public void ExWithoutBody(HttpWebRequest req)
            {
                this.ExecuteRequestWithoutBody(req);
            }
        }


        

        [TestMethod]
        public void TestProcessCallbackFailsOnWebException() 
        {
            Mutex requestMutex = new Mutex();
            
            webRequestMock.Setup(w => w.EndGetResponse(It.IsAny<IAsyncResult>())).Throws(new WebException());
            RequestWrapper req = new RequestWrapper();
            AsyncCallback cb = req.ProcessCallback((headers, stream) =>
            {
                Assert.Fail();
            },
            (webex) =>
            {
                Assert.IsTrue(true);
                requestMutex.ReleaseMutex();
            });
            requestMutex.WaitOne();
            cb.Invoke(result);
        }




        [TestMethod]
        public void TestProcessCallbackContainsStreamOnResponse()
        {
            Stream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write("testing");
            sw.Flush();

            ms.Seek(0, SeekOrigin.Begin);



            webResponseMock.Setup((r) => r.Headers).Returns(new WebHeaderCollection());
            webResponseMock.Setup((r) => r.GetResponseStream()).Returns(ms);

            
            Mutex requestMutex = new Mutex();

            RequestWrapper req = new RequestWrapper();
            AsyncCallback cb = req.ProcessCallback((headers, stream) =>
            {
                StreamReader sr = new StreamReader(stream);
                String contents  = sr.ReadToEnd();

                Assert.AreEqual("testing", contents);
                requestMutex.ReleaseMutex();
            },
            (webex) =>
            {
                Assert.Fail();
            });
            requestMutex.WaitOne();
            cb.Invoke(result);
        }



        [TestMethod]
        public void TestProcessCallbackContainsHeadersOnResponse()
        {
            WebHeaderCollection hc = new WebHeaderCollection();

            webResponseMock.Setup((r) => r.Headers).Returns(hc);
            webResponseMock.Setup((r) => r.GetResponseStream()).Returns(new MemoryStream());


            Mutex requestMutex = new Mutex();

            RequestWrapper req = new RequestWrapper();
            AsyncCallback cb = req.ProcessCallback((headers, stream) =>
            {
                Assert.AreEqual(hc, headers);
                requestMutex.ReleaseMutex();
            },
            (webex) =>
            {
                Assert.Fail();
            });
            requestMutex.WaitOne();
            cb.Invoke(result);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMakeRequestThrowsExceptionWhenUrlNull()
        {
            RequestWrapper req = new RequestWrapper();
            req.MakeRequest(HttpVerb.Get, null, null, null, null);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMakeRequestThrowsExceptionWithBodyWhenUrlNull()
        {
            RequestWrapper req = new RequestWrapper();
            req.MakeRequest(HttpVerb.Post, null, null, null, null);

        }


        [TestMethod]
        public void TestDefaultMethodIsGet()
        {
            Request r = new Request();
            Assert.AreEqual(HttpVerb.Get,r.Method);
        }

        [TestMethod]
        public void TestRequestCreatesNewRequestwithInputtedUri()
        {
            RequestWrapper r = new RequestWrapper();
            r.MakeRequest(HttpVerb.Get, "http://test.com", null, null, null);
            Assert.AreEqual(new Uri("http://test.com").AbsoluteUri, r.WebRequest.RequestUri.AbsoluteUri);
        }

        [TestMethod]
        public void TestMethodIsPassedToHttpRequest()
        {
            webRequestMock.SetupSet(req => req.Method=It.IsAny<String>()).Verifiable();
            RequestWrapper r = new RequestWrapper(webRequestMock.Object);
            r.MakeRequest(HttpVerb.Head, "http://test.com", null, null, null);
            webRequestMock.VerifySet(req => req.Method = It.Is<String>(s => s.Equals("HEAD")));

        }

        [TestMethod]
        public void TestGetUrl()
        {
            Request req = new Request
            {
                Url = "a"
            };

            Assert.AreEqual("a", req.Url);

        }

        [TestMethod]
        public void TestGetMethod()
        {
            Request req = new Request
            {
                Method = HttpVerb.Get
            };

            Assert.AreEqual(HttpVerb.Get, req.Method);
        }


        [TestMethod]
        public void TestGetAction()
        {
            Mock<ActionProvider> prov = new Mock<ActionProvider>();
   
            Request req = new Request
            {
                Action = prov.Object
            };

            Assert.AreEqual(prov.Object, req.Action);
        }


        [TestMethod]
        public void TestGetHeader()
        {
            Mock<HeaderProvider> prov = new Mock<HeaderProvider>();

            Request req = new Request
            {
                Headers = prov.Object
            };

            Assert.AreEqual(prov.Object, req.Headers);
        }




        [TestMethod]
        public void TestGetAuth()
        {
            Mock<AuthenticationProvider> prov = new Mock<AuthenticationProvider>();

            Request req = new Request
            {
                Auth = prov.Object
            };

            Assert.AreEqual(prov.Object, req.Auth);
        }




        [TestMethod]
        public void TestSetUrl()
        {
            Request req = new Request();
            req.Url = "a";

            Assert.AreEqual("a", req.Url);

        }

        [TestMethod]
        public void TestSetMethod()
        {
            Request req = new Request();
            req.Method = HttpVerb.Delete;
            Assert.AreEqual(HttpVerb.Delete, req.Method);
        }


        [TestMethod]
        public void TestSetAction()
        {
            Mock<ActionProvider> prov = new Mock<ActionProvider>();

            Request req = new Request();
            req.Action = prov.Object;

            Assert.AreEqual(prov.Object, req.Action);
        }


        [TestMethod]
        public void TestSetHeader()
        {
            Mock<HeaderProvider> prov = new Mock<HeaderProvider>();

            Request req = new Request();
            req.Headers = prov.Object;

            Assert.AreEqual(prov.Object, req.Headers);
        }




        [TestMethod]
        public void TestSetAuth()
        {
            Mock<AuthenticationProvider> prov = new Mock<AuthenticationProvider>();

            Request req = new Request();
            req.Auth = prov.Object;


            Assert.AreEqual(prov.Object, req.Auth);
        }



        [TestMethod]
        public void TestWithBody()
        {
            Mock<HttpWebRequest> req = new Mock<HttpWebRequest>();
            RequestWrapper2 rw = new RequestWrapper2();
            rw.ExWithBody(req.Object);
        }

        [TestMethod]
        public void TestWithoutBody()
        {
            Mock<HttpWebRequest> req = new Mock<HttpWebRequest>();
            RequestWrapper2 rw = new RequestWrapper2();
            rw.ExWithBody(req.Object);
        }



        //Test cookies
        //Test method
        //Test GET/HEAD/OPTIONS method
        //Test POST/PUT body
        //Test Fail Callback on Exception
    }
}
