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
            this.serviceConfiguration = nonNull(MobileCore.Instance.GetServiceConfiguration(Type), "configuration");
            this.keycloakConfig = new KeycloakConfig(serviceConfiguration);
        }

        private AuthService(ServiceConfiguration configuration)
        {
            this.serviceConfiguration = nonNull(configuration, "configuration");
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

        public static IAuthService InitializeService(ServiceConfiguration configuration)
        {
            return MobileCore.Instance.RegisterService<IAuthService>(new AuthService(configuration));
        }

        public static IAuthService InitializeService()
        {
            return MobileCore.Instance.RegisterService<IAuthService>(new AuthService());
        }
    }
}
