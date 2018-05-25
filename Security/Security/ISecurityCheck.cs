using System;
namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Interface for a single check to be executed.
    /// </summary>
    public interface ISecurityCheck
    {
        /// <summary>
        /// Perform the check and return a result.
        /// </summary>
        /// <returns>The result of the test</returns>
        SecurityCheckResult Check();

        /// <summary>
        /// Gets the name of the check to be used for display purposes in reports. This value should
        /// match other checks that implement the same verification across different platforms (i.e. iOS,
        /// cordova)
        /// </summary>
        /// <returns>The name of the check</returns>
        string GetName();

        /// <summary>
        /// Gets the type of the check. It must be a unique string. The default implementation is to
        /// return the check class name.
        /// </summary>
        /// <returns>The identifier of teh check.</returns>
        string GetId();
    }
}
