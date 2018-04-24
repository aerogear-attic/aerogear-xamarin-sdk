using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Authenticator;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Core;

namespace AeroGear.Mobile.Auth
{
    /// <summary>
    /// Interface for an auth service module <see cref="IServiceModule"/>.
    /// </summary>
    public interface IAuthService : IServiceModule
    {
        /// <summary>
        /// Retrieve the current user.
        /// </summary>
        /// <returns>The current user.</returns>
        User CurrentUser();

        /// <summary>
        /// Initiate an authentication flow.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task<User> Authenticate(IAuthenticateOptions authenticateOptions);

        /// <summary>
        /// Configure the service module.
        /// </summary>
        /// <param name="authConfig">Authentication configuration.</param>
        void Configure(AuthenticationConfig authConfig);

        /// <summary>
        /// Logout the specified user.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task<bool> Logout(User user);
    }
}