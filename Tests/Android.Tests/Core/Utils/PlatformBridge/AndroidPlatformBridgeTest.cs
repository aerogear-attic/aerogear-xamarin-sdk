using System;
using AeroGear.Mobile.Core.Utils;
using Android.App;
using NUnit.Framework;

namespace AeroGear.Mobile.Android.Core.Utils.Tests
{
    public class AndroidPlatformBridgeTest
    {
        private IPlatformBridge bridge = new AndroidPlatformBridge(Application.Context.ApplicationContext);

        public AndroidPlatformBridgeTest()
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
            Assert.AreEqual("Core.Platform.Android.Tests", bridge.ApplicationRuntimeInfo.Identifier);
            Assert.AreEqual("1.0", bridge.ApplicationRuntimeInfo.Version);
        }

        [Test]
        public void PlatformInfoTest()
        {
            Assert.AreEqual("android", bridge.PlatformInfo.Name);
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
