using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Routing;

namespace RestfulWebAPI.Versioning
{
    /// <summary>
    /// For proper usage of this class, refer - 
    /// http://geekswithblogs.net/EltonStoneman/archive/2014/12/18/versioning-in-webapi-using-route-constraints.aspx
    /// </summary>
    public class ApiVersionRouteConstraint : IHttpRouteConstraint
    {
        public double Minimum { get; set; }

        public double Maximum { get; set; }

        public bool IsDefault { get; set; }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            var version = GetVersion(request.Headers) ?? GetVersion(request.RequestUri) ?? 0;
            if (version == 0 && IsDefault)
            {
                return true;
            }
            return version >= Minimum && (Maximum == 0 || version <= Maximum);
        }

        #region Private Methods

        private double? GetVersion(HttpRequestHeaders headers)
        {
            if (!headers.Contains("x-api-version"))
                return null;

            var versionNumber = 0d;
            var versionHeader = headers.GetValues("x-api-version").FirstOrDefault();
            if (versionHeader.IsNotNull())
            {
                double.TryParse(versionHeader, out versionNumber);
            }
            return versionNumber;
        }

        private double? GetVersion(Uri requestUri)
        {
            if (requestUri.Query.IsNullOrEmpty())
                return null;
            var query = HttpUtility.ParseQueryString(requestUri.Query);
            var versionNumber = 0d;
            var version = query["version"];
            double.TryParse(version, out versionNumber);
            return versionNumber;
        }

        #endregion
    }
}