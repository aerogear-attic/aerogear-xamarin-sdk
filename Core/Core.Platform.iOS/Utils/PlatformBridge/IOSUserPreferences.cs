using System;
using Foundation;

namespace AeroGear.Mobile.Core.Utils
{
    public class IOSUserPreferences : IUserPreferences
    {
        private readonly NSUserDefaults userDefaults;

        public IOSUserPreferences()
        {
            this.userDefaults = NSUserDefaults.StandardUserDefaults;
        }

        public string GetString(string key, string defaultValue = null)
        {
            return userDefaults.StringForKey(key) ?? defaultValue;
        }

        public void PutString(string key, string value)
        {
            userDefaults.SetString(value, key);
            userDefaults.Synchronize();
        }
    }
}
