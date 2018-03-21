using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Core.AeroGear.Mobile.Core.Configuration
{
    /// <summary>
    /// This represents a parsed singleThreadService configuration from JSON configuration.
    /// </summary>
    public class ServiceConfiguration : IReadOnlyDictionary<String,String>
    {

        public String Name
        {
            get; private set;
        }

        public String Type
        {
            get; private set;
        }

        public String Url
        {
            get; private set;
        }

        public IEnumerable<string> Keys => properties.Keys;

        public IEnumerable<string> Values => properties.Values;

        public int Count => properties.Count;

        public string this[string key] => properties[key];

        private IDictionary<String, String> properties;

        private ServiceConfiguration(String name, IDictionary<String, String> properties, String type, String url)
        {
            Name = name;
            this.properties = properties;
            Type = type;
            Url = url;
        }

        public static ServiceConfigurationBuilder Builder => new ServiceConfigurationBuilder();

        public class ServiceConfigurationBuilder
        {

            protected String name;

            protected Dictionary<String, String> properties = new Dictionary<string, string>();

            protected String type;

            protected String url;

            public ServiceConfigurationBuilder Name(String name) {
                Contract.Requires(string.IsNullOrEmpty(name) == false);
                this.name = name;
                return this;
            }

            public ServiceConfigurationBuilder Property(String name, String value)
            {
                this.properties[name]=value;
                return this;
            }

            public ServiceConfigurationBuilder Type(String type)
            {
                this.type = type;
                return this;
            }

            public ServiceConfigurationBuilder Url(String url)
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
