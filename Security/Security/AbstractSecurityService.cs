using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Executors;
using AeroGear.Mobile.Security.Executors.Sync;

namespace AeroGear.Mobile.Security
{
    public abstract class AbstractSecurityService : ISecurityService
    {
        public AbstractSecurityService(IDeviceCheckFactory checkFactory)
        {
            ServiceFinder.RegisterInstance<IDeviceCheckFactory>(checkFactory);
        }

        public string Type => "security";

        public bool RequiresConfiguration => false;

        public string Id => null;

        public void Configure(MobileCore core, ServiceConfiguration config)
        {
        }

        public void Destroy() 
        {
        }

        public Builder GetSyncExecutor()
        {
            return new Builder();
        }

        public DeviceCheckResult Check(IDeviceCheckType securityCheckType)
        {
            DeviceCheckResult[] results = new DeviceCheckResult[1];
            GetSyncExecutor()
                .WithDeviceCheck(securityCheckType)
                .Build()
                .Execute().Values.CopyTo(results, 0);
            return results[0];
        }

        public DeviceCheckResult Check(IDeviceCheck securityCheck)
        {
            return DeviceCheckExecutor
                .newSyncExecutor()
                .WithDeviceCheck(securityCheck)
                .Build()
                .Execute()[securityCheck.GetId()];
        }
    }
}
