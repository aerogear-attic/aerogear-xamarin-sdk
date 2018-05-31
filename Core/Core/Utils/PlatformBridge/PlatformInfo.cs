using System;
namespace AeroGear.Mobile.Core.Utils
{
    /// <summary>
    /// This class allows the shared code to have info of the real platform the app is running in.
    /// </summary>
    public class PlatformInfo
    {
        /// <summary>
        /// Operating system name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Operating system version.
        /// </summary>
        public readonly string Version;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Core.Utils.PlatformInfo"/> class.
        /// </summary>
        /// <param name="name">Operating system name.</param>
        /// <param name="version">Operating system version.</param>
        public PlatformInfo(string name, string version)
        {
            this.Name = name;
            this.Version = version;
        }
    }
}
