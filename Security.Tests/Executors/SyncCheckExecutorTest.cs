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
        private ISecurityCheck mockSecurityCheck;
        private ISecurityCheckFactory mockCheckFactory;
        private ISecurityCheckType mockSecurityCheckType;

        [SetUp]
        public void SetUp()
        {
            mockSecurityCheckType = new Mock<ISecurityCheckType>().Object;

            var mock = new Mock<ISecurityCheck>();
            mockSecurityCheck = mock.Object;
            mock.Setup(mockSecurityCheck => mockSecurityCheck.GetId()).Returns("test-id");
            mock.Setup(mockSecurityCheck => mockSecurityCheck.GetName()).Returns("test-name");

            SecurityCheckResult result = new SecurityCheckResult(mockSecurityCheck, true);

            mock.Setup(mockSecurityCheck => mockSecurityCheck.Check()).Returns(result);

            var mockSecurityCheckFactory = new Mock<ISecurityCheckFactory>();
            this.mockCheckFactory = mockSecurityCheckFactory.Object;

            mockSecurityCheckFactory.Setup(mockCheckFactory => mockCheckFactory.create(mockSecurityCheckType)).Returns(mockSecurityCheck);

            ServiceFinder.RegisterInstance<ISecurityCheckFactory>(mockCheckFactory);
        }

        [Test]
        public void TestExecuteSyncSingle()
        {
            
            Dictionary<String, SecurityCheckResult> results =
                SecurityCheckExecutor.newSyncExecutor()
                                     .WithSecurityCheck(mockSecurityCheckType)
                                     .Build()
                                     .Execute();

            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.ContainsKey(mockSecurityCheck.GetId()));
            Assert.IsTrue(results[mockSecurityCheck.GetId()].Passed);
        }
    }
}
