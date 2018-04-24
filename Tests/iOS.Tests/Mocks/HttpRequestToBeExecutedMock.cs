using System;
using System.IO;
using System.Threading.Tasks;
using AeroGear.Mobile.Core.Http;

namespace AeroGear.Mobile.iOS.Tests.Mocks
{
    public class HttpRequestToBeExecutedMock : IHttpRequestToBeExecuted
    {
        private readonly Task<IHttpResponse> ExecuteResult;
        private readonly Task<Stream> ExecuteGetStreamResult;

        public HttpRequestToBeExecutedMock(Task<IHttpResponse> executeResult, Task<Stream> executeGetStreamResult)
        {
            this.ExecuteResult = executeResult;
            this.ExecuteGetStreamResult = executeGetStreamResult;
        }

        public Task<IHttpResponse> Execute() => ExecuteResult;

        public Task<Stream> ExecuteGetStream() => ExecuteGetStreamResult;
    }
}
