using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using AeroGear.Mobile.Auth.Credentials;
using JWT.Builder;
using Newtonsoft.Json;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace Aerogear.Mobile.Auth.User
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class KeycloakProfile
    {

        internal class AccessRoles
        {
            [JsonProperty(PropertyName = "roles")]
            internal string[] Roles { get; set; }
        }

        [JsonProperty(PropertyName = "name")]
        internal string Name { get; set; }

        [JsonProperty(PropertyName = "preferred_name")]
        internal string PreferredName { get; set; }

        [JsonProperty(PropertyName = "email")]
        internal string Email { get; set; }

        [JsonProperty(PropertyName = "given_name")]
        internal string FirstName { get; set; }

        [JsonProperty(PropertyName = "family_name")]
        internal string LastName { get; set; }

        [JsonProperty(PropertyName = "realm_access")]
        internal AccessRoles RealmAccess { get; set; }

        [JsonProperty(PropertyName = "resource_access")]
        internal IDictionary<string, AccessRoles> ResourceAccess { get; set; }

        internal string UserName
        {
            get
            {
                return PreferredName == null ? (Name == null ? null : Name) : PreferredName;
            }
        }

        internal ISet<UserRole> GetUserRoles(string resource)
        {
            var userRoles = new HashSet<UserRole>();
            if (RealmAccess != null && RealmAccess.Roles != null && RealmAccess.Roles.Length > 0)
            {
                var realmRoles = RealmAccess.Roles;
                foreach (var realmRole in realmRoles)
                {
                    userRoles.Add(new UserRole(realmRole, RoleType.REALM, null));
                }
            }

            if (ResourceAccess != null && ResourceAccess.ContainsKey(resource) && ResourceAccess[resource].Roles != null && ResourceAccess[resource].Roles.Length > 0)
            {
                var resourceRoles = ResourceAccess[resource].Roles;
                foreach (var resourceRole in resourceRoles)
                {
                    userRoles.Add(new UserRole(resourceRole, RoleType.RESOURCE, resource));
                }
            }

            return userRoles;
        }
    }

    /// <summary>
    /// This class represent an authenticated user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username of the principal.</value>
        public string Username { get; }

        /// <summary>
        /// Gets the firstname.
        /// </summary>
        /// <value>The first name of the principal.</value>
        public string Firstname { get; }

        /// <summary>
        /// Gets the lastname.
        /// </summary>
        /// <value>The last name of the principal.</value>
        public string Lastname { get; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>The email associated with this user.</value>
        public string Email { get; }

        /// <summary>
        /// Gets the identity token.
        /// </summary>
        /// <value>Identity token. It is used for logout.</value>
        public string IdentityToken { get; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>Access token for http request authorisation.</value>
        public string AccessToken { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <value>The refresh token.</value>
        public string RefreshToken { get; }

        /// <summary>
        /// Roles associated with this principal.
        /// </summary>
        public List<UserRole> Roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aerogear.Mobile.Auth.User.User"/> class.
        /// </summary>
        /// <param name="username">the username of the authenticated user.</param>
        /// <param name="firstName">the first name of the authenticated user.</param>
        /// <param name="lastName">the last name of the authenticated user.</param>
        /// <param name="email">the email of the authenticated user.</param>
        /// <param name="identityToken">Identity token.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="refreshToken">Refresh token.</param>
        /// <param name="roles">roles assigned to the user.</param>
        internal User(string username,
                     string firstName,
                     string lastName,
                     string email,
                     string identityToken,
                     string accessToken,
                     string refreshToken,
                     IList<UserRole> roles)
        {
            this.Username = nonEmpty(username, "username");
            this.Firstname = firstName;
            this.Lastname = lastName;
            this.Email = email;
            this.IdentityToken = identityToken;
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            Roles = new List<UserRole>();
            this.Roles.AddRange(roles);
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <returns>All the roles associated with this user.</returns>
        public ICollection<UserRole> getRoles()
        {
            return new ReadOnlyCollection<UserRole>(Roles);
        }

        /// <summary>
        /// Checks if the user has the specified Resource role.
        /// </summary>
        /// <returns><c>true</c>, if the passed in role is associated with this user, <c>false</c> otherwise.</returns>
        /// <param name="role">role to be checked.</param>
        /// <param name="resourceId">resourceId related to role.</param>
        public bool HasResourceRole(string role, string resourceId)
        {
            nonEmpty(role, "role");
            return Roles.Contains(new UserRole(role, RoleType.RESOURCE, resourceId));
        }

        /// <summary>
        /// Checks if the user has the specified Realm role..
        /// </summary>
        /// <returns><c>true</c>, if the passed in role is associated with this user, <c>false</c> otherwise.</returns>
        /// <param name="role">role to be checked.</param>
        public bool HasRealmRole(string role)
        {
            nonEmpty(role, "role");
            return Roles.Contains(new UserRole(role, RoleType.REALM, null));
        }

        /// <summary>
        /// Instantiate a new user object with a fluent api.
        /// </summary>
        /// <returns>The user, ready to be configured with fluent api.</returns>
        public static UserBuilder NewUser()
        {
            return new UserBuilder();
        }

        /// <summary>
        /// Get a user instance from a unverified credential.
        /// </summary>
        /// <returns>The user instance from the unverified credential.</returns>
        /// <param name="credential">Credential.</param>
        /// <param name="resource">Resource.</param>
        public static User NewUserFromUnverifiedCredential(ICredential credential, string resource)
        {
            nonNull(credential, "credential");
            var accessToken = nonNull(credential.AccessToken, "credential.AccessToken");
            KeycloakProfile kcProfile = new JwtBuilder()
                .DoNotVerifySignature()
                .Decode<KeycloakProfile>(accessToken);
            return NewUser()
                .WithAccessToken(credential.AccessToken)
                .WithRefreshToken(credential.RefreshToken)
                .WithIdentityToken(credential.IdentityToken)
                .WithEmail(kcProfile.Email)
                .WithUsername(kcProfile.UserName)
                .WithFirstName(kcProfile.FirstName)
                .WithLastName(kcProfile.LastName)
                .WithRoles(kcProfile.GetUserRoles(resource));

        }
    }

    /// <summary>
    /// Builder for User objects.
    /// </summary>
    public class UserBuilder
    {
        private string Username;
        private string Firstname;
        private string Lastname;
        private string Email;
        private string IdentityToken;
        private string AccessToken;
        private string RefreshToken;
        List<UserRole> Roles = new List<UserRole>();

        internal UserBuilder()
        {
        }

        public UserBuilder WithFirstName(string firstName)
        {
            this.Firstname = firstName;
            return this;
        }

        public UserBuilder WithLastName(string lastName)
        {
            this.Lastname = lastName;
            return this;
        }

        public UserBuilder WithUsername(string username)
        {
            this.Username = nonEmpty(username, "username");
            return this;
        }


        public UserBuilder WithEmail(string email)
        {
            this.Email = email;
            return this;
        }

        public UserBuilder WithRoles(ISet<UserRole> roles)
        {
            if (roles != null)
            {
                this.Roles.AddRange(roles);
            }
            return this;
        }

        public UserBuilder WithIdentityToken(string idToken)
        {
            this.IdentityToken = idToken;
            return this;
        }

        public UserBuilder WithAccessToken(string accessToken)
        {
            this.AccessToken = accessToken;
            return this;
        }

        public UserBuilder WithRefreshToken(string refreshToken)
        {
            this.RefreshToken = refreshToken;
            return this;
        }

        public static implicit operator User(UserBuilder ub)
        {
            return new User(ub.Username, ub.Firstname, ub.Lastname, ub.Email,
                            ub.IdentityToken, ub.AccessToken, ub.RefreshToken, ub.Roles);
        }
    }
}
