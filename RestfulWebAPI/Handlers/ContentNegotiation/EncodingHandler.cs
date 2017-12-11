using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RestfulWebAPI.Encoding;

namespace RestfulWebAPI.Handlers.ContentNegotiation
{
    public class EncodingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            try
            {
                var schema = new EncodingSchema();
                Func<Stream, Stream> encoder = schema.GetEncoder(response.RequestMessage.Headers.AcceptEncoding);
                if (encoder.IsNotNull())
                {
                    response.Content = new EncodedContent(response.Content, encoder);
                    // Add Content-Encoding response header
                    response.Content.Headers.ContentEncoding.Add(schema.ContentEncoding);
                }
            }
            catch (NegotiationFailedException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message);
            }
            return response;
        }
    }
}