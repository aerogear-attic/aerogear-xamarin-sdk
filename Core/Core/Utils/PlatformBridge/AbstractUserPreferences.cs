using System;
using static AeroGear.Mobile.Core.Utils.SanityCheck;
namespace AeroGear.Mobile.Core.Utils
{
    public abstract class AbstractUserPreferences : IUserPreferences
    {
        protected abstract string doGetString(string key, string defaultValue = null);
        protected abstract void doPutString(string key, string value);
        protected abstract void doRemoveValue(string key);

        public string GetString(string key, string defaultValue = null)
        {
            return doGetString(NonNull(key, "key"), defaultValue);
        }

        public void PutString(string key, string value)
        {
            doPutString(NonNull(key, "key"), NonNull(value, "value"));
        }

        public void RemoveValue(string key)
        {
            doRemoveValue(NonNull(key, "key"));
        }
    }
}
