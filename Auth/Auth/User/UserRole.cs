using System;
using static AeroGear.Mobile.Core.Utils.SanityCheck;

namespace Aerogear.Mobile.Auth.User
{
    public enum RoleType { REALM, RESOURCE };

    /// <summary>
    /// Represents a user's keycloak roles information.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Role name. Can't be null.
        /// </summary>
        private string Name { get; }

        /// <summary>
        /// Role name space/client ID. Can be null.
        /// </summary>
        private string RoleNamespace;

        /// <summary>
        /// Role type. Can't be null.
        /// </summary>
        /// <value>The type of the role.</value>
        private RoleType RoleType { get; }

        private UserRole() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aerogear.Mobile.Auth.UserRole"/> class.
        /// </summary>
        /// <param name="name">Role Name.</param>
        /// <param name="type">Role Type.</param>
        /// <param name="roleNamespace">Role name space/client ID.</param>
        public UserRole(string name, RoleType type, string roleNamespace) {
            Name = NonEmpty(name, "name");
            RoleType = NonNull(type, "type");
            RoleNamespace = roleNamespace;
        }

        /// <summary>
        /// Get's the namespace/client ID of the role.
        /// </summary>
        /// <returns>The namespace/client ID.</returns>
        public string getNamespace() {
            return RoleNamespace;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Auth.Aerogear.Mobile.Auth.UserRole"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Auth.Aerogear.Mobile.Auth.UserRole"/>.</returns>
        public override string ToString(){
            if (RoleNamespace != null) {
                return String.Format("{0}:{1}", RoleNamespace, Name);
            } else {
                return Name;
            }
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Auth.Aerogear.Mobile.Auth.UserRole"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:Auth.Aerogear.Mobile.Auth.UserRole"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:Auth.Aerogear.Mobile.Auth.UserRole"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:Auth.Aerogear.Mobile.Auth.UserRole"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            var other = obj as UserRole;

            if (this.RoleType == RoleType.RESOURCE) {
                return this.Name.Equals(other.Name)
                           && this.RoleType == other.RoleType
                           && this.RoleNamespace.Equals(other.RoleNamespace);
            }
            else {
                return this.Name.Equals(other.Name)
                           && this.RoleType == other.RoleType;
            }
        }
    }
}
