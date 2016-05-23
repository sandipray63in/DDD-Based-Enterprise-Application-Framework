using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebCaching
{
    public class EnableETag : ActionFilterAttribute
    {
        private static ConcurrentDictionary<string, EntityTagHeaderValue> etags = new ConcurrentDictionary<string, EntityTagHeaderValue>();

        public override void OnActionExecuting(HttpActionContext context)
        {
            var request = context.Request;
            if (request.Method == HttpMethod.Get)
            {
                var key = GetKey(request);
                var etagsFromClient = request.Headers.IfNoneMatch;
                if (etagsFromClient.Count > 0)
                {
                    EntityTagHeaderValue etag = null;
                    if (etags.TryGetValue(key, out etag) && etagsFromClient.Any(t => t.Tag == etag.Tag))
                    {
                        context.Response = new HttpResponseMessage(HttpStatusCode.NotModified);
                    }
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var request = context.Request;
            var key = GetKey(request);
            EntityTagHeaderValue etag = null;
            bool isGet = request.Method == HttpMethod.Get;
            bool isPutOrPost = request.Method == HttpMethod.Put || request.Method == HttpMethod.Post;
            if ((isGet && !etags.TryGetValue(key, out etag)) || isPutOrPost)
            {
                etag = new EntityTagHeaderValue("\"" + Guid.NewGuid().ToString() + "\"");
                etags.AddOrUpdate(key, etag, (k, val) => etag);
            }
            if (isGet)
            {
                context.Response.Headers.ETag = etag;
            }
        }

        private string GetKey(HttpRequestMessage request)
        {
            return request.RequestUri.ToString();
        }
    }
}