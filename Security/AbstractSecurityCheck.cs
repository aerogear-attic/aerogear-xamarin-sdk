using System;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Abstract implementation of <see cref="ISecurityCheck"/>.
    /// </summary>
    public abstract class AbstractSecurityCheck: ISecurityCheck
    {
        protected abstract string Name { get; }

        public string GetId()
        {
            return this.GetType().FullName;
        }

        public string GetName()
        {
            return Name;
        }

        public abstract SecurityCheckResult Check();
    }
}
