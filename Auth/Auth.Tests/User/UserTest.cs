using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Aerogear.Mobile.Auth.User.Test
{
    [TestFixture]
    public class UserTest
    {
        private const string FIRST_NAME = "Donald";
        private const string LAST_NAME = "Duck";
        private const string EMAIL = "donald.duck@nowhere.ie";
        private const string USERNAME = "donaldduck";
        private const string ACCESS_TOKEN = "ACESS_TOKEN";
        private const string REFRESH_TOKEN = "REFRESH_TOKEN";
        private const string IDENTITY_TOKEN = "IDENTITY_TOKEN";
        private const string ROLE1 = "Role1";
        private const string ROLE2 = "Role2";
        private const string NAMESPACE = "namespace";

        public UserTest()
        {
        }

        [Test]
        public void testCreateUser() {
            User user = User.newUser()
                            .WithFirstName(FIRST_NAME)
                            .WithLastName(LAST_NAME)
                            .WithEmail(EMAIL)
                            .WithUsername(USERNAME)
                            .WithAccessToken(ACCESS_TOKEN)
                            .WithRefreshToken(REFRESH_TOKEN)
                            .WithIdentityToken(IDENTITY_TOKEN)
                            .WithRoles(new HashSet<UserRole>(new UserRole[] { new UserRole(ROLE1, RoleType.REALM, null), new UserRole(ROLE2, RoleType.RESOURCE, NAMESPACE) }));

            Assert.AreEqual(user.Firstname, FIRST_NAME);
            Assert.AreEqual(user.Lastname, LAST_NAME);
            Assert.AreEqual(user.Email, EMAIL);
            Assert.AreEqual(user.Username, USERNAME);
            Assert.AreEqual(user.AccessToken, ACCESS_TOKEN);
            Assert.AreEqual(user.RefreshToken, REFRESH_TOKEN);
            Assert.AreEqual(user.IdentityToken, IDENTITY_TOKEN);

            Assert.AreEqual(2, user.getRoles().Count);
            Assert.IsTrue(user.HasRealmRole(ROLE1));
            Assert.IsTrue(user.HasResourceRole(ROLE2, NAMESPACE));
        }
    }
}
