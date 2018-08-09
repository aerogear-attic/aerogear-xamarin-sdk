using System;
using System.Json;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;

namespace AeroGear.Mobile.Security.Metrics
{
    public class DeviceCheckResultMetric : IMetrics
    {
        private readonly DeviceCheckResult[] results;

        private const string KEY_ID = "id";
        private const string KEY_NAME = "name";
        private const string KEY_VALUE = "passed";

        public DeviceCheckResultMetric()
        {
        }

        public string Identifier() => "security";

        public DeviceCheckResultMetric(params DeviceCheckResult[] results)
        {
            this.results = results;
        }

        public DeviceCheckResultMetric(ICollection<DeviceCheckResult> results)
        {
            this.results = new DeviceCheckResult[results.Count];
            results.CopyTo(this.results, 0);
        }


        public JsonValue ToJson()
        {
            JsonArray jsonResult = new JsonArray();
            foreach (DeviceCheckResult result in results)
            {
                JsonObject jsonObject = new JsonObject();
                jsonObject.Add(KEY_ID, result.Id);
                jsonObject.Add(KEY_NAME, result.Name);
                jsonObject.Add(KEY_VALUE, result.Passed);

                jsonResult.Add(jsonObject);
            }
            return jsonResult;
        }
    }
}
