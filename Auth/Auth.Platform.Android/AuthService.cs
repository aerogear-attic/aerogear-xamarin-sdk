using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Auth;
using Android.Content;
using AeroGear.Mobile.Core.Utils;
using OpenId.AppAuth;

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
            Core.Logger.Info("AuthService construct storage");
            CredentialManager = new CredentialManager(ServiceFinder.Resolve<IPlatformBridge>().GetUserPreferences("AeroGear.Mobile.Auth.Credentials"));
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
        /// 
        /// No check is performed if the credentials are expired or not.
        /// </summary>
        /// <returns>The current user if authenticated, <c>null</c> otherwise.</returns>
        public override User CurrentUser()
        {
            return CurrentUser(true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Retrieves the current authenticated user. If there is no currently
        /// authenticated user then <c>null</c> is returned.
        /// If <paramref name="autoRefresh"/> is <c>true</c>, then 
        /// the access token is automatically refreshed if the refresh token is not expired.
        /// If <paramref name="autoRefresh"/> is <c>true</c>, the access token needs to 
        /// be refreshed but the refresh is not possible, <c>null</c> is returned.
        /// </summary>
        /// <returns>The current user if authenticated, <c>null</c> otherwise.</returns>
        /// <param name="autoRefresh">Whether the access token should be silenty refreshed or not.</param>
        public override async Task<User> CurrentUser(bool autoRefresh)
        {
            var serializedCredential = CredentialManager.LoadSerialized();
            if (serializedCredential == null)
            {
                return null;
            }
            var parsedCredential = new OIDCCredential(serializedCredential);

            if (autoRefresh && parsedCredential.NeedsRenewal)
            {
                try 
                {
                    await parsedCredential.Refresh().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    // Credential needs renewal but we have not been able to refresh
                    return null;
                }

                CredentialManager.Store(parsedCredential);
            }

            User currentUser = User.NewUser().FromUnverifiedCredential(parsedCredential, KeycloakConfig.ResourceId);
            return currentUser;
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