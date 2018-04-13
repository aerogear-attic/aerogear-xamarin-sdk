using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Configuration;

namespace AeroGear.Mobile.Core.Http
{
    internal class SystemNetHttpServiceModule : IHttpServiceModule
    {
        public string Type => "http";

        public bool RequiresConfiguration => true;

        private ServiceConfiguration serviceConfiguration;
        private HttpClient httpClient;

        public SystemNetHttpServiceModule() : this(new HttpClient()) { }

        internal SystemNetHttpServiceModule(HttpClient httpClient) : this(httpClient, ServiceConfiguration.Builder.Build()) { }

        public SystemNetHttpServiceModule(HttpClient httpClient, ServiceConfiguration serviceConfiguration)
        {
            this.httpClient = httpClient;
            this.serviceConfiguration = serviceConfiguration;            
        }

        public void Configure(MobileCore core, ServiceConfiguration serviceConfiguration)
        {
            this.serviceConfiguration = serviceConfiguration;
        }

        public void Destroy()
        {
           //nothing to do
        }

        public IHttpRequest NewRequest() 
        {
            return new SystemNetHttpRequest(httpClient);
        }
    }
}
