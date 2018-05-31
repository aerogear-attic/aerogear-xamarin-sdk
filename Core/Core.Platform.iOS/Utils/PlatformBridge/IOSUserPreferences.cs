using System;
using Foundation;

namespace AeroGear.Mobile.Core.Utils
{
    public class IOSUserPreferences : AbstractUserPreferences
    {
        private readonly NSUserDefaults userDefaults;

        public IOSUserPreferences()
        {
            this.userDefaults = NSUserDefaults.StandardUserDefaults;
        }

        protected override string doGetString(string key, string defaultValue = null)
        {
            return userDefaults.StringForKey(key) ?? defaultValue;
        }

        protected override void doPutString(string key, string value)
        {
            userDefaults.SetString(value, key);
            userDefaults.Synchronize();
        }

        protected override void doRemoveValue(string key)
        {
            userDefaults.RemoveObject(key);
            userDefaults.Synchronize();
        }
    }
}
