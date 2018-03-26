using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Json;

namespace AeroGear.Mobile.Core.Configuration
{
    /// <summary>
    ///  This class is responsible for consuming a readable stream and producing a tree of config values to be consumed by modules.
    /// </summary>
    public class MobileCoreJsonParser
    {
        private Dictionary<String, ServiceConfiguration> values = new Dictionary<string, ServiceConfiguration>();

        private MobileCoreJsonParser(Stream jsonStream)
        {
            Contract.Requires(jsonStream != null);
            var jsonText = ReadJsonStream(jsonStream);
            var jsonDocument = JsonValue.Parse(jsonText);
            ParseMobileCoreArray((JsonArray)jsonDocument["services"]);
        }

        private String ReadJsonStream(Stream jsonStream)
        {
            Contract.Requires(jsonStream != null);
            Contract.Requires(jsonStream.CanRead);
            using (var streamReader = new StreamReader(jsonStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private void ParseMobileCoreArray(JsonArray array)
        {
            Contract.Requires(array != null);
            int length = array.Count;
            for (int i = 0; (i < length); i++)
            {
                var value = array[i];
                if (value is JsonObject) ParseConfigObject((JsonObject)value);
            }
        }

        private void ParseConfigObject(JsonObject jsonObject)
        {
            Contract.Requires(jsonObject != null);

            var serviceConfigBuilder = ServiceConfiguration.Builder;
            serviceConfigBuilder.Name(jsonObject["name"]).Url(jsonObject["url"]).Type(jsonObject["type"]);
            JsonObject config = (JsonObject)jsonObject["config"];
            var names = config.Keys;
            if ((names != null))
            {
                foreach (var name in names)
                {
                    serviceConfigBuilder.Property(name, config[name]);
                }
            }

            ServiceConfiguration serviceConfig = serviceConfigBuilder.Build();
            values[serviceConfig.Name] = serviceConfig;
        }

        /// <summary>
        /// Parses JSON configuration stream.
        /// </summary>
        /// <returns>A dictionary of ServiceConfigs mapped by their name.</returns>
        /// <param name="jsonStream">a readable Stream for mobile-core.json file. 
        /// Please note that this should be managed by the calling core. 
        /// The parser will not close the resource when it is finished.</param>
        public static Dictionary<String, ServiceConfiguration> Parse(Stream jsonStream)
        {
            MobileCoreJsonParser parser = new MobileCoreJsonParser(jsonStream);
            return parser.values;
        }
    }
}
