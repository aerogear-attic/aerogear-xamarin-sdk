using System;
using AeroGear.Mobile.Core;
using NUnit.Framework;

namespace Aerogear.Mobile.Core
{
    [TestFixture]
    public class MobileCoreTests
    {
        private MobileCore mobileCore;


        [SetUp]
        public void SetUp() {
            MobileCore.Init();
        }


        [Test]
        public void TestCase()
        {
            Assert.Pass("Test Passes");
        }
    }
}
