using System;
using Android.Content;
using Android.OS;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether a debugger is attached to the current application.
    /// </summary>
    public class NoDebuggerCheck : AbstractSecurityCheck
    {
        protected override string Name => "Debugger Check";

        private readonly Context context;

        public NoDebuggerCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            return new SecurityCheckResult(this, !Debug.IsDebuggerConnected);
        }
    }
}
