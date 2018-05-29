using System;
using System.Json;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Metrics
{
    public abstract class AbstractMetricsPublisher
    {
        private readonly IMetrics<JsonObject>[] DefaultMetrics;
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public AbstractMetricsPublisher()
        {
            DefaultMetrics = new IMetrics<JsonObject>[] { new AppMetrics(), new DeviceMetrics() };
        }

        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        protected abstract string getClientId();

        /**
         * Parse metrics into a JSONObject and add common information for all metrics requests:
         *
         * @param type the type of metrics
         * @param metrics metrics
         * @return a JSONObject
         */
        protected JsonObject createMetricsJSONObject(string type, IMetrics<JsonObject>[] metrics)
        {
            JsonObject json = new JsonObject();

            json.Add("clientId", getClientId());
            json.Add("timestamp", CurrentTimeMillis());
            json.Add("type", type);

            JsonObject data = new JsonObject();

            // first put the default metrics (app and device info)
            foreach (IMetrics<JsonObject> m in DefaultMetrics)
            {
                data.Add(m.Identifier(), m.Data());
            }

            // then put the specific ones
            foreach (IMetrics<JsonObject> m in metrics)
            {
                data.Add(m.Identifier(), m.Data());
            }

            json.Add("data", data);

            return json;

        }

        protected abstract Task Publish(string type, IMetrics<JsonObject>[] metrics);
    }
}
