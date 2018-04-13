﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace Aerogear.Mobile.Auth.User
{
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
        public ICollection<UserRole> getRoles() {
            return new ReadOnlyCollection<UserRole>(Roles);
        }

        /// <summary>
        /// Checks if the user has the specified Resource role.
        /// </summary>
        /// <returns><c>true</c>, if the passed in role is associated with this user, <c>false</c> otherwise.</returns>
        /// <param name="role">role to be checked.</param>
        /// <param name="resourceId">resourceId related to role.</param>
        public bool HasResourceRole(string role, string resourceId) {
            nonEmpty(role, "role");
            return Roles.Contains(new UserRole(role, RoleType.RESOURCE, resourceId));
        }

        /// <summary>
        /// Checks if the user has the specified Realm role..
        /// </summary>
        /// <returns><c>true</c>, if the passed in role is associated with this user, <c>false</c> otherwise.</returns>
        /// <param name="role">role to be checked.</param>
        public bool HasRealmRole(string role) {
            nonEmpty(role, "role");
            return Roles.Contains(new UserRole(role, RoleType.REALM, null));
        }

        /// <summary>
        /// Instantiate a new user object with a fluent api.
        /// </summary>
        /// <returns>The user, ready to be configured with fluent api.</returns>
        public static UserBuilder newUser() {
            return new UserBuilder();
        }
    }

    /// <summary>
    /// Builder for User objects.
    /// </summary>
    public class UserBuilder {
        private string Username;
        private string Firstname;
        private string Lastname;
        private string Email;
        private string IdentityToken;
        private string AccessToken;
        private string RefreshToken;
        List<UserRole> Roles = new List<UserRole>();

        internal UserBuilder() {
        }

        public UserBuilder WithFirstName(string firstName) {
            this.Firstname = firstName;
            return this;
        }

        public UserBuilder WithLastName(string lastName) {
            this.Lastname = lastName;
            return this;
        }

        public UserBuilder WithUsername(string username) {
            this.Username = nonEmpty(username, "username");
            return this;
        }


        public UserBuilder WithEmail(string email) {
            this.Email = email;
            return this;
        }

        public UserBuilder WithRoles(ISet<UserRole> roles) {
            if (roles != null) {
                this.Roles.AddRange(roles);
            }
            return this;
        }

        public UserBuilder WithIdentityToken(string idToken) {
            this.IdentityToken = idToken;
            return this;
        }

        public UserBuilder WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public UserBuilder WithRefreshToken(string refreshToken) {
            this.RefreshToken = refreshToken;
            return this;
        }

        public static implicit operator User(UserBuilder ub) {
            return new User(ub.Username, ub.Firstname, ub.Lastname, ub.Email,
                            ub.IdentityToken, ub.AccessToken, ub.RefreshToken, ub.Roles);
        }
    }
}
