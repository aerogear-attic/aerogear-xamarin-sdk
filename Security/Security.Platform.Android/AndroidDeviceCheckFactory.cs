using System;
using System.Collections.Generic;
using Android.Content;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Factory for Android security checks.
    /// </summary>
    internal class AndroidDeviceCheckFactory : IDeviceCheckFactory
    {
        public static readonly AndroidDeviceCheckFactory INSTANCE = new AndroidDeviceCheckFactory();

        private readonly Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Security.AndroidDeviceCheckFactory"/> class.
        /// </summary>
        public AndroidDeviceCheckFactory(Context ctx = null)
        {
            this.context = ctx != null ? ctx.ApplicationContext : Android.App.Application.Context;
        }

        /// <summary>
        /// Returns an initialized instance of the check identified by the passed in pseudo enumeration.
        /// </summary>
        /// <returns>The initialized instance of the check.</returns>
        /// <param name="type">Type of the check to be instantiated. This must be an instance of <see cref="T:AeroGear.Mobile.Security.DeviceChecks"/></param>
        public IDeviceCheck create(IDeviceCheckType type)
        {
            DeviceChecks checkType = type as DeviceChecks;

            if (checkType == null) 
            {
                throw new Exception("Passed in device check type is not supported");
            }

            return Activator.CreateInstance(checkType.CheckType, this.context) as IDeviceCheck;
        }

        /// <summary>
        /// Returns an initialized instance of the check identified by the passed in pseudo enumeration.
        /// An exception is thrown if no check with the given name exists.
        /// </summary>
        /// <returns>The initialized instance of the check.</returns>
        /// <param name="typeName">The name of the check to be instantiated.</param>
        public IDeviceCheck create(string typeName)
        {
            IDeviceCheckType securityCheckType = DeviceChecks.GetDeviceCheck(typeName);
            if (securityCheckType == null)
            {
                throw new Exception(String.Format("No device check with name {0} is known", typeName));
            }

            return create(securityCheckType);
        }
    }
}
