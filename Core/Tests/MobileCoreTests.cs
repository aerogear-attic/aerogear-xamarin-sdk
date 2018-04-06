using System;
using System.Json;
using System.Reflection;
using System.Threading.Tasks;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Exception;
using AeroGear.Mobile.Core.Logging;
using NUnit.Framework;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Aerogear.Mobile.Core
{
    [TestFixture(Category = "Core")]
    public class MobileCoreTests
    {
        private FluentMockServer server;
        private string localUrl;
        private const string HELLO_WORLD = "Hello World!";
        private const string GET_TEST_BODY = "{\"text\":\"" + HELLO_WORLD + "\"}";
        private const String GET_TEST_PATH = "/test";

        private class TestInjector : IPlatformInjector
        {
            public Assembly ExecutingAssembly { get; set; }

            public string PlatformName => "Test";

            public string DefaultResources => "AeroGear.Mobile.Core.Tests.Resources";

            public ILogger CreateLogger() => new NullLogger();

            public TestInjector(Assembly assembly)
            {
                ExecutingAssembly = assembly;
            }
        }

        [SetUp]
        public void SetUp()
        {
            server = FluentMockServer.Start();
            localUrl = server.Urls[0];
            server.Given(Request.Create().WithPath(GET_TEST_PATH).UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(GET_TEST_BODY));
        }

        [TearDown]
        public void TearDown()
        {
            server.Stop();            
        }


        [Test]
        public void TestCoreInitAndDestroy()
        {
            Assert.Catch<InitializationException>(() => MobileCore.Instance.GetType(), "check uninitialized exception before init");
            MobileCore.Init(new TestInjector(Assembly.GetExecutingAssembly()));
            Assert.NotNull(MobileCore.Instance, "check core initialized successfuly");
            MobileCore.Instance.Destroy();
            Assert.Catch<InitializationException>(() => MobileCore.Instance.GetType(), "check uninitialized exception after destroy");
        }

        [Test]
        public async Task TestCallHttpLayerAsync()
        {
            MobileCore.Init(new TestInjector(Assembly.GetExecutingAssembly()));
            var httpLayer = MobileCore.Instance.HttpLayer;
            Assert.IsNotNull(httpLayer, "Mobile Core must return the HTTP layer");
            var request = httpLayer.NewRequest();
            Assert.IsNotNull(request, "NewRequest() must create a request");
            UriBuilder uriBuilder = new UriBuilder(localUrl);
            uriBuilder.Path = GET_TEST_PATH;

            var response = await request.Get(uriBuilder.Uri.ToString()).Execute();

            Assert.NotNull(response);
            Assert.Null(response.Error);
            Assert.IsTrue(response.Successful);
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(GET_TEST_BODY, response.Body);
            var jsonObject = JsonObject.Parse(response.Body);
            Assert.AreEqual(HELLO_WORLD, (string)jsonObject["text"]);
            MobileCore.Instance.Destroy();
        }
    }
}
