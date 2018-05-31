using System;
using AeroGear.Mobile.Core.Utils;
using Android.App;
using NUnit.Framework;

namespace AeroGear.Mobile.Android.Tests.Core.Storage
{
    [TestFixture]
    public class StorageManagerTests
    {
        private static readonly string StoreKey = "AuthState";
        private IUserPreferences StorageManager;

        [SetUp]
        public void Setup()
        {
            StorageManager = new AndroidUserPreferences(Application.Context.ApplicationContext, "StorageManagerTest");
        }

        [Test]
        public void TestSaveRead()
        {
            string testValue = "test";

            StorageManager.PutString(StoreKey, testValue);
            string retrievedValue = StorageManager.GetString(StoreKey);
            Assert.AreEqual(testValue, retrievedValue);
        }

        [Test]
        public void TestSaveNull()
        {
            StorageManager.PutString(StoreKey, "test");
            StorageManager.PutString(StoreKey, null);
            Assert.IsNull(StorageManager.GetString(StoreKey));
        }

        [Test]
        public void TestRemove()
        {
            StorageManager.PutString(StoreKey, "test");
            StorageManager.RemoveValue(StoreKey);
            Assert.IsNull(StorageManager.GetString(StoreKey));
        }
    }
}
