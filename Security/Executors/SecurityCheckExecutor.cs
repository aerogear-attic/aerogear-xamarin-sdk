using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Executors.Sync;

namespace AeroGear.Mobile.Security.Executors
{
    /// <summary>
    /// Entry point for the SecurityCheckExecutor. This class provides the builders.
    /// </summary>
    public class SecurityCheckExecutor
    {
        public SecurityCheckExecutor()
        {
        }

        /// <summary>
        /// Returns a new SyncSecurityCheckExecutor Builder object.
        /// </summary>
        /// <returns>The sync executor builder.</returns>
        public static Builder newSyncExecutor()
        {
            return new Builder();
        }
    }

    public abstract class AbstractExecutorBuilder<T,K> where T : AbstractExecutorBuilder<T,K>
    {
        protected readonly List<ISecurityCheck> Checks = new List<ISecurityCheck>();

        public T WithSecurityCheck(ISecurityCheck check)
        {
            Checks.Add(check);
            return (T)this;
        }

        public T WithSecurityCheck(ISecurityCheckType checkType)
        {
            ISecurityCheck check = ServiceFinder.Resolve<ISecurityCheckFactory>().create(checkType);
            Checks.Add(check);
            return (T)this;
        }

        /// <summary>
        /// Builds the executor
        /// </summary>
        /// <returns>The executor.</returns>
        public abstract K Build();
    }
}
