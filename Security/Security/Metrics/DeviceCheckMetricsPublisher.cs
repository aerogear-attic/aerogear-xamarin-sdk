using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;
using AeroGear.Mobile.Security.Executors;

namespace AeroGear.Mobile.Security.Metrics
{
    /// <summary>
    /// This object will manage the communication with the metric service, batching the results to be published.
    /// </summary>
    internal class DeviceCheckMetricsPublisher
    {
        private const string SECURITY_METRICS_EVENT_TYPE = "security";
        private List<DeviceCheckResult> checkResults = new List<DeviceCheckResult>();
        private readonly MetricsService metricService;

        public DeviceCheckMetricsPublisher(MetricsService metricService)
        {
            this.metricService = metricService;
        }

        public void DeviceCheckExecuted(object sender, DeviceCheckExecuterArgs args)
        {
            DeviceCheckResult result = args.CheckResult;

            this.checkResults.Add(result);
        }

        public void DeviceCheckExecutionEnded(object sender, EventArgs args) {
            metricService?.Publish(SECURITY_METRICS_EVENT_TYPE, new DeviceCheckResultMetric(checkResults));
        }
    }
}
