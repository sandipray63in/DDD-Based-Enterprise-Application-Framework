using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using TextEncoding = System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RestfulWebAPI.Binding.MediaFormatters
{
    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private const string JAVASCRIPT_MIME = "application/javascript";

        private string queryStringParameterName = "callback";
        private string Callback { get; set; }
        private bool IsJsonp { get; set; }

        public JsonpMediaTypeFormatter()
        {
            // Do not want to inherit supported media types or
            // media type mappings of JSON
            SupportedMediaTypes.Clear();
            MediaTypeMappings.Clear();

            // We have our own!
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(JAVASCRIPT_MIME));
            MediaTypeMappings.Add(new QueryStringMapping("frmt", "jsonp", JAVASCRIPT_MIME));
        }

        // other members go here
        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type,HttpRequestMessage request,MediaTypeHeaderValue mediaType)
        {
            var isGet = request.IsNotNull() && request.Method == HttpMethod.Get;
            var callback = String.Empty;
            if (request.RequestUri.IsNotNull())
            {
                callback = HttpUtility.ParseQueryString(request.RequestUri.Query)[queryStringParameterName];
            }

            // Only if this is an HTTP GET and there is a callback, we consider
            // the request a valid JSONP request and service the same. If not,
            // fallback to JSON
            var isJsonp = isGet && callback.IsNotNullOrEmpty();

            // Returning a new instance since callback must be stored at the
            // class level for WriteToStreamAsync to output. Our formatter is not
            // stateless, unlike the out-of-box formatters.
            return new JsonpMediaTypeFormatter() { Callback = callback, IsJsonp = isJsonp };
        }

        public override void SetDefaultContentHeaders(Type type,HttpContentHeaders headers,MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            if (!this.IsJsonp)
            {
                // Fallback to JSON content type
                headers.ContentType = DefaultMediaType;

                // If the encodings supported by us include the charset of the 
                // authoritative media type passed to us, we can take that as the charset
                // for encoding the output stream. If not, pick the first one from 
                // the encodings we support
                if (this.SupportedEncodings.Any(e => e.WebName.Equals(mediaType.CharSet, StringComparison.OrdinalIgnoreCase)))
                {
                    headers.ContentType.CharSet = mediaType.CharSet;
                }
                else
                {
                    headers.ContentType.CharSet = this.SupportedEncodings.First().WebName;
                }
            }
        }

        public override async Task WriteToStreamAsync(Type type, object value,Stream stream,HttpContent content,TransportContext transportContext)
        {
            using (stream)
            {
                if (this.IsJsonp) // JSONP
                {
                    var encoding = TextEncoding.Encoding.GetEncoding(content.Headers.ContentType.CharSet);
                    using (var writer = new StreamWriter(stream, encoding))
                    {
                        writer.Write(this.Callback + "(");
                        await writer.FlushAsync();
                        await base.WriteToStreamAsync(type, value, stream, content, transportContext);
                        writer.Write(")");
                        await writer.FlushAsync();
                    }
                }
                else // fallback to JSON
                {
                    await base.WriteToStreamAsync(type, value, stream, content, transportContext);
                    return;
                }
            }
        }
    }
}