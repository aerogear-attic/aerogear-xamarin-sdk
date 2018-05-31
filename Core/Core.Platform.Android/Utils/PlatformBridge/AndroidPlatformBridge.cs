using System;
using Android.Content;

namespace AeroGear.Mobile.Core.Utils
{
    public class AndroidPlatformBridge : IPlatformBridge
    {
        private readonly Context appContext;
        public ApplicationRuntimeInfo ApplicationRuntimeInfo { get; }
        public PlatformInfo PlatformInfo { get; }

        public AndroidPlatformBridge(Context appContext)
        {
            this.ApplicationRuntimeInfo = new ApplicationRuntimeInfo(
                appContext.PackageName,
                appContext.PackageManager.GetPackageInfo(appContext.PackageName, 0).VersionName,
                "Xamarin.Android"
            );

            this.PlatformInfo = new PlatformInfo("android", Android.OS.Build.VERSION.Release);
            this.appContext = appContext;
        }

        public IUserPreferences GetUserPreferences(string storageName)
        {
            return new AndroidUserPreferences(appContext, storageName);
        }
    }
}
