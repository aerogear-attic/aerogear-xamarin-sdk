using System;
using System.Json;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Metrics;

namespace AeroGear.Mobile.Security.Metrics
{
    public class SecurityCheckResultMetric : IMetrics
    {
        private readonly SecurityCheckResult[] results;

        private const string KEY_ID = "id";
        private const string KEY_NAME = "name";
        private const string KEY_VALUE = "passed";

        public SecurityCheckResultMetric()
        {
        }

        public string Identifier() => "security";

        public SecurityCheckResultMetric(params SecurityCheckResult[] results)
        {
            this.results = results;
        }

        public SecurityCheckResultMetric(ICollection<SecurityCheckResult> results)
        {
            this.results = new SecurityCheckResult[results.Count];
            results.CopyTo(this.results, 0);
        }


        public JsonValue ToJson()
        {
            JsonArray jsonResult = new JsonArray();
            foreach (SecurityCheckResult result in results)
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
