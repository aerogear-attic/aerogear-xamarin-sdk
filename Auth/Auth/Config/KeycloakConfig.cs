using System;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Exception;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth.Config
{
    /// <summary>
    /// Represents Keycloak configuration.
    /// </summary>
    public class KeycloakConfig
    {
        private const string ServerUrlName = "auth-server-url";
        private const string RealmIdName = "realm";
        private const string ResourceIdName = "resource";

        private const string TokenHintFragment = "id_token_hint";
        private const string RedirectFragment = "redirect_uri";

        private const string BaseUrlTemplate = "{0}/realms/{1}/protocol/openid-connect";
        private const string LogoutUrlTemplate = "{0}/logout?{1}={2}&{3}={4}";
        private const string IssuerTemplate = "{0}/realms/{1}";

        private readonly string BaseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Config.KeycloakConfig"/> class.
        /// </summary>
        /// <param name="configuration">the Keycloak <see cref="ServiceConfiguration"/>.</param>
        public KeycloakConfig(ServiceConfiguration configuration)
        {
            NonNull(configuration, "configuration");
            if (configuration.Id != "keycloak")
            {
                throw new ConfigurationNotFoundException("Missing Keycloak configuration");
            }
            HostUrl = configuration[ServerUrlName];
            RealmName = configuration[RealmIdName];
            ResourceId = configuration[ResourceIdName];
            BaseUrl = String.Format(BaseUrlTemplate, HostUrl, RealmName);

        }

        /// <summary>
        /// Gets the keycloak server URL.
        /// </summary>
        /// <value>The host URL.</value>
        public string HostUrl { get; private set; }

        /// <summary>
        /// Gets the name of the realm.
        /// </summary>
        /// <value>The name of the realm.</value>
        public string RealmName { get; private set; }

        /// <summary>
        /// Gets the resource identifier.
        /// </summary>
        /// <value>The resource identifier.</value>
        public string ResourceId { get; private set; }

        /// <summary>
        /// Gets the authentication endpoint.
        /// </summary>
        /// <value>The authentication endpoint.</value>
        public Uri AuthenticationEndpoint
        {
            get { return new Uri(BaseUrl + "/auth"); }
        }

        /// <summary>
        /// Gets the token endpoint.
        /// </summary>
        /// <value>The token endpoint.</value>
        public Uri TokenEndpoint
        {
            get { return new Uri(BaseUrl + "/token"); }
        }

        /// <summary>
        /// Gets the Json Web Key Set (JWKS) URL.
        /// </summary>
        /// <value>The jwks URL.</value>
        public string JwksUrl
        {
            get { return BaseUrl + "/certs"; }
        }

        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get { return String.Format(IssuerTemplate, HostUrl, RealmName); }
        }

        /// <summary>
        /// Constructs the logout the URL.
        /// </summary>
        /// <returns>The logout URL.</returns>
        /// <param name="identityToken">Identity token.</param>
        /// <param name="redirectUri">Redirect URI.</param>
        public string LogoutUrl(string identityToken, string redirectUri)
        {
            return String.Format(LogoutUrlTemplate, BaseUrl, TokenHintFragment, identityToken, RedirectFragment, redirectUri);
        }
    }
}