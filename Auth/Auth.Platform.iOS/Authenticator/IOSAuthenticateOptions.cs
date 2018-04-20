using System;
using UIKit;
using Aerogear.Mobile.Auth.User;
using Foundation;
using AeroGear.Mobile.Auth.Credentials;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public delegate void OIDAuthenticatorCallback(User  user, NSError error);

    public class IOSAuthenticateOptions
    {
        public UIViewController PresentingViewController { get; }

        public IOSAuthenticateOptions(UIViewController presentingViewController)
        {
            this.PresentingViewController = presentingViewController;
        }
    }
}
