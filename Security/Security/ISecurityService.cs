using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Metrics;
using AeroGear.Mobile.Security.Executors.Sync;

namespace AeroGear.Mobile.Security
{
    public interface ISecurityService : IServiceModule
    {
        Builder GetSyncExecutor();
        DeviceCheckResult Check(IDeviceCheckType deviceCheckType, MetricsService metricsService = null);
        DeviceCheckResult Check(IDeviceCheck deviceCheck, MetricsService metricsService = null);
    }
}
