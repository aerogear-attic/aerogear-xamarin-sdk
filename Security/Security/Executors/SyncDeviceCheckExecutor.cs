using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;

namespace AeroGear.Mobile.Security.Executors.Sync
{
    /// <summary>
    /// Synchronously executes provided SecurityChecks.
    /// </summary>
    public class SyncDeviceCheckExecutor : AbstractSecurityCheckExecutor
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AeroGear.Mobile.Security.Executors.Sync.SyncSecurityCheckExecutor"/> class.
        /// </summary>
        /// <param name="checks">List of checks to be executed.</param>
        public SyncDeviceCheckExecutor(List<IDeviceCheck> checks, MetricsService metricsService) : base(checks, metricsService)
        {
        }

        /// <summary>
        /// Executes the provided checks and returns the results. Blocks until all checks are executed.
        /// </summary>
        /// <returns>A Dictionary containing the results of each executed test. 
        /// The key of the Dictionary will be the output of SecurityCheck.getId(), 
        /// while the value will be the SecurityCheckResult of the check.</returns>
        public Dictionary<string, DeviceCheckResult> Execute()
        {
            var results = new Dictionary<string, DeviceCheckResult>();

            foreach (var securityCheck in checks)
            {
                var result = securityCheck.Check();
                results[result.Id] = result;

                OnCheckExecuted(result);
            }

            OnCheckExecutionFinished();
            return results;
        }
    }

    public class Builder : AbstractExecutorBuilder<Builder, SyncDeviceCheckExecutor>
    {
        public override SyncDeviceCheckExecutor Build()
        {
            return new SyncDeviceCheckExecutor(CheckList, MetricsService);
        }
    }
}
