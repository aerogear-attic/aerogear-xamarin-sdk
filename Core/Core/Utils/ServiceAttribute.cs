using System;

namespace AeroGear.Mobile.Core.Utils
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    ///<summary>
    ///This Attribute is used to inform <see cref="MobileCore"/> of implementations of Services.  This is used to provide the correct concrete implementation
    ///for a platform.
    ///</summary>
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(Type implementorType)
        {
            Implementor = implementorType;
        }

        internal Type Implementor { get; private set; }
    }
}
