using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace RestfulWebAPI.Filters
{
    public class CacheAttribute : ActionFilterAttribute
    {
        public double MaxAgeSeconds { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (this.MaxAgeSeconds > 0)
            {
                context.Response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromSeconds(this.MaxAgeSeconds),
                    MustRevalidate = true,
                    Private = true
                };
            }
        }
    }
}