using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Platform.Android;
using AeroGear.Mobile.Core.Platform.Android.Logging;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidPlatformInjector))]
namespace AeroGear.Mobile.Core.Platform.Android
{
    public class AndroidPlatformInjector : IPlatformInjector
    {
        public ILogger CreateLogger() => new AndroidLogger();

        public String PlatformName => "Android";
    }
}
