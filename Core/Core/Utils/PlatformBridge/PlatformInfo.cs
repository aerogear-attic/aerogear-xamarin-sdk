using System;
namespace AeroGear.Mobile.Core.Utils
{
    public class PlatformInfo
    {
        public readonly string Name;
        public readonly string Version;

        public PlatformInfo(string name, string version)
        {
            this.Name = name;
            this.Version = version;
        }
    }
}
