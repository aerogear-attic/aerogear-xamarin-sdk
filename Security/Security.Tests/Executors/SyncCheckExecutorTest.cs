using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Executors;
using Moq;
using NUnit.Framework;

namespace AeroGear.Mobile.Security.Tests
{
    [TestFixture(Category = "Executors")]
    public class SyncCheckExecutorTest
    {
        private IDeviceCheck mockSecurityCheck;
        private IDeviceCheckFactory mockCheckFactory;
        private IDeviceCheckType mockSecurityCheckType;

        [SetUp]
        public void SetUp()
        {
            mockSecurityCheckType = new Mock<IDeviceCheckType>().Object;

            var mock = new Mock<IDeviceCheck>();
            mockSecurityCheck = mock.Object;
            mock.Setup(mockSecurityCheck => mockSecurityCheck.GetId()).Returns("test-id");
            mock.Setup(mockSecurityCheck => mockSecurityCheck.GetName()).Returns("test-name");

            DeviceCheckResult result = new DeviceCheckResult(mockSecurityCheck, true);

            mock.Setup(mockSecurityCheck => mockSecurityCheck.Check()).Returns(result);

            var mockSecurityCheckFactory = new Mock<IDeviceCheckFactory>();
            this.mockCheckFactory = mockSecurityCheckFactory.Object;

            mockSecurityCheckFactory.Setup(mockCheckFactory => mockCheckFactory.create(mockSecurityCheckType)).Returns(mockSecurityCheck);

            ServiceFinder.RegisterInstance<IDeviceCheckFactory>(mockCheckFactory);
        }

        [Test]
        public void TestExecuteSyncSingle()
        {
            
            Dictionary<String, DeviceCheckResult> results =
                DeviceCheckExecutor.newSyncExecutor()
                                     .WithDeviceCheck(mockSecurityCheckType)
                                     .Build()
                                     .Execute();

            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.ContainsKey(mockSecurityCheck.GetId()));
            Assert.IsTrue(results[mockSecurityCheck.GetId()].Passed);
        }
    }
}
