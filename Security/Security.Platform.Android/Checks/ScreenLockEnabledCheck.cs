using System;
using Android.Content;
using Android.App;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on has a screen lock.
    /// </summary>
    public class ScreenLockEnabledCheck : AbstractDeviceCheck
    {
        protected override string Name => "Screen Lock Check";

        private readonly Context context;

        public ScreenLockEnabledCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override DeviceCheckResult Check()
        {
            KeyguardManager manager = (KeyguardManager)context.GetSystemService(Context.KeyguardService);
            return new DeviceCheckResult(this, manager.IsDeviceSecure);
        }
    }
}
