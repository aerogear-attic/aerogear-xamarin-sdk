using System;
using LocalAuthentication;
using Foundation;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if a lock screen is set on the device.
    /// </summary>
    public class DeviceLockCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Device Lock Check"; } }

        public DeviceLockCheck()
        {
        }

        public override SecurityCheckResult Check()
        {
            NSError error;
            bool deviceLockSet = new LAContext().CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out error);
            return new SecurityCheckResult(this, deviceLockSet);
        }
    }
}
