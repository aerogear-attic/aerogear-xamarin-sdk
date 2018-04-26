using System;
using AeroGear.Mobile.Auth.Authenticator;

namespace Example.Auth
{
    public interface IAuthenticateOptionsProvider
    {
        IAuthenticateOptions GetOptions();
    }
}
