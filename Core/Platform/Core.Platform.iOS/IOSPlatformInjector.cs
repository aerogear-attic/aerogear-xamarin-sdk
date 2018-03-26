using System;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Platform.iOS;
using Core.Platform.Logging;

[assembly: Xamarin.Forms.Dependency(typeof(IOSPlatformInjector))]
namespace AeroGear.Mobile.Core.Platform.iOS
{
    public class IOSPlatformInjector : IPlatformInjector
    {
        public ILogger CreateLogger() => new IOSLogger();

        public String PlatformName => "iOS";
    }
}
