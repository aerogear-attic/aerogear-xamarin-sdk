using AeroGear.Mobile.Core.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace AeroGear.Mobile.Core.Tests.Http
{
    [TestFixture(Category = "Http")]
    class SystemNetHttpServiceModuleTest
    {
        private FluentMockServer server;
        private string localUrl;

        private const string GET_TEST_PATH = "/getTest";
        private const string GET_TEST_WITH_HEADER_PATH = "/getTestWithHeader";

        private const string HELLO_WORLD = "Hello World!";
        private static string GET_TEST_BODY = "{\"text\":\"" + HELLO_WORLD + "\"}";


        private const string GET_TEST_WITH_HEADER_BODY = "{\"text\":\"" + HELLO_WORLD + "\",\"headerValues\":\"{{request.headers.TestHeader}}\"}";

        private const string TEST_HEADER = "TestHeader";
        private const string TEST_HEADER1_VALUE = "TestHeader1Val";
        private const string TEST_HEADER2_VALUE = "TestHeader2Val";

        [SetUp]
        public void SetUp()
        {
            server = FluentMockServer.Start();
            localUrl = server.Urls[0];

            server.Given(Request.Create().WithPath(GET_TEST_PATH).UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(GET_TEST_BODY));
            server.Given(Request.Create().WithPath(GET_TEST_WITH_HEADER_PATH).UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(GET_TEST_WITH_HEADER_BODY).WithTransformer());
        }

        [TearDown]
        public void TearDown()
        {
            server.Stop();
        }

        private string localPath(string path)
        {
            UriBuilder uriBuilder = new UriBuilder(localUrl);
            uriBuilder.Path = path;
            return uriBuilder.Uri.ToString();
        }

        [Test]
        public void TestType()
        {
            SystemNetHttpServiceModule module = new SystemNetHttpServiceModule();
            Assert.AreEqual("http", module.Type);
        }

        [Test]
        public async Task TestGetRequestSuccessfulLocal()
        {
            SystemNetHttpServiceModule module = new SystemNetHttpServiceModule();

            IHttpRequest request = module.NewRequest();

            IHttpResponse response = await request.Get(localPath(GET_TEST_PATH)).Execute();

            Assert.NotNull(response);
            Assert.Null(response.Error);
            Assert.IsTrue(response.Successful);
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(GET_TEST_BODY, response.Body);
            var jsonObject = JsonObject.Parse(response.Body);
            Assert.AreEqual(HELLO_WORLD, (string)jsonObject["text"]);
        }

        [Test]
        public async Task TestGetRequestWithHeader()
        {
            SystemNetHttpServiceModule module = new SystemNetHttpServiceModule();

            IHttpRequest request = module.NewRequest();

            IHttpResponse response = await request
                .AddHeader(TEST_HEADER, TEST_HEADER1_VALUE)
                .AddHeader(TEST_HEADER, TEST_HEADER2_VALUE).Get(localPath(GET_TEST_WITH_HEADER_PATH)).Execute();

            Assert.NotNull(response);
            Assert.Null(response.Error);
            Assert.IsTrue(response.Successful);
            Assert.AreEqual(200, response.StatusCode);
            var jsonObject = JsonObject.Parse(response.Body);
            Assert.AreEqual(HELLO_WORLD, (string)jsonObject["text"]);
            Assert.AreEqual($"{TEST_HEADER1_VALUE}, {TEST_HEADER2_VALUE}", (string)jsonObject["headerValues"], "header check");            
        }

        [Test]
        public async Task TestGetRequestSuccessfulNetwork()
        {
            SystemNetHttpServiceModule module = new SystemNetHttpServiceModule();

            IHttpRequest request = module.NewRequest();

            IHttpResponse response = await request.Get("http://www.mocky.io/v2/5a5f74172e00006e260a8476").Execute();

            Assert.NotNull(response);
            Assert.Null(response.Error);
            Assert.IsTrue(response.Successful);
            Assert.AreEqual("{\n" + " \"story\": {\n"
                        + "     \"title\": \"Test Title\"\n" + " }    \n" + "}", response.Body);
        }

        [Test]
        public async Task TestGetRequestNotFound()
        {
            SystemNetHttpServiceModule module = new SystemNetHttpServiceModule();

            IHttpRequest request = module.NewRequest();

            IHttpResponse response = await request.Get("http://does.not.exist.com").Execute();
            Assert.IsFalse(response.Successful);
            Assert.NotNull(response.Error);
        }
    }
}