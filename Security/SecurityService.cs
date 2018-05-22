using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Security
{
    public class SecurityService : IServiceModule
    {
        public SecurityService()
        {
        }

        public string Type => "security";

        public bool RequiresConfiguration => false; // this is useless

        public string Id => null;

        public void Configure(MobileCore core, ServiceConfiguration config)
        {
        }

        // If the lifecycle is not managed by MobileCore, who will invoke this?
        public void Destroy() 
        {
        }
    }
}
