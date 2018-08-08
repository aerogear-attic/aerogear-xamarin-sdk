using System;
using Android.Content;
using Android.OS;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether a debugger is attached to the current application.
    /// </summary>
    public class NoDebuggerCheck : AbstractDeviceCheck
    {
        protected override string Name => "Debugger Check";

        private readonly Context context;

        public NoDebuggerCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override DeviceCheckResult Check()
        {
            return new DeviceCheckResult(this, !(Debug.IsDebuggerConnected || System.Diagnostics.Debugger.IsAttached));
        }
    }
}
