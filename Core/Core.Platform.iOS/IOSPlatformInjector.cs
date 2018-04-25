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
        public ILogger CreateLogger() => new IOSLogger();

        public String PlatformName => "iOS";

        public Assembly ExecutingAssembly { get;  set; }

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

        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
