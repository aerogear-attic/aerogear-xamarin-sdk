using System;
using AeroGear.Mobile.Core.Utils;

namespace AeroGear.Mobile.Auth.Credentials
{
    /// <summary>
    /// A class to persist/remove the credential data on a device.
    /// </summary>
    public class CredentialManager : ICredentialManager
    {
        private static readonly string KeyName = "AuthState";
        private readonly IUserPreferences userPreferences;

        /// <summary>
        /// Create a new instance of <see cref="T:AeroGear.Mobile.Auth.Credentials.CredentialManager"/>
        /// using the provided storage manager.
        /// </summary>
        /// <param name="storageManager">Storage manager.</param>
        public CredentialManager(IUserPreferences userPreferences)
        {
            this.userPreferences = userPreferences;
        }

        /// <summary>
        /// Clear the credential.
        /// </summary>
        public void Clear()
        {
            userPreferences.RemoveValue(KeyName);
        }

        /// <summary>
        /// Load the serialized string representation of the credential.
        /// </summary>
        /// <returns>The serialized credential.</returns>
        public string LoadSerialized()
        {
            return userPreferences.GetString(KeyName);
        }

        /// <summary>
        /// Store the specified credential.
        /// </summary>
        /// <param name="credential">The credential to store.</param>
        public void Store(ICredential credential)
        {
            userPreferences.PutString(KeyName, credential.SerializedCredential);
        }
    }
}
