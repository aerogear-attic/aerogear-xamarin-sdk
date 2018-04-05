using System;
using System.Reflection;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;
using NUnit.Framework;

namespace Aerogear.Mobile.Core
{
    [TestFixture(Category ="Core")]
    public class MobileCoreTests
    {
        private MobileCore mobileCore;
        private IPlatformInjector platformInjector;

        private class TestInjector : IPlatformInjector
        {
            public Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();

            public string PlatformName => "Test";

            public string DefaultResources => throw new NotImplementedException();

            public ILogger CreateLogger() => new NullLogger();
        }


        [SetUp]
        public void SetUp() {
            
        }


        [Test]
        public void TestCoreInit()
        {
            MobileCore.Init(new TestInjector());
            Assert.NotNull(MobileCore.Instance);
        }
    }
}
