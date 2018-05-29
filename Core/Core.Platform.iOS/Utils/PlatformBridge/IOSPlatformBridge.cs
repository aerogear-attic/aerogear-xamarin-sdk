using System;
using Foundation;
using UIKit;

namespace AeroGear.Mobile.Core.Utils
{
    public class IOSPlatformBridge : IPlatformBridge
    {
        public ApplicationRuntimeInfo ApplicationRuntimeInfo { get; }
        public PlatformInfo PlatformInfo { get; }

        public IOSPlatformBridge()
        {
            this.ApplicationRuntimeInfo = new ApplicationRuntimeInfo(
                NSBundle.MainBundle.InfoDictionary["CFBundleDisplayName"].ToString(),
                NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString(),
                "Xamarin.iOS"
            );

            this.PlatformInfo = new PlatformInfo("iOS", UIDevice.CurrentDevice.SystemVersion);
        }


        public IUserPreferences GetUserPreferences(string storageName = null)
        {
            throw new NotImplementedException();
        }
    }
}
