﻿using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Auth.Credentials;
using Auth.Platform.Authenticator;
using OpenId.AppAuth;

namespace AeroGear.Mobile.Auth.Credentials
{
    /// <summary>
    /// Credential for OpenID Connect. This contains the access, identity and
    /// refresh tokens and other metadata for the credential.
    /// </summary>
    public class OIDCCredential : ICredential
    {
        /// <summary>
        /// The <see cref="AuthState"/> that backs the wrapping credential.
        /// </summary>
        internal AuthState AuthState;

        /// <summary>
        /// Retrieve a JSON form of the OpenID Connect credential.
        /// </summary>
        /// <value>Serialized credential.</value>
        public string SerializedCredential => AuthState.JsonSerializeString();

        /// <summary>
        /// Retrieve the access token of the OpenID Connect credential.
        /// </summary>
        /// <value>OpenID Connect access token.</value>
        public string AccessToken => AuthState.AccessToken;

        /// <summary>
        /// Get the identity token of the OpenID Connect credential.
        /// </summary>
        /// <value>OpenID Connect identity token.</value>
        public string IdentityToken => AuthState.IdToken;

        /// <summary>
        /// Retrieve the refresh token of the OpenID Connect credential.
        /// </summary>
        /// <value>OpenID Connect refresh token.</value>
        public string RefreshToken => AuthState.RefreshToken;

        /// <summary>
        /// Whether this <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> is authorized.
        /// </summary>
        /// <value><c>true</c> if is authorized; otherwise, <c>false</c>.</value>
        public bool IsAuthorized => AuthState.IsAuthorized;

        /// <summary>
        /// Whether this <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> is expired.
        /// </summary>
        /// <value><c>true</c> if is expired; otherwise, <c>false</c>.</value>
        public bool IsExpired => AuthState.HasClientSecretExpired;

        public bool NeedsRenewal => AuthState.NeedsTokenRefresh;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> class
        /// using the provided serialized form of the credential.
        /// </summary>
        /// <param name="serializedCredential">Serialized credential <see cref="SerializedCredential"/>.</param>
        internal OIDCCredential(string serializedCredential)
        {
            AuthState = AuthState.JsonDeserialize(serializedCredential);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Credentials.OIDCCredential"/> class.
        /// </summary>
        public OIDCCredential()
        {
            AuthState = new AuthState();
        }

        public async Task Refresh() {

            if (RefreshToken == null)
            {
                throw new Exception("currentCredentials did not have a refresh token");
            }

            try
            {
                var requestTask = new TokenRequestTaskNative(AuthState.CreateTokenRefreshRequest());
                TokenResponse tokenResponse = await requestTask.RequestAsync().ConfigureAwait(false);
                AuthState.Update(tokenResponse, null);
            }
            catch (AuthorizationException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
