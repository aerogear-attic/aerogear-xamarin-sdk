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
        class DummyModule : IServiceModule
        {
            public string Type => "dummy";

            public String Data1 { get; private set; }
            public int Data2 { get; private set; }

            public bool Data3 { get; private set; }

            public bool RequiresConfiguration => true;

            public DummyModule(MobileCore core, ServiceConfiguration serviceConfiguration)
            {
                var serviceConfig = core.GetServiceConfiguration(Type);
                Data1 = serviceConfig["data1"];
                Data2 = int.Parse(serviceConfig["data2"]);
                Data3 = bool.Parse(serviceConfig["data3"]);
            }

            public void Destroy()
            {

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
            var module = MobileCore.Instance.GetInstance<DummyModule>();
            Assert.IsNotNull(module);
            Assert.AreEqual("dummy", module.Type);
            Assert.AreEqual("Hello world!", module.Data1);
            Assert.AreEqual(42, module.Data2);
            Assert.IsTrue(module.Data3);

        }


    }

   

}
