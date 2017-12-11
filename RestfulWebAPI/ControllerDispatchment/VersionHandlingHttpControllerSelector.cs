using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Xml.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Net.Http.Headers;
using System.Web.Http.Routing;
using Microsoft.Practices.Unity;
using Domain.Base.Entities;
using Infrastructure.Caching;
using Infrastructure.DI;
using Infrastructure.Utilities;
using RestfulWebAPI.Base;
using RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping;
using RestfulWebAPI.Utilities;

namespace RestfulWebAPI.ControllerDispatchment
{
    /// <summary>
    /// https://www.asp.net/media/4071077/aspnet-web-api-poster.pdf
    /// https://github.com/mono/aspnetwebstack/blob/master/src/System.Web.Http/Dispatcher/DefaultHttpControllerSelector.cs
    /// https://stackoverflow.com/questions/389169/best-practices-for-api-versioning
    /// https://www.strathweb.com/2013/08/customizing-controller-discovery-in-asp-net-web-api/
    /// api is expected to be in the form - somesite/api/{controllerVersion}/entityName/{id}
    /// </summary>
    public class VersionHandlingHttpControllerSelector : DefaultHttpControllerSelector
    {
        private static readonly ControllersXML _controllersXML;
        private static readonly EntitiesXML _entitiesXML;
        private static readonly ICache<string, HttpControllerDescriptor> _cache;
        private const string CONTROLLER_KEY = "controller";
        private readonly HttpConfiguration _configuration;
        private double _controllerVersionInRequestObject;
        private string _controllerNameInRequestObject;

        static VersionHandlingHttpControllerSelector()
        {
            Assembly currentAssembly = typeof(VersionHandlingHttpControllerSelector).Assembly;
            string currentAssemblyLocation = currentAssembly.Location;
            string currentAssemblyName = currentAssembly.GetName().Name;
            string controllersXMLPath = ControllersXMLPath.IsNotNullOrEmpty() ? ControllersXMLPath : Path.Combine(currentAssemblyLocation, "ControllerDispatchment", "ControllerEntityVersionsAndNamesMapping", "ControllerEntityVersionsAndNamesMapping.xml");
            XDocument xDocForControllersXML = XDocument.Load(controllersXMLPath);
            _controllersXML = XMLUtility.DeSerialize<ControllersXML>(xDocForControllersXML.ToString());
            string controllersXSDPath = ControllersXSDPath.IsNotNullOrEmpty() ? ControllersXSDPath : ("RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping.ControllerEntityVersionsAndNamesMapping.xsd");
            string controllersXSDAssemblyName = ControllersXSDAssemblyName.IsNotNullOrEmpty() ? ControllersXSDAssemblyName : currentAssemblyName;
            string xmlValidationErrors = XSDUtility.Validate(controllersXSDAssemblyName, ControllersXSDPath, xDocForControllersXML.ToString());
            if (xmlValidationErrors.IsNotNullOrEmpty())
            {
                HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.InternalServerError, xmlValidationErrors);
            }

            string entitiesXMLPath = EntitiesXMLPath.IsNotNullOrEmpty() ? EntitiesXMLPath : Path.Combine(currentAssemblyLocation, "ControllerDispatchment", "ControllerEntityVersionsAndNamesMapping", "EntitiesAndIDTypesMapping.xml");
            XDocument xDocForEntitiesXML = XDocument.Load(entitiesXMLPath);
            _entitiesXML = XMLUtility.DeSerialize<EntitiesXML>(xDocForEntitiesXML.ToString());
            string entitiesXSDPath = EntitiesXSDPath.IsNotNullOrEmpty() ? EntitiesXSDPath : ("RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping.EntitiesAndIDTypesMapping.xsd");
            string entitiesXSDAssemblyName = EntitiesXSDAssemblyName.IsNotNullOrEmpty() ? EntitiesXSDAssemblyName : currentAssemblyName;
            xmlValidationErrors = XSDUtility.Validate(entitiesXSDAssemblyName, entitiesXSDPath, xDocForEntitiesXML.ToString());
            if (xmlValidationErrors.IsNotNullOrEmpty())
            {
                HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.InternalServerError, xmlValidationErrors);
            }

