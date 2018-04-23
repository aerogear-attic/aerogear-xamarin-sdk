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

        private AuthService()
        {
            this.serviceConfiguration = NonNull(MobileCore.Instance.GetServiceConfiguration(Type), "configuration");
            this.keycloakConfig = new KeycloakConfig(serviceConfiguration);
        }

        private AuthService(ServiceConfiguration configuration)
        {
            this.serviceConfiguration = NonNull(configuration, "configuration");
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
        /// <param name="configuration">The service configuration.</param>
        public static IAuthService InitializeService(ServiceConfiguration configuration)
        {
            return MobileCore.Instance.RegisterService<IAuthService>(new AuthService(configuration));
        }

        /// <summary>
        /// Initializes the service demanding to the SDK to retrieve the service configuration.
        /// </summary>
        /// <returns>The initialized service.</returns>
        public static IAuthService InitializeService()
        {
            return MobileCore.Instance.RegisterService<IAuthService>(new AuthService());
        }
    }
}
