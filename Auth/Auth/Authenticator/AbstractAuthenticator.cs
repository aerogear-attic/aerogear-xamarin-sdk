using System;
using System.Net;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core.Http;
using AeroGear.Mobile.Core.Logging;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace AeroGear.Mobile.Auth.Authenticator
{
    /// <summary>
    /// Abstract base class for authenticators.
    /// </summary>
    public abstract class AbstractAuthenticator : IAuthenticator
    {
        protected readonly AuthenticationConfig authenticationConfig;
        protected readonly KeycloakConfig keycloakConfig;
        protected readonly ICredentialManager credentialManager;
        protected readonly IHttpServiceModule httpService;
        protected readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Authenticator.AbstractAuthenticator"/> class.
        /// </summary>
        /// <param name="authenticationConfig">Authentication config.</param>
        /// <param name="keycloakConfig">Keycloak config.</param>
        /// <param name="credentialManager">Credential manager.</param>
        /// <param name="httpServiceModule">Http service module.</param>
        /// <param name="logger">Logger.</param>
        public AbstractAuthenticator(AuthenticationConfig authenticationConfig, KeycloakConfig keycloakConfig, ICredentialManager credentialManager, IHttpServiceModule httpServiceModule, ILogger logger)
        {
            this.authenticationConfig = NonNull(authenticationConfig, "authenticationConfig");
            this.keycloakConfig = NonNull(keycloakConfig, "keycloakConfig");
            this.credentialManager = NonNull(credentialManager, "credentialManager");
            this.httpService = NonNull(httpServiceModule, "httpServiceModule");
            this.logger = NonNull(logger, "logger");
        }

        /// <summary>
        /// Logout the specified currentUser.
        /// </summary>
        /// <returns>The logout result.</returns>
        /// <param name="currentUser">User to be logged out.</param>
        public async Task<bool> Logout(User currentUser)
        {
            NonNull(currentUser, "current user");
            string identityToken = currentUser.IdentityToken;
            var logoutUrl = keycloakConfig.LogoutUrl(identityToken, authenticationConfig.RedirectUri.ToString());

            TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
            IHttpResponse response = await httpService.NewRequest().Get(logoutUrl).Execute();
            if (response.StatusCode == (int)HttpStatusCode.OK || response.StatusCode == (int)HttpStatusCode.Redirect)
            {
                credentialManager.Clear();
                completionSource.TrySetResult(true);
            }
            else
            {
                Exception error = response.Error;
                if (error == null)
                {
                    error = new Exception("Non HTTP 200 or 302 status code");
                }
                completionSource.TrySetException(error);
            }
            return await completionSource.Task;
        }

        public abstract Task<User> Authenticate(IAuthenticateOptions authenticateOptions);
    }
}
