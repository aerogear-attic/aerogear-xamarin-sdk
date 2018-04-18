using System;
using AeroGear.Mobile.Core.Storage;
using Android.App;
using NUnit.Framework;

namespace Core.Platform.Android.Tests.Storage
{
    [TestFixture]
    public class StorageManagerTests
    {
        private static readonly string StoreKey = "AuthState";
        private StorageManager StorageManager;

        [SetUp]
        public void Setup()
        {
            StorageManager = new StorageManager("StorageManagerTest", Application.Context.ApplicationContext);
        }

        [Test]
        public void TestSaveRead()
        {
            string testValue = "test";

            StorageManager.Save(StoreKey, testValue);
            string retrievedValue = StorageManager.Read(StoreKey);
            Assert.AreEqual(testValue, retrievedValue);
        }

        [Test]
        public void TestSaveNull()
        {
            StorageManager.Save(StoreKey, "test");
            StorageManager.Save(StoreKey, null);
            Assert.IsNull(StorageManager.Read(StoreKey));
        }

        [Test]
        public void TestRemove()
        {
            StorageManager.Save(StoreKey, "test");
            StorageManager.Remove(StoreKey);
            Assert.IsNull(StorageManager.Read(StoreKey));
        }
    }
}
