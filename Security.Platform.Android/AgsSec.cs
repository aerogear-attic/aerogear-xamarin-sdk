using System;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Security
{
    public class AgsSec
    {
        public AgsSec()
        {
            ServiceFinder.RegisterInstance<ISecurityCheckFactory>(new AndroidSecurityCheckFactory(Android.App.Application.Context));
        }

        // TODO: add code to access the security check executors
    }
}
