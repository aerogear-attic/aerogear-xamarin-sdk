using System;
using Android.Content;
using Android.OS;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on an emulator.
    /// </summary>
    public class NotInEmulatorCheck : AbstractSecurityCheck
    {
        protected override string Name => "Emulator Check";

        private readonly Context context;

        public NotInEmulatorCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            if (Build.Fingerprint != null)
            {
                if (Build.Fingerprint.Contains("vbox") ||
                    Build.Fingerprint.Contains("generic"))
                    return new SecurityCheckResult(this, false);
            }
            return new SecurityCheckResult(this, true);
        }
    }
}
