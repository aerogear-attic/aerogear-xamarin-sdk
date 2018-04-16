using System;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.Core
{

    public sealed class Options
    {

        public String ConfigFileName {
            get; private set;
        }

        //  Don't have a default implementation because it should use configuration
        public IHttpServiceModule HttpServiceModule {get; private set;}

        public ILogger Logger { get; private set; }

        public static OptionsBuilder Builder
        {
            get => new OptionsBuilder();
        }

        public string ConfigJson { get; private set; }

        public Options()
        {
            ConfigFileName = MobileCore.DEFAULT_CONFIG_FILE_NAME;            
        }

        public sealed class OptionsBuilder
        {
            private ILogger logger;
            private String configFileName;
            private string configJSON;
            private IHttpServiceModule httpServiceModule;

            internal OptionsBuilder() { }

            /// <summary>
            /// Sets specific logger to be used with the core.
            /// </summary>
            /// <param name="logger">logger</param>
            /// <returns>itself</returns>
            public OptionsBuilder Logger(ILogger logger)
            {
                this.logger = logger;
                return this;
            }

            /// <summary>
            /// Sets name of the JSON configuration file that will be read from resources.
            /// </summary>
            /// <param name="configFileName">file name</param>
            /// <returns>itself</returns>
            public OptionsBuilder ConfigFileName(string configFileName)
            {
                this.configFileName = configFileName;
                return this;
            }

            /// <summary>
            /// Sets configuration directly from string.
            /// </summary>
            /// <param name="configJson">JSON string with configuration</param>
            /// <returns>itself</returns>
            public OptionsBuilder ConfigJSON(string configJson)
            {
                this.configFileName = null;
                this.configJSON = configJson;
                return this;
            }

            /// <summary>
            /// Set the specific implementation of HTTP to be used with the core.
            /// </summary>
            /// <param name="httpServiceModule">http service module</param>
            /// <returns>itself</returns>
            public OptionsBuilder HttpServiceModule(IHttpServiceModule httpServiceModule)
            {
                this.httpServiceModule = httpServiceModule;
                return this;
            }

            /// <summary>
            /// Creates options to be used to initialize the core.
            /// </summary>
            /// <returns>initialization options</returns>
            public Options Build()
            {
                var options = new Options();
                options.ConfigFileName = configFileName;
                options.HttpServiceModule = httpServiceModule;
                options.Logger = logger;
                options.ConfigJson = configJSON;
                return options;
            }
        }
    }
}