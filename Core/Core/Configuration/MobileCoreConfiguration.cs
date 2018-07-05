using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Core.Configuration
{
    public class MobileCoreConfiguration
    {
        public string Version { get; private set; }
        public string ClusterName { get; private set; }
        public string Namespace { get; private set; }
        public string ClientId { get; private set; }

        private Dictionary<string, ServiceConfiguration> serviceConfigurations = new Dictionary<string, ServiceConfiguration>();

        public ICollection<ServiceConfiguration> ServiceConfigurations => this.serviceConfigurations.Values;
        public ServiceConfiguration GetServiceConfiguration(string id) => this.serviceConfigurations.ContainsKey(id) ? this.serviceConfigurations[id] : null;
        public ServiceConfiguration GetServiceConfigurationById(String id) => this.serviceConfigurations[id];

        private MobileCoreConfiguration()
        {
        }

        internal static MobileCoreConfiguration Parse(string configuration)
        {
            var jsonDocument = JsonValue.Parse(configuration);
            return parse(jsonDocument);
        }

        internal static MobileCoreConfiguration Parse(Stream configuration)
        {
            NonNull(configuration, "jsonStream");
            string jsonText;

            using (var streamReader = new StreamReader(configuration))
            {
                jsonText = streamReader.ReadToEnd();
            }

            var jsonDocument = JsonValue.Parse(jsonText);
            return parse(jsonDocument);
        }

        private static MobileCoreConfiguration parse(JsonValue config)
        {
            MobileCoreConfiguration mobileCoreConfiguration = new MobileCoreConfiguration();

            mobileCoreConfiguration.Version = config["version"].ToString();
            mobileCoreConfiguration.ClusterName = config["clusterName"];
            mobileCoreConfiguration.Namespace = config["namespace"];
            mobileCoreConfiguration.ClientId = config["clientId"];

            JsonArray servicesArray = (JsonArray)config["services"];

            int length = servicesArray.Count;
            for (int i = 0; (i < length); i++)
            {
                var value = servicesArray[i];
                if (value is JsonObject)
                {
                    ServiceConfiguration serviceConfig = ServiceConfiguration.FromJson((JsonObject)value);
                    mobileCoreConfiguration.serviceConfigurations[serviceConfig.Id] = serviceConfig;
                }
            }

            return mobileCoreConfiguration;
        }

        /// <summary>
        /// Returns array of ServiceConfiguration, filtered by type
        /// </summary>
        /// <param name="type">type field of the configuration</param>
        /// <returns>array of ServiceConfiguration</returns>
        public ServiceConfiguration[] GetServiceConfigurationByType(String type)
        {
            List<ServiceConfiguration> listOfConfigs = new List<ServiceConfiguration>();

            foreach (var config in ServiceConfigurations)
            {
                if (config.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                {
                    listOfConfigs.Add(config);
                }
            }

            return listOfConfigs.ToArray();
        }

        public ServiceConfiguration GetFirstServiceConfigurationByType(String type)
        {
            ServiceConfiguration[] confs = GetServiceConfigurationByType(type);

            if (confs.Length > 0)
            {
                return confs[0];
            }

            return null;
        }
    }
}
