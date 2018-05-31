using System;
using AeroGear.Mobile.Core.Utils;
using Android.App;
using NUnit.Framework;

namespace AeroGear.Mobile.Android.Tests.Core.Storage
{
    [TestFixture]
    public class AndroidUserPreferencesTests
    {
        private static readonly string StoreKey = "AuthState";
        private IUserPreferences userPreferences;

        [SetUp]
        public void Setup()
        {
            userPreferences = new AndroidUserPreferences(Application.Context.ApplicationContext, "StorageManagerTest");
        }

        [Test]
        public void TestSaveRead()
        {
            string testValue = "test";

            userPreferences.PutString(StoreKey, testValue);
            string retrievedValue = userPreferences.GetString(StoreKey);
            Assert.AreEqual(testValue, retrievedValue);
        }

        [Test]
        public void TestSaveNull()
        {
            userPreferences.PutString(StoreKey, "test");
            userPreferences.PutString(StoreKey, null);
            Assert.IsNull(userPreferences.GetString(StoreKey));
        }

        [Test]
        public void TestRemove()
        {
            userPreferences.PutString(StoreKey, "test");
            userPreferences.RemoveValue(StoreKey);
            Assert.IsNull(userPreferences.GetString(StoreKey));
        }
    }
}
