using System;
using AeroGear.Mobile.Auth.Credentials;
using NUnit.Framework;

namespace Auth.Platform.Android.Tests.Credentials
{
    [TestFixture]
    public class CredentialsTest
    {
        private OIDCCredentialManager CredStore;
        private readonly string CredStoreKey = "authState";

        [SetUp]
        public void Setup()
        {
            CredStore = OIDCCredentialManager.Instance;
        }


        [TearDown]
        public void Tear() { }

        [Test]
        public void TestSaveReadSuccess()
        {
            ICredential credential = new OIDCCredential();

            CredStore.Save(CredStoreKey, credential);
            ICredential readCredential = CredStore.Read(CredStoreKey);

            Assert.AreEqual(credential.AccessToken, readCredential.AccessToken);
        }

        [Test]
        public void TestSaveNull()
        {
            ICredential credential = new OIDCCredential();

            CredStore.Save(CredStoreKey, credential);
            CredStore.Save(CredStoreKey, null);
            ICredential readCredential = CredStore.Read(CredStoreKey);

            Assert.AreEqual(null, credential.AccessToken);
        }

        [Test]
        public void TestRemoveSuccess()
        {
            ICredential credential = new OIDCCredential();

            CredStore.Save(CredStoreKey, credential);
            CredStore.Remove(CredStoreKey);
            ICredential readCredential = CredStore.Read(CredStoreKey);

            Assert.AreEqual(null, credential.AccessToken);
        }
    }
}