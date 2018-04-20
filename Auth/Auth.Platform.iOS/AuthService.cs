using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Auth
{
    public class AuthService : IServiceModule
    {
        public AuthService()
        {
        }

        public string Type => throw new NotImplementedException();

        public bool RequiresConfiguration => throw new NotImplementedException();

        public void Configure(MobileCore core, ServiceConfiguration serviceConfiguration)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
