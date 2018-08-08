using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;
using AeroGear.Mobile.Security.Metrics;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Security.Executors
{
    /// <summary>
    /// Base class for security check executors.
    /// </summary>
    public class AbstractSecurityCheckExecutor
    {
        // TODO: when metrics will be passed, add the metrics publisher to the following 2 events
        /// <summary>
        /// Occurs when a check has just been executed.
        /// </summary>
        private event EventHandler<DeviceCheckExecuterArgs> OnCheckExecutedEvent;

        /// <summary>
        /// Occurs when all checks have been executed.
        /// </summary>
        private event EventHandler OnCheckExecutionFinishedEvent;

        protected readonly List<IDeviceCheck> checks = new List<IDeviceCheck>();

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AeroGear.Mobile.Security.Executors.AbstractSecurityCheckExecutor"/> class.
        /// </summary>
        /// <param name="checks">A list of checks to be executed.</param>
        public AbstractSecurityCheckExecutor(List<IDeviceCheck> checks, MetricsService metricsService)
        {
            this.checks.AddRange(NonNull(checks, "checks"));

            if (metricsService != null)
            {
                var publisher = new DeviceCheckMetricsPublisher(metricsService);

                OnCheckExecutedEvent += publisher.DeviceCheckExecuted;
                OnCheckExecutionFinishedEvent += publisher.DeviceCheckExecutionEnded;
            }
        }

        /// <summary>
        /// Invoked after every check execution.
        /// </summary>
        /// <param name="result">Result of the check just executed.</param>
        protected void OnCheckExecuted(DeviceCheckResult result) 
        {
            if (OnCheckExecutedEvent != null) 
            {
                OnCheckExecutedEvent(this, new DeviceCheckExecuterArgs(result));
            }
        }

        /// <summary>
        /// Invoked after all checks have been executed.
        /// </summary>
        protected void OnCheckExecutionFinished() 
        {
            if (OnCheckExecutionFinishedEvent != null)
            {
                OnCheckExecutionFinishedEvent(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Event argument containing a DeviceCheckResult object.
    /// </summary>
    internal class DeviceCheckExecuterArgs : EventArgs
    {
        public readonly DeviceCheckResult CheckResult;

        public DeviceCheckExecuterArgs(DeviceCheckResult result)
        {
            this.CheckResult = result;
        }
    }
}
