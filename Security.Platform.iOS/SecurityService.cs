using System;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security;

namespace Security.Platform
{
    public class SecurityService
    {
        public SecurityService()
        {
            ServiceFinder.RegisterInstance<ISecurityCheckFactory>(new IOSSecurityCheckFactory());
        }

        // TODO: add code to access the security check executors
    }
}
