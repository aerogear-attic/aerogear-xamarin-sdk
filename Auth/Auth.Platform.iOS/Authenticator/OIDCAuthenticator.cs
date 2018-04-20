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


namespace AeroGear.Mobile.Auth.Authenticator
{
    public class OIDCAuthenticator : IAuthenticator
    {
        private AuthenticationConfig authenticationConfig;
        private KeycloakConfig keycloakConfig;
        private ICredentialManager credentialManager;

        private IAuthorizationFlowSession currentAuthorizationFlow;

        private delegate void OIDAuthFlowCallback(OIDCCredential credential, NSError error);

        private TaskCompletionSource<User> authenticateTaskComplete;
        private Task<User> authenticateTask;

        private IHttpServiceModule httpService;

        public OIDCAuthenticator(AuthenticationConfig authenticationConfig, KeycloakConfig keycloakConfig, ICredentialManager credentialManager, IHttpServiceModule httpServiceModule)
        {
            this.authenticationConfig = nonNull(authenticationConfig, "authenticationConfig");
            this.keycloakConfig = nonNull(keycloakConfig, "keycloakConfig");
            this.credentialManager = nonNull(credentialManager, "credentialManager");
            this.httpService = nonNull(httpServiceModule, "httpServiceModule");
        }

        public Task<User> Authenticate(IAuthenticateOptions authenticateOptions)
        {
            IOSAuthenticateOptions options = nonNull((IOSAuthenticateOptions)authenticateOptions, "authenticateOptions");
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
