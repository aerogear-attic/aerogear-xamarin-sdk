using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Core.Utils;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth
{
    /// <summary>
    /// Abstract implementation of <see cref="IAuthService"/>.
    /// </summary>
    public abstract class AbstractAuthService : IAuthService
    {
        protected CredentialManager CredentialManager { get; set; }
        protected KeycloakConfig KeycloakConfig;
        protected AuthenticationConfig AuthenticationConfig { get; private set; }
        protected MobileCore MobileCore;
        protected IAuthenticator Authenticator { get; set; }
        public string Type => "keycloak";
        public bool RequiresConfiguration => true;

        public abstract User CurrentUser();

        public AbstractAuthService() 
        {
            var storageManager = ServiceFinder.Resolve<IStorageManager>();
            CredentialManager = new CredentialManager(storageManager);
        }

        /// <summary>
        /// Configure the service module.
        /// </summary>
        /// <param name="serviceConfig">Service configuration.</param>
        public void Configure(MobileCore mobileCore, ServiceConfiguration serviceConfig) 
        {
            MobileCore = mobileCore ?? MobileCore.Instance;
            var serviceConfiguration = NonNull(serviceConfig ?? MobileCore.GetServiceConfiguration(Type), "serviceConfig");
            KeycloakConfig = new KeycloakConfig(serviceConfiguration);    
        }

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

        public abstract void Init(AuthenticationConfig authConfig);
    }
}