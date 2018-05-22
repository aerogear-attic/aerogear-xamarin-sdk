using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Configuration;
using NUnit.Framework;
using static Aerogear.Mobile.Core.MobileCoreTests;

namespace AeroGear.Mobile.Core.Tests
{

    [TestFixture(Category="Core")]
    class DummyModuleInitTest
    {
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

            public static void InitializeService()
            {
                var serviceConfig = MobileCore.Instance.GetFirstServiceConfigurationByType("dummy");
                MobileCore.Instance.RegisterService<IDummyModule>(new DummyModule(serviceConfig));

            }

            public void Configure(MobileCore core, ServiceConfiguration config)
            {
                throw new NotImplementedException();
            }
        }

        [SetUp]
        public void SetUp()
        {
            MobileCore.Init(new TestInjector(Assembly.GetExecutingAssembly()));
        }

        [TearDown]
        public void TearDown()
        {
            MobileCore.Instance.Destroy();
        }

        [Test]
        public void TestModuleInitialized()
        {
            DummyModule.InitializeService();
            var module = MobileCore.Instance.GetService<IDummyModule>();
            Assert.IsNotNull(module);
            Assert.AreEqual("dummy", module.Type);
            Assert.AreEqual("Hello world!", module.Data1);
            Assert.AreEqual(42, module.Data2);
            Assert.IsTrue(module.Data3);

        }

    }

   


}
