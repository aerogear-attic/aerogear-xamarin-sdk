using System;
using NUnit.Framework;

namespace Aerogear.Mobile.Auth.User.Test
{
    [TestFixture]
    public class UserRoleTest
    {
        public UserRoleTest()
        {
        }

        [Test]
        public void TestEqualWithNamespaceSuccess() {
            UserRole userRole1 = new UserRole("rolename", RoleType.RESOURCE, "namespace");
            UserRole userRole2 = new UserRole("rolename", RoleType.RESOURCE, "namespace");

            Assert.AreEqual(userRole1, userRole2);
        }

        [Test]
        public void TestEqualWithNamespaceFail()
        {
            UserRole userRole1 = new UserRole("rolename", RoleType.RESOURCE, "namespace");
            UserRole userRole2 = new UserRole("rolename", RoleType.RESOURCE, "namespace1");

            Assert.AreNotEqual(userRole1, userRole2);
        }

        [Test]
        public void TestEqualWithoutNamespaceSuccess()
        {
            UserRole userRole1 = new UserRole("rolename", RoleType.REALM, null);
            UserRole userRole2 = new UserRole("rolename", RoleType.REALM, null);

            Assert.AreEqual(userRole1, userRole2);
        }

        [Test]
        public void TestEqualWithoutNamespaceFail()
        {
            UserRole userRole1 = new UserRole("rolename", RoleType.REALM, null);
            UserRole userRole2 = new UserRole("rolename1", RoleType.REALM, null);

            Assert.AreNotEqual(userRole1, userRole2);
        }

        [Test]
        public void TestEqualDifferentTypeFail()
        {
            UserRole userRole1 = new UserRole("rolename", RoleType.RESOURCE, "namespace");
            UserRole userRole2 = new UserRole("rolename1", RoleType.REALM, null);

            Assert.AreNotEqual(userRole1, userRole2);
        }
    }
}
