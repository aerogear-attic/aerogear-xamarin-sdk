using System;
using NUnit.Framework;
using AeroGear.Mobile.Core.Storage;

namespace iOS.Tests.Storage
{
    [TestFixture]
    public class KeychainWrapperTests
    {
        private static readonly string StoreKey = "authState";
		private SecureKeyValueStore KeyValueStore;

        [SetUp]
        public void Setup()
        {
			KeyValueStore = new SecureKeyValueStore("org.aerogear.Core-Platform-iOS-Tests");
        }

        [Test]
        public void TestSaveLoad()
        {
            string testValue = "test";
			KeyValueStore.Save(StoreKey, testValue);
			Assert.AreEqual(testValue, KeyValueStore.Load(StoreKey));
        }

        [Test]
        public void TestRemoveSuccess()
        {
			KeyValueStore.Save(StoreKey, "test");
			bool result = KeyValueStore.Remove(StoreKey);
            Assert.True(result);
        }

        [Test]
        public void TestRemoveFailure()
        {
			bool result = KeyValueStore.Remove("emptyState");
            Assert.False(result);
        }
    }
}
