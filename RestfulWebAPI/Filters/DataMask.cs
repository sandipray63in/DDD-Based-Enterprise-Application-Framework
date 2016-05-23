using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RestfulWebAPI.Filters
{
    /// <summary>
    /// TODO - Need to implement the logic to data mask the ID properties of 
    /// entities(even if they are nested entities) so that the actual ID values 
    /// are not visible at the client side.If actual ID values are visible at the 
    /// client side then that can be a security risk.
    /// </summary>
    public class DataMask : IActionFilter
    {
        public bool AllowMultiple
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }
    }
}