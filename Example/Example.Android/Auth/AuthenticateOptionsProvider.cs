using System;
using AeroGear.Mobile.Auth.Authenticator;
using Example.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(Example.Android.Auth.AuthenticateOptionsProvider))]
namespace Example.Android.Auth
{
    public class AuthenticateOptionsProvider : IAuthenticateOptionsProvider
    {
        public static int RequestCode = 0x42;

        public AuthenticateOptionsProvider()
        {
        }

        public IAuthenticateOptions GetOptions()
        {
            return new AndroidAuthenticateOptions(MainActivity.Instance, RequestCode);
        }
    }
}
