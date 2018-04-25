using AeroGear.Mobile.Core.Logging;
using Android.Content;
using System;
using System.IO;
using System.Reflection;

namespace AeroGear.Mobile.Core.Platform.Android
{
    ///<summary>
    ///Class handles injection of Android specific classes and access to resoureces.
    ///</summary>    internal class AndroidPlatformInjector : IPlatformInjector
    internal class AndroidPlatformInjector : IPlatformInjector
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>The logger.</returns>
        public ILogger CreateLogger() => new AndroidLogger();

        /// <summary>
        /// Gets the name of the platform.
        /// </summary>
        /// <value>The name of the platform.</value>
        public String PlatformName => "Android";

        /// <summary>
        /// Gets or sets the executing assembly.
        /// </summary>
        /// <value>The executing assembly.</value>
        public Assembly ExecutingAssembly { get; set; }

        private readonly Context context;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AeroGear.Mobile.Core.Platform.Android.AndroidPlatformInjector"/> class.
        /// </summary>
        /// <param name="ctx">Context.</param>
        public AndroidPlatformInjector(Context ctx)
        {
            this.context = ctx;
        }

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
                return context.Assets.Open(fileName);
            }
        }
    }
}