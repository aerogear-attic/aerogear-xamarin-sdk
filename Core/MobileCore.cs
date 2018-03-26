using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Exception;

namespace AeroGear.Mobile.Core
{
	public sealed class MobileCore
	{

        public const String DEFAULT_CONFIG_FILE_NAME = "mobile-services.json";

        private static String TAG = "AEROGEAR/CORE";

        private static int DEFAULT_READ_TIMEOUT = 30;

        private static int DEFAULT_CONNECT_TIMEOUT = 10;

        private static int DEFAULT_WRITE_TIMEOUT = 10;

        public static ILogger Logger {
            get;
            private set;
        }

        private static String appVersion;

        public String configFileName {
            get;
            private set;
        }

        private static MobileCore instance;

        public static MobileCore Instance {
            get {
                if (instance == null)
                {
                    throw new InitializationException("Core is not initialized, don't forget to call MobileCore.init() before using this instance.");
                }

                return instance;   
            }
        }

        //private HttpServiceModule httpLayer;

        private Dictionary<String, ServiceConfiguration> servicesConfig;

        //private Dictionary<Type, ServiceModule> services = new HashMap();

        private MobileCore(Options options)
        {
            
        }

        public static MobileCore Init()
        {
            throw new NotImplementedException();
        }

        public static MobileCore Init(IPlatformInjector injector)
        {
            instance = new MobileCore(null);
            Logger = injector.CreateLogger();
            return instance;
        }


        public void destroy()
        {
            throw new NotImplementedException();

        }

        //public T getInstance<T, extends, ServiceModule>(Class<T> serviceClass)
        //{
        //    return ((T)(this.getInstance(serviceClass, null)));
        //}
       
        //public T getInstance<T, extends, ServiceModule>(Class<T> serviceClass, ServiceConfiguration serviceConfiguration)
        //{
        //    throw new NotImplementedException();
        //}

        public ServiceConfiguration getServiceConfiguration(String type)
        {
            return this.servicesConfig[type];
        }

        //private String getAppVersion()
        //{
           
        //}

        //public static String getSdkVersion()
        //{
            
        //}

        //public static String getAppVersion()
        //{
        //    return appVersion;
        //}
    }

    public sealed class Options
    {

        private String configFileName = MobileCore.DEFAULT_CONFIG_FILE_NAME;

        //  Don't have a default implementation because it should use configuration
        //private HttpServiceModule httpServiceModule;

        private ILogger logger;

        public Options()
        {

        }

	}
}

