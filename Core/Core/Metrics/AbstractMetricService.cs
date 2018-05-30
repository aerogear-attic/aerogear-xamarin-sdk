using System;
using System.Json;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Metrics.Publishers;

namespace AeroGear.Mobile.Core.Metrics
{
    public class AbstractMetricService : IMetricsService
    {
        private string identifier;
        private const string INIT_METRICS_TYPE = "init";
        private static readonly IMetrics[] EMPTY_METRICS = new IMetrics[0];

        private IMetricsPublisher publisher;

        public string Id => identifier;
        public string Type => "metrics";
        public bool RequiresConfiguration => true;

        public AbstractMetricService()
        {
        }

        public void Configure(MobileCore core, ServiceConfiguration config)
        {
            this.identifier = config.Id;
            string metricsUrl = config.Url;
            this.publisher = new NetworkMetricsPublisher(metricsUrl);
        }

        public void Destroy()
        {
            
        }

        public Task Publish(string type, params IMetrics[] metrics)
        {
            if (publisher == null)
            {
                throw new NullReferenceException("Publisher has not been initialized");
            }
            return publisher.Publish(type, metrics);
        }

        public Task SendAppAndDeviceMetrics()
        {
            return this.Publish(INIT_METRICS_TYPE, EMPTY_METRICS);
        }
    }
}
