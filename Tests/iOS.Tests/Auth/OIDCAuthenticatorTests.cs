using System;
using AeroGear.Mobile.Auth.Config;
using NUnit.Framework;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core.Http;
using AeroGear.Mobile.Auth.Authenticator;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.iOS.Tests.Mocks;

namespace AeroGear.Mobile.iOS.Tests.Auth
{
    [TestFixture]
    public class OIDCAuthenticatorTests
    {

        ServiceConfiguration kcConfig = ServiceConfiguration
            .Builder
            .Id("keycloak")
            .Type("keycloak")
            .Url("https://keycloak.url.com")
            .Property("auth-server-url", "https://keycloak.url.com")
            .Property("realm", "test")
            .Property("resource", "test")
            .Build();
        AuthenticationConfig authenticationConfig;
        KeycloakConfig keycloakConfig;

        ICredentialManager credentialManagerMock;
        IHttpServiceModule httpModuleMock;
        ILogger logger = new LoggerMock();

        IAuthenticator authenticatorToTest;

        [SetUp]
        public void SetUp()
        {
            authenticationConfig = AuthenticationConfig.Builder.RedirectUri("test://example.com").Build();
            keycloakConfig = new KeycloakConfig(kcConfig);
            credentialManagerMock = new CredentialManagerMock();
            IHttpResponse httpResponse = (HttpResponseMock)HttpResponseMock.newResponse().withStatusCode(200);
            var requestToBeExecuted = new HttpRequestToBeExecutedMock(Task.FromResult(httpResponse), null);
            IHttpRequest httpRequestMock = (HttpRequestMock)HttpRequestMock.newRequest().withGetResult(requestToBeExecuted);
            httpModuleMock = (HttpServiceModuleMock)HttpServiceModuleMock.newMock().withNewRequest(httpRequestMock);
            authenticatorToTest = new OIDCAuthenticator(authenticationConfig, keycloakConfig, credentialManagerMock, httpModuleMock, logger);
        }

        [Test]
        public async void TestLogout()
        {
            var currentUer = User.NewUser().WithUsername("testuser").WithIdentityToken("testid");

            bool result = await authenticatorToTest.Logout(currentUer);
            Assert.IsTrue(result);
        }
    }
}
