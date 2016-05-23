using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http.ValueProviders;

namespace RestfulWebAPI.Binding.ValueProviders
{
    public class HeaderValueProvider : IValueProvider
    {
        private readonly HttpRequestHeaders headers;

        // The function to test each element of the header, which is a KeyValuePair
        // for matching key ignoring the dashes. For example, If-Match header
        // will be chosen if the parameter is defined with a name ifmatch, ifMatch, etc.
        private Func<KeyValuePair<string, IEnumerable<string>>, string, bool> predicate =
            (header, key) =>
            {
                return header.Key.Replace("-", String.Empty).Equals(key, StringComparison.OrdinalIgnoreCase);
            };

        public HeaderValueProvider(HttpRequestHeaders headers)
        {
            this.headers = headers;
        }

        public bool ContainsPrefix(string prefix)
        {
            return headers.Any(h => predicate(h, prefix));
        }

        public ValueProviderResult GetValue(string key)
        {
            var header = headers.FirstOrDefault(h => predicate(h, key));
            if (header.Key.IsNotNullOrEmpty())
            {
                key = header.Key; // Replace the passed in key with the header name
                var values = headers.GetValues(key);

                if (values.Count() > 1) // We got a list of values
                {
                    return new ValueProviderResult(values, null, CultureInfo.CurrentCulture);
                }
                else
                {
                    // We could have received multiple values (comma separated) or just one value
                    var value = values.First();
                    values = value.Split(',').Select(x => x.Trim()).ToArray();
                    if (values.Count() > 1)
                    {
                        return new ValueProviderResult(values, null, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        return new ValueProviderResult(value, value, CultureInfo.CurrentCulture);
                    }
                }
            }
            return null;
        }
    }
}