using System;
using System.Diagnostics;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if the device is running in Debug mode.
    /// </summary>
    public class DebuggerAttachedCheck : AbstractDeviceCheck
    {
        protected override string Name => "Debugger Check";

        public DebuggerAttachedCheck()
        {
        }

        public override DeviceCheckResult Check()
        {
            return new DeviceCheckResult(this, Debugger.IsAttached);
        }
    }
}
