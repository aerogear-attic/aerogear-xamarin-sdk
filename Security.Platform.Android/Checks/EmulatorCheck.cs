using System;
using Android.Content;
using Android.OS;

namespace AeroGear.Mobile.Security.Checks
{
    public class EmulatorCheck : ISecurityCheck
    {
        private const string NAME = "Emulator Check";

        private readonly Context context;

        public EmulatorCheck(Context ctx)
        {
            this.context = ctx;
        }

        public string GetId()
        {
            return typeof(EmulatorCheck).FullName;
        }

        public string GetName()
        {
            return NAME;
        }

        public SecurityCheckResult Check()
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
