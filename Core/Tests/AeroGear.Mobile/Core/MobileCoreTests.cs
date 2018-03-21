using System;
using NUnit.Framework;

namespace Aerogear.Mobile.Core
{
    [TestFixture]
    public class MobileCoreTests
    {
        private MobileCore mobileCore;


        [SetUp]
        public void SetUp() {
            mobileCore = MobileCore.init();
        }


        [Test]
        public void TestCase()
        {
            Assert.Pass("Test Passes");
        }
    }
}
