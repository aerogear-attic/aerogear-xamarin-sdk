using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Http
{
    internal class SystemNetHttpResponse : IHttpResponse
    {
        public SystemNetHttpResponse(bool successful, int statusCode,string body, System.Exception error) {
            Successful = successful;
            StatusCode = statusCode;
            Body = body;
            Error = error;
        }

        public bool Successful { get; private set; }

        public int StatusCode { get; private set; }

        public string Body { get; private set; }

        public System.Exception Error { get; private set; }
    }
}
