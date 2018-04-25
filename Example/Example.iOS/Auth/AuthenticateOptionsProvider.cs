using System;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Core;
using Example.Android.Events;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Example.iOS.Events.AuthenticateOptionsProvider))]
namespace Example.iOS.Events
{
    public class AuthenticateOptionsProvider : IAuthenticateOptionsProvider
    {
        public AuthenticateOptionsProvider()
        {
        }

        public IAuthenticateOptions GetOptions()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var rootViewController = window.RootViewController;
            var currentViewController = rootViewController;
            if (rootViewController.PresentedViewController != null)
            {
                currentViewController = rootViewController.PresentedViewController;
            }
            return new IOSAuthenticateOptions(currentViewController);
        }
    }
}
