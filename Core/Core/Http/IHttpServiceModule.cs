using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Http
{
    public interface IHttpServiceModule : IServiceModule
    {
        /// <summary>
        /// Creates a new HttpRequest and prepends common configuration such as certificate pinning, user
        /// agent headers, etc.
        /// </summary>
        /// <returns>a new IHttpRequest object</returns>
        IHttpRequest NewRequest();
    }
}
