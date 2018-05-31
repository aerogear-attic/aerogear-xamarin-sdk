using System;
namespace AeroGear.Mobile.Core.Utils
{
    public class ApplicationRuntimeInfo
    {
        public readonly string Identifier;
        public readonly string Version;
        public readonly string Framework;

        public ApplicationRuntimeInfo(string id, 
                                      string version, 
                                      string framework)
        {
            this.Identifier = id;
            this.Version = version;
            this.Framework = framework;
        }
    }
}
