using JumpKick.HttpLib.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class NonActionProviderTest
    {
        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void TestFailMethodThrowsException()
        {
            NonActionProvider nap = new NonActionProvider();
            nap.Fail(new WebException());
        }

        [TestMethod]
        public void TestSuccessNoErr()
        {
            NonActionProvider nap = new NonActionProvider();
            nap.Success(new WebHeaderCollection(), new MemoryStream());
        }
    }
}
