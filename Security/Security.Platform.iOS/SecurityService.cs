using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Security
{
    public class SecurityService : AbstractSecurityService
    {
        private SecurityService() : base(IOSSecurityCheckFactory.INSTANCE)
        {
        }

        public static ISecurityService InitializeService(MobileCore core = null, ServiceConfiguration config = null)
        {
            return (core ?? MobileCore.Instance).RegisterService<ISecurityService>(new SecurityService());
        }
    }
}
