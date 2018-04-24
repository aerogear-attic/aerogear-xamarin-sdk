using System;
using System.IO;
using System.Reflection;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;

namespace AeroGear.Mobile.Core
{
    /// <summary>
    /// Class handles injection of iOS specific classes and access to resoureces.
    /// </summary>
    internal class IOSPlatformInjector : IPlatformInjector
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>The logger.</returns>
        public ILogger CreateLogger() => new IOSLogger();

        /// <summary>
        /// Gets the name of the platform.
        /// </summary>
        /// <value>The name of the platform.</value>
        public String PlatformName => "iOS";

        /// <summary>
        /// Gets or sets the executing assembly.
        /// </summary>
        /// <value>The executing assembly.</value>
        public Assembly ExecutingAssembly { get;  set; }

        /// <summary>
        /// Gets the bundled file stream.
        /// </summary>
        /// <returns>The bundled file stream.</returns>
        /// <param name="fileName">File name.</param>
        public Stream GetBundledFileStream(string fileName)
        {
            if (ExecutingAssembly != null)
            {
                var extendedName = $"{ExecutingAssembly.GetName().Name}.Resources.{fileName}";
                return ExecutingAssembly.GetManifestResourceStream(extendedName);
            }
            else
            {
                return System.IO.File.OpenRead(fileName);
            }
        }
    }
}
