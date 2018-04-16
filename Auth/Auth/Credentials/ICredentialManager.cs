using System;
using AeroGear.Mobile.Core.Storage;

namespace AeroGear.Mobile.Auth.Credentials
{
    public interface ICredentialManager : IStorageManager<ICredential>
    {
    }
}