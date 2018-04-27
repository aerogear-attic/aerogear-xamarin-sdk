using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Platform.Android;
using AeroGear.Mobile.Core.Storage;
using AeroGear.Mobile.Core.Utils;
using Android.App;
using Android.Content;
using System.Reflection;

namespace AeroGear.Mobile.Core
{
    public sealed class MobileCoreAndroid
    {
        /// <summary>
        /// Initializes Mobile core for Android.
        /// </summary>
        /// <param name="app">Android application context</param>
        public static void Init(Context appContext)
        {
            Init(null, appContext, new Options());
        }

        /// <summary>
        /// Initializes Mobile core for Android.
        /// </summary>
        /// <param name="appContext">Android application contextt</param>
        /// <param name="options">additional initialization options</param>
        public static void Init(Context appContext, Options options)
        {
            Init(null, appContext, options);
        }


        /// <summary>
        /// Initializes Mobile core for Android using custom assembly for storing resources. Best to be used with Xamarin.Forms.
        /// Resources needs to be stored in ./Resources directory of Xamarin.Forms platform-independent project.
        /// </summary>
        /// <param name="assembly">Assembly of the platform-independent project</param>
        /// <param name="appContext">Android application contextt</param>
        public static void Init(Assembly assembly, Context appContext)
        {
            Init(assembly, appContext, new Options());
        }


        /// <summary>
        /// Initializes Mobile core for Android using custom assembly for storing resources. Best to be used with Xamarin.Forms.
        /// Resources needs to be stored in ./Resources directory of Xamarin.Forms platform-independent project.
        /// </summary>
        /// <param name="assembly">Assembly of the platform-independent project</param>
        /// <param name="appContext">Android application contextt</param>
        /// <param name="options">additional initialization options</param>
        public static void Init(Assembly assembly, Context appContext, Options options)
        {
            // TODO: check if already initialized
            RegisterServices();
            IPlatformInjector platformInjector = new AndroidPlatformInjector(appContext);
            platformInjector.ExecutingAssembly = assembly;
            MobileCore.Init(platformInjector, options);
        }

        private static void RegisterServices()
        {
            ServiceFinder.RegisterType<IAuthService, AuthService>();
            ServiceFinder.RegisterType<ILogger, AndroidLogger>();
            ServiceFinder.RegisterInstance<IStorageManager>(new StorageManager("AeroGear.Mobile.Auth.Credentials", Application.Context.ApplicationContext));
        }
    }
}