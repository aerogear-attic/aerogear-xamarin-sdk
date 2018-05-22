using System;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Checks;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// This class enums all the provided security checks.
    /// 
    /// To get an instance of the check, use the following code:
    /// <code>
    /// <![CDATA[
    /// var securityChek = ServiceFinder.Resolve<ISecurityCheckFactory>().create(SecurityChecks.NOT_ROOTED);
    /// ]]>
    /// </code>
    /// </summary>
    public class SecurityChecks : ISecurityCheckType
    {
        public static readonly SecurityChecks NOT_JAILBROKEN = new SecurityChecks(typeof(NonJailbrokenCheck));
        public static readonly SecurityChecks NOT_IN_EMULATOR = new SecurityChecks(typeof(NotInEmulatorCheck));
        public static readonly SecurityChecks NO_DEBUGGER = new SecurityChecks(typeof(NoDebuggerCheck));
        public static readonly SecurityChecks DEVICE_LOCK = new SecurityChecks(typeof(DeviceLockCheck));
        // add others checks here
        // i.e. 
        // public static readonly SecurityChecks NO_DEBUGGER = new SecurityChecks(typeof(NoDebuggerCheck));
        // this way the user will be able to do an enum like selection:
        // SecurityChecks.NOT_JAILBROKEN


        internal readonly Type CheckType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Security.SecurityChecks"/> class.
        /// Private so that it can't be instantiated externally: useful to emulate an enum.
        /// </summary>
        /// <param name="checkType">The class type of the check represented by this instance.</param>
        private SecurityChecks(Type checkType)
        {
            if (!ServiceFinder.IsRegistered<ISecurityCheckFactory>()) 
            {
                ServiceFinder.RegisterInstance<ISecurityCheckFactory>(new IOSSecurityCheckFactory());    
            }

            this.CheckType = checkType;
        }
    }
}
