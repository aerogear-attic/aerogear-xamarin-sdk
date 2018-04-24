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
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.Auth.Authenticator
{
    public class OIDCAuthenticator : AbstractAuthenticator
    {
        private AuthState authState;
        private AuthorizationService authorizationService;

        private TaskCompletionSource<User> authenticateTaskComplete;
        private Task<User> authenticateTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Auth.Authenticator.OIDCAuthenticator"/> class.
        /// </summary>
        /// <param name="authenticationConfig">Authentication config.</param>
        /// <param name="keycloakConfig">Keycloak config.</param>
        /// <param name="credentialManager">Credential manager.</param>
        /// <param name="httpServiceModule">Http service module.</param>
        public OIDCAuthenticator(AuthenticationConfig authenticationConfig, 
                                 KeycloakConfig keycloakConfig, 
                                 ICredentialManager credentialManager, 
                                 IHttpServiceModule httpServiceModule, 
                                 ILogger logger) : base(authenticationConfig, keycloakConfig, credentialManager, httpServiceModule, logger)
        {
        }

        /// <summary>
        /// Perform the authentication request
        /// </summary>
        /// <returns>The authenticate.</returns>
        /// <param name="authenticateOptions">Authenticate options.</param>
        override public Task<User> Authenticate(IAuthenticateOptions authenticateOptions)
        {
            AndroidAuthenticateOptions authOptions = (AndroidAuthenticateOptions)NonNull(authenticateOptions, "authenticateOptions");
            Activity fromActivity = NonNull(authOptions.FromActvity, "fromActivity");
            int resultCode = NonNull(authOptions.ResultCode, "resultCode");

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
            NonNull(intent, "intent");
            NonNull(authenticateTaskComplete, "authenticateTaskComplete");

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
            return User.NewUser().FromUnverifiedCredential(credential, keycloakConfig.ResourceId);
        }
    }
}
