using System;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security;

namespace Security.Platform
{
    public class AgsSec
    {
        public AgsSec()
        {
            ServiceFinder.RegisterInstance<ISecurityCheckFactory>(new IOSSecurityCheckFactory());
        }

        // TODO: add code to access the security check executors
    }
}
