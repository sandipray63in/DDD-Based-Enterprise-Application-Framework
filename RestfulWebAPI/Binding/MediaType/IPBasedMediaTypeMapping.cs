using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Web;

namespace RestfulWebAPI.Binding.MediaType
{
    public class IPBasedMediaTypeMapping : MediaTypeMapping
    {
        public IPBasedMediaTypeMapping() : base(new MediaTypeHeaderValue("application/json")) { }

        public override double TryMatchMediaType(HttpRequestMessage request)
        {
            var ipAddress = String.Empty;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var httpContext = request.Properties["MS_HttpContext"] as HttpContextBase;
                ipAddress = httpContext.Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var prop = request.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                ipAddress = prop.Address;
            }

            //::1 is the loopback address in IPv6, same as 127.0.0.1 in IPv4
            // Using the loopback address only for illustration
            return "::1".Equals(ipAddress) ? 1.0 : 0.0;
        }
    }
}