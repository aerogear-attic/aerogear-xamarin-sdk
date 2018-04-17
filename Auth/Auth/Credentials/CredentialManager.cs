using System;
using AeroGear.Mobile.Core.Storage;

namespace AeroGear.Mobile.Auth.Credentials
{
	public class CredentialManager : ICredentialManager
    {
		private static readonly string KeyName = "AuthState";
		private readonly IStorageManager StorageManager;

        /// <summary>
        /// Create a new instance of <see cref="T:AeroGear.Mobile.Auth.Credentials.CredentialManager"/>
		/// using the provided storage manager.
        /// </summary>
        /// <param name="storageManager">Storage manager.</param>
		public CredentialManager(IStorageManager storageManager)
        {
			StorageManager = storageManager;
        }

        /// <summary>
        /// Clear the credential.
        /// </summary>
		public void Clear()
		{
			StorageManager.Remove(KeyName);
		}

        /// <summary>
        /// Load the serialized string representation of the credential.
        /// </summary>
        /// <returns>The serialized credential.</returns>
		public string LoadSerialized()
		{
			return StorageManager.Read(KeyName);
		}

        /// <summary>
        /// Store the specified credential.
        /// </summary>
        /// <param name="credential">The credential to store.</param>
		public void Store(ICredential credential)
		{
			StorageManager.Save(KeyName, credential.SerializedCredential);
		}
	}
}
