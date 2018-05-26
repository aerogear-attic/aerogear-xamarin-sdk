using System;
namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Placeholder interface that must be implemented by all the pseudo checks enumerations.
    /// </summary>
    public interface ISecurityCheckType
    {
        
    }

    /// <summary>
    /// Interface that must be implemented by the platform specific security check factory.
    /// </summary>
    public interface ISecurityCheckFactory
    {
        /// <summary>
        /// Create the security check identified by the passed in security check type.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="type">The type of the security check to be instantiated.</param>
        ISecurityCheck create(ISecurityCheckType type);

        /// <summary>
        /// Create the security check identified by the passed in security check type name.
        /// This is meant to be used in shared code where the type may not be linked.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="typeName">The name of the security check to be instantiated.</param>
        ISecurityCheck create(string typeName);
    }
}
