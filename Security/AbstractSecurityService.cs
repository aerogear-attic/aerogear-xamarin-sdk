using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security.Executors.Sync;

namespace AeroGear.Mobile.Security
{
    public abstract class AbstractSecurityService : ISecurityService
    {
        public AbstractSecurityService(ISecurityCheckFactory checkFactory)
        {
            ServiceFinder.RegisterInstance<ISecurityCheckFactory>(checkFactory);
        }

        public string Type => "security";

        public bool RequiresConfiguration => false;

        public string Id => null;

        public void Configure(MobileCore core, ServiceConfiguration config)
        {
        }

        public void Destroy() 
        {
        }

        public Builder GetSyncExecutor()
        {
            return new Builder();
        }

        public SecurityCheckResult Check(ISecurityCheckType securityCheckType)
        {
            SecurityCheckResult[] results = new SecurityCheckResult[1];
            GetSyncExecutor().WithSecurityCheck(securityCheckType).Build().Execute().Values.CopyTo(results, 0);
            return results[0];
        }

        public SecurityCheckResult Check(ISecurityCheck securityCheck)
        {
            return securityCheck.Check();
        }
    }
}
