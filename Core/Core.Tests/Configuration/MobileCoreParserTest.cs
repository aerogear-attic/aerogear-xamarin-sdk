using System;
using System.IO;
using System.Reflection;
using AeroGear.Mobile.Core.Configuration;
using NUnit.Framework;

namespace AeroGear.Mobile.Core.Tests.Configuration
{
    [TestFixture(Category ="Configuration")]
    public class MobileCoreParserTest
    {
        [Test]
        public void TestMobileCoreParsing()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var configStream = assembly.GetManifestResourceStream("AeroGear.Mobile.Core.Tests.Resources.mobile-services.json"))
            using (var reader = new StreamReader(configStream))
            {
                var config = MobileCoreConfiguration.Parse(configStream);
                Assert.NotNull(config);
                Assert.NotNull(config.ServiceConfigurations);

                var keyCloakServiceConfiguration = config.GetFirstServiceConfigurationByType("keycloak");
                Assert.AreEqual("https://keycloak-myproject.192.168.64.74.nip.io/auth", keyCloakServiceConfiguration["auth-server-url"]);
            }
        }
    }
}
