using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;
using AeroGear.Mobile.Security.Executors;

namespace AeroGear.Mobile.Security.Metrics
{
    /// <summary>
    /// This object will manage the communication with the metric service, batching the results to be published.
    /// </summary>
    internal class SecurityCheckMetricsPublisher
    {
        private const string SECURITY_METRICS_EVENT_TYPE = "security";
        private List<SecurityCheckResult> checkResults = new List<SecurityCheckResult>();
        private readonly MetricsService metricService;

        public SecurityCheckMetricsPublisher(MetricsService metricService)
        {
            this.metricService = metricService;
        }

        public void SecurityCheckExecuted(object sender, SecurityCheckExecuterArgs args)
        {
            SecurityCheckResult result = args.CheckResult;

            this.checkResults.Add(result);
        }

        public void SecurityCheckExecutionEnded(object sender, EventArgs args) {
            metricService?.Publish(SECURITY_METRICS_EVENT_TYPE, new SecurityCheckResultMetric(checkResults));
        }
    }
}
