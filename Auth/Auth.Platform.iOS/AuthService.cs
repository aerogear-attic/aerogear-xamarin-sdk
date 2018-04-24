using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Storage;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth
{
    public class AuthService : AbstractAuthService
    {
        private AuthService(MobileCore mobileCore = null, ServiceConfiguration serviceConfig = null) : base(mobileCore, serviceConfig)
        {
            var storageManager = new StorageManager("AeroGear.Mobile.Auth.Credentials");
            CredentialManager = new CredentialManager(storageManager);
        }

        public override void Configure(AuthenticationConfig authConfig)
        {
            // TODO: Initialize the authenticator here.
        }

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
