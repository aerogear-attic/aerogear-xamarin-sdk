using System;
namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Placeholder interface that must be implemented by all the pseudo checks enumerations.
    /// </summary>
    public interface IDeviceCheckType
    {
        
    }

    /// <summary>
    /// Interface that must be implemented by the platform specific security check factory.
    /// </summary>
    public interface IDeviceCheckFactory
    {
        /// <summary>
        /// Create the security check identified by the passed in security check type.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="type">The type of the security check to be instantiated.</param>
        IDeviceCheck create(IDeviceCheckType type);

        /// <summary>
        /// Create the security check identified by the passed in security check type name.
        /// This is meant to be used in shared code where the type may not be linked.
        /// </summary>
        /// <returns>The security check.</returns>
        /// <param name="typeName">The name of the security check to be instantiated.</param>
        IDeviceCheck create(string typeName);
    }
}
