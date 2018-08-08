using System;
namespace AeroGear.Mobile.Security
{
    public class DeviceCheckResult
    {
        /// <summary>
        /// The identifier of the check that produced this result.
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// The human readable name of the check that produced this result.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Whether the check has passed or not.
        /// </summary>
        public readonly bool Passed;

        public DeviceCheckResult(IDeviceCheck check, bool passed)
        {
            this.Id = check.GetId();
            this.Name = check.GetName();
            this.Passed = passed;
        }
    }
}
