using System;
using Android.Content;

namespace AeroGear.Mobile.Security
{
    /// <summary>
    /// Factory for Android security checks.
    /// </summary>
    internal class AndroidSecurityCheckFactory : ISecurityCheckFactory
    {
        private readonly Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AeroGear.Mobile.Security.AndroidSecurityCheckFactory"/> class.
        /// </summary>
        public AndroidSecurityCheckFactory(Context ctx = null)
        {
            this.context = ctx != null ? ctx.ApplicationContext : Android.App.Application.Context;
        }

        /// <summary>
        /// Returns an initialized instance of the check identified by the passed in pseudo enumeration.
        /// </summary>
        /// <returns>The initialized instance of the check.</returns>
        /// <param name="type">Type of the check to be instantiated. This must be an instance of <see cref="T:AeroGear.Mobile.Security.SecurityChecks"/></param>
        public ISecurityCheck create(ISecurityCheckType type)
        {
            SecurityChecks checkType = type as SecurityChecks;

            if (checkType == null) 
            {
                throw new Exception("Passed in security check type is not supported");
            }

            return Activator.CreateInstance(checkType.CheckType, this.context) as ISecurityCheck;
        }
    }
}
