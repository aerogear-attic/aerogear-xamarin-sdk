using System;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// A check for whether the device the application is running on an emulator.
    /// </summary>
    public class NotInEmulatorCheck : AbstractSecurityCheck
    {
        protected override string Name { get { return "Emulator Check"; } }

        public NotInEmulatorCheck()
        {
        }

        public override SecurityCheckResult Check()
        {
            bool inDevice = ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.DEVICE;
            return new SecurityCheckResult(this, inDevice);
        }
    }
}
