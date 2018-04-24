using System;
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.iOS.Tests.Mocks
{
    public class HttpResponseMock : IHttpResponse
    {
        private readonly bool Successful;
        private readonly int StatusCode;
        private readonly string Body;
        private readonly Exception Error;

        internal HttpResponseMock(bool successful, int statusCode, string body, Exception error)
        {
            this.Successful = successful;
            this.StatusCode = statusCode;
            this.Body = body;
            this.Error = error;
        }

        bool IHttpResponse.Successful => this.Successful;

        int IHttpResponse.StatusCode => this.StatusCode;

        string IHttpResponse.Body => this.Body;

        Exception IHttpResponse.Error => this.Error;

        public static HttpResponseMockBuilder newResponse() => new HttpResponseMockBuilder();
    }

    public class HttpResponseMockBuilder
    {
        private bool Successful = true;
        private int StatusCode = 200;
        private string Body = "";
        private Exception Error = null;

        internal HttpResponseMockBuilder() {}

        public HttpResponseMockBuilder withSuccessfulStatus(bool status)
        {
            this.Successful = status;
            return this;
        }

        public HttpResponseMockBuilder withStatusCode(int statusCode)
        {
            this.StatusCode = statusCode;
            return this;
        }

        public HttpResponseMockBuilder withBody(string body)
        {
            this.Body = body;
            return this;
        }

        public HttpResponseMockBuilder withError(Exception error)
        {
            this.Error = error;
            return this;
        }

        public static implicit operator HttpResponseMock(HttpResponseMockBuilder ub) => new HttpResponseMock(ub.Successful, ub.StatusCode, ub.Body, ub.Error);

    }
}
