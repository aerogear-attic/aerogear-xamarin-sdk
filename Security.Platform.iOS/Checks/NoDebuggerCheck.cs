using System;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if the device is running in Debug mode.
    /// </summary>
    public class NoDebuggerCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Debugger Check"; } }

        public NoDebuggerCheck()
        {
        }

        public override SecurityCheckResult Check()
        {
#if DEBUG
            return new SecurityCheckResult(this, false);
#else
            return new SecurityCheckResult(this, true);
#endif
        }
    }
}
