using System;
using System.IO;
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
        Assembly ExecutingAssembly { get; set; }
        
        /// <summary>
        /// Returns stream of file budled withhin your application.
        /// </summary>
        /// <returns>readable stream</returns>
        Stream GetBundledFileStream(string fileName);

        /// <summary>
        /// Returns platform name of the current injector.
        /// </summary>
        /// <value>The name of the platform. (e.g. "Android","iOS") </value>
        String PlatformName {
            get;
        }

        Assembly[] GetAssemblies();
      
    }
}
