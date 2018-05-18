using System;
namespace AeroGear.Mobile.Security.Checks
{
    /// <summary>
    /// This is just a stub implementation. Fill with the real implementation.
    /// </summary>
    public class NonJailbrokenCheck : ISecurityCheck
    {
        private const string NAME = "Jailbreak Check";

        public NonJailbrokenCheck()
        {
        }

        public string GetId()
        {
            return typeof(NonJailbrokenCheck).FullName;
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
