using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Executors.Sync;

using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Security.Executors
{
    /// <summary>
    /// Entry point for the SecurityCheckExecutor. This class provides the builders.
    /// </summary>
    public class SecurityCheckExecutor
    {
        private SecurityCheckExecutor()
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
        protected readonly List<ISecurityCheck> CheckList = new List<ISecurityCheck>();

        /// <summary>
        /// Adds a security check by name. This method requires that the Security Check Factory is registered.
        /// Registering a Service Factory happens when you initialize the SecurityService class.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="checksNames">Checks names.</param>
        public T WithSecurityCheck(params string[] checksNames)
        {
            foreach (var checkName in NonNull(checksNames, "checksNames"))
            {
                ISecurityCheck check = ServiceFinder.Resolve<ISecurityCheckFactory>().create(checkName);
                CheckList.Add(check);
            }

            return (T)this;
        }

        public T WithSecurityCheck(params ISecurityCheck[] checks)
        {
            foreach (var check in NonNull(checks, "checks"))
            {
                CheckList.Add(check);
            }

            return (T)this;
        }

        public T WithSecurityCheck(params ISecurityCheckType[] checkTypes)
        {
            foreach (var checkType in NonNull(checkTypes, "checkTypes"))
            {
                ISecurityCheck check = ServiceFinder.Resolve<ISecurityCheckFactory>().create(checkType);
                CheckList.Add(check);
            }
            return (T)this;
        }

        private ISecurityCheckFactory GetSecurityCheckFactory()
        {
            ISecurityCheckFactory factory = ServiceFinder.Resolve<ISecurityCheckFactory>();

            if (factory == null) 
            {
                throw new Exception("Security check factory has not been registered");
            }

            return factory;
        }

        /// <summary>
        /// Builds the executor
        /// </summary>
        /// <returns>The executor.</returns>
        public abstract K Build();
    }
}
