using System;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth
{
    public class AuthService : IAuthService
    {
        private readonly KeycloakConfig keycloakConfig;
        private readonly ServiceConfiguration serviceConfiguration;
        private readonly MobileCore core;

        private AuthService(MobileCore core = null, ServiceConfiguration configuration = null)
        {
            this.core = core ?? MobileCore.Instance;
            this.serviceConfiguration = NonNull(configuration ?? core.GetServiceConfiguration(Type), "configuration");
            this.keycloakConfig = new KeycloakConfig(serviceConfiguration);
        }

        public string Type => "keycloak";

        public bool RequiresConfiguration => true;

        public void Configure(MobileCore core, ServiceConfiguration serviceConfiguration)
        {
        }

        public void Destroy()
        {
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
