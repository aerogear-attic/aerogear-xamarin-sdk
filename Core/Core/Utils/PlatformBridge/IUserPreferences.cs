using System;
namespace AeroGear.Mobile.Core.Utils
{
    public interface IUserPreferences
    {
        string getString(string key, string defaultValue = null);
        void putString(string key, string value);
    }
}
