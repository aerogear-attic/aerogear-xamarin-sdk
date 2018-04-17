using System;
using AeroGear.Mobile.Auth.Credentials;
using NUnit.Framework;

namespace Auth.Tests.Storage
{
    public class CredentialManagerTest
    {
		private CredentialManager CredentialManager;

		[SetUp]
        public void Setup()
		{
			CredentialManager = new CredentialManager(new MockStorageManager());
		}

        [Test]
        public void TestStoreLoad()
		{
			CredentialManager.Store(new MockCredential());
			Assert.AreEqual("testSerialized", CredentialManager.LoadSerialized());
		}

        [Test]
        public void TestClear()
		{
			CredentialManager.Store(new MockCredential());
			CredentialManager.Clear();
			Assert.IsNull(CredentialManager.LoadSerialized());
		}
    }
}
