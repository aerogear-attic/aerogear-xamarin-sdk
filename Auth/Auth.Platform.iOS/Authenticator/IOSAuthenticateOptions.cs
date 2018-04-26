using System;
using UIKit;
using Aerogear.Mobile.Auth.User;
using Foundation;
using AeroGear.Mobile.Auth.Credentials;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public delegate void OIDAuthenticatorCallback(User  user, NSError error);

    public class IOSAuthenticateOptions : IAuthenticateOptions
    {
        /// <summary>
        /// Gets the view controller from which to present the SafariViewController.
        /// </summary>
        /// <value>The view controller from which to present the SafariViewController.</value>
        public readonly UIViewController PresentingViewController;

        public IOSAuthenticateOptions(UIViewController presentingViewController)
        {
            this.PresentingViewController = presentingViewController;
        }
    }
}
