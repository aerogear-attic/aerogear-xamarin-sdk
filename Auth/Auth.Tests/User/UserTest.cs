using System;
using System.Collections.Generic;
using AeroGear.Mobile.Auth.Credentials;
using Moq;
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

        private const string TEST_ACCESS_TOKEN = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhZFNveVhOQWdReFY0M2VxSFNpUlpmNmhOOXl0dkJOUXliMmZGU2RDVFZNIn0.eyJqdGkiOiJlMzkzOGU2Zi0zOGQzLTQ2MmYtYTg1OS04YjNiODA0N2NlNzkiLCJleHAiOjE5NDg2MzI2NDgsIm5iZiI6MCwiaWF0IjoxNTE2NjMyNjQ4LCJpc3MiOiJodHRwczovL2tleWNsb2FrLnNlY3VyaXR5LmZlZWRoZW5yeS5vcmcvYXV0aC9yZWFsbXMvc2VjdXJlLWFwcCIsImF1ZCI6ImNsaWVudC1hcHAiLCJzdWIiOiJiMTYxN2UzOC0zODczLTRhNDctOGE2Yy01YjgyMmFkYTI3NWUiLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJjbGllbnQtYXBwIiwiYXV0aF90aW1lIjoxNTE2NjMyNjQ3LCJzZXNzaW9uX3N0YXRlIjoiYzI1NWYwYWMtODA5MS00YzkyLThmM2EtNDhmZmI4ODFhNzBiIiwiYWNyIjoiMSIsImFsbG93ZWQtb3JpZ2lucyI6WyIqIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJtb2JpbGUtdXNlciJdfSwicmVzb3VyY2VfYWNjZXNzIjp7ImNsaWVudC1hcHAiOnsicm9sZXMiOlsiaW9zLWFjY2VzcyJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwibmFtZSI6IlVzZXIgMSIsInByZWZlcnJlZF91c2VybmFtZSI6InVzZXIxIiwiZ2l2ZW5fbmFtZSI6IlVzZXIiLCJmYW1pbHlfbmFtZSI6IjEiLCJlbWFpbCI6InVzZXIxQGZlZWRoZW5yeS5vcmcifQ.RvsLrOrLB3EFkZvYZM8-QXf6rRllCap-embNwa2V-NTMpcR7EKNMkKUQI9MbBlVSkTEBckZAK0DGSdo5CYuFoFH-xVWkzU0yQKBuFYAK1Etd50yQWwS1vHiThT95ZgeGGCB3ptafY5UCoqyg41kKqO5rb8iGyZ3ACp2xoEOE5S1rPAPszcQrbKBryOOk7H6MDZgqhZxxGUJkDVAT2v3jAd1aJ4K17qH6raabtDrAy_541vn6c0LS1ay0ooW4IVFzjFSH1-jMJvCYM6oku7brPonl2qHO8jMLrrhxylw2VXIAlregih6aNJ5c87729UtEJNTEFyqGI6GCunt2DQt7cw";

        public UserTest()
        {
        }

        [Test]
        public void testCreateUser()
        {
            User user = User.NewUser()
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

        [Test]
        public void testCreateUserWithCredentials()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(arg => arg.AccessToken).Returns(TEST_ACCESS_TOKEN);

            User user = User.NewUser().FromUnverifiedCredential(credentialMock.Object, "client-app");
            Assert.AreEqual(user.Email, "user1@feedhenry.org");
            Assert.AreEqual(user.Username, "User 1");
            Assert.IsTrue(user.HasRealmRole("mobile-user"));
            Assert.IsTrue(user.HasResourceRole("ios-access", "client-app"));
        }
    }
}
