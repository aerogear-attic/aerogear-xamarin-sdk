using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Json;

namespace AeroGear.Mobile.Core.Configuration
{
    /// <summary>
    /// This represents a parsed singleThreadService configuration from JSON configuration.
    /// </summary>
    public class ServiceConfiguration : IReadOnlyDictionary<string,string>
    {

        public string Name
        {
            get; private set;
        }

        public string Type
        {
            get; private set;
        }

        public string Url
        {
            get; private set;
        }

        public IEnumerable<string> Keys => properties.Keys;

        public IEnumerable<string> Values => properties.Values;

        public int Count => properties.Count;

        public string this[string key] => properties[key];

        private IDictionary<string, string> properties;

        private ServiceConfiguration(string name, IDictionary<string, string> properties, string type, string url)
        {
            Name = name;
            this.properties = properties;
            Type = type;
            Url = url;
        }

        public static ServiceConfigurationBuilder Builder => new ServiceConfigurationBuilder();

        public class ServiceConfigurationBuilder
        {

            private string name;

            private Dictionary<string, string> properties = new Dictionary<string, string>();

            private string type;

            private string url;

            public ServiceConfigurationBuilder Name(string name) {
                Contract.Requires(string.IsNullOrEmpty(name) == false);
                this.name = name;
                return this;
            }

            public ServiceConfigurationBuilder Property(string name, string value)
            {
                properties[name] = value;
                return this;
            }

            public ServiceConfigurationBuilder Property(string name, JsonValue value)
            {
                properties[name] = value.JsonType == JsonType.String ? (string)value:value.ToString();
                return this;
            }

            public ServiceConfigurationBuilder Type(string type)
            {
                this.type = type;
                return this;
            }

            public ServiceConfigurationBuilder Url(string url)
            {
                this.url = url;
                return this;
            }

            public ServiceConfiguration Build()
            {
                return new ServiceConfiguration(this.name, this.properties, this.type, this.url);
            }
        }

        public bool ContainsKey(string key) => properties.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => properties.GetEnumerator();

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
