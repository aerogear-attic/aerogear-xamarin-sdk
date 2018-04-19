using System;
using AeroGear.Mobile.Auth.Config;
using NUnit.Framework;
using Moq;
using AeroGear.Mobile.Auth.Credentials;
using AeroGear.Mobile.Core.Http;
using AeroGear.Mobile.Auth.Authenticator;
using System.Threading.Tasks;
using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Logging;

namespace AeroGear.Mobile.Android.Tests.Auth
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

        Mock<ICredentialManager> credentialManagerMock;
        Mock<IHttpServiceModule> httpModuleMock;
        Mock<ILogger> logger = new Mock<ILogger>();

        IAuthenticator authenticatorToTest;

        [SetUp]
        public void SetUp()
        {
            authenticationConfig = AuthenticationConfig.Builder.RedirectUri("test://example.com").Build();
            keycloakConfig = new KeycloakConfig(kcConfig);

            credentialManagerMock = new Mock<ICredentialManager>();
            httpModuleMock = new Mock<IHttpServiceModule>();

            var httpRequestMock = new Mock<IHttpRequest>();
            var requestToBeExecuted = new Mock<IHttpRequestToBeExecuted>();
            var httpResponse = new Mock<IHttpResponse>();
            httpResponse.Setup(response => response.StatusCode).Returns(200);
            requestToBeExecuted.Setup(arg => arg.Execute()).Returns(Task.FromResult(httpResponse.Object));
            httpRequestMock.Setup(request => request.Get(It.IsAny<string>())).Returns(requestToBeExecuted.Object);
            httpModuleMock.Setup(arg => arg.NewRequest()).Returns(httpRequestMock.Object);

            authenticatorToTest = new OIDCAuthenticator(authenticationConfig, keycloakConfig, credentialManagerMock.Object, httpModuleMock.Object, logger.Object);
        }

        [Test]
        public async void TestLogout()
        {
            var currentUer = User.NewUser().WithUsername("testuser").WithIdentityToken("testid");

            bool result = await authenticatorToTest.Logout(currentUer);
            credentialManagerMock.Verify(arg => arg.Clear(), Times.Once);
            Assert.IsTrue(result);
        }
    }
}
