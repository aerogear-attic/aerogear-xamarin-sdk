using System;
namespace AeroGear.Mobile.Core.Utils
{
    public interface IPlatformBridge
    {
        ApplicationRuntimeInfo ApplicationRuntimeInfo { get; }
        PlatformInfo PlatformInfo { get; }
        IUserPreferences GetUserPreferences(string storageName = null);
    }
}
