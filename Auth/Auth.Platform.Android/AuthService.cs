using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Auth;

[assembly: AeroGear.Mobile.Core.Utils.Service(typeof(AuthService))]
namespace AeroGear.Mobile.Auth
{
    /// <summary>
    /// <see cref="IAuthService"/> implementation for the Android platform.
    /// </summary>
    public class AuthService : AbstractAuthService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.AuthService"/> class.
        /// </summary>
        /// <param name="mobileCore">Mobile core.</param>
        /// <param name="serviceConfig">Service configuration.</param>
        public AuthService(MobileCore mobileCore, ServiceConfiguration serviceConfig) : base(mobileCore, serviceConfig)
        {
            var storageManager = new StorageManager("AeroGear.Mobile.Auth.Credentials", Android.App.Application.Context);
            CredentialManager = new CredentialManager(storageManager);
        }

        /// <summary>
        /// Configure the Android auth service module.
        /// </summary>
        /// <param name="authConfig">Auth config.</param>
        public override void Configure(AuthenticationConfig authConfig)
        {
            Authenticator = new OIDCAuthenticator(authConfig, KeycloakConfig, CredentialManager, MobileCore.HttpLayer, MobileCore.Logger);
        }

        /// <summary>
        /// Retrieves the current authenticated user. If there is no currently
        /// authenticated user then <c>null</c> is returned.
        /// </summary>
        /// <returns>The current user if authenticated. Else <c>null</c>.</returns>
        public override User CurrentUser()
        {
            var serializedCredential = CredentialManager.LoadSerialized();
            if (serializedCredential == null)
            {
                return null;
            }
            var parsedCredential = new OIDCCredential(serializedCredential);
            return User.NewUser().FromUnverifiedCredential(parsedCredential, KeycloakConfig.ResourceId);
        }

        /// <summary>
        /// Initializes the service and pass the configuration to be used to configure it
        /// </summary>
        /// <returns>The initialized service.</returns>
        /// <param name="core">The Mobile core instance. If <code>null</code> then <code>MobileCore.Instance</code> is used.</param>
        /// <param name="configuration">The service configuration. If <code>null</code> then <code>MobileCore.GetServiceConfiguration(Type)</code> is used.</param>
        public static IAuthService InitializeService(MobileCore core = null, ServiceConfiguration configuration = null)
        {
            return MobileCore.Instance.RegisterService<IAuthService>(new AuthService(core, configuration));
        }
    }
}