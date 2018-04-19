using System;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using Android.App;
using static AeroGear.Mobile.Core.Utils.SanityCheck;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;

using OpenId.AppAuth;
using Android.Content;
using OpenId.AppAuth.Browser;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Http;
using System.Net;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class OIDCAuthenticator : IAuthenticator
    {
        private AuthenticationConfig authenticationConfig;
        private KeycloakConfig keycloakConfig;
        private ICredentialManager credentialManager;
        private IHttpServiceModule httpService;

        private AuthState authState;
        private AuthorizationService authorizationService;

        private TaskCompletionSource<User> authenticateTaskComplete;
        private Task<User> authenticateTask;

        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Authenticator.OIDCAuthenticator"/> class.
        /// </summary>
        /// <param name="authenticationConfig">Authentication config.</param>
        /// <param name="keycloakConfig">Keycloak config.</param>
        /// <param name="credentialManager">Credential manager.</param>
        /// <param name="httpServiceModule">Http service module.</param>
        public OIDCAuthenticator(AuthenticationConfig authenticationConfig, KeycloakConfig keycloakConfig, ICredentialManager credentialManager, IHttpServiceModule httpServiceModule, ILogger logger)
        {
            this.authenticationConfig = nonNull(authenticationConfig, "authenticationConfig");
            this.keycloakConfig = nonNull(keycloakConfig, "keycloakConfig");
            this.credentialManager = nonNull(credentialManager, "credentialManager");
            this.httpService = nonNull(httpServiceModule, "httpServiceModule");
            this.logger = nonNull(logger, "logger");
        }

        /// <summary>
        /// Perform the authentication request
        /// </summary>
        /// <returns>The authenticate.</returns>
        /// <param name="authenticateOptions">Authenticate options.</param>
        public Task<User> Authenticate(IAuthenticateOptions authenticateOptions)
        {

            AndroidAuthenticateOptions authOptions = (AndroidAuthenticateOptions)nonNull(authenticateOptions, "authenticateOptions");
            Activity fromActivity = nonNull(authOptions.FromActvity, "fromActivity");
            int resultCode = nonNull(authOptions.ResultCode, "resultCode");

            authenticateTaskComplete = new TaskCompletionSource<User>();
            authenticateTask = authenticateTaskComplete.Task;

            AuthorizationServiceConfiguration authorizationServiceConfiguration = GetAuthorizationServiceConfiguration();

            authorizationService = GetAuthorizationService(fromActivity);
            authState = new AuthState(authorizationServiceConfiguration);

            AuthorizationRequest authorizationRequest = new AuthorizationRequest.Builder(authorizationServiceConfiguration, keycloakConfig.ResourceId, ResponseTypeValues.Code, parseUri(authenticationConfig.RedirectUri))
                                                                                .SetScopes(authenticationConfig.Scopes)
                                                                                .Build();

            Intent authIntent = authorizationService.GetAuthorizationRequestIntent(authorizationRequest);
            fromActivity.StartActivityForResult(authIntent, resultCode);
            return authenticateTask;
        }

        /// <summary>
        /// Handles the auth result.Should be called from <see cref="Activity.OnActivityResult(int, Result, Intent)">
        /// </summary>
        /// <returns>The auth result.</returns>
        /// <param name="intent">Intent.</param>
        public async Task HandleAuthResult(Intent intent)
        {
            nonNull(intent, "intent");
            nonNull(authenticateTaskComplete, "authenticateTaskComplete");

            AuthorizationResponse response = AuthorizationResponse.FromIntent(intent);
            AuthorizationException error = AuthorizationException.FromIntent(intent);

            authState.Update(response, error);

            if (response != null)
            {
                try
                {
                    User user = await exchangeTokens(response);
                    authenticateTaskComplete.TrySetResult(user);
                }
                catch (Exception ex)
                {
                    this.logger.Error("Unexpected error in token exchange", ex);
                    authenticateTaskComplete.TrySetException(ex);
                }
            }
            else
            {
                authenticateTaskComplete.TrySetException(error);
            }
        }

        private AuthorizationServiceConfiguration GetAuthorizationServiceConfiguration()
        {
            var authEndpoint = parseUri(keycloakConfig.AuthenticationEndpoint);
            var tokenEndpoint = parseUri(keycloakConfig.TokenEndpoint);

            return new AuthorizationServiceConfiguration(authEndpoint, tokenEndpoint);
        }

        private AuthorizationService GetAuthorizationService(Context appContext)
        {
            AppAuthConfiguration appAuthConfiguration = new AppAuthConfiguration.Builder()
                                                                                .SetBrowserMatcher(new BrowserBlacklist(VersionedBrowserMatcher.ChromeCustomTab))
                                                                                .Build();
            return new AuthorizationService(appContext.ApplicationContext, appAuthConfiguration);
        }

        private Android.Net.Uri parseUri(Uri origin)
        {
            return Android.Net.Uri.Parse(origin.ToString());
        }

        private async Task<User> exchangeTokens(AuthorizationResponse response)
        {
            TokenResponse tokenResponse = await authorizationService.PerformTokenRequestAsync(response.CreateTokenExchangeRequest());
            authState.Update(tokenResponse, null);
            ICredential credential = new OIDCCredential(authState.JsonSerializeString());
            credentialManager.Store(credential);
            User user = GetUser(credential);
            return user;
        }

        private User GetUser(ICredential credential)
        {
            //TODO: Implement me!! Need to decode the JWT token from credentials and return the user info
            return User.newUser().WithFirstName("test").WithLastName("test").WithEmail("test@example.com");
        }

        /// <summary>
        /// Logout the specified currentUser.
        /// </summary>
        /// <returns>True of the user is logged out successfully</returns>
        /// <param name="currentUser">Current user.</param>
        public async Task<bool> Logout(User currentUser)
        {
            nonNull(currentUser, "current user");
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
    }
}
