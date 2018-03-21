using System;
using Core.AeroGear.Mobile.Core.Configuration;
using NUnit.Framework;

namespace AeroGear.Mobile.Core.Tests.AeroGear.Mobile.Core.Configuration
{
    [TestFixture]
    public class ServiceConfigurationTest
    {
        [Test]
        public void TestCreateConfig() {
            var config = ServiceConfiguration.Builder.Name("conf-name").Type("conf-type").Url("http://test-uri.feedhenry.org").Property("prop1", "value1").Property("prop2", "value2").Build();

            Assert.AreEqual("conf-name", config.Name);
            Assert.AreEqual("conf-type", config.Type);
            Assert.AreEqual("http://test-uri.feedhenry.org", config.Url);
            Assert.AreEqual(2, config.Count);
            Assert.AreEqual("value1", config["prop1"]);
            Assert.AreEqual("value2", config["prop2"]);
        }
    }
}
