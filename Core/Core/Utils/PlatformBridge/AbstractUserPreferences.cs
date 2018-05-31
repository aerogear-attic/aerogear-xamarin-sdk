using System;
using static AeroGear.Mobile.Core.Utils.SanityCheck;
namespace AeroGear.Mobile.Core.Utils
{
    /// <summary>
    /// Base class for user preferences. Automatically performs sanity checks on parameters.
    /// </summary>
    public abstract class AbstractUserPreferences : IUserPreferences
    {
        /// <summary>
        /// Returns the string associated with the given key.
        /// </summary>
        /// <returns>The get string.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        protected abstract string doGetString(string key, string defaultValue = null);

        /// <summary>
        /// Adds a new key,value pair to the user preferences.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        protected abstract void doPutString(string key, string value);

        /// <summary>
        /// Remove the value associated to the specific key from the user preferences.
        /// </summary>
        /// <param name="key">Key.</param>
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
