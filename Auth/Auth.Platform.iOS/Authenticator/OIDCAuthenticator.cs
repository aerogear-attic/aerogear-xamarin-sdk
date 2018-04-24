using System;
using System.Net;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Auth.Credentials;
using OpenId.AppAuth;
using UIKit;
using AeroGear.Mobile.Core.Http;

using Foundation;

using static AeroGear.Mobile.Core.Utils.SanityCheck;
using AeroGear.Mobile.Core.Logging;

namespace AeroGear.Mobile.Auth.Authenticator
{
    /// <summary>
    /// Concrete OIDC Authenticator implementation for iOS
    /// </summary>
    public class OIDCAuthenticator : AbstractAuthenticator
    {
        private IAuthorizationFlowSession currentAuthorizationFlow;

        private delegate void OIDAuthFlowCallback(OIDCCredential credential, NSError error);

        private TaskCompletionSource<User> authenticateTaskComplete;
        private Task<User> authenticateTask;

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
            IOSAuthenticateOptions options = NonNull((IOSAuthenticateOptions)authenticateOptions, "authenticateOptions");
            ServiceConfiguration oidServiceConfiguration = new ServiceConfiguration(this.keycloakConfig.AuthenticationEndpoint, this.keycloakConfig.TokenEndpoint); // = GetAuthorizationServiceConfiguration();
            AuthorizationRequest oidAuthRequest = new AuthorizationRequest(oidServiceConfiguration,
                                                                           this.keycloakConfig.ResourceId,
                                                                           new String[] { Scope.OpenId, Scope.Profile },
                                                                           this.authenticationConfig.RedirectUri,
                                                                           ResponseType.Code, 
                                                                           null);

            authenticateTaskComplete = new TaskCompletionSource<User>();
            authenticateTask = authenticateTaskComplete.Task;

            this.currentAuthorizationFlow = startAuthorizationFlow(oidAuthRequest, options.PresentingViewController, (OIDCCredential credential, NSError error) =>
            {
                if (credential != null) {
                    authenticateTaskComplete.TrySetResult(User.NewUser().FromUnverifiedCredential(credential, this.keycloakConfig.ResourceId));
                } else {
                    authenticateTaskComplete.TrySetException(new Exception(error.LocalizedDescription));
                }
            });

            return authenticateTask;
        }

        /// <summary>
        /// Sends a request to the Keycloak server to perform token exchange.
        /// On successfully completing the token exchange the callback is invoked with the `openid` credentials for the user.
        /// Otherwise the callback is invoked with the error that occured during token exchange.
        /// </summary>
        /// <returns>The authorization flow.</returns>
        /// <param name="request">an openid authorisation request.</param>
        /// <param name="presentingViewController">The view controller from which to present the SafariViewController.</param>
        /// <param name="callback">a callback function that will be invoked when the token exchange is completed.</param>
        private IAuthorizationFlowSession startAuthorizationFlow(AuthorizationRequest request, UIViewController presentingViewController, OIDAuthFlowCallback callback)
        {
            return AuthState.PresentAuthorizationRequest(request, presentingViewController, (authState, error) =>
            {
                if (authState == null || error != null) 
                {
                    callback(null, error); 
                } 
                else 
                {
                    callback(new OIDCCredential(authState), null);
                }
            });
        }
    }
}
