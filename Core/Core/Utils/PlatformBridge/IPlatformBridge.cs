using System;
namespace AeroGear.Mobile.Core.Utils
{
    /// <summary>
    /// The platform bridge is an object that allows access to platform dependent informations/feature
    /// from the shared, platform agnostic, code.
    /// To get an instance of the platform bridge, the <see cref="ServiceFinder"/> class must be used:
    /// <code>
    /// <![CDATA[
    /// IPlatformBridge bridge = ServiceFinder.Resolve<IPlatformBridge>();
    /// ]]>
    /// </code>
    /// </summary>
    public interface IPlatformBridge
    {
        /// <summary>
        /// Gets the application runtime info.
        /// </summary>
        /// <value>The application runtime info.</value>
        ApplicationRuntimeInfo ApplicationRuntimeInfo { get; }
        /// <summary>
        /// Gets the platform info.
        /// </summary>
        /// <value>The platform info.</value>
        PlatformInfo PlatformInfo { get; }
        /// <summary>
        /// Gets the user preferences store.
        /// </summary>
        /// <returns>The user preferences store.</returns>
        /// <param name="storageName">Storage name. Can be null.</param>
        IUserPreferences GetUserPreferences(string storageName = null);
    }
}
