using System;
using System.Json;
using System.Net.Http;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Logging;

namespace AeroGear.Mobile.Core.Metrics.Publishers
{
    public class NetworkMetricsPublisher : AbstractMetricsPublisher
    {
        private static readonly ILogger LOGGER = MobileCore.Instance.Logger;
        private readonly string url;

        private HttpClient httpClient = new HttpClient();

        public NetworkMetricsPublisher(string url)
        {
            this.url = url;
        }

        public override Task Publish(string type, IMetrics[] metrics)
        {
            JsonObject json = createMetricsJSONObject(type, metrics);
            return httpClient.PostAsync(url, new StringContent(json.ToString()));
        }
    }
}
