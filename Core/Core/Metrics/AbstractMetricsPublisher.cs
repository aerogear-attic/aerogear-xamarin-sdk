using System;
using System.Json;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Core.Metrics
{
    public abstract class AbstractMetricsPublisher : IMetricsPublisher
    {
        private static readonly ILogger LOGGER = MobileCore.Instance.Logger;

        private const string STORAGE_NAME = "org.aerogear.mobile.metrics";
        private const string STORAGE_KEY = "metrics-sdk-installation-id";

        private readonly IMetrics[] DefaultMetrics;
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public AbstractMetricsPublisher()
        {
            DefaultMetrics = new IMetrics[] { new AppMetrics(), new DeviceMetrics() };
        }

        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        /// <summary>
        /// Get or create the client ID that identifies a device as long as the user doesn't reinstall
        /// the app or delete the app storage. A random UUID is created and stored in the application
        /// shared preferences.
        /// </summary>
        /// <returns>Client ID.</returns>
        private static String GetClientId()
        {
            IUserPreferences preferences = ServiceFinder.Resolve<IPlatformBridge>().GetUserPreferences(STORAGE_NAME);

            String clientId = preferences.GetString(STORAGE_KEY, null);

            if (clientId == null)
            {
                clientId = System.Guid.NewGuid().ToString();
                LOGGER.Info("Generated a new client ID: " + clientId);
                preferences.PutString(STORAGE_KEY, clientId);
            }

            return clientId;
        }

        /**
         * Parse metrics into a JSONObject and add common information for all metrics requests:
         *
         * @param type the type of metrics
         * @param metrics metrics
         * @return a JSONObject
         */
        protected JsonObject createMetricsJSONObject(string type, IMetrics[] metrics)
        {
            JsonObject json = new JsonObject();

            json.Add("clientId", GetClientId());
            json.Add("timestamp", CurrentTimeMillis());
            json.Add("type", type);

            JsonObject data = new JsonObject();

            // first put the default metrics (app and device info)
            foreach (IMetrics m in DefaultMetrics)
            {
                data.Add(m.Identifier(), m.ToJson());
            }

            // then put the specific ones
            foreach (IMetrics m in metrics)
            {
                data.Add(m.Identifier(), m.ToJson());
            }

            json.Add("data", data);

            return json;

        }

        protected abstract Task DoPublish(string type, IMetrics[] metrics);

        public Task Publish(string type, IMetrics[] metrics)
        {
            Task ret = DoPublish(type, metrics);
            ret.ContinueWith((task) => { 
                if (task.IsFaulted)
                {
                    MobileCore.Instance.Logger.Info($"Error publishing metrics of type <{type}>: {task.Exception.InnerException.Message}");
                    task.Exception.Handle(ex => { MobileCore.Instance.Logger.Debug(ex.ToString()); return false; });
                } 
            });

            return ret;
        }
    }
}
