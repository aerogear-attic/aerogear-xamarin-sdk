using System;
using System.IO;
using System.Json;
using System.Reflection;
using System.Threading.Tasks;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
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

        public interface ITestService : IServiceModule { };

        interface IDummyModule : IServiceModule
        {
            string Data1 { get; }
            int Data2 { get; }
            bool Data3 { get; }
        }

        class DummyModule : IDummyModule
        {
            public string Type => "dummy";

            public String Data1 { get; private set; }
            public int Data2 { get; private set; }

            public bool Data3 { get; private set; }
                
            public bool RequiresConfiguration => true;

            public string Id => null;

            public DummyModule(ServiceConfiguration serviceConfig)
            {
                Data1 = serviceConfig["data1"];
                Data2 = int.Parse(serviceConfig["data2"]);
                Data3 = bool.Parse(serviceConfig["data3"]);
            }

            public void Destroy()
            {

            }

            public void Configure(MobileCore core, ServiceConfiguration config)
            {
                throw new NotImplementedException();
            }
        }

        public class TestService : ITestService
        {
            public string Type => throw new NotImplementedException();

            public bool RequiresConfiguration => false;

            public string Id => null;

            public void Configure(MobileCore core, ServiceConfiguration serviceConfiguration)
            {
                
            }

            public void Destroy()
            {
                throw new NotImplementedException();
            }
        }

        public class TestInjector : IPlatformInjector
        {
            public Assembly ExecutingAssembly { get; set; }

            public string PlatformName => "Test";

            public string DefaultResources => "AeroGear.Mobile.Core.Tests.Resources";

            public ILogger CreateLogger() => new NullLogger();

            public Stream GetBundledFileStream(string fileName)
            {
                return ExecutingAssembly.GetManifestResourceStream($"{DefaultResources}.{fileName}");
            }

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
        public void TestRegisterService()
        {
            var testInstance = new TestService();
            MobileCore.Init(new TestInjector(Assembly.GetExecutingAssembly()));
            MobileCore.Instance.RegisterService<ITestService>(testInstance);

            var registeredInstance = MobileCore.Instance.GetService<ITestService>();

            Assert.NotNull(registeredInstance);
            Assert.AreSame(testInstance, registeredInstance);
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

        [Test]
        public void TestFirstServiceConfigByType()
        {
            MobileCore.Init(new TestInjector(Assembly.GetExecutingAssembly()));
            var serviceConfig = MobileCore.Instance.GetFirstServiceConfigurationByType("dummy");
            MobileCore.Instance.RegisterService<IDummyModule>(new DummyModule(serviceConfig));

            var module = MobileCore.Instance.GetService<IDummyModule>();
            Assert.IsNotNull(module);
            Assert.AreEqual("dummy", module.Type);
            Assert.AreEqual("Hello world!", module.Data1);
            Assert.AreEqual(42, module.Data2);
            Assert.IsTrue(module.Data3);

            MobileCore.Instance.Destroy();
        }

        [Test]
        public void TestServiceConfigByType()
        {
            MobileCore.Init(new TestInjector(Assembly.GetExecutingAssembly()));
            var serviceConfigByType = MobileCore.Instance.GetServiceConfigurationByType("dummy");
            MobileCore.Instance.RegisterService<IDummyModule>(new DummyModule(serviceConfigByType[1]));

            var module = MobileCore.Instance.GetService<IDummyModule>();
            Assert.IsNotNull(module);
            Assert.AreEqual("dummy", module.Type);
            Assert.AreEqual("Hello world, from anotherdummy!", module.Data1);
            Assert.AreEqual(420, module.Data2);
            Assert.IsFalse(module.Data3);

            MobileCore.Instance.Destroy();
        }

    }
}
