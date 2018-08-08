using System;
using LocalAuthentication;
using Foundation;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// Check if a lock screen is set on the device.
    /// </summary>
    public class DeviceLockCheck : AbstractDeviceCheck
    {
        protected override string Name => "Device Lock Check";

        public DeviceLockCheck()
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
