using System;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Configuration;
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.iOS.Tests.Mocks
{
    public class HttpServiceModuleMock : IHttpServiceModule
    {
        private readonly string ModuleType;
        private readonly bool RequiresConfigurationResult;
        private readonly IHttpRequest NewRequestResult;

        internal HttpServiceModuleMock(string type, bool requiresConfiguration, IHttpRequest newRequest)
        {
            this.ModuleType = type;
            this.RequiresConfigurationResult = requiresConfiguration;
            this.NewRequestResult = newRequest;
        }

        public string Type => ModuleType;

        public bool RequiresConfiguration => RequiresConfigurationResult;

        public string Id => null;

        public void Configure(MobileCore core, ServiceConfiguration serviceConfiguration)
        {
        }

        public void Destroy()
        {
        }

        public IHttpRequest NewRequest() => NewRequestResult;

        public static HttpServiceModuleMockBuilder newMock() => new HttpServiceModuleMockBuilder();
    }

    public class HttpServiceModuleMockBuilder 
    {
        private string Type;
        private bool RequiresConfiguration = false;
        private IHttpRequest NewRequest;

        internal HttpServiceModuleMockBuilder() {}

        public HttpServiceModuleMockBuilder withType(string type)
        {
            this.Type = type;
            return this;
        }

        public HttpServiceModuleMockBuilder withRequiresConfiguration(bool requiresConfiguration)
        {
            this.RequiresConfiguration = requiresConfiguration;
            return this;
        }

        public HttpServiceModuleMockBuilder withNewRequest(IHttpRequest newRequest)
        {
            this.NewRequest = newRequest;
            return this;
        }

        public static implicit operator HttpServiceModuleMock(HttpServiceModuleMockBuilder ub) => new HttpServiceModuleMock(ub.Type, ub.RequiresConfiguration, ub.NewRequest);
    }
}
