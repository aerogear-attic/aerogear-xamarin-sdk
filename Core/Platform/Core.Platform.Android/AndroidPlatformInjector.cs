using System;
using System.Reflection;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Platform.Android;
using AeroGear.Mobile.Core.Platform.Android.Logging;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidPlatformInjector))]
namespace AeroGear.Mobile.Core.Platform.Android
{
    /// <summary>
    /// Class handles injection of Android specific classes and access to resoureces.
    /// </summary>
    internal class AndroidPlatformInjector : IPlatformInjector
    {
        public ILogger CreateLogger() => new AndroidLogger();

        public String PlatformName => "Android";

        public Assembly ExecutingAssembly { get; set; }

        public String DefaultResources
        {
            get
            {
                var name = ExecutingAssembly.GetName().Name;
                return $"{name}.Resources";
            }
        }       
    }
}
