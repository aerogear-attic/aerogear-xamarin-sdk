using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Core.Metrics
{
    public abstract class AbstractMetricService : IMetricsService
    {
        private const string INIT_METRICS_TYPE = "init";
        private static readonly IMetrics<Object>[] EMPTY_METRICS = new IMetrics<Object>[0];

        public AbstractMetricService()
        {
        }

        public abstract string Id { get; }
        public string Type => "metrics";
        public bool RequiresConfiguration => true;

        public abstract void Configure(MobileCore core, ServiceConfiguration config);
        public abstract void Destroy();
        public abstract Task Publish<T>(string type, params IMetrics<T>[] metrics);

        public Task SendAppAndDeviceMetrics()
        {
            return this.Publish(INIT_METRICS_TYPE, EMPTY_METRICS);
        }
    }
}
