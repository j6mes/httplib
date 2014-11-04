using JumpKick.HttpLib.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Tests.Provider
{
    [TestClass]
    public class MultiHeaderProviderTest
    {
        [TestMethod]
        public void TestGetHeadersGetsHeadersFromHeaderProvider1()
        {
            Mock<HeaderProvider> hp1 = new Mock<HeaderProvider>();
            Mock<HeaderProvider> hp2 = new Mock<HeaderProvider>();

            MultiHeaderProvider mhp = new MultiHeaderProvider(hp1.Object,hp2.Object);
            mhp.GetHeaders();

            hp1.Verify(a => a.GetHeaders());
        }

        [TestMethod]
        public void TestGetHeadersGetsHeadersFromHeaderProvider2()
        {
            Mock<HeaderProvider> hp1 = new Mock<HeaderProvider>();
            Mock<HeaderProvider> hp2 = new Mock<HeaderProvider>();

            MultiHeaderProvider mhp = new MultiHeaderProvider(hp1.Object, hp2.Object);
            mhp.GetHeaders();

            hp2.Verify(a => a.GetHeaders());
        }

        [TestMethod]
        public void TestGetHeadersReturnsHeadersFromHeaderProviders()
        {
            Mock<HeaderProvider> hp1 = new Mock<HeaderProvider>();
            Mock<HeaderProvider> hp2 = new Mock<HeaderProvider>();
            Header h1 = new Header("a", "b");
            Header h2 = new Header("a", "b");
            hp1.Setup(a => a.GetHeaders()).Returns(new Header[] { h1 });
            hp2.Setup(a => a.GetHeaders()).Returns(new Header[] { h2 });

            MultiHeaderProvider mhp = new MultiHeaderProvider(hp1.Object, hp2.Object);
            Header[] headers = mhp.GetHeaders();

            Assert.AreEqual(h1, headers[0]);
            Assert.AreEqual(h2, headers[1]);
        }


        [TestMethod]
        public void TestGetHeadersGetsHeadersFromHeaderProvider1List()
        {
            Mock<HeaderProvider> hp1 = new Mock<HeaderProvider>();
            Mock<HeaderProvider> hp2 = new Mock<HeaderProvider>();

            List<HeaderProvider> providers = new List<HeaderProvider>();
            providers.Add(hp1.Object);
            providers.Add(hp2.Object);

            MultiHeaderProvider mhp = new MultiHeaderProvider(providers);
            mhp.GetHeaders();

            hp1.Verify(a => a.GetHeaders());
        }

        [TestMethod]
        public void TestGetHeadersGetsHeadersFromHeaderProvider2List()
        {
            Mock<HeaderProvider> hp1 = new Mock<HeaderProvider>();
            Mock<HeaderProvider> hp2 = new Mock<HeaderProvider>();

            List<HeaderProvider> providers = new List<HeaderProvider>();
            providers.Add(hp1.Object);
            providers.Add(hp2.Object);

            MultiHeaderProvider mhp = new MultiHeaderProvider(providers);
            mhp.GetHeaders();

            hp2.Verify(a => a.GetHeaders());
        }

        [TestMethod]
        public void TestGetHeadersReturnsHeadersFromHeaderProvidersList()
        {
            Mock<HeaderProvider> hp1 = new Mock<HeaderProvider>();
            Mock<HeaderProvider> hp2 = new Mock<HeaderProvider>();
            Header h1 = new Header("a", "b");
            Header h2 = new Header("a", "b");
            hp1.Setup(a => a.GetHeaders()).Returns(new Header[] { h1 });
            hp2.Setup(a => a.GetHeaders()).Returns(new Header[] { h2 });

            List<HeaderProvider> providers = new List<HeaderProvider>();
            providers.Add(hp1.Object);
            providers.Add(hp2.Object);

            MultiHeaderProvider mhp = new MultiHeaderProvider(providers);
            Header[] headers = mhp.GetHeaders();

            Assert.AreEqual(h1, headers[0]);
            Assert.AreEqual(h2, headers[1]);
        }
       
    }
}
