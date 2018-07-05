using System;
using System.Collections.Generic;
using static AeroGear.Mobile.Core.Utils.SanityCheck;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Exception;
using AeroGear.Mobile.Core.Http;
using System.Net.Http;
using AeroGear.Mobile.Core.Metrics;

namespace AeroGear.Mobile.Core
{
    /// <summary>
    /// MobileCore is the entry point into AeroGear mobile services.
    /// </summary>
    public sealed class MobileCore
    {
        /// <summary>
        /// The default name of the config file.
        /// </summary>
        public const String DEFAULT_CONFIG_FILE_NAME = "mobile-services.json";

        private static String TAG = "AEROGEAR/CORE";

        private static int DEFAULT_TIMEOUT = 20;

        /// <summary>
        /// Gets the logger for the current platform
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the config file.
        /// </summary>
        /// <value>The name of the config file.</value>
        public String ConfigFileName
        {
            get;
            private set;
        }

        private static MobileCore instance;

        private MobileCoreConfiguration mobileConfiguration;

        private ServiceInstanceCache serviceInstanceCache = new ServiceInstanceCache();

        /// <summary>
        /// Gets the Mobile Core configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public MobileCoreConfiguration Configuration => this.mobileConfiguration;

        /// <summary>
        /// Gets HTTP service module of the MobileCore.
        /// </summary>
        public IHttpServiceModule HttpLayer { get; private set; }

        /// <summary>
        /// Holds MobileCore singleton instance. It's needed to initialize core before using this.
        /// </summary>
        public static MobileCore Instance => instance ?? throw new InitializationException("Core is not initialized, don't forget to call MobileCore.init() before using this instance.");

        /// <summary>
        /// Returns the service of the given type, if present.
        /// </summary>
        /// <returns>The service.</returns>
        /// <typeparam name="T">The type of the service to be returned.</typeparam>
        public T GetService<T>() where T : IServiceModule => GetService<T>(typeof(T));

        /// <summary>
        /// Returns the service identified by the given id if it exists.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="serviceId">Service identifier.</param>
        /// <typeparam name="T">The type of the service to be returned.</typeparam>
        public T GetService<T>(string serviceId) where T : IServiceModule => GetService<T>(typeof(T), GetServiceConfigurationById(serviceId));

        /// <summary>
        /// Returns array of ServiceConfiguration, filtered by type
        /// </summary>
        /// <param name="type">type field of the configuration</param>
        /// <returns>array of ServiceConfiguration</returns>
        public ServiceConfiguration[] GetServiceConfigurationByType(String type) => this.mobileConfiguration.GetServiceConfigurationByType(type);

        /// <summary>
        /// Returns the first instance of a ServiceConfiguration based on the type key
        /// </summary>
        /// <param name="type">type field of the configuration</param>
        /// <returns>a single ServiceConfiguration</returns>
        public ServiceConfiguration GetFirstServiceConfigurationByType(String type) => this.mobileConfiguration.GetFirstServiceConfigurationByType(type);

        /// <summary>
        /// Returns a ServiceConfiguration based on the id key
        /// </summary>
        /// <param name="id">id field of the configuration</param>
        /// <returns>a single ServiceConfiguration</returns>
        public ServiceConfiguration GetServiceConfigurationById(String id) => this.mobileConfiguration.GetServiceConfigurationById(id);

        /// <summary>
        /// Initializes MobileCore with defaults and without platform-specific injector. 
        /// </summary>
        /// <returns>MobileCore singleton instance</returns>
        public static MobileCore Init() => Init(null, new Options());

        /// <summary>
        /// Initializes MobileCore with defaults and with platform-specific injector. 
        /// </summary>
        /// <param name="injector">platform specific implementation dependency injection module</param>
        /// <returns>MobileCore singleton instance</returns>
        public static MobileCore Init(IPlatformInjector injector) => Init(injector, new Options());

