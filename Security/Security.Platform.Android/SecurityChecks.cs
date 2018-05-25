using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Checks;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// This class enums all the provided security checks.
    /// 
    /// To get an instance of the check, use the following code:
    /// <code>
    /// <![CDATA[
    /// var securityChek = ServiceFinder.Resolve<ISecurityCheckFactory>().create(SecurityChecks.NOT_ROOTED);
    /// ]]>
    /// </code>
    /// </summary>
    public class SecurityChecks : ISecurityCheckType
    {
        private static Dictionary<string, SecurityChecks> typesByName = new Dictionary<string, SecurityChecks>();

        public static readonly SecurityChecks NOT_ROOTED = new SecurityChecks(typeof(NonRootedCheck));
        public static readonly SecurityChecks DEVELOPER_MODE_DISABLED = new SecurityChecks(typeof(DeveloperModeDisabledCheck));
        public static readonly SecurityChecks NOT_IN_EMULATOR = new SecurityChecks(typeof(NotInEmulatorCheck));
        public static readonly SecurityChecks SCREEN_LOCK = new SecurityChecks(typeof(ScreenLockCheck));
        public static readonly SecurityChecks BACKUP_DISALLOWED = new SecurityChecks(typeof(BackupDisallowedCheck));
        public static readonly SecurityChecks ENCRYPTION = new SecurityChecks(typeof(EncryptionCheck));
        public static readonly SecurityChecks NO_DEBUGGER = new SecurityChecks(typeof(NoDebuggerCheck));

        // add others checks here
        // i.e. 
        // public static readonly SecurityChecks NO_DEBUGGER = new SecurityChecks(typeof(NoDebuggerCheck));
        // this way the user will be able to do an enum like selection:
        // SecurityChecks.NOT_JAILBROKEN

        internal readonly Type CheckType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Security.SecurityChecks"/> class.
        /// Private so that it can't be instantiated externally: useful to emulate an enum.
        /// </summary>
        /// <param name="checkType">The class type of the check represented by this instance.</param>
        private SecurityChecks(Type checkType, string friendlyName = null)
        {
            if (!ServiceFinder.IsRegistered<ISecurityCheckFactory>())
            {
                ServiceFinder.RegisterInstance<ISecurityCheckFactory>(AndroidSecurityCheckFactory.INSTANCE);
            }

            this.CheckType = checkType;
            typesByName[friendlyName ?? checkType.Name] = this;
        }

        /// <summary>
        /// Returns an the SecurityChecks instance identified by the passed in name.
        /// </summary>
        /// <returns>The SecurityChecks instance identified by the passed in name or <code>null</code> if not found.</returns>
        /// <param name="name">Name.</param>
        public static SecurityChecks GetSecurityCheck(string name)
        {
            if (typesByName.ContainsKey(name))
            {
                return typesByName[name];
            }
            return null;
        }

        /// <summary>
        /// Returns all the checks.
        /// </summary>
        /// <returns>All the checks.</returns>
        public static ICollection<SecurityChecks> GetAllChecks()
        {
            return typesByName.Values;
        }
    }
}
