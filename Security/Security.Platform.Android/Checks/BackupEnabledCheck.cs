using System;
using Android.Content;
using Android.Content.PM;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check to determine whether the allowBackup flag is enabled for the application.
    /// </summary>
    public class BackupEnabledCheck : AbstractDeviceCheck
    {
        protected override string Name => "Backup Flag Check";

        private readonly Context context;

        public BackupEnabledCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override DeviceCheckResult Check()
        {
            PackageInfo packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            bool enabled = (packageInfo.ApplicationInfo.Flags & ApplicationInfoFlags.AllowBackup) != 0;
            return new DeviceCheckResult(this, enabled);
        }
    }
}
