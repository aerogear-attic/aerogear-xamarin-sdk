using System;
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.iOS.Tests.Mocks
{
    public class HttpRequestMock : IHttpRequest
    {
        private readonly IHttpRequestToBeExecuted GetResult;
        private readonly IHttpRequestToBeExecuted PostResult;
        private readonly IHttpRequestToBeExecuted PutResult;
        private readonly IHttpRequestToBeExecuted DeleteResult;
        private readonly IHttpRequestToBeExecuted CustomResult;

        internal HttpRequestMock(IHttpRequestToBeExecuted getResult, 
                               IHttpRequestToBeExecuted postResult,
                               IHttpRequestToBeExecuted putResult,
                               IHttpRequestToBeExecuted deleteResult,
                               IHttpRequestToBeExecuted customResult)
        {
            this.GetResult = getResult;
            this.PostResult = postResult;
            this.PutResult = putResult;
            this.DeleteResult = deleteResult;
            this.CustomResult = customResult;
        }

        public IHttpRequest AddHeader(string key, string value) => this;

        public IHttpRequestToBeExecuted Custom(string method, string url, byte[] body) => CustomResult;

        public IHttpRequestToBeExecuted Delete(string url) => DeleteResult;

        public IHttpRequestToBeExecuted Get(string url) => GetResult;

        public IHttpRequestToBeExecuted Post(string url, byte[] body) => PostResult;

        public IHttpRequestToBeExecuted Put(string url, byte[] body) => PutResult;

        public static HttpRequestMockBuilder newRequest() => new HttpRequestMockBuilder(); 
    }

    public class HttpRequestMockBuilder
    {
        private IHttpRequestToBeExecuted GetResult;
        private IHttpRequestToBeExecuted PostResult;
        private IHttpRequestToBeExecuted PutResult;
        private IHttpRequestToBeExecuted DeleteResult;
        private IHttpRequestToBeExecuted CustomResult;

        internal HttpRequestMockBuilder() {}

        public HttpRequestMockBuilder withGetResult(IHttpRequestToBeExecuted result)
        {
            this.GetResult = result;
            return this;
        }

        public HttpRequestMockBuilder withPostResult(IHttpRequestToBeExecuted result)
        {
            this.PostResult = result;
            return this;
        }
        public HttpRequestMockBuilder withPutResult(IHttpRequestToBeExecuted result)
        {
            this.PutResult = result;
            return this;
        }
        public HttpRequestMockBuilder withDeleteResult(IHttpRequestToBeExecuted result)
        {
            this.DeleteResult = result;
            return this;
        }
        public HttpRequestMockBuilder withCustomResult(IHttpRequestToBeExecuted result)
        {
            this.CustomResult = result;
            return this;
        }

        public static implicit operator HttpRequestMock(HttpRequestMockBuilder rb) => new HttpRequestMock(rb.GetResult, rb.PostResult, rb.PutResult, rb.DeleteResult, rb.CustomResult);
    }
}
