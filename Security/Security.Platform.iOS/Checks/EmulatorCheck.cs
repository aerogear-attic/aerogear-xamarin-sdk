using System;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on an emulator.
    /// </summary>
    public class EmulatorCheck : AbstractDeviceCheck
    {
        protected override string Name => "Emulator Check";

        public EmulatorCheck()
        {
        }

        public override DeviceCheckResult Check()
        {
            bool inDevice = ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.DEVICE;
            return new DeviceCheckResult(this, !inDevice);
        }
    }
}
