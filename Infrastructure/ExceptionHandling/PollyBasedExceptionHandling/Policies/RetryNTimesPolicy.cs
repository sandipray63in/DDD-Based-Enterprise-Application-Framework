using System;
using Polly;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies
{
    public class RetryNTimesPolicy : IPolicy
    {
        private readonly ILogger _logger;
        private readonly int _retryCount;

        public RetryNTimesPolicy(ILogger logger, int retryCount)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _retryCount = retryCount;
        }

        public Policy GetPolicy(PolicyBuilder policyBuilder)
        {
            return policyBuilder.Retry(_retryCount, (x, y) =>_logger.LogException(new Exception(string.Format("GetPolicy in RetryNTimesPolicy : Tried {0} number of time(s)", y), x)));
        }

        public Policy GetPolicyAsync(PolicyBuilder policyBuilder)
        {
            return policyBuilder.RetryAsync(_retryCount, (x, y) => _logger.LogException(new Exception(string.Format("GetPolicyAsync in RetryNTimesPolicy : Tried {0} number of time(s)", y), x)));
        }
    }
}
