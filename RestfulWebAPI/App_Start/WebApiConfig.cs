using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.Unity;
using Microsoft.Owin.Security.OAuth;
using RestfulWebAPI.ControllerDispatchment;
using RestfulWebAPI.ControllerDispatchment.ActionDispatchment;
using RestfulWebAPI.Handlers.ExceptionHandling;
using Infrastructure.DI;
using Infrastructure.Logging.Loggers;

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
                name: "DefaultApi",//Set the proper site name here
                routeTemplate: "api/{controllerVersion}/{entityName}/{id}",//TODO - Need to support Actions at various levels
                defaults: new { controllerVersion=RouteParameter.Optional, id = RouteParameter.Optional }
            );

            //Message handlers are called in the same order that they appear in MessageHandlers collection.
            //Because they are nested, the response message travels in the other direction.
            //That is, the last handler is the first to get the response message. 
            ILogger logger = Container.Instance.Resolve<ILogger>();
            config.MessageHandlers.Add(new ExceptionHandler(logger));

            config.Services.Replace(typeof(IHttpControllerSelector), new VersionHandlingHttpControllerSelector(config));
            config.Services.Replace(typeof(IHttpControllerActivator), new HttpControllerActivator());
            config.Services.Replace(typeof(IHttpActionSelector), new HybridActionSelector());
        }
    }
}
