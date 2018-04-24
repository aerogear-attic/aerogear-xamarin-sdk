using System;
using System.Collections.Generic;
using System.Text;

namespace AeroGear.Mobile.Core.Utils
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(Type implementorType)
        {
            Implementor = implementorType;
        }

        internal Type Implementor { get; private set; }
    }
}
