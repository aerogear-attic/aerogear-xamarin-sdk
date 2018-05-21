using System;
using Android.Content;
using Android.Provider;

namespace AeroGear.Mobile.Security.Checks
{
    public class DeveloperModeCheck : ISecurityCheck
    {
        private const string NAME = "Developer Mode Check";

        private readonly Context context;

        public DeveloperModeCheck(Context ctx)
        {
            this.context = ctx;
        }

        public string GetId()
        {
            return typeof(DeveloperModeCheck).FullName;
        }

        public string GetName()
        {
            return NAME;
        }

        public SecurityCheckResult Check()
        {
            bool devModeEnabled = Settings.Secure.GetInt(
                context.ContentResolver,
                Settings.Global.DevelopmentSettingsEnabled) != 0;
            return new SecurityCheckResult(this, !devModeEnabled);
        }
    }
}