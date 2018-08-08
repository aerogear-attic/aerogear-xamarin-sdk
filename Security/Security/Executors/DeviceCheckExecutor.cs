using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Executors.Sync;

using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Security.Executors
{
    /// <summary>
    /// Entry point for the SecurityCheckExecutor. This class provides the builders.
    /// </summary>
    public class DeviceCheckExecutor
    {
        private DeviceCheckExecutor()
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
        protected readonly List<IDeviceCheck> CheckList = new List<IDeviceCheck>();
        protected MetricsService MetricsService { private set; get; }

        /// <summary>
        /// Adds a security check by name. This method requires that the Security Check Factory is registered.
        /// Registering a Service Factory happens when you initialize the SecurityService class.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="checksNames">Checks names.</param>
        public T WithDeviceCheck(params string[] checksNames)
        {
            foreach (var checkName in NonNull(checksNames, "checksNames"))
            {
                IDeviceCheck check = ServiceFinder.Resolve<IDeviceCheckFactory>().create(checkName);
                CheckList.Add(check);
            }

            return (T)this;
        }

        public T WithDeviceCheck(ICollection<IDeviceCheck> checks)
        {
            CheckList.AddRange(checks);

            return (T)this;
        }

        public T WithDeviceCheck(params IDeviceCheck[] checks)
        {
            foreach (var check in NonNull(checks, "checks"))
            {
                CheckList.Add(check);
            }

            return (T)this;
        }

        public T WithDeviceCheck(params IDeviceCheckType[] checkTypes)
        {
            foreach (var checkType in NonNull(checkTypes, "checkTypes"))
            {
                IDeviceCheck check = ServiceFinder.Resolve<IDeviceCheckFactory>().create(checkType);
                CheckList.Add(check);
            }
            return (T)this;
        }



        public T WithMetricsService(MetricsService metricsService)
        {
            this.MetricsService = metricsService;
            return (T)this;
        }

        private IDeviceCheckFactory GetSecurityCheckFactory()
        {
            IDeviceCheckFactory factory = ServiceFinder.Resolve<IDeviceCheckFactory>();

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
