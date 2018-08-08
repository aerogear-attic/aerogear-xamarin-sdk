using System;
using Android.Content;
using Android.OS;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on an emulator.
    /// </summary>
    public class NotInEmulatorCheck : AbstractDeviceCheck
    {
        protected override string Name => "Emulator Check";

        private readonly Context context;

        public NotInEmulatorCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override DeviceCheckResult Check()
        {
            if (Build.Fingerprint != null)
            {
                if (Build.Fingerprint.Contains("vbox") ||
                    Build.Fingerprint.Contains("generic"))
                    return new DeviceCheckResult(this, false);
            }
            return new DeviceCheckResult(this, true);
        }
    }
}
