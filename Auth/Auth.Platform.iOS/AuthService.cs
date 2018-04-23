using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Auth
{
    public class AuthService : IAuthService
    {
        public AuthService()
        {
        }

        public string Type => throw new NotImplementedException();

        public bool RequiresConfiguration => throw new NotImplementedException();

        public void Configure(MobileCore core, ServiceConfiguration serviceConfiguration)
        {
        }

        public void Destroy()
        {
        }

        public void Init()
        {
            ServiceFinder.RegisterType<IAuthService, AuthService>();
        }
    }
}
