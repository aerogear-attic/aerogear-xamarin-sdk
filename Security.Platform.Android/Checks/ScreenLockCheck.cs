using System;
using Android.Content;
using Android.App;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on has a screen lock.
    /// </summary>
    public class ScreenLockCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Screen Lock Check"; } }

        private readonly Context context;

        public ScreenLockCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            KeyguardManager manager = (KeyguardManager)context.GetSystemService(Context.KeyguardService);             return new SecurityCheckResult(this, manager.IsDeviceSecure);
        }
    }
}
