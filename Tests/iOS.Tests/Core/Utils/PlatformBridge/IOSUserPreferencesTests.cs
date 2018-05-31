using System;
using NUnit.Framework;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Core.Utils;

namespace iOS.Tests.Storage
{
    public class IOSUserPreferencesTests
    {
        private static readonly string StoreKey = "authState";
        private IUserPreferences Store;

        [SetUp]
        public void Setup()
        {
            Store = new IOSUserPreferences();
        }

        [Test]
        public void TestSaveRead()
        {
            string testValue = "test";
            Store.PutString(StoreKey, testValue);
            Assert.AreEqual(testValue, Store.GetString(StoreKey));
        }

        [Test]
        public void TestSaveNull()
        {
            Store.PutString(StoreKey, "test");
            Store.PutString(StoreKey, null);
            Assert.IsNull(Store.GetString(StoreKey));
        }

        [Test]
        public void TestRemove()
        {
            Store.PutString(StoreKey, "test");
            Store.RemoveValue(StoreKey);
            Assert.IsNull(Store.GetString(StoreKey));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSaveKeyNull()
        {
            Store.PutString(null, "test");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReadKeyNull()
        {
            Store.GetString(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveKeyNull()
        {
            Store.RemoveValue(null);
        }
    }
}
