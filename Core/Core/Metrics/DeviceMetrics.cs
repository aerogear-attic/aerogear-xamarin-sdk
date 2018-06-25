using System;
using System.Json;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Core.Metrics
{
    public class DeviceMetrics : IMetrics
    {
        private readonly PlatformInfo platformInfo = ServiceFinder.Resolve<IPlatformBridge>().PlatformInfo;

        public DeviceMetrics()
        {
        }

        public string Identifier() => "Device";

        public JsonValue ToJson()
        {
            JsonObject data = new JsonObject();
            data.Add("platform", platformInfo.Name);
            data.Add("platformVersion", platformInfo.Version);
            return data;
        }
    }
}
