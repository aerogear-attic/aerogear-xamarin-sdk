using System;
using System.Json;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Metrics.Publishers
{
    public class NetworkMetricsPublisher : AbstractMetricsPublisher
    {
        public NetworkMetricsPublisher()
        {
        }

        protected override string getClientId()
        {
            throw new NotImplementedException();
        }

        protected override Task Publish(string type, IMetrics<JsonObject>[] metrics)
        {
            throw new NotImplementedException();
        }
    }
}
