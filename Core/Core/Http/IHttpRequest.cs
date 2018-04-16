using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Http
{
    public static class Constants
    {
        const String CONTENT_TYPE_HEADER = "Content-Type";
        const String JSON_MIME_TYPE = "application/json";
    }

    /// <summary>
    /// Interface for requests to HTTP services.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Adds HTTP header to the request
        /// </summary>
        /// <param name="key">HTTP header name</param>
        /// <param name="value">HTTP header value</param>
        /// <returns>request itself</returns>
        IHttpRequest AddHeader(string key, string value);


        /// <summary>
        /// Prepares request for GET method on the target.
        /// </summary>
        /// <param name="url">request target</param>
        /// <returns>request ready for execution</returns>
        IHttpRequestToBeExecuted Get(string url);


        /// <summary>
        /// Prepares request for POST method on the target.
        /// </summary>
        /// <param name="url">request target</param>
        /// <param name="body">request body</param>
        /// <returns>request ready for execution</returns>
        IHttpRequestToBeExecuted Post(string url, byte[] body);

        /// <summary>
        /// Prepares request for PUT method on the target.
        /// </summary>
        /// <param name="url">request target</param>
        /// <param name="body">request body</param>
        /// <returns>request ready for execution</returns>
        IHttpRequestToBeExecuted Put(string url,byte[] body);

        /// <summary>
        /// Prepares request for DELETE method on the target.
        /// </summary>
        /// <param name="url">request target</param>
        /// <returns>request ready for execution</returns>
        IHttpRequestToBeExecuted Delete(string url);
        
        /// <summary>
        /// Prepares request to be executed with a custom HTTP method
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="url">request target</param>
        /// <param name="body">request body</param>
        /// <returns>request ready for execution</returns>
        IHttpRequestToBeExecuted Custom(string method,string url, byte[] body);
    }

    /// <summary>
    /// Interface for request to be executed.
    /// </summary>
    public interface IHttpRequestToBeExecuted
    {
        /// <summary>
        /// Create a HTTPResponse and begin executing the request. Awaits the whole response to be retrieved. The request will be executed asynchronously.
        /// </summary>
        /// <returns>response (async)</returns>
        Task<IHttpResponse> Execute();

        /// <summary>
        /// Executes the request and returns response as stream. The request will be executed asynchronously.
        /// </summary>
        /// <returns>stream (async)</returns>
        Task<Stream> ExecuteGetStream();
    }
}
