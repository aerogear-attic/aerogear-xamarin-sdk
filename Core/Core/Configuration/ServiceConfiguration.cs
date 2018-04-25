using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Core.Configuration
{
    /// <summary>
    /// This represents a parsed service configuration from JSON configuration.
    /// </summary>
    public class ServiceConfiguration : IReadOnlyDictionary<string,string>
    {

        /// <summary>
        /// Id of the service
        /// </summary>
        /// <value>the service id</value>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Type of the service
        /// </summary>
        /// <value>the service type</value>
        public string Type
        {
            get; private set;
        }

        /// <summary>
        /// Url of the service
        /// </summary>
        /// <value>the service url</value>
        public string Url
        {
            get; private set;
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<string> Keys => properties.Keys;

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<string> Values => properties.Values;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => properties.Count;

        /// <summary>
        /// Gets the <see cref="T:AeroGear.Mobile.Core.Configuration.ServiceConfiguration"/> with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public string this[string key] => properties[key];

        private IDictionary<string, string> properties;

        private ServiceConfiguration(string id, IDictionary<string, string> properties, string type, string url)
        {
            Id = id;
            this.properties = properties;
            Type = type;
            Url = url;
        }

        /// <summary>
        /// Gets the builder to create a new <see cref="T:AeroGear.Mobile.Core.Configuration.ServiceConfiguration"/>
        /// </summary>
        /// <value>The builder.</value>
        public static ServiceConfigurationBuilder Builder => new ServiceConfigurationBuilder();

        /// <summary>
        /// Service configuration builder.
        /// </summary>
        public class ServiceConfigurationBuilder
        {

            private string id;

            private Dictionary<string, string> properties = new Dictionary<string, string>();

            private string type;

            private string url;

            /// <summary>
            /// The id of the serivce
            /// </summary>
            /// <returns>the builder instance</returns>
            /// <param name="id">Identifier.</param>
            public ServiceConfigurationBuilder Id(string id) {
                NonEmpty(id, "id");
                this.id = id;
                return this;
            }

            /// <summary>
            /// Add a new property for the service
            /// </summary>
            /// <returns>the builder instance</returns>
            /// <param name="name">Property name.</param>
            /// <param name="value">Property value</param>
            public ServiceConfigurationBuilder Property(string name, string value)
            {
                properties[name] = value;
                return this;
            }

            /// <summary>
            /// Add a new property for the service
            /// </summary>
            /// <returns>the builder instance</returns>
            /// <param name="name">Property name.</param>
            /// <param name="value">Property value</param>
            public ServiceConfigurationBuilder Property(string name, JsonValue value)
            {
                properties[name] = value.JsonType == JsonType.String ? (string)value:value.ToString();
                return this;
            }

            /// <summary>
            /// The type of the service
            /// </summary>
            /// <returns>the builder instance</returns>
            /// <param name="type">service type</param>
            public ServiceConfigurationBuilder Type(string type)
            {
                this.type = type;
                return this;
            }

            /// <summary>
            /// The url of the service
            /// </summary>
            /// <returns>the builder instance</returns>
            /// <param name="url">Service url</param>
            public ServiceConfigurationBuilder Url(string url)
            {
                this.url = url;
                return this;
            }

            /// <summary>
            /// Build the service configuration
            /// </summary>
            /// <returns>A new ServiceConfiguration instance</returns>
            public ServiceConfiguration Build()
            {
                return new ServiceConfiguration(this.id, this.properties, this.type, this.url);
            }
        }

        /// <summary>
        /// If the properties contain the given key
        /// </summary>
        /// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key name</param>
        public bool ContainsKey(string key) => properties.ContainsKey(key);

        /// <summary>
        /// Gets the enumerator of the properties
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => properties.GetEnumerator();

        /// <summary>
        /// Try get value of the given key from the properties
        /// </summary>
        /// <returns><c>true</c>, if get value was tryed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public bool TryGetValue(string key, out string value)
        {
            return ((IReadOnlyDictionary<string, string>)properties).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyDictionary<string, string>)properties).GetEnumerator();
        }
    }
}
