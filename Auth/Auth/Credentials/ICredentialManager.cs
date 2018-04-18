using System;
using AeroGear.Mobile.Core.Storage;

namespace AeroGear.Mobile.Auth.Credentials
{
    public interface ICredentialManager
    {
        /// <summary>
        /// Store the specified credential.
        /// </summary>
        /// <param name="credential">Credential to store.</param>
        void Store(ICredential credential);

        /// <summary>
        /// Loads the serialized string representation of the credential.
        /// </summary>
        /// <returns>The serialized credential.</returns>
        string LoadSerialized();

        /// <summary>
        /// Clear the credential.
        /// </summary>
        void Clear();
    }
}
