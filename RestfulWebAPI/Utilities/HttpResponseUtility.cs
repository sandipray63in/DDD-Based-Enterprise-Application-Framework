using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestfulWebAPI.Utilities
{
    public static class HttpResponseUtility
    {
        public static void ThrowHttpResponseError(HttpStatusCode statusCode, string message, HttpRequestMessage request = null)
        {
            HttpResponseMessage errResponse = request.IsNotNull() ? request.CreateErrorResponse(statusCode, message) : new HttpResponseMessage(statusCode) { ReasonPhrase = message };
            throw new HttpResponseException(errResponse);
        }
    }
}