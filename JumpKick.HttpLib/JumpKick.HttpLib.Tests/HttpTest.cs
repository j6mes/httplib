using JumpKick.HttpLib.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestThrowsExceptionWithInvalidURL()
        {
            String httpUrl = "http://testurl.com9/testes";
            Http.Post(httpUrl).Form(new { admin = "admin", password = "password" }).OnSuccess(response =>
             {
                 if (response == null)
                 {
                     throw new NullReferenceException("null response");
                 }

             }).Go();
        }

        [TestMethod]
        public void TestDeleteMethodWithoutBodyDoesntThrowException()
        {
            Http.Delete("http://test.com").Go();


        }

        [TestMethod]
        public void TestMakeActionIsEnter()
        {
            bool isEnter = false;
            Http.Get("http://test.com").OnMake((hwr) =>
            {
                hwr.AddRange(0, 100);
                isEnter = true;
            }).Go();
            Assert.IsTrue(isEnter);
        }

        private async void DoGoAsyncTest(SemaphoreSlim sem)
        {
            int doneCount = 0;
            for (int i = 0; i < 10; i++)
            {
                await Http.Get("https://bing.com").OnMake((req) => { req.Timeout = 50; })
                    .OnSuccess((text) =>
                    {
                        Assert.IsNotNull(text);
                        Assert.IsTrue(text.Length > 0);
                        doneCount++;
                    }).OnFail((e) =>
                    {
                        doneCount++;
                    }).GoAsync();

                //should be executed once time and done once time
                Assert.IsTrue(doneCount == i + 1);
            }

            //notify all test done
            sem.Release();
        }

        [TestMethod]
        public void TestGoAsync()
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(0);
            Task.Run(() =>
            {
                DoGoAsyncTest(semaphore);
            });

            //wait DoGoAsyncTest() done
            semaphore.Wait();
        }

        private async void DoGoAsyncErrorTest(SemaphoreSlim sem)
        {
            int doneCount = 0;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await Http.Get("1://123.com").GoAsync();
                }
                catch (Exception)
                {
                }

                doneCount++;
                //should be executed once time and done once time
                Assert.IsTrue(doneCount == i + 1);
            }

            //notify all test done
            sem.Release();
        }

        [TestMethod]
        public void TestGoAsyncError()
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(0);
            Task.Run(() =>
            {
                DoGoAsyncErrorTest(semaphore);
            });

            //wait DoGoAsyncErrorTest() done
            semaphore.Wait();
        }

        //download a image
        private string _downloadToTestURL = "https://ci.appveyor.com/api/projects/status/cfxsekd76ap47fej/branch/2.0.16";
        private string _downloadToTestFileName = "downloadfile";

        private void ClearDownloadToTestFile()
        {
            if (System.IO.File.Exists(_downloadToTestFileName))
                System.IO.File.Delete(_downloadToTestFileName);
        }

        [TestMethod]
        public void TestDownloadTo3Param()
        {
            ClearDownloadToTestFile();
            bool isDone = false;
            Http.Get(_downloadToTestURL).DownloadTo(_downloadToTestFileName,
                    onProgressChanged: (bytesCopied, totalBytes) => { },
                    onSuccess: (headers) =>
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(_downloadToTestFileName);
                        Assert.IsTrue(fi.Exists);
                        int len = Convert.ToInt32(headers["Content-Length"]);
                        Assert.IsTrue(fi.Length == len);
                        isDone = true;
                    }
                    ).OnFail((e) =>
                    {
                        Assert.Fail();
                        isDone = true;
                    }).Go();
            while (!isDone)
            {
                Thread.Sleep(100);
            }
            ClearDownloadToTestFile();

        }

        [TestMethod]
        public void TestDownloadTo2ParamAnd1Param()
        {
            int len = 0;//fileLen
            ClearDownloadToTestFile();
            bool isDone = false;
            Http.Get(_downloadToTestURL).DownloadTo(_downloadToTestFileName,
                    onSuccess: (headers) =>
                    {
                        System.IO.FileInfo fi2p = new System.IO.FileInfo(_downloadToTestFileName);
                        Assert.IsTrue(fi2p.Exists);
                        len = Convert.ToInt32(headers["Content-Length"]);
                        Assert.IsTrue(fi2p.Length == len);
                        isDone = true;
                    }
                    ).OnFail((e) =>
                    {
                        Assert.Fail();
                        isDone = true;
                    }).Go();
            while (!isDone)
            {
                Thread.Sleep(100);
            }
            ClearDownloadToTestFile();

            isDone = false;
            Http.Get(_downloadToTestURL).DownloadTo(_downloadToTestFileName).OnFail((e) =>
            {
                Assert.Fail();
                isDone = true;
            }).Go();

            for (int i = 0; i < 1000; i++)
            {
                if (isDone)
                {
                    break;
                }

                System.IO.FileInfo fi1p = new System.IO.FileInfo(_downloadToTestFileName);
                if (!fi1p.Exists || fi1p.Length != len)
                    Thread.Sleep(100); // wait download done;
                else
                    break;
            }

            System.IO.FileInfo fi = new System.IO.FileInfo(_downloadToTestFileName);
            Assert.IsTrue(fi.Length == len);

            ClearDownloadToTestFile();
        }

        private async void DoDownloadToGoAsyncTest(SemaphoreSlim sem)
        {
            int doneCount = 0;
            int len = 0;
            for (int i = 0; i < 4; i++)
            {
                ClearDownloadToTestFile();//delete file

                //DownloadTo() has 2 param
                await Http.Get(_downloadToTestURL).DownloadTo(_downloadToTestFileName, onSuccess: (headers) =>
                {
                    System.IO.FileInfo fi2p = new System.IO.FileInfo(_downloadToTestFileName);
                    Assert.IsTrue(fi2p.Exists);
                    len = Convert.ToInt32(headers["Content-Length"]);
                    Assert.IsTrue(fi2p.Length == len);

                }).OnFail((e) => { }).GoAsync();

                ClearDownloadToTestFile();//delete file

                //DownloadTo() has 1 param
                await Http.Get(_downloadToTestURL).DownloadTo(_downloadToTestFileName).OnFail((e) => { }).GoAsync();

                System.IO.FileInfo fi1p = new System.IO.FileInfo(_downloadToTestFileName);
                Assert.IsTrue(fi1p.Exists);
                Assert.IsTrue(fi1p.Length == len);

                doneCount++;
                //should be executed once time and done once time
                Assert.IsTrue(doneCount == i + 1);
            }

            ClearDownloadToTestFile();//delete file
            //notify all test done
            sem.Release();
        }

        [TestMethod]
        public void TestDownloadToGoAsync()
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(0);
            Task.Run(() =>
            {
                DoDownloadToGoAsyncTest(semaphore);
            });

            //wait DoGoAsyncTest() done
            semaphore.Wait();
        }


    }
}