            //Register this cache in Unity DI Container with Singleton lifetime
            _cache = Container.Instance.Resolve<ICache<string, HttpControllerDescriptor>>();
        }

        public VersionHandlingHttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public static string ControllersXMLPath { get; set; }

        public static string ControllersXSDPath { get; set; }

        public static string ControllersXSDAssemblyName { get; set; }

        public static string EntitiesXMLPath { get; set; }

        public static string EntitiesXSDAssemblyName { get; set; }

        public static string EntitiesXSDPath { get; set; }

        public static string DomainAssemblyName { get; set; }

        public override string GetControllerName(HttpRequestMessage request)
        {
            ContractUtility.Requires<ArgumentNullException>(request.IsNotNull(), "request cannot be null");
            if (_controllerNameInRequestObject.IsNullOrEmpty())
            {
                _controllerNameInRequestObject = GetControllerNameFromRequest(request);
                if (_controllerNameInRequestObject.IsNullOrEmpty())
                {
                    HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.NotFound, "Controller not found", request);
                }
            }
            if (_controllerVersionInRequestObject == 0)
            {
                _controllerVersionInRequestObject = GetControllerVersion(request);
            }
            IEnumerable<ControllerXML> controllerXMLsInControllersXML = _controllersXML.ControllerXMLs.Where(x => x.EntityName.Equals(_controllerNameInRequestObject, StringComparison.InvariantCultureIgnoreCase));
            if (controllerXMLsInControllersXML.IsNullOrEmpty())
            {
                return string.Empty;
            }
            controllerXMLsInControllersXML.ToList().ForEach(x =>
                {
                    x.Version = x.Version.Replace("v", string.Empty).Replace("V", string.Empty);
                }
            );
            ControllerXML controllerXMLForSuppliedVersionNumber = null;
            if (_controllerVersionInRequestObject == 0)
            {
                double maxVersionNumber = controllerXMLsInControllersXML.Max(x => Convert.ToDouble(x.Version.IsNotNullOrEmpty() ? x.Version : "0"));
                controllerXMLForSuppliedVersionNumber = _controllersXML.ControllerXMLs.SingleOrDefault(x => x.EntityName == _controllerNameInRequestObject && (x.Version.IsNotNullOrEmpty() ? x.Version : "0") == maxVersionNumber.ToString());
            }
            else
            {
                controllerXMLForSuppliedVersionNumber = _controllersXML.ControllerXMLs.SingleOrDefault(x => x.EntityName == _controllerNameInRequestObject && x.Version == _controllerVersionInRequestObject.ToString());
            }
            if (controllerXMLForSuppliedVersionNumber.IsNull())
            {
                return string.Empty;
            }
            return controllerXMLForSuppliedVersionNumber.ControllerName.IsNotNullOrEmpty() ? controllerXMLForSuppliedVersionNumber.ControllerName : _controllerNameInRequestObject;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (_controllerNameInRequestObject.IsNullOrEmpty())
            {
                _controllerNameInRequestObject = GetControllerNameFromRequest(request);
                if (_controllerNameInRequestObject.IsNullOrEmpty())
                {
                    HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.NotFound, "Controller not found", request);
                }
            }
            if (_controllerVersionInRequestObject == 0)
            {
                _controllerVersionInRequestObject = GetControllerVersion(request);
            }
            string cacheKey = _controllerNameInRequestObject + ":" + _controllerVersionInRequestObject + ":" + request.Method.ToString();
            if (_cache.Contains(cacheKey))
            {
                return _cache.Get(cacheKey);
            }
            string controllerName = GetControllerName(request);
            if (controllerName.IsNullOrEmpty())
            {
                HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.NotFound, "Controller not found", request);
            }
            string currentAssemblyName = this.GetType().Assembly.GetName().Name;
            Type controllerType = null;
            try
            {
                controllerType = GetControllerTypeForControllerNotPresentInControllersXMLFile(request, controllerName);
                if (controllerType.IsNull())
                {
                    controllerType = MetaDataUtility.GetType(currentAssemblyName, controllerName);
                }
            }
            catch
            {
                controllerType = MetaDataUtility.GetType(currentAssemblyName, controllerName);
            }
            if (controllerType.IsNull())
            {
                HttpResponseUtility.ThrowHttpResponseError(HttpStatusCode.NotFound, controllerName + " not found", request);
            }
            HttpControllerDescriptor httpControllerDescriptor = new HttpControllerDescriptor();
            httpControllerDescriptor.Configuration = _configuration;
            httpControllerDescriptor.ControllerType = controllerType;
            httpControllerDescriptor.ControllerName = controllerType.Name;
            _cache.Add(cacheKey, httpControllerDescriptor);
            return httpControllerDescriptor;
        }

        #region Private Methods

        private string GetControllerNameFromRequest(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();
            object entityName = null;
            routeData.Values.TryGetValue(CONTROLLER_KEY, out entityName);
            return entityName as string;
        }

        private double GetControllerVersion(HttpRequestMessage request)
        {
            return GetControllerVersion(request.Headers) == 0 ? GetControllerVersion(request.RequestUri) : 0;
        }

        private double GetControllerVersion(HttpRequestHeaders headers)
        {
            if (!headers.Contains("x-api-controller-version"))
            {
                return 0;
            }
            double versionNumber = 0d;
            string versionHeader = headers.GetValues("x-api-controller-version").FirstOrDefault();
            if (versionHeader.IsNotNull())
            {
                versionHeader = versionHeader.Replace("v", string.Empty).Replace("V", string.Empty);
                double.TryParse(versionHeader, out versionNumber);
            }
            return versionNumber;
        }

        private double GetControllerVersion(Uri requestUri)
        {
            if (requestUri.Query.IsNullOrEmpty())
            {
                return 0;
            }
            NameValueCollection query = HttpUtility.ParseQueryString(requestUri.Query);
            double versionNumber = 0d;
            string version = query["controllerVersion"];
            if (version.IsNotNullOrEmpty())
            {
                version = version.Replace("v", string.Empty).Replace("V", string.Empty);
            }
            double.TryParse(version, out versionNumber);
            return versionNumber;
        }

        private Type GetControllerTypeForControllerNotPresentInControllersXMLFile(HttpRequestMessage request, string controllerName)
        {
            string domainAssemblyName = DomainAssemblyName.IsNotNullOrEmpty() ? DomainAssemblyName : typeof(BaseEntity<>).Assembly.GetName().Name;
            string[] genericArgumentsClassNames = new string[1];
            genericArgumentsClassNames[0] = controllerName;
            if (request.Method == HttpMethod.Get)
            {
                EntityXML entityXML = _entitiesXML.EntityXMLs.SingleOrDefault(x => x.Name.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase));
                string TIdType = entityXML.IsNotNull() ? entityXML.IdType : _entitiesXML.DefaultIDType;
                Type[] extraExplicitGenericArgumentTypes = new Type[1];
                extraExplicitGenericArgumentTypes[0] = MetaDataUtility.GetType("mscorlib", TIdType);
                return MetaDataUtility.GetGenericType(typeof(RestfulAPIQuery<,>), domainAssemblyName, genericArgumentsClassNames, extraExplicitGenericArgumentTypes);
            }
            else
            {
                return MetaDataUtility.GetGenericType(typeof(RestfulAPICommand<>), domainAssemblyName, genericArgumentsClassNames);
            }
            //TODO - Need to take care of the Elastic Search API Controller as well
        }
        #endregion
    }
}