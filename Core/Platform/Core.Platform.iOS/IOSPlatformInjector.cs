using System;
using System.Reflection;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;

[assembly: Xamarin.Forms.Dependency(typeof(IOSPlatformInjector))]
namespace AeroGear.Mobile.Core
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
