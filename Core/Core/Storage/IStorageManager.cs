using System;
namespace AeroGear.Mobile.Core.Storage
{
    /// <summary>
    /// Interface for a storage manager. Used to store, retrieve, update and
    /// remove data.
    /// </summary>
    public interface IStorageManager
    {
        /// <summary>
        /// Save the value using the specified key. The key acts as an
        /// identifier for the value and can be used to retrive, delete or
        /// update the value at a later date.
        /// </summary>
        /// <param name="key">The key to save the value with.</param>
        /// <param name="value">The value to be saved.</param>
        void Save(string key, string value);

        /// <summary>
        /// Retrieve the value associated with the specified key.
        /// </summary>
        /// <returns>The value associated with the key.</returns>
        /// <param name="key">The identifier of the value to retrieve.</param>
        string Read(string key);

        /// <summary>
        /// Remove the value assocated with the specified key.
        /// </summary>
        /// <param name="key">The identifier if the value to remove.</param>
        void Remove(string key);
    }
}