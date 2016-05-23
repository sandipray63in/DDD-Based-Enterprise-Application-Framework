using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace RestfulWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ////
            //// For the time being commented but add whatever is required.
            ////

            //var fwtMediaFormatter = new FixedWidthTextMediaFormatter();
            //fwtMediaFormatter.MediaTypeMappings.Add(
            //    new QueryStringMapping("frmt", "fwt",
            //        new MediaTypeHeaderValue("text/plain")));

            //config.Formatters.Add(fwtMediaFormatter);

            //config.Formatters.Add(new JsonpMediaTypeFormatter());
            //config.Formatters.JsonFormatter.MediaTypeMappings.Add(new IPBasedMediaTypeMapping());

            //config.MessageHandlers.Add(new EncodingHandler());
            //config.MessageHandlers.Add(new CultureHandler());

            //var rules = config.ParameterBindingRules;
            //rules.Insert(0, p =>
            //{
            //    if (p.ParameterType == typeof(Employee))
            //    {
            //        return new AllRequestParameterBinding(p);
            //    }

            //    return null;
            //});

            //config.Services.Add(typeof(System.Web.Http.ValueProviders.ValueProviderFactory),
            //                                       new HeaderValueProviderFactory());
        }
    }
}
