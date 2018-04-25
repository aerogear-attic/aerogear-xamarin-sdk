using System;
using AeroGear.Mobile.Auth.Authenticator;

namespace Example.Android.Events
{
    public interface IAuthenticateOptionsProvider
    {
        IAuthenticateOptions GetOptions();
    }
}
