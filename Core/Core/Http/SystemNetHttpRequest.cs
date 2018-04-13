using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Http
{
    /// <summary>
    /// Implementation of IHttpRequest based on System.Net library.
    /// </summary>
    internal class SystemNetHttpRequest : IHttpRequest
    {
        private readonly HttpClient httpClient;

        private readonly List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();

        internal SystemNetHttpRequest(HttpClient httpClient)
        {
            Contract.Requires(httpClient != null);
            this.httpClient = httpClient;
        }

        public IHttpRequest AddHeader(string key, string value)
        {
            Contract.Requires(key != null && value != null);
            headers.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }


        public IHttpRequestToBeExecuted Custom(string method, string url, byte[] body)
        {
            Contract.Requires(method != null && url != null && body != null);
            var message = new HttpRequestMessage();
            message.Method = new HttpMethod(method);
            message.RequestUri = new Uri(url);
            foreach (var header in headers)
                message.Headers.Add(header.Key, header.Value);
            if (body != null)
                message.Content = new ByteArrayContent(body);
            return new SystemNetHttpRequestToBeExecuted(httpClient, message);
        }

        public IHttpRequestToBeExecuted Delete(string url)
        {
            return Custom(HttpMethod.Delete.ToString(), url, null);
        }

        public IHttpRequestToBeExecuted Get(string url)
        {
            return Custom(HttpMethod.Get.ToString(), url, null);
        }

        public IHttpRequestToBeExecuted Post(string url, byte[] body)
        {
            return Custom(HttpMethod.Post.ToString(), url, body);
        }

        public IHttpRequestToBeExecuted Put(string url, byte[] body)
        {
            return Custom(HttpMethod.Put.ToString(), url, body);
        }
    }

    /// <summary>
    /// Class used for holding requests that are ready for execution.
    /// </summary>
    internal class SystemNetHttpRequestToBeExecuted : IHttpRequestToBeExecuted
    {
        private readonly HttpRequestMessage message;
        private readonly HttpClient httpClient;

        internal SystemNetHttpRequestToBeExecuted(HttpClient httpClient, HttpRequestMessage message)
        {
            this.message = message;
            this.httpClient = httpClient;
        }

              
        public async Task<IHttpResponse> Execute()
        {
            SystemNetHttpResponse httpResponse = null;
            try
            {
                var response = await httpClient.SendAsync(message);
                int statusCode = (int)response.StatusCode;
                var successful = !(statusCode >= 400 && statusCode <= 599);
                var body = await response.Content.ReadAsStringAsync();
                httpResponse = new SystemNetHttpResponse(successful, statusCode, body, null);
            }
            catch (System.Exception e)
            {
                httpResponse = new SystemNetHttpResponse(false, -1, null, e);
            }
            return httpResponse;
        }

        /// <summary>
        /// Executes the request and returns asynchronous stream.
        /// </summary>
        /// <returns>stream (async)</returns> 
        public Task<Stream> ExecuteGetStream()
        {
            throw new NotImplementedException();
        }
    }
}
