using System;
using System.Reflection;
using AeroGear.Mobile.Core.Logging;

namespace AeroGear.Mobile.Core
{
    /// <summary>
    /// Interface for factory methods creating objects with platform-specific implementations.
    /// </summary>
    public interface IPlatformInjector
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>The logger.</returns>
        ILogger CreateLogger();

        /// <summary>
        /// Returns executing assembly. For accessing resources, etc.
        /// </summary>
        Assembly ExecutingAssembly { get; }

        /// <summary>
        /// Returns platform name of the current injector.
        /// </summary>
        /// <value>The name of the platform. (e.g. "Android","iOS") </value>
        String PlatformName {
            get;
        }

        /// <summary>
        /// Returns prefix for default resources. By default it would be "&lt;DefaultNamespace&gt;.Resources".
        /// </summary>
        String DefaultResources { get; }
    }
}
