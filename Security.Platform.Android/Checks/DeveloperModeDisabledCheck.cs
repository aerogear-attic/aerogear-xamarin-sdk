using System;
using Android.Content;
using Android.Provider;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Security check that detects if developer mode is enabled in the device.
    /// </summary>
    public class DeveloperModeDisabledCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Developer Mode Check"; } }

        private readonly Context context;

        public DeveloperModeDisabledCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            bool devModeEnabled = Settings.Secure.GetInt(
                context.ContentResolver,
                Settings.Global.DevelopmentSettingsEnabled) != 0;
            return new SecurityCheckResult(this, !devModeEnabled);
        }
    }
}
