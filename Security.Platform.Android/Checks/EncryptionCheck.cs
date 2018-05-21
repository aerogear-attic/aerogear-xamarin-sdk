using System;
using Android.Content;
using Android.App.Admin;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Detects whether a devices filesystem is encrypted.
    /// </summary>
    public class EncryptionCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Encryption Check"; } }

        private readonly Context context;

        public EncryptionCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override SecurityCheckResult Check()
        {
            DevicePolicyManager policyManager = (DevicePolicyManager)context
                .GetSystemService(Context.DevicePolicyService);
            bool enabled = policyManager != null && policyManager
                .StorageEncryptionStatus == EncryptionStatus.Active;
            return new SecurityCheckResult(this, enabled);
        }
    }
}
