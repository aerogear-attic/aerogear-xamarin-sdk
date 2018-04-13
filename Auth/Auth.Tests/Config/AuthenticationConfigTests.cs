using AeroGear.Mobile.Auth.Config;
using NUnit.Framework;
using System;

namespace AeroGear.Mobile.Auth.Tests.Config
{
    [TestFixture(Category = "Configuration")]
    public class AuthenticationConfigTests
    {
        AuthenticationConfig authConfig;

        [SetUp]
        public void SetUp()
        {
            authConfig = AuthenticationConfig.Builder
                                             .RedirectUri("org.aerogear.mobile.example:/callback")
                                             .Scopes("dummyScope")
                                             .MinTimeBetweenJwksRequests(8000)
                                             .Build();
        }

        [Test]
        public void TestAuthConfig()
        {
            var expectedRedirectUri = new Uri("org.aerogear.mobile.example:/callback");
            var actualRedirectUri = authConfig.RedirectUri;

            var expectedScopes = "dummyScope";
            var actualScopes = authConfig.Scopes;

            var expectedMinTimeBetweenJwksRequests = 8000;
            var actualMinTimeBetweenJwksRequests = authConfig.MinTimeBetweenJwksRequests;

            Assert.AreEqual(expectedRedirectUri, actualRedirectUri);
            Assert.AreEqual(expectedScopes, actualScopes);
            Assert.AreEqual(expectedMinTimeBetweenJwksRequests, actualMinTimeBetweenJwksRequests);
        }

        [Test]
        public void TestDefaultConfig()
        {
            AuthenticationConfig defaultAuthConfig = AuthenticationConfig
                .Builder
                .RedirectUri("org.aerogear.mobile.example:/callback")
                .Build();

            var expectedRedirectUri = new Uri("org.aerogear.mobile.example:/callback");
            var actualRedirectUri = defaultAuthConfig.RedirectUri;

            var expectedScopes = "openid";
            var actualScopes = defaultAuthConfig.Scopes;

            var expectedMinTimeBetweenJwksRequests = 24 * 60;
            var actualMinTimeBetweenJwksRequests = defaultAuthConfig.MinTimeBetweenJwksRequests;

            Assert.AreEqual(expectedRedirectUri, actualRedirectUri);
            Assert.AreEqual(expectedScopes, actualScopes);
            Assert.AreEqual(expectedMinTimeBetweenJwksRequests, actualMinTimeBetweenJwksRequests);
        }

        [Test]
        public void TestNullAttributes()
        {
            AuthenticationConfig.AuthenticationConfigBuilder authConfigBuilder = AuthenticationConfig.Builder;

            Assert.Throws<ArgumentNullException>(() => authConfigBuilder.RedirectUri(null).Build());

            var expectedDefaultScope = "openid";
            var actualDefaulScope = authConfigBuilder.Scopes(null).Build();
            Assert.AreEqual(expectedDefaultScope, actualDefaulScope.Scopes);
        }
    }
}