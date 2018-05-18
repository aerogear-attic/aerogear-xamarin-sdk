using System;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Security
{
    public class SecurityService
    {
        public SecurityService()
        {
            ServiceFinder.RegisterInstance<ISecurityCheckFactory>(new AndroidSecurityCheckFactory(Android.App.Application.Context));
        }

        // TODO: add code to access the security check executors
    }
}
