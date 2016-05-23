using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RestfulWebAPI.Handlers.ContentNegotiation
{
    public class CultureHandler : DelegatingHandler
    {
        /// <summary>
        /// Ideally in a prod environment this code should be re-factored to fetch the allowed languages from the DB
        /// (some master data table) using some Repository.
        /// </summary>
        private ISet<string> supportedCultures = new HashSet<string>() { "en-us", "en", "fr-fr", "fr" };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            var acceptedLanguages = request.Headers.AcceptLanguage;
            if (acceptedLanguages.IsNotNullOrEmpty())
            {
                var headerValue = acceptedLanguages.OrderByDescending(e => e.Quality ?? 1.0D)
                                      .Where(e => !e.Quality.HasValue || e.Quality.Value > 0.0D)
                                      .FirstOrDefault(e => supportedCultures.Contains(e.Value, StringComparer.OrdinalIgnoreCase));

                string culture = null;
                // Case 1: We can support what client has asked for
                if (headerValue.IsNotNull())
                {
                    culture = headerValue.Value;
                }
                // Case 2: Client is okay to accept any thing we support except
                // the ones explicitly specified as not preferred by setting q=0
                else if (acceptedLanguages.Any(e => e.Value == "*" && (!e.Quality.HasValue || e.Quality.Value > 0.0D)))
                {
                    culture = supportedCultures.Where(sc =>
                                            !acceptedLanguages.Any(e =>
                                                    e.Value.Equals(sc, StringComparison.OrdinalIgnoreCase) &&
                                                        e.Quality.HasValue &&
                                                            e.Quality.Value == 0.0D))
                                                                .FirstOrDefault();
                }
                
                if (culture.IsNotNull())
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}