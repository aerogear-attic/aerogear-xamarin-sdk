using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Auth.Authenticator;
using static Auth.Platform.Authenticator.extensions.TokenLifecycleManagerExtensions;
using Foundation;
using OpenId.AppAuth;

namespace AeroGear.Mobile.Auth.Credentials
{
    /// <summary>
    /// Credential for OpenID Connect. This contains the access, identity and
    /// refresh tokens and other metadata for the credential.
    /// </summary>
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
        public string RefreshToken => AuthState.RefreshToken;
        
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
                NSDate expirationDate = AuthState.LastTokenResponse.AccessTokenExpirationDate;
                NSDate now = new NSDate();
                return expirationDate.Compare(now) == NSComparisonResult.Ascending;
            }
        }

        public bool NeedsRenewal => this.IsExpired;

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

        /// <summary>
        /// Refresh the current credentials if a valid refresh token is present.
        /// </summary>
        /// <returns>The refresh.</returns>
        public async Task Refresh()
        {
            if (RefreshToken == null)
            {
                throw new Exception("currentCredentials did not have a refresh token");
            }

            TokenLifecycleManager tlcm = new TokenLifecycleManager();
            TokenResponse tokenResponse = await tlcm.RefreshTokenAsync(AuthState.TokenRefreshRequest()).ConfigureAwait(false);

            AuthState.Update(tokenResponse, null);
        }
    }
}
