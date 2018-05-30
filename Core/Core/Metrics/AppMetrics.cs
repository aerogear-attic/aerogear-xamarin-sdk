using System;
using System.Json;
using System.Reflection;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Core.Metrics
{
    public class AppMetrics : IMetrics
    {
        private readonly string appId;
        private readonly string appVersion;
        private readonly string sdkVersion;
        private readonly string framework;

        public AppMetrics()
        {
            ApplicationRuntimeInfo appInfo = ServiceFinder.Resolve<IPlatformBridge>().ApplicationRuntimeInfo;

            this.appId = appInfo.Identifier;
            this.appVersion = appInfo.Version;
            this.sdkVersion = typeof(MobileCore).GetTypeInfo().Assembly.GetName().Version.ToString();
            this.framework = appInfo.Framework;
        }

        public string Identifier() => "app";

        public JsonObject Data()
        {
            JsonObject data = new JsonObject();
            data.Add("appId", appId);
            data.Add("appVersion", appVersion);
            data.Add("sdkVersion", sdkVersion);
            data.Add("framework", framework);
            return data;
        }
    }
}
