﻿using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Checks;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// This class enums all the provided device checks.
    /// 
    /// To get an instance of the check, use the following code:
    /// <code>
    /// <![CDATA[
    /// var deviceCheck = ServiceFinder.Resolve<IDeviceCheckFactory>().create(DeviceChecks.NOT_ROOTED);
    /// ]]>
    /// </code>
    /// </summary>
    public class DeviceChecks : IDeviceCheckType
    {
        private static Dictionary<string, DeviceChecks> typesByName = new Dictionary<string, DeviceChecks>();

        public static readonly DeviceChecks JAILBREAK_ENABLED = new DeviceChecks(typeof(JailbrokenDeviceCheck));
        public static readonly DeviceChecks IS_EMULATOR = new DeviceChecks(typeof(IsEmulatorCheck));
        public static readonly DeviceChecks DEBUGGER_ATTACHED = new DeviceChecks(typeof(DebuggerAttachedCheck));
        public static readonly DeviceChecks DEVICE_LOCK_ENABLED = new DeviceChecks(typeof(DeviceLockEnabledCheck));
        // add others checks here
        // i.e. 
        // public static readonly DeviceChecks NO_DEBUGGER = new DeviceChecks(typeof(NoDebuggerCheck));
        // this way the user will be able to do an enum like selection:
        // DeviceChecks.NOT_JAILBROKEN

        internal readonly Type CheckType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Security.DeviceChecks"/> class.
        /// Private so that it can't be instantiated externally: useful to emulate an enum.
        /// </summary>
        /// <param name="checkType">The class type of the check represented by this instance.</param>
        private DeviceChecks(Type checkType, string friendlyName = null)
        {
            if (!ServiceFinder.IsRegistered<IDeviceCheckFactory>())
            {
                ServiceFinder.RegisterInstance<IDeviceCheckFactory>(IOSDeviceCheckFactory.INSTANCE);
            }

            this.CheckType = checkType;
            typesByName[friendlyName ?? checkType.Name] = this;
        }

        /// <summary>
        /// Returns an the DeviceChecks instance identified by the passed in name.
        /// </summary>
        /// <returns>The DeviceChecks instance identified by the passed in name or <code>null</code> if not found.</returns>
        /// <param name="name">Name.</param>
        public static DeviceChecks GetDeviceCheck(string name)
        {
            if (typesByName.ContainsKey(name))
            {
                return typesByName[name];    
            }
            return null;
        }

        /// <summary>
        /// Returns all the checks.
        /// </summary>
        /// <returns>All the checks.</returns>
        public static ICollection<DeviceChecks> GetAllChecks()
        {
            return typesByName.Values;
        }
    }
}
