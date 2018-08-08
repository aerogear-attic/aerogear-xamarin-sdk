using System;
using System.Collections.Generic;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Factory for iOS device checks.
    /// </summary>
    internal class IOSDeviceCheckFactory : IDeviceCheckFactory
    {
        public static readonly IOSDeviceCheckFactory INSTANCE = new IOSDeviceCheckFactory();

        private IOSDeviceCheckFactory()
        {
            
        }

        /// <summary>
        /// Create the device check identified by the passed in device check type.
        /// </summary>
        /// <returns>The device check.</returns>
        /// <param name="type">The type of the device check to be instantiated.</param>
        public IDeviceCheck create(IDeviceCheckType type)
        {
            DeviceChecks checkType = type as DeviceChecks;

            if (checkType == null) {
                throw new Exception("Passed in device check type is not supported");
            }

            return Activator.CreateInstance(checkType.CheckType) as IDeviceCheck;
        }

        /// <summary>
        /// Returns an initialized instance of the check identified by the passed in pseudo enumeration.
        /// An exception is thrown if no check with the given name exists.
        /// </summary>
        /// <returns>The initialized instance of the check.</returns>
        /// <param name="typeName">The name of the check to be instantiated.</param>
        public IDeviceCheck create(string typeName)
        {
            IDeviceCheckType deviceCheckType = DeviceChecks.GetDeviceCheck(typeName);
            if (deviceCheckType == null)
            {
                throw new Exception(String.Format("No device check with name {0} is known", typeName));
            }

            return create(deviceCheckType);
        }
    }
}
