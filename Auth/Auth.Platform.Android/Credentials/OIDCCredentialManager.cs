using System;
using AeroGear.Mobile.Auth.Credentials;
using Android.App;
using Android.Content;
using Java.Lang;

namespace AeroGear.Mobile.Auth.Credentials
{
    /// <summary>
    /// Class for peristing, retrieving, updating and removing OpenID Connect
    /// credentials on a device.
    /// </summary>
    public class OIDCCredentialManager : ICredentialManager
    {
        private static readonly string StoreName = "AeroGear.Mobile.Auth.Credentials";

        private ISharedPreferences SharedPreferences;

        private static OIDCCredentialManager _Instance;

        /// <summary>
        /// Instance of the credential manager using the application context.
        /// </summary>
        /// <value>Instance of <see cref="OIDCCredentialManager"/></value>
        public static OIDCCredentialManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new OIDCCredentialManager(Application.Context.ApplicationContext);
                }
                return _Instance;
            }
        }

        private OIDCCredentialManager(Context context)
        {
            SharedPreferences = context.GetSharedPreferences(StoreName, FileCreationMode.Private);
        }

        /// <summary>
        /// Retrieve an <see cref="OIDCCredential"/> using the specified key.
        /// If there is nothing previously stored using the key a new
        /// <see cref="OIDCCredential"/> will be returned.
        /// </summary>
        /// <returns><see cref="OIDCCredential"/></returns>
        /// <param name="key">Key to retrieve the credential from.</param>
        public ICredential Read(string key)
        {
            string currentState = SharedPreferences.GetString(key, null);
            return currentState == null ? new OIDCCredential() : new OIDCCredential(currentState);
        }

        /// <summary>
        /// Remove the <see cref="OIDCCredential"/> at the specified key. If
        /// this removal fails an <see cref="IllegalStateException"/> will be
        /// thrown.
        /// </summary>
        /// <param name="key">Key of the credential to remove.</param>
        public void Remove(string key)
        {
            if (!SharedPreferences.Edit().Remove(key).Commit())
            {
                throw new IllegalStateException("Failed to clear state from shared preferences");
            }
        }

        /// <summary>
        /// Store a <see cref="OIDCCredential"/> using the specified key. This
        /// key will act as an identifier for retrieving, updating or removing
        /// the <see cref="OIDCCredential"/> at a later time. If the saving of
        /// the credential fails an <see cref="IllegalStateException"/> will be
        /// thrown.
        /// </summary>
        /// <param name="key">The key to store the credential with.</param>
        /// <param name="credential">The credential to store.</param>
        public void Save(string key, ICredential credential)
        {
            if (credential == null)
            {
                Remove(key);
                return;
            }
            if (!SharedPreferences.Edit().PutString(key, credential.SerializedCredential).Commit())
            {
                throw new IllegalArgumentException("Failed to update state from shared preferences");
            }
        }
    }
}