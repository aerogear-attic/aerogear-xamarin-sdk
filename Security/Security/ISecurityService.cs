using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Security.Executors.Sync;

namespace AeroGear.Mobile.Security
{
    public interface ISecurityService : IServiceModule
    {
        Builder GetSyncExecutor();
        DeviceCheckResult Check(IDeviceCheckType securityCheckType);
        DeviceCheckResult Check(IDeviceCheck securityCheck);
    }
}
