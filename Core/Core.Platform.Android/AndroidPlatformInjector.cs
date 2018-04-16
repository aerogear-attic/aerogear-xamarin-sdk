using System;
using System.Reflection;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidPlatformInjector))]
namespace AeroGear.Mobile.Core
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
