using System;
using AeroGear.Mobile.Security.Executors;

namespace AeroGear.Mobile.Security.Metrics
{
    /// <summary>
    /// This object will manage the communication with the metric service, batching the results to be published.
    /// </summary>
    internal class SecurityCheckMetricsPublisher
    {
        // TODO: this should receive the metric service
        public SecurityCheckMetricsPublisher()
        {
        }

        public void SecurityCheckExecuted(object sender, SecurityCheckExecuterArgs args)
        {
            SecurityCheckResult result = args.CheckResult;

            // TODO: store the metrics somewhere in this class
        }

        public void SecurityCheckExecutionEnded(object sender, EventArgs args) {
            // TODO: publish the metrics
        }
    }
}