        private MobileCore(IPlatformInjector injector, Options options)
        {

            Logger = options.Logger ?? injector?.CreateLogger() ?? new NullLogger();

            if (injector != null && options.ConfigFileName != null)
            {
                try
                {
                    using (var stream = injector.GetBundledFileStream(options.ConfigFileName))
                    {
                        mobileConfiguration = MobileCoreConfiguration.Parse(stream);
                    }
                }
                catch (System.Exception e)
                {
                    throw new InitializationException($"{options.ConfigFileName} could not be loaded", e);
                }
            }
            else
            {
                if (options.ConfigJson != null)
                {
                    try
                    {
                        mobileConfiguration = MobileCoreConfiguration.Parse(options.ConfigJson);
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
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = options.HttpAllowAutoRedirect;

                HttpClient httpClient = new HttpClient(httpClientHandler);
                httpClient.Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
                var httpServiceModule = new SystemNetHttpServiceModule(httpClient);
                var configuration = GetFirstServiceConfigurationByType(httpServiceModule.Type);
                if (configuration == null)
                {
                    configuration = ServiceConfiguration.Builder.Build();
                }
                httpServiceModule.Configure(this, configuration);
                HttpLayer = httpServiceModule;
            }
            else 
            {
                HttpLayer = options.HttpServiceModule;   
            }
        }

        /// <summary>
        /// Initializes MobileCore with specific options and with platform-specific injector. 
        /// </summary>
        /// <param name="injector">platform specific implementation dependency injection module</param>
        /// <param name="options">initialization options</param>
        /// <returns>MobileCore singleton instance</returns>
        public static MobileCore Init(IPlatformInjector injector, Options options)
        {
            if (instance != null) 
            {
                return instance;
            }
            NonNull<Options>(options, "init options");
            instance = new MobileCore(injector, options);

            sendAppAndDeviceMetrics();

            return instance;
        }

        private static void sendAppAndDeviceMetrics()
        {
            MetricsService metricsService = MobileCore.Instance.GetService<MetricsService>();
            metricsService.SendAppAndDeviceMetrics();
        }

        /// <summary>
        /// Called when mobile core instance needs to be destroyed.
        /// </summary>
        public void Destroy()
        {
            foreach (var serviceModule in serviceInstanceCache.GetAllCachedServices())
            {
                serviceModule.Destroy();
            }
            instance = null;
        }

        /// <summary>
        /// Registers an instance of a service module.
        /// </summary>
        /// <returns>The registered service module.</returns>
        /// <param name="serviceModule">The service module instance.</param>
        /// <typeparam name="T">service module type.</typeparam>
        public T RegisterService<T>(T serviceModule) where T : IServiceModule
        {
            serviceInstanceCache.Add<T>(NonNull(serviceModule, "serviceModule"));
            return serviceModule;
        }

        private T GetService<T>(Type serviceClass, ServiceConfiguration serviceConfiguration = null)
            where T : IServiceModule
        {
            NonNull<Type>(serviceClass, "serviceClass");

            if (serviceInstanceCache.IsCached(serviceClass))
            {
                return (T)serviceInstanceCache.GetCachedInstance(serviceClass);
            }

            // Try to instantiate it
            IServiceModule service = TryToInstantiate(serviceClass);

            if (service == null)
            {
                // There are no services registered for this interface.
                throw new ServiceModuleInstanceNotFoundException(String.Format("No instance has been registered for interface {0}", serviceClass.Name));
            }

            serviceConfiguration = ResolveServiceConfiguration(service, serviceConfiguration);

            if (service.RequiresConfiguration && serviceConfiguration == null)
            {
                throw new ConfigurationNotFoundException(String.Format("No configuration has been found for service {0}", service.Type));
            }

            service.Configure(this, serviceConfiguration);

            serviceInstanceCache.Add<T>(service);

            return (T)service;
        }

        private ServiceConfiguration ResolveServiceConfiguration(IServiceModule serviceModule, ServiceConfiguration conf)
        {
            ServiceConfiguration result = conf;
            if (conf == null)
            {
                ServiceConfiguration[] confs = GetServiceConfigurationByType(serviceModule.Type);
                if (confs != null && confs.Length != 0)
                {
                    result = confs[0];
                }
            }

            return result;
        }

        private IServiceModule TryToInstantiate(Type serviceClass) {
            try
            {
                return (IServiceModule)Activator.CreateInstance(serviceClass);
            } 
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Service instance cache.
    /// </summary>
    class ServiceInstanceCache
    {
        /// <summary>
        /// The cache by identifier.
        /// </summary>
        private Dictionary<String, IServiceModule> cacheById = new Dictionary<String, IServiceModule>();

        /// <summary>
        /// The cache by type. This is used only if the service has no configuration (hence, it has no id).
        /// </summary>
        private Dictionary<Type, IServiceModule> cacheByType = new Dictionary<Type, IServiceModule>();

        public IServiceModule GetCachedInstance(Type type) => cacheByType[type];
        public IServiceModule GetCachedInstance(string id) => cacheById[id];
        public bool IsCached(Type type) => cacheByType.ContainsKey(type);
        public bool IsCached(string id) => cacheById.ContainsKey(id);

        public void Add<T>(IServiceModule serviceModule)
        {
            if (serviceModule.Id != null)
            {
                cacheById[serviceModule.Id] = serviceModule;
            }

            Type type = typeof(T);
            if (!cacheByType.ContainsKey(type))
            {
                // We always cache the first instance of a service by type
                cacheByType[typeof(T)] = serviceModule;    
            }
        }

        public List<IServiceModule> GetAllCachedServices()
        {
            List<IServiceModule> result = new List<IServiceModule>();
            result.AddRange(cacheById.Values);
            result.AddRange(cacheByType.Values);
            return result;
        }
    }
}

