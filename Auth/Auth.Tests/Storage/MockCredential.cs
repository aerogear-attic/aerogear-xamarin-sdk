using System;
using AeroGear.Mobile.Auth.Credentials;

namespace Auth.Tests.Storage
{
	public class MockCredential : ICredential
    {
        public MockCredential()
        {
        }

		public string AccessToken => "testAccessToken";

		public string IdentityToken => "testIdentityToken";

		public string RefreshToken => "testRefreshToken";

		public bool IsAuthorized => true;

		public bool IsExpired => false;

		public string SerializedCredential => "testSerialized";
	}
}
