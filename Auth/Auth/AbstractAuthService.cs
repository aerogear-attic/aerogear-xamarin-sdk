using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth
{
    /// <summary>
    /// Abstract implementation of <see cref="IAuthService"/>.
    /// </summary>
    public abstract class AbstractAuthService : IAuthService
    {
        protected CredentialManager CredentialManager { get; set; }
        protected KeycloakConfig KeycloakConfig { get; set; }
        protected AuthenticationConfig AuthenticationConfig { get; private set; }
        protected MobileCore MobileCore { get; set; }
        protected IAuthenticator Authenticator { get; set; }
        public string Type => "keycloak";
        public bool RequiresConfiguration => true;

        public abstract User CurrentUser();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.AbstractAuthService"/> class.
        /// </summary>
        /// <param name="mobileCore">Mobile core.</param>
        /// <param name="serviceConfig">Service config.</param>
        public AbstractAuthService(MobileCore mobileCore = null, ServiceConfiguration serviceConfig = null)
        {
            MobileCore = mobileCore ?? MobileCore.Instance;
            var serviceConfiguration = NonNull(serviceConfig ?? MobileCore.GetServiceConfiguration(Type), "serviceConfig");
            KeycloakConfig = new KeycloakConfig(serviceConfiguration);
        }

        /// <summary>
        /// Configure the service module.
        /// </summary>
        /// <param name="authConfig">Authentication configuration.</param>
        public abstract void Configure(AuthenticationConfig authConfig);

        /// <summary>
        /// Initiate an authentication flow.
        /// </summary>
        /// <returns>The authenticate.</returns>
        /// <param name="authenticateOptions">Authenticate options.</param>
        public Task<User> Authenticate(IAuthenticateOptions authenticateOptions)
        {
            return Authenticator.Authenticate(authenticateOptions);
        }

        /// <summary>
        /// Perform teardown steps that should be done before destroying the instance.
        /// </summary>
        public void Destroy()
        {
        }

        /// <summary>
        /// Logout the specified user.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        /// <param name="user">The user to logout.</param>
        public Task<bool> Logout(User user)
        {
            return Authenticator.Logout(user);
        }
    }
}