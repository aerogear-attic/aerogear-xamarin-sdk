using System;
using System.Diagnostics;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if the device is running in Debug mode.
    /// </summary>
    public class NoDebuggerCheck : AbstractSecurityCheck
    {
        protected override string Name => "Debugger Check";

        public NoDebuggerCheck()
        {
        }

        public override SecurityCheckResult Check()
        {
            return new SecurityCheckResult(this, !Debugger.IsAttached);
        }
    }
}
