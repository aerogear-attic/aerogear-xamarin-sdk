using System;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.Core
{
    /// <summary>
    /// Specify the options for the Core module
    /// </summary>
    public sealed class Options
    {
        /// <summary>
        /// Gets the name of the config file.
        /// </summary>
        /// <value>The name of the config file.</value>
        public String ConfigFileName {
            get; private set;
        }

        /// <summary>
        /// Gets the http service module.
        /// </summary>
        /// <value>The http service module.</value>
        public IHttpServiceModule HttpServiceModule {get; private set;}
        public bool HttpAllowAutoRedirect { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the builder to create a new instance of <see cref="Options"/>
        /// </summary>
        /// <value>an instance of <see cref="OptionsBuilder"/></value>
        public static OptionsBuilder Builder
        {
            get => new OptionsBuilder();
        }

        /// <summary>
        /// Gets the json object from the config file.
        /// </summary>
        /// <value>The config json.</value>
        public string ConfigJson { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Core.Options"/> class.
        /// </summary>
        public Options()
        {
            HttpAllowAutoRedirect = false;
            ConfigFileName = MobileCore.DEFAULT_CONFIG_FILE_NAME;            
        }

        /// <summary>
        /// Builder class to create a new instance of the <see cref="T:AeroGear.Mobile.Core.Options"/> class.
        /// </summary>
        public sealed class OptionsBuilder
        {
            private ILogger logger;
            private String configFileName;
            private string configJSON;
            private IHttpServiceModule httpServiceModule;
            private bool httpAllowAutoRedirect = false;

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
            /// Set whether the default HTTP Client should allow redirects.
            /// This does not apply when specifying a custom HTTP Client using
            /// <see cref="HttpServiceModule(IHttpServiceModule)"/>.
            /// </summary>
            /// <returns>itself</returns>
            /// <param name="allowAutoRedirect">If set to <c>true</c> allow auto redirect.</param>
            public OptionsBuilder HttpAllowAutoRedirect(bool allowAutoRedirect)
            {
                this.httpAllowAutoRedirect = allowAutoRedirect;
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
                options.HttpAllowAutoRedirect = httpAllowAutoRedirect;
                return options;
            }
        }
    }
}
