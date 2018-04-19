using System;
using NUnit.Framework;
using AeroGear.Mobile.Core.Storage;

namespace Core.Platform.iOS.Tests.Storage
{
    [TestFixture]
    public class KeychainWrapperTests
    {
        private static readonly string StoreKey = "authState";
        private KeychainWrapper Keychain;

        [SetUp]
        public void Setup()
        {
            Keychain = new KeychainWrapper("org.aerogear.Core-Platform-iOS-Tests");
        }

        [Test]
        public void TestSaveLoad()
        {
            string testValue = "test";
            Keychain.Save(StoreKey, testValue);
            Assert.AreEqual(testValue, Keychain.Load(StoreKey));
        }

        [Test]
        public void TestRemoveSuccess()
        {
            Keychain.Save(StoreKey, "test");
            bool result = Keychain.Remove(StoreKey);
            Assert.True(result);
        }

        [Test]
        public void TestRemoveFailure()
        {
            bool result = Keychain.Remove("emptyState");
            Assert.False(result);
        }
    }
}
