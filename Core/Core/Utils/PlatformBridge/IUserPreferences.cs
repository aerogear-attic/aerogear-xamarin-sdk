using System;
namespace AeroGear.Mobile.Core.Utils
{
    public interface IUserPreferences
    {
        string GetString(string key, string defaultValue = null);
        void PutString(string key, string value);
    }
}
