using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            webResponseMock.Setup((r) => r.Headers).Returns(new WebHeaderCollection());
            webResponseMock.Setup((r) => r.GetResponseStream()).Returns(new MemoryStream());


            webRequestMock = new Mock<HttpWebRequest>();
            webRequestMock.Setup((w) => w.EndGetResponse(It.IsAny<IAsyncResult>())).Returns(webResponseMock.Object);


          
            asyncResultMock = new Mock<IAsyncResult>();
            asyncResultMock.Setup((r) => r.AsyncState).Returns(webRequestMock.Object);

          


            result = asyncResultMock.Object;
        }

        public class RequestWrapper : Request
        {
            public new AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
            {
                return base.ProcessCallback(success, fail);
            }
        }


        [TestMethod]
        public void TestProcessCallback() 
        {
            RequestWrapper w = new RequestWrapper();
            AsyncCallback cb = w.ProcessCallback((headers, stream) =>
            {
                
            },
            (webex) =>
            {

            });

            cb.Invoke(result);
        }
    }
}
