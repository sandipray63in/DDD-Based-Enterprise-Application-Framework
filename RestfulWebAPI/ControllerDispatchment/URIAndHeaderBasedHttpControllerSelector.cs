using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Net.Http.Headers;
using Infrastructure.Utilities;

namespace RestfulWebAPI.ControllerDispatchment
{
    public class URIAndHeaderBasedHttpControllerSelector : DefaultHttpControllerSelector
    {
        private const string ControllerKey = "controller";

        public URIAndHeaderBasedHttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {

        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            ContractUtility.Requires<ArgumentNullException>(request.IsNotNull(), "request cannot be null");
            var routeData = request.GetRouteData();
            // Get the controller version from header or url.
            var controllerVersionInRequestObject = GetControllerVersion(request.Headers) ?? GetControllerVersion(request.RequestUri) ?? 0;
            // Look up controller in route data
            object controllerName = null;
            routeData.Values.TryGetValue(ControllerKey, out controllerName);
            var controllerNameInRequestObject = controllerName as string;
            return controllerVersionInRequestObject == 0 ? controllerNameInRequestObject : string.Empty;
        }

        #region Private Methods

        private double? GetControllerVersion(HttpRequestHeaders headers)
        {
            if (!headers.Contains("x-api-controller-version"))
            {
                return null;
            }

            var versionNumber = 0d;
            var versionHeader = headers.GetValues("x-api-controller-version").FirstOrDefault();
            if (versionHeader.IsNotNull())
            {
                double.TryParse(versionHeader, out versionNumber);
            }
            return versionNumber;
        }

        private double? GetControllerVersion(Uri requestUri)
        {
            if (requestUri.Query.IsNullOrEmpty())
            {
                return null;
            }
            var query = HttpUtility.ParseQueryString(requestUri.Query);
            var versionNumber = 0d;
            var version = query["controllerVersion"];
            double.TryParse(version, out versionNumber);
            return versionNumber;
        }

        #endregion
    }
}