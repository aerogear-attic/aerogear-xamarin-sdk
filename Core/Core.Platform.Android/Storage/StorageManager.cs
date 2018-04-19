using System;
using Android.Content;
using Java.Lang;

namespace AeroGear.Mobile.Core.Storage
{
    public class StorageManager : IStorageManager
    {
        private readonly ISharedPreferences SharedPreferences;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Core.Storage.StorageManager"/> class.
        /// </summary>
        /// <param name="storeName">Store name.</param>
        /// <param name="context">Context.</param>
        public StorageManager(string storeName, Context context)
        {
            SharedPreferences = context.ApplicationContext.GetSharedPreferences(storeName, FileCreationMode.Private);
        }

        /// <summary>
        /// Read the specified key.
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="key">Key.</param>
        public string Read(string key)
        {
            return SharedPreferences.GetString(key, null);
        }

        /// <summary>
        /// Remove the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(string key)
        {
            if (!SharedPreferences.Edit().Remove(key).Commit())
            {
                throw new IllegalArgumentException("Failed to clear state from shared preferences");
            }
        }

        /// <summary>
        /// Save the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Save(string key, string value)
        {
            if (!SharedPreferences.Edit().PutString(key, value).Commit())
            {
                throw new IllegalArgumentException("Failed to update state from shared preferences");
            }
        }
    }
}
