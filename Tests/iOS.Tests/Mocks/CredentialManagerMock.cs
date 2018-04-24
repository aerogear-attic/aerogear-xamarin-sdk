using System;
using AeroGear.Mobile.Auth.Credentials;

namespace AeroGear.Mobile.iOS.Tests.Mocks
{
    public class CredentialManagerMock : ICredentialManager
    {
        private readonly string LoadSerializedReturn;

        public CredentialManagerMock(string loadSerialized = null)
        {
            this.LoadSerializedReturn = loadSerialized;
        }

        public void Clear()
        {
        }

        public string LoadSerialized()
        {
            return LoadSerializedReturn;
        }

        public void Store(ICredential credential)
        {
        }
    }
}
