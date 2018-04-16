using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Http
{
    public interface IHttpResponse
    {

        /// <summary>
        /// Is true when request is considered successful.
        /// </summary>
        bool Successful { get; }

        /// <summary>
        /// HTTP status code of the response
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// Response body as a string.
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Underlying error exception, if any.
        /// </summary>
        System.Exception Error { get; }

    }
}
