using System.Threading.Tasks;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Auth
{
    /// <summary>
    /// Auth service implementation for iOS platform
    /// </summary>
    public class AuthService : AbstractAuthService
    {
        public AuthService(MobileCore mobileCore = null, ServiceConfiguration serviceConfig = null) : base(mobileCore, serviceConfig)
        {
            CredentialManager = new CredentialManager(ServiceFinder.Resolve<IPlatformBridge>().GetUserPreferences("AeroGear.Mobile.Auth.Credentials"));
        }

        /// <summary>
        /// Provide authentication configuration to the service.
        /// </summary>
        /// <param name="authConfig">Authentication config.</param>
        public override void Configure(AuthenticationConfig authConfig)
        {
            Authenticator = new OIDCAuthenticator(authConfig, KeycloakConfig, CredentialManager, Core.HttpLayer, Core.Logger);
        }

        public override User CurrentUser()
        {
            return null;
        }

        /// <summary>
        /// Retrieve the current user.
        /// </summary>
        /// <returns>The current user.</returns>
        public override Task<User> CurrentUser(bool autoRefresh)
        {
            return null;
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