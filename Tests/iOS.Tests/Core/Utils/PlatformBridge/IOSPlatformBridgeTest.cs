using System;
using AeroGear.Mobile.Core.Utils;
using NUnit.Framework;

namespace iOS.Tests.Core.Utils.Tests
{
    [TestFixture]
    public class IOSPlatformBridgeTest
    {
        private IPlatformBridge bridge = new IOSPlatformBridge();

        public IOSPlatformBridgeTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            bridge.GetUserPreferences().RemoveValue("test");
        }

        [Test]
        public void ApplicationRuntimeInfoTest()
        {
            Assert.AreEqual("iOS.Tests", bridge.ApplicationRuntimeInfo.Identifier);
            Assert.AreEqual("1.0", bridge.ApplicationRuntimeInfo.Version);
        }

        [Test]
        public void PlatformInfoTest()
        {
            Assert.AreEqual("iOS", bridge.PlatformInfo.Name);
        }

        [Test]
        public void UserPreferencesTest()
        {
            IUserPreferences preferences = bridge.GetUserPreferences();
            Assert.IsNull(preferences.GetString("test"));
            Assert.AreEqual("testvalue", preferences.GetString("test", "testvalue"));
            preferences.PutString("test", "storedvalue");
            Assert.AreEqual("storedvalue", preferences.GetString("test", "testvalue"));
        }
    }
}
