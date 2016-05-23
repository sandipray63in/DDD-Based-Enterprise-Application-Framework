using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using TextEncoding = System.Text;
using System.Threading.Tasks;

namespace RestfulWebAPI.Binding.MediaFormatters
{
    public abstract class BaseFixedWidthTextMediaFormatter<TEntity> : MediaTypeFormatter
        where TEntity : class
    {
        public BaseFixedWidthTextMediaFormatter()
        {
            SupportedEncodings.Add(TextEncoding.Encoding.UTF8);
            SupportedEncodings.Add(TextEncoding.Encoding.Unicode);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable<TEntity>).IsAssignableFrom(type);
        }

        public override async Task WriteToStreamAsync(Type type,object value,Stream stream,HttpContent content,TransportContext transportContext)
        {
            using (stream)
            {
                var encoding = SelectCharacterEncoding(content.Headers);
                using (var writer = new StreamWriter(stream, encoding))
                {
                    var entities = value as IEnumerable<TEntity>;
                    if (entities.IsNotNull())
                    {
                        ProcessEntities(writer, entities);
                        await writer.FlushAsync();
                    }
                }
            }
        }

        protected abstract void ProcessEntities(StreamWriter writer, IEnumerable<TEntity> entities);
    }
}