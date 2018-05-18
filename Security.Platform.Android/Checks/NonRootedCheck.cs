using System;
using Android.Content;

namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// This is just a stub implementation. Fill with the real implementation.
    /// </summary>
    public class NonRootedCheck : ISecurityCheck
    {
        private const string NAME = "Rooted Check";

        private readonly Context context;

        public NonRootedCheck(Context ctx)
        {
            this.context = ctx;
        }

        public string GetId()
        {
            return typeof(NonRootedCheck).FullName;
        }

        public string GetName()
        {
            return NAME;
        }

        public SecurityCheckResult Check()
        {
            throw new NotImplementedException();
        }
    }
}
