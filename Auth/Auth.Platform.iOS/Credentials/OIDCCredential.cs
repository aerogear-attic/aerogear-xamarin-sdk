using System;
using Foundation;
using OpenId.AppAuth;

namespace AeroGear.Mobile.Auth.Credentials
{
    public class OIDCCredential : ICredential
    {
        private AuthState AuthState;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> class.
        /// </summary>
        /// <param name="authState">Auth state backing the credential.</param>
        public OIDCCredential(AuthState authState)
        {
            AuthState = authState;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> class
        /// using the specified serialized credential.
        /// </summary>
        /// <param name="serializedCredential">Serialized credential.</param>
        public OIDCCredential(string serializedCredential)
        {
            NSData authData = new NSData(serializedCredential, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            AuthState = (AuthState)NSKeyedUnarchiver.UnarchiveObject(authData);
        }
        
        /// <summary>
        /// Retrieve the access token of the credential.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken => AuthState.LastTokenResponse.AccessToken;
        
        /// <summary>
        /// Retrieve the identity token of the credential.
        /// </summary>
        /// <value>The identity token.</value>
        public string IdentityToken => AuthState.LastTokenResponse.IdToken;
        
        /// <summary>
        /// Retrieve the refresh token of the credential.
        /// </summary>
        /// <value>The refresh token.</value>
        public string RefreshToken => AuthState.LastTokenResponse.RefreshToken;
        
        /// <summary>
        /// Whether the credential <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> is authorized.
        /// </summary>
        /// <value><c>true</c> if authorized. Else <c>false</c>.</value>
        public bool IsAuthorized => AuthState.IsAuthorized;
        
        /// <summary>
        /// Whether the credential <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> is expired.
        /// </summary>
        /// <value><c>true</c> if expired. Else <c>false</c>.</value>
        public bool IsExpired
        {
            get
            {
                NSDate expirationDate = AuthState.LastAuthorizationResponse.AccessTokenExpirationDate;
                NSDate now = new NSDate();
                return now.Compare(expirationDate) == NSComparisonResult.Ascending;
            }
        }
        
        /// <summary>
        /// Retrieve a serialized string representation of the credential.
        /// </summary>
        /// <value>The serialized credential.</value>
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
