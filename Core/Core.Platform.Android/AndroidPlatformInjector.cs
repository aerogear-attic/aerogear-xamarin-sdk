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

        public ILogger CreateLogger() => new AndroidLogger();

        public String PlatformName => "Android";

        public Assembly ExecutingAssembly { get; set; }

        private readonly Context context;

        public AndroidPlatformInjector(Context ctx)
        {
            this.context = ctx;
        }

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

        public Assembly[] GetAssemblies() {
                return AppDomain.CurrentDomain.GetAssemblies();
        }

    }
}