using System;
using System.Collections.Generic;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Factory for iOS security checks.
    /// </summary>
    internal class IOSSecurityCheckFactory : ISecurityCheckFactory
    {
        public static readonly IOSSecurityCheckFactory INSTANCE = new IOSSecurityCheckFactory();

        private IOSSecurityCheckFactory()
        {
            
        }

        /// <summary>
        /// Create the security check identified by the passed in security check type.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="type">The type of the security check to be instantiated.</param>
        public ISecurityCheck create(ISecurityCheckType type)
        {
            SecurityChecks checkType = type as SecurityChecks;

            if (checkType == null) {
                throw new Exception("Passed in security check type is not supported");
            }

            return Activator.CreateInstance(checkType.CheckType) as ISecurityCheck;
        }

        /// <summary>
        /// Returns an initialized instance of the check identified by the passed in pseudo enumeration.
        /// An exception is thrown if no check with the given name exists.
        /// </summary>
        /// <returns>The initialized instance of the check.</returns>
        /// <param name="typeName">The name of the check to be instantiated.</param>
        public ISecurityCheck create(string typeName)
        {
            ISecurityCheckType securityCheckType = SecurityChecks.GetSecurityCheck(typeName);
            if (securityCheckType == null)
            {
                throw new Exception(String.Format("No security check with name {0} is known", typeName));
            }

            return create(securityCheckType);
        }
    }
}
