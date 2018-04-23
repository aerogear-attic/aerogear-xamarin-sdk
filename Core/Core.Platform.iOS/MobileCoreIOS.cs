using System;
using System.Reflection;

namespace AeroGear.Mobile.Core
{
    public sealed class MobileCoreIOS
    {
        /// <summary>
        /// Initializes Mobile core for iOS.
        /// </summary>
        public static void Init(Type[] services)
        {
            Init(null, new Options(), services);
        }

        /// <summary>
        /// Initializes Mobile core for iOS.
        /// </summary>
        /// <param name="options">additional initialization options</param>
        public static void Init(Options options, Type[] services)
        {
            Init(null, options, services);
        }

        /// <summary>
        /// Initializes Mobile core for iOS using custom assembly for storing resources. Best to be used with Xamarin.Forms.
        /// Resources needs to be stored in ./Resources directory of Xamarin.Forms platform-independent project.
        /// </summary>
        /// <param name="assembly">Assembly of the platform-independent project</param>
        public static void Init(Assembly assembly, Type[] services)
        {
            Init(assembly, new Options(), services);
        }

        /// <summary>
        /// Initializes Mobile core for iOS using custom assembly for storing resources. Best to be used with Xamarin.Forms.
        /// Resources needs to be stored in ./Resources directory of Xamarin.Forms platform-independent project.
        /// </summary>
        /// <param name="assembly">Assembly of the platform-independent project</param>
        /// <param name="options">additional initialization options</param>
        public static void Init(Assembly assembly, Options options, Type[] services)
        {
            IPlatformInjector platformInjector = new IOSPlatformInjector();
            platformInjector.ExecutingAssembly = assembly;
            MobileCore.Init(platformInjector, options);

            foreach (Type t in services) 
            {
                if (!typeof(IServiceModule).IsAssignableFrom(t))
                {
                    throw new ArgumentException("Passed in services must be types implementing the IServiceModule interface");
                }

                MobileCore.Instance.RegisterService(t);
            }
        }
    }
}