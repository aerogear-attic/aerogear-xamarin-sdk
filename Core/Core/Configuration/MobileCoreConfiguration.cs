using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Core.Configuration
{
    /// <summary>
    /// This class represent the MobileCore configuration as parsed from the <code>mobile-services.json</code> file.
    /// </summary>
    public class MobileCoreConfiguration
    {
        public string Version { get; private set; }
        public string ClusterName { get; private set; }
        public string Namespace { get; private set; }
        public string ClientId { get; private set; }

        private Dictionary<string, ServiceConfiguration> serviceConfigurations = new Dictionary<string, ServiceConfiguration>();

        /// <summary>
        /// Gets all the service configurations available.
        /// </summary>
        /// <value>The service configurations.</value>
        public ICollection<ServiceConfiguration> ServiceConfigurations => this.serviceConfigurations.Values;

        /// <summary>
        /// Gets the configuration for the service identified by the given id.
        /// </summary>
        /// <returns>The configuration for the service identified by the given id.</returns>
        /// <param name="id">The service identifier.</param>
        public ServiceConfiguration GetServiceConfigurationById(String id) => this.serviceConfigurations.ContainsKey(id) ? this.serviceConfigurations[id] : null;

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

        /// <summary>
        /// Gets the configuration found for services of the given type.
        /// </summary>
        /// <returns>The configuration found for services of the given type.</returns>
        /// <param name="type">The service type.</param>
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
