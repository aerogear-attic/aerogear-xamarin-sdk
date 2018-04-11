using System;
namespace AeroGear.Mobile.Auth.Credentials
{
    /// <summary>
    /// Credential manager used to persist and retrieve credentials on a device.
    /// </summary>
    public interface ICredentialManager
    {
        /// <summary>
        /// Save a credential using the specified key. The key can later be
        /// use for retrieval of removal of the credential.
        /// </summary>
        /// <param name="key">The key to save a credential to.</param>
        /// <param name="value">The credential to save.</param>
        void Save(string key, ICredential value);

        /// <summary>
        /// Read a credential from the specified key.
        /// </summary>
        /// <returns>Credential at the specified key.</returns>
        /// <param name="key">The key to read a credential from.</param>
        ICredential Read(string key);

        /// <summary>
        /// Remove the credential at the specified key.
        /// </summary>
        /// <param name="key">The key to remove a credential from.</param>
        void Remove(string key);

        /// <summary>
        /// Remove all credentials.
        /// </summary>
        void Clear();
    }
}
