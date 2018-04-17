using System;
using System.Reflection;
using System.IO;
using NUnit.Framework;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Exception;
using AeroGear.Mobile.Auth.Config;

namespace AeroGear.Mobile.Auth.Tests.Config
{
    [TestFixture(Category = "Configuration")]
    public class KeycloakConfigTests
    {
        KeycloakConfig keycloakConfig;

        [SetUp]
        public void SetUp()
        {

            var assembly = Assembly.GetExecutingAssembly();
            using (var configStream = assembly.GetManifestResourceStream("AeroGear.Mobile.Auth.Tests.Resources.mobile-services.json"))
            {
                var serviceConfig = MobileCoreJsonParser.Parse(configStream);
                var authConfig = serviceConfig["keycloak"];
                keycloakConfig = new KeycloakConfig(authConfig);
            }


        }

        [Test]
        public void TestDifferentServiceConfig()
        {
            ServiceConfiguration metricConfig = ServiceConfiguration
                .Builder
                .Id("metrics")
                .Type("metrics")
                .Url("https://metrics.url.com")
                .Property("metricData", "metricValue")
                .Build();
            Assert.Throws<ConfigurationNotFoundException>(() => new KeycloakConfig(metricConfig));
        }

        [Test]
        public void TestLogoutUrl()
        {
            var expectedLogoutUrl = "https://keycloak-myproject.192.168.64.74.nip.io/auth/realms/myproject/protocol/openid-connect/logout?id_token_hint=testToken&redirect_uri=com.aerogear.mobile.test://calback";
            var actualLogoutUrl = keycloakConfig.LogoutUrl("testToken", "com.aerogear.mobile.test://calback");
            Assert.AreEqual(expectedLogoutUrl, actualLogoutUrl);
        }

        [Test]
        public void TestHostUrl()
        {
            var expectedHostUrl = "https://keycloak-myproject.192.168.64.74.nip.io/auth";
            var actualHostUrl = keycloakConfig.HostUrl;
            Assert.AreEqual(expectedHostUrl, actualHostUrl);
        }

        [Test]
        public void TestRealmName()
        {
            var expectedRealmName = "myproject";
            var actualRealmName = keycloakConfig.RealmName;
            Assert.AreEqual(expectedRealmName, actualRealmName);
        }

        [Test]
        public void TestResourceId()
        {
            var expectedResourceId = "juYAlRlhTyYYmOyszFa";
            var actualResourceId = keycloakConfig.ResourceId;
            Assert.AreEqual(expectedResourceId, actualResourceId);
        }

        [Test]
        public void TestAuthenticationEndpoint()
        {
            var expectedAuthEndpoint = new Uri("https://keycloak-myproject.192.168.64.74.nip.io/auth/realms/myproject/protocol/openid-connect/auth");
            var actualAuthendpoint = keycloakConfig.AuthenticationEndpoint;
            Assert.AreEqual(expectedAuthEndpoint, actualAuthendpoint);
        }

        [Test]
        public void TestTokenEndpoint()
        {
            var expectedTokenEndpoint = new Uri("https://keycloak-myproject.192.168.64.74.nip.io/auth/realms/myproject/protocol/openid-connect/token");
            var actualTokenEndpoint = keycloakConfig.TokenEndpoint;
            Assert.AreEqual(expectedTokenEndpoint, actualTokenEndpoint);
        }

        [Test]
        public void TestJwksUrl()
        {
            var expectedJwksUrl = "https://keycloak-myproject.192.168.64.74.nip.io/auth/realms/myproject/protocol/openid-connect/certs";
            var actualJwksUrl = keycloakConfig.JwksUrl;
            Assert.AreEqual(expectedJwksUrl, actualJwksUrl);
        }

        [Test]
        public void TestIssuer()
        {
            var expectedIssuer = "https://keycloak-myproject.192.168.64.74.nip.io/auth/realms/myproject";
            var actualIssuer = keycloakConfig.Issuer;
            Assert.AreEqual(expectedIssuer, actualIssuer);
        }
    }
}