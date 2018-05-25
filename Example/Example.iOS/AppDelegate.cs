using System;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Security;
using FFImageLoading.Forms.Touch;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using UIKit;
using static AeroGear.Mobile.Core.Configuration.ServiceConfiguration;

namespace Example.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            var xamApp = new App();
            MobileCore core = MobileCoreIOS.Init(xamApp.GetType().Assembly);
            var authService = AuthService.InitializeService();
            var authConfig = AuthenticationConfig.Builder.RedirectUri("org.aerogear.mobile.example:/callback").Build();
            authService.Configure(authConfig);

            SecurityService.InitializeService();

            LoadApplication(xamApp);
            CachedImageRenderer.Init();
            ImageCircleRenderer.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}
