using System;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth.Config
{
    /// <summary>
    /// Configurations for the authentication service
    /// </summary>
    public class AuthenticationConfig
    {
        /// <returns>the redirect uri used during the authentication process</returns>
        public Uri RedirectUri { get; private set; }

        private string _Scopes;

        private const string defaultScope = "openid";

        /// <returns>the OIDC scopes for the auth request. If no scopes are defined, the default 'openid' scope will be sent</returns>
        public string Scopes 
        { 
            get {
                return _Scopes;
            } 
            private set {
                if (value != null) {
                    _Scopes = value;
                } else {
                    _Scopes = defaultScope;
                }
            } 
        }

        /// <returns>the minimum time between Json Web Key Set (JWKS) requests in minutes. Default value is 1440 (1 day)</returns>
        public ushort MinTimeBetweenJwksRequests { get; private set; }

        /// <summary>
        /// Creates a new AuthenticationConfig object.
        /// </summary>
        /// <param name="redirectUri">Redirect URI</param>
        /// <param name="scopes">OIDC Scopes of auth request</param>
        /// <param name="minTimeBetweenJwksRequests">the minimum time between Json Web Key Set (JWKS) requests in minutes</param>
        private AuthenticationConfig(Uri redirectUri, string scopes, ushort minTimeBetweenJwksRequests)
        {
            RedirectUri = redirectUri;
            Scopes = scopes;
            MinTimeBetweenJwksRequests = minTimeBetweenJwksRequests;
        }

        /// <summary>Creates a new AuthenticationConfigBuilder instance.</summary>
        public static AuthenticationConfigBuilder Builder
        {
            get => new AuthenticationConfigBuilder();
        }

        /// <summary>Builds and returns an AuthenticationConfigBuilder object.</summary>
        public class AuthenticationConfigBuilder
        {
            private Uri redirectUri;
            private string scopes;
            private ushort minTimeBetweenJwksRequests = 24 * 60;

            /// <summary>Specify the redirect value</summary>
            /// <param name="redirectUri">redirect uri value</param>
            /// <returns>AuthenticationConfigBuilder instance</returns>
            public AuthenticationConfigBuilder RedirectUri(string redirectUri)
            {
                NonEmpty(redirectUri, "redirectUri");
                this.redirectUri = new Uri(redirectUri);
                return this;
            }

            /// <summary>Specify the OIDC scopes of the auth request.</summary>
            /// <param name="scopes">the OIDC scopes</param>
            /// <returns>AuthenticationConfigBuilder instance</returns>
            public AuthenticationConfigBuilder Scopes(string scopes)
            {
                this.scopes = scopes;
                return this;
            }

            /// <summary>Specify the minimum time between Json Web Key Set (JWKS) requests in minutes.</summary>
            /// <param name="minTimeBetweenJwksRequests">the minimum time between Json Web Key Set (JWKS) requests in minutes</param>
            /// <returns>AuthenticationConfigBuilder instance</returns>
            public AuthenticationConfigBuilder MinTimeBetweenJwksRequests(ushort minTimeBetweenJwksRequests)
            {
                this.minTimeBetweenJwksRequests = minTimeBetweenJwksRequests;
                return this;
            }

            /// <summary>Creates a new AuthenticationConfig object.</summary>
            /// <returns>an AuthenticationConfig object</returns>
            public AuthenticationConfig Build()
            {
                return new AuthenticationConfig(this.redirectUri, this.scopes, this.minTimeBetweenJwksRequests);
            }
        }
    }
}
