using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

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
            NonNull(jsonStream, "jsonStream");
            var jsonText = ReadJsonStream(jsonStream);
            var jsonDocument = JsonValue.Parse(jsonText);
            ParseMobileCoreArray((JsonArray)jsonDocument["services"]);
        }

        private MobileCoreJsonParser(string jsonString)
        {
            NonNull(jsonString, "jsonString");
            var jsonDocument = JsonValue.Parse(jsonString);
            ParseMobileCoreArray((JsonArray)jsonDocument["services"]);
        }

        private String ReadJsonStream(Stream jsonStream)
        {
            NonNull(jsonStream, "jsonStream");
            using (var streamReader = new StreamReader(jsonStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private void ParseMobileCoreArray(JsonArray array)
        {
            NonNull(array, "array");
            int length = array.Count;
            for (int i = 0; (i < length); i++)
            {
                var value = array[i];
                if (value is JsonObject) ParseConfigObject((JsonObject)value);
            }
        }

        private void ParseConfigObject(JsonObject jsonObject)
        {
            NonNull(jsonObject, "jsonObject");
            var serviceConfigBuilder = ServiceConfiguration.Builder;
            serviceConfigBuilder.Id(jsonObject["id"]).Url(jsonObject["url"]).Type(jsonObject["type"]);
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
            values[serviceConfig.Id] = serviceConfig;
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

        /// <summary>
        /// Parses JSON configuration string.
        /// </summary>
        /// <returns>A dictionary of ServiceConfigs mapped by their name.</returns>
        /// <param name="jsonString">string with json configuration
        /// Please note that this should be managed by the calling core. 
        /// The parser will not close the resource when it is finished.</param>
        public static Dictionary<String, ServiceConfiguration> Parse(string jsonString)
        {
            MobileCoreJsonParser parser = new MobileCoreJsonParser(jsonString);
            return parser.values;
        }
    }
}
