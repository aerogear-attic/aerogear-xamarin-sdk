using System;
namespace AeroGear.Mobile.Core.Utils
{
    /// <summary>
    /// Represents an user preferences store.
    /// </summary>
    public interface IUserPreferences
    {
        /// <summary>
        /// Returns the value associated with the provided key. <paramref name="defaultValue"/> or <see langword="null"/> if it does not exists.
        /// </summary>
        /// <returns>The svalue associated with the given key.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value. Returned if the key is not present</param>
        string GetString(string key, string defaultValue = null);

        /// <summary>
        /// Adds a value to the user preferences store.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void PutString(string key, string value);

        /// <summary>
        /// Removes a value from the preferences store.
        /// </summary>
        /// <param name="key">Key.</param>
        void RemoveValue(string key);
    }
}
