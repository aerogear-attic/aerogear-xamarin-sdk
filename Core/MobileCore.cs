using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Exception;
using System.Diagnostics.Contracts;
using AeroGear.Mobile.Core.Http;
using System.Reflection;
using System.Net.Http;

namespace AeroGear.Mobile.Core
{
    public sealed class MobileCore
    {

        public const String DEFAULT_CONFIG_FILE_NAME = "mobile-services.json";

        private static String TAG = "AEROGEAR/CORE";

        private static int DEFAULT_TIMEOUT = 20;

        public ILogger Logger
        {
            get;
            private set;
        }

        public String ConfigFileName
        {
            get;
            private set;
        }

        private static MobileCore instance;

        public static MobileCore Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InitializationException("Core is not initialized, don't forget to call MobileCore.init() before using this instance.");
                }

                return instance;
            }
        }

        public IHttpServiceModule HttpLayer
        {
            get; private set;
        }

        private Dictionary<String, ServiceConfiguration> servicesConfig;

        private Dictionary<Type, IServiceModule> services = new Dictionary<Type, IServiceModule>();

        private MobileCore(IPlatformInjector injector, Options options)
        {
            Contract.Requires(options != null);
            Logger = options.Logger ?? injector?.CreateLogger() ?? new NullLogger();

            if (injector != null && options.ConfigFileName != null)
            {
                var filename = $"{injector.DefaultResources}.{options.ConfigFileName}";
                try
                {
                    using (var stream = injector.ExecutingAssembly.GetManifestResourceStream(filename))
                    {
                        servicesConfig = MobileCoreJsonParser.Parse(stream);
                    }
                }
                catch (System.Exception e)
                {
                    throw new InitializationException($"{filename} could not be loaded", e);
                }
            }
            else
            {
                if (options.ConfigJson != null)
                {
                    try
                    {
                        MobileCoreJsonParser.Parse(options.ConfigJson);
                    }
                    catch (System.Exception e)
                    {
                        throw new InitializationException("invalid JSON configuration file", e);
                    }
                }
                else
                    throw new InitializationException("Must provide either filename or JSON configuration in Init() options");
            }

            if (options.HttpServiceModule == null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
                var httpServiceModule = new SystemNetHttpServiceModule(httpClient);
                var configuration = getServiceConfiguration(httpServiceModule.Type);
                if (configuration == null)
                {
                    configuration = ServiceConfiguration.Builder.Build();
                }
                httpServiceModule.Configure(this, configuration);
                HttpLayer = httpServiceModule;
            }
            else HttpLayer = options.HttpServiceModule;
        }

        public static MobileCore Init() => Init(null, new Options());

        public static MobileCore Init(IPlatformInjector injector) => Init(injector, new Options());

        public static MobileCore Init(IPlatformInjector injector, Options options)
        {
            Contract.Requires(options != null);
            instance = new MobileCore(injector, options);
            return instance;
        }

        /// <summary>
        /// Called when mobile core instance needs to be destroyed.
        /// </summary>
        public void Destroy()
        {
            foreach (var serviceModule in services.Values)
            {
                serviceModule.Destroy();
            }
            instance = null;
        }

        /// <summary>
        /// Returns instance of a service module.
        /// </summary>
        /// <typeparam name="T">service module type</typeparam>
        /// <param name="serviceClass">service module class type</param>
        /// <returns></returns>
        public T GetInstance<T>(Type serviceClass) where T : IServiceModule => GetInstance<T>(serviceClass, null);

        /// <summary>
        /// Returns instance of a service module.
        /// </summary>
        /// <typeparam name="T">service module type</typeparam>
        /// <param name="serviceClass">service module class type</param>
        /// <param name="serviceConfiguration">service configuration</param>
        /// <returns></returns>
        public T GetInstance<T>(Type serviceClass, ServiceConfiguration serviceConfiguration)
            where T : IServiceModule
        {
            Contract.Requires(serviceClass != null);
            if (services.ContainsKey(serviceClass))
            {
                return (T)services[serviceClass];
            }

            IServiceModule serviceModule = Activator.CreateInstance(serviceClass) as IServiceModule;
            var serviceCfg = serviceConfiguration;
            if (serviceCfg == null) serviceCfg = getServiceConfiguration(serviceModule.Type);
            if (serviceCfg == null && serviceModule.RequiresConfiguration) throw new ConfigurationNotFoundException($"{serviceModule.Type} not found on " + ConfigFileName);
            serviceModule.Configure(this, serviceCfg);
            services[serviceClass] = serviceModule;
            return (T)serviceModule;
        }

        public ServiceConfiguration getServiceConfiguration(String type)
        {
            return servicesConfig.ContainsKey(type) ? servicesConfig[type] : null;
        }
    }
}

