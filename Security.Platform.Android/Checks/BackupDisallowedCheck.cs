using System;
using Android.Content;
using Android.Content.PM;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check to determine whether the allowBackup flag is enabled for the application.
    /// </summary>
    public class BackupDisallowedCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Backup Flag Check"; } }

        private readonly Context context;

        public BackupDisallowedCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            PackageInfo packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            bool disabled = (packageInfo.ApplicationInfo.Flags & ApplicationInfoFlags.AllowBackup) == 0;
            return new SecurityCheckResult(this, disabled);
        }
    }
}
