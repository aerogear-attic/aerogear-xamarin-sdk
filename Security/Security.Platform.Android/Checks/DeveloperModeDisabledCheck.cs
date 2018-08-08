using System;
using Android.Content;
using Android.Provider;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Security check that detects if developer mode is enabled in the device.
    /// </summary>
    public class DeveloperModeDisabledCheck : AbstractDeviceCheck
    {
        protected override string Name => "Developer Mode Check";

        private readonly Context context;

        public DeveloperModeDisabledCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override DeviceCheckResult Check()
        {
            bool devModeEnabled = Settings.Global.GetInt(
                context.ContentResolver,
                Settings.Global.DevelopmentSettingsEnabled, 0) != 0;
            return new DeviceCheckResult(this, !devModeEnabled);
        }
    }
}
