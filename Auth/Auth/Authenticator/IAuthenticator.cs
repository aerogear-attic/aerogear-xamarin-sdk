using System;
using System.Threading.Tasks;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Auth.Credentials;

namespace AeroGear.Mobile.Auth.Authenticator
{
    /// <summary>
    /// Interface for authenticators. Authenticator will perform the actual authentication actions.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Perform the authentication request
        /// </summary>
        /// <returns>The authenticated user</returns>
        /// <param name="authenticateOptions">Authenticate options. See <see cref="IAuthenticateOptions"/></param>
        Task<User> Authenticate(IAuthenticateOptions authenticateOptions);

        /// <summary>
        /// Logout the specified currentUser.
        /// </summary>
        /// <returns>If the logout operation is successful.</returns>
        /// <param name="currentUser">Current user.</param>
        Task<bool> Logout(User currentUser);
    }
}
