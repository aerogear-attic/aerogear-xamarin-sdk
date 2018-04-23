using System;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Core.Storage
{
    public class StorageManager : IStorageManager
    {
        private readonly SecureKeyValueStore KeyValStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Core.Storage.StorageManager"/> class.
        /// </summary>
        /// <param name="storeName">Store name.</param>
        /// <param name="context">Context.</param>
        public StorageManager(string storeName)
        {
            KeyValStore = new SecureKeyValueStore(storeName);
        }

        /// <summary>
        /// Read the specified key.
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="key">Key.</param>
        /// <exception cref="ArgumentNullException">Thrown when the key is <c>null</c>.</exception>
        public string Read(string key)
        {
            NonNull(key, "key");
            return KeyValStore.Load(key);
        }

        /// <summary>
        /// Remove the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <exception cref="ArgumentNullException">Thrown when the key is <c>null</c>.</exception>
        public void Remove(string key)
        {
            NonNull(key, "key");
            KeyValStore.Remove(key);
        }

        /// <summary>
        /// Save the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentNullException">Thrown when the key is <c>null</c>.</exception>
        public void Save(string key, string value)
        {
            NonNull(key, "key");
            if (value == null)
            {
                KeyValStore.Remove(key);
                return;
            }
            KeyValStore.Save(key, value);
        }
    }
}
