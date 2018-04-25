using System;
using AeroGear.Mobile.Auth.Authenticator;
using Example.Auth;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Example.iOS.Auth.AuthenticateOptionsProvider))]
namespace Example.iOS.Auth
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
