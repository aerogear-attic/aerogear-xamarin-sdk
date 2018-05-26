using System;
using System.Collections.Generic;
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
        private event EventHandler<SecurityCheckExecuterArgs> OnCheckExecutedEvent;

        /// <summary>
        /// Occurs when all checks have been executed.
        /// </summary>
        private event EventHandler OnCheckExecutionFinishedEvent;

        protected readonly List<ISecurityCheck> checks = new List<ISecurityCheck>();

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AeroGear.Mobile.Security.Executors.AbstractSecurityCheckExecutor"/> class.
        /// </summary>
        /// <param name="checks">A list of checks to be executed.</param>
        public AbstractSecurityCheckExecutor(List<ISecurityCheck> checks)
        {
            this.checks.AddRange(NonNull(checks, "checks"));
        }

        /// <summary>
        /// Invoked after every check execution.
        /// </summary>
        /// <param name="result">Result of the check just executed.</param>
        protected void OnCheckExecuted(SecurityCheckResult result) 
        {
            if (OnCheckExecutedEvent != null) 
            {
                OnCheckExecutedEvent(this, new SecurityCheckExecuterArgs(result));
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
    /// Event argument containing a SecurityCheckResult object.
    /// </summary>
    internal class SecurityCheckExecuterArgs : EventArgs
    {
        public readonly SecurityCheckResult CheckResult;

        public SecurityCheckExecuterArgs(SecurityCheckResult result)
        {
            this.CheckResult = result;
        }
    }
}
