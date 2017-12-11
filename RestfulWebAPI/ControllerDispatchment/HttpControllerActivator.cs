using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.Unity;
using Infrastructure.DI;
using RestfulWebAPI.Utilities;

namespace RestfulWebAPI.ControllerDispatchment
{
    /// <summary>
    /// https://www.asp.net/media/4071077/aspnet-web-api-poster.pdf
    /// https://github.com/mono/aspnetwebstack/blob/master/src/System.Web.Http/Dispatcher/DefaultHttpControllerActivator.cs
    /// http://blog.ploeh.dk/2012/09/28/DependencyInjectionandLifetimeManagementwithASP.NETWebAPI/
    /// https://www.strathweb.com/2013/09/dynamic-per-controller-httpconfiguration-asp-net-web-api/
    /// </summary>
    public class HttpControllerActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController httpController = null;
            string controllerName = controllerDescriptor.ControllerName.IsNotNullOrEmpty() ? controllerDescriptor.ControllerName : controllerDescriptor.ControllerType.Name;
            try
            {
                httpController = Container.Instance.Resolve(controllerDescriptor.ControllerType) as IHttpController;
                if (httpController.IsNull())
                {
                    httpController = Container.Instance.Resolve(controllerDescriptor.ControllerType,controllerDescriptor.ControllerName) as IHttpController;
                }
            }
            catch
            {
                httpController = Container.Instance.Resolve(controllerDescriptor.ControllerType, controllerDescriptor.ControllerName) as IHttpController;
            }
            if (httpController.IsNull())
            {
                HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.InternalServerError, controllerName + " not registered in Unity DI container", request);
            }
            return httpController;
        }
    }
}