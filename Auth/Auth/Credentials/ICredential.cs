using System;

namespace AeroGear.Mobile.Auth.Credentials
{
    /// <summary>
    /// Credentials for OpenID Connect.
    /// </summary>
    public interface ICredential
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        string AccessToken { get; }

        /// <summary>
        /// Gets the identity token.
        /// </summary>
        /// <value>The identity token.</value>
        string IdentityToken { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <value>The refresh token.</value>
        string RefreshToken { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AeroGear.Mobile.Auth.Credentials.ICredential"/> is authorized.
        /// </summary>
        /// <value><c>true</c> if is authorized; otherwise, <c>false</c>.</value>
        bool IsAuthorized { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AeroGear.Mobile.Auth.Credentials.ICredential"/> is expired.
        /// </summary>
        /// <value><c>true</c> if is expired; otherwise, <c>false</c>.</value>
        bool IsExpired { get; }

        /// <summary>
        /// Serialized version of the credential. Can be used for logging etc.
        /// </summary>
        /// <value>The serialized credential.</value>
        string SerializedCredential { get; }
    }
}