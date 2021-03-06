﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;
using Infrastructure.Extensions;

namespace RestfulWebAPI.Handlers.ExceptionHandling
{
    public class ExceptionHandler : DelegatingHandler
    {
        private const string GENERIC_EXCEPTION_MESSAGE = "An internal server error occurred.Please try after sometime or contact the administrator";
        private readonly ILogger _logger;

        public ExceptionHandler(ILogger logger)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage errResponse = null;
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch(HttpResponseException ex)
            {
                await _logger.LogExceptionAsync(ex);
                errResponse = request.CreateErrorResponse(ex.Response.StatusCode, ex.Message);
                return await GetErrorResponseTask(errResponse);
            }
            catch(Exception ex)
            {
                await _logger.LogExceptionAsync(ex);
                errResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, GENERIC_EXCEPTION_MESSAGE);
                return await GetErrorResponseTask(errResponse);
            }
        }

        private async Task<HttpResponseMessage> GetErrorResponseTask(HttpResponseMessage errResponse)
        {
            // Note: TaskCompletionSource creates a task that does not contain a delegate.
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(errResponse);   // Also sets the task state to "RanToCompletion"
            return await tsc.Task;
        }
    }
}   