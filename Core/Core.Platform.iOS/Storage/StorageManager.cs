using System;
namespace AeroGear.Mobile.Core.Storage
{
	public class StorageManager : IStorageManager
    {
		private readonly KeychainWrapper Keychain;

		/// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Core.Storage.StorageManager"/> class.
        /// </summary>
        /// <param name="storeName">Store name.</param>
        /// <param name="context">Context.</param>
        public StorageManager(string storeName)
        {
			Keychain = new KeychainWrapper(storeName);
        }

		/// <summary>
        /// Read the specified key.
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="key">Key.</param>
		/// <exception cref="ArgumentNullException">Thrown when the key is <c>null</c>.</exception>
		public string Read(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key cannot be null");
			}
			return Keychain.Load(key);
		}

		/// <summary>
        /// Remove the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
		/// <exception cref="ArgumentNullException">Thrown when the key is <c>null</c>.</exception>
		public void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key cannot be null");
			}
			Keychain.Remove(key);
		}

        /// <summary>
        /// Save the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
		/// <exception cref="ArgumentNullException">Thrown when the key is <c>null</c>.</exception>
		public void Save(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key cannot be null");
			}
			if (value == null)
			{
				Keychain.Remove(key);
				return;
			}
			Keychain.Save(key, value);
		}
	}
}
