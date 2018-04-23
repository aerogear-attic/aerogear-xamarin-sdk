using System;
using NUnit.Framework;
using AeroGear.Mobile.Core.Storage;

namespace iOS.Tests.Storage
{
    public class StorageManagerTests
    {
        private static readonly string StoreKey = "authState";
        private StorageManager Store;

        [SetUp]
        public void Setup()
        {
            Store = new StorageManager("org.aerogear.Core-Platform-iOS-Tests");
        }

        [Test]
        public void TestSaveRead()
        {
            string testValue = "test";
            Store.Save(StoreKey, testValue);
            Assert.AreEqual(testValue, Store.Read(StoreKey));
        }

        [Test]
        public void TestSaveNull()
        {
            Store.Save(StoreKey, "test");
            Store.Save(StoreKey, null);
            Assert.IsNull(Store.Read(StoreKey));
        }

        [Test]
        public void TestRemove()
        {
            Store.Save(StoreKey, "test");
            Store.Remove(StoreKey);
            Assert.IsNull(Store.Read(StoreKey));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSaveKeyNull()
        {
            Store.Save(null, "test");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReadKeyNull()
        {
            Store.Read(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveKeyNull()
        {
            Store.Remove(null);
        }
    }
}
