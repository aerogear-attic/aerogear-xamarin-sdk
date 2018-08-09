using System;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Abstract implementation of <see cref="ISecurityCheck"/>.
    /// </summary>
    public abstract class AbstractDeviceCheck: IDeviceCheck
    {
        protected abstract string Name { get; }

        public string GetId() => this.GetType().FullName;

        public string GetName() => Name;

        public abstract DeviceCheckResult Check();
    }
}
