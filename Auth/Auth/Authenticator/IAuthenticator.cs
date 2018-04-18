using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public interface IAuthenticator
    {
        Task<User> Authenticate(IAuthenticateOptions authenticateOptions);

        Task<bool> Logout(User currentUser);
    }
}
