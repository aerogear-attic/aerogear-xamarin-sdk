using System;
using System.Reflection;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Platform.iOS;
using Core.Platform.Logging;

[assembly: Xamarin.Forms.Dependency(typeof(IOSPlatformInjector))]
namespace AeroGear.Mobile.Core.Platform.iOS
{
    /// <summary>
    /// Class handles injection of iOS specific classes and access to resoureces.
    /// </summary>
    internal class IOSPlatformInjector : IPlatformInjector
    {
        public ILogger CreateLogger() => new IOSLogger();

        public String PlatformName => "iOS";

        public Assembly ExecutingAssembly { get;  set; }
        
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
