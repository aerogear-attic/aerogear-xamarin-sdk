using System;
using System.Collections.Generic;

namespace AeroGear.Mobile.Security.Executors.Sync
{
    /// <summary>
    /// Synchronously executes provided SecurityChecks.
    /// </summary>
    public class SyncSecurityCheckExecutor : AbstractSecurityCheckExecutor
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AeroGear.Mobile.Security.Executors.Sync.SyncSecurityCheckExecutor"/> class.
        /// </summary>
        /// <param name="checks">List of checks to be executed.</param>
        public SyncSecurityCheckExecutor(List<ISecurityCheck> checks) : base(checks)
        {
        }

        /// <summary>
        /// Executes the provided checks and returns the results. Blocks until all checks are executed.
        /// </summary>
        /// <returns>A Dictionary containing the results of each executed test. 
        /// The key of the Dictionary will be the output of SecurityCheck.getId(), 
        /// while the value will be the SecurityCheckResult of the check.</returns>
        public Dictionary<string, SecurityCheckResult> Execute()
        {
            var results = new Dictionary<string, SecurityCheckResult>();

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

    public class Builder : AbstractExecutorBuilder<Builder, SyncSecurityCheckExecutor>
    {
        public override SyncSecurityCheckExecutor Build()
        {
            return new SyncSecurityCheckExecutor(CheckList);
        }
    }
}
