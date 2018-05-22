using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Auth;
using Android.Content;
using AeroGear.Mobile.Core.Utils;

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
        public AuthService(MobileCore mobileCore = null, ServiceConfiguration serviceConfig = null) : base(mobileCore, serviceConfig)
        {
            Core.Logger.Info("AuthService construct start");
            var storageManager = new StorageManager("AeroGear.Mobile.Auth.Credentials", Android.App.Application.Context);
            Core.Logger.Info("AuthService construct storage");
            CredentialManager = new CredentialManager(storageManager);
            Core.Logger.Info("AuthService construct credential manager");
        }

        /// <summary>
        /// Provide authentication configuration to the service.
        /// </summary>
        /// <param name="authConfig">Authentication config.</param>
        public override void Configure(AuthenticationConfig authConfig)
        {
            Authenticator = new OIDCAuthenticator(authConfig, KeycloakConfig, CredentialManager, Core.HttpLayer, Core.Logger);
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
        /// Handles the auth result.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        /// <param name="data">Intent.</param>
        public Task HandleAuthResult(Intent data)
        {
            return ((OIDCAuthenticator)Authenticator).HandleAuthResult(data);
        }

        /// <summary>
        /// Initializes the service and pass the configuration to be used to configure it
        /// </summary>
        /// <returns>The initialized service.</returns>
        /// <param name="core">The Mobile core instance. If <code>null</code> then <code>MobileCore.Instance</code> is used.</param>
        /// <param name="config">The service configuration. If <code>null</code> then <code>MobileCore.GetServiceConfiguration(Type)</code> is used.</param>
        public static IAuthService InitializeService(MobileCore core = null, ServiceConfiguration config = null)
        {
            return (core ?? MobileCore.Instance).RegisterService<IAuthService>(new AuthService(core, config));
        }
    }
}