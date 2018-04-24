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
