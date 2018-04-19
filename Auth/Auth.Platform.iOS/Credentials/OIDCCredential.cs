using System;
using Foundation;
using OpenId.AppAuth;

namespace AeroGear.Mobile.Auth.Credentials
{
    public class OIDCCredential : ICredential
    {
        private AuthState AuthState;

        public OIDCCredential(AuthState authState)
        {
            AuthState = authState;
        }

        public OIDCCredential(string serializedCredential)
        {
            NSData authData = new NSData(serializedCredential, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            AuthState = (AuthState)NSKeyedUnarchiver.UnarchiveObject(authData);
        }

        public string AccessToken => AuthState.LastTokenResponse.AccessToken;

        public string IdentityToken => AuthState.LastTokenResponse.IdToken;

        public string RefreshToken => AuthState.LastTokenResponse.RefreshToken;

        public bool IsAuthorized => AuthState.IsAuthorized;

        public bool IsExpired
        {
            get
            {
                NSDate expirationDate = AuthState.LastAuthorizationResponse.AccessTokenExpirationDate;
                NSDate now = new NSDate();
                return now.Compare(expirationDate) == NSComparisonResult.Ascending;
            }
        }

        public string SerializedCredential
        {
            get
            {
                NSData encodedData = NSKeyedArchiver.ArchivedDataWithRootObject(AuthState);
                return encodedData.GetBase64EncodedString(NSDataBase64EncodingOptions.EndLineWithLineFeed);
            }
        }
    }
}
