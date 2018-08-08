using System;
using LocalAuthentication;
using Foundation;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if a lock screen is set on the device.
    /// </summary>
    public class DeviceLockEnabledCheck : AbstractDeviceCheck
    {
        protected override string Name => "Device Lock Check";

        public DeviceLockEnabledCheck()
        {
        }

        public override DeviceCheckResult Check()
        {
            NSError error;
            bool deviceLockSet = new LAContext().CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out error);
            return new DeviceCheckResult(this, deviceLockSet);
        }
    }
}
