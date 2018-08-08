using System;
using Android.Content;
using Android.App.Admin;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Detects whether a devices filesystem is encrypted.
    /// </summary>
    public class EncryptionEnabledCheck : AbstractDeviceCheck
    {
        protected override string Name => "Encryption Check";

        private readonly Context context;

        public EncryptionEnabledCheck(Context ctx)
        {
            this.context = ctx;
        }

        public override DeviceCheckResult Check()
        {
            DevicePolicyManager policyManager = (DevicePolicyManager)context
                .GetSystemService(Context.DevicePolicyService);
            bool enabled = policyManager != null && policyManager
                .StorageEncryptionStatus == EncryptionStatus.Active;
            return new DeviceCheckResult(this, enabled);
        }
    }
}
