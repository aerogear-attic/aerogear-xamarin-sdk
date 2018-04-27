using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core;
using Android.Content;

namespace AeroGear.Mobile.Auth
{
    /// <summary>
    /// <see cref="IAuthService"/> implementation for the Android platform.
    /// </summary>
    public class AuthService : AbstractAuthService
    {
        public AuthService()
        {
        }

        /// <summary>
        /// Retrieves the current authenticated user. If there is no currently
        /// authenticated user then <c>null</c> is returned.
        /// </summary>
        /// <returns>The current user if authenticated. Else <c>null</c>.</returns>
        public override User CurrentUser()
        {
            var serializedCredential = CredentialManager.LoadSerialized();
            if (serializedCredential == null)
            {
                return null;
            }
            var parsedCredential = new OIDCCredential(serializedCredential);
            return User.NewUser().FromUnverifiedCredential(parsedCredential, KeycloakConfig.ResourceId);
        }

        /// <summary>
        /// Handles the auth result.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        /// <param name="data">Intent.</param>
        public Task HandleAuthResult(Intent data)
        {
            return ((OIDCAuthenticator)Authenticator).HandleAuthResult(data);
        }

        /// <summary>
        /// Initializes the service with the provided authentication configuration.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="authConfig">Auth config.</param>
        public override void Init(AuthenticationConfig authConfig)
        {
            Authenticator = new OIDCAuthenticator(authConfig, KeycloakConfig, CredentialManager, MobileCore.HttpLayer, MobileCore.Logger);
        }
    }
}