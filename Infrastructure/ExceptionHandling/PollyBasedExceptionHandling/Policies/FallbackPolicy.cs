using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies
{
    public class FallbackPolicy : IPolicy, IFallbackActionPolicy
    {
        private readonly ILogger _logger;
        private Action _fallbackAction;
        private Func<CancellationToken, Task> _fallbackActionAsync;

        public FallbackPolicy(ILogger logger)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
        }

        public void SetFallbackAction(Action fallbackAction)
        {
            _fallbackAction = fallbackAction;
        }

        public void SetFallbackAction(Func<CancellationToken, Task> fallbackAction)
        {
            _fallbackActionAsync = fallbackAction;
        }

        public Policy GetPolicy(PolicyBuilder policyBuilder)
        {
            return policyBuilder.Fallback(_fallbackAction, x => _logger.LogException(x));
        }

        public Policy GetPolicyAsync(PolicyBuilder policyBuilder)
        {
            return policyBuilder.FallbackAsync(_fallbackActionAsync, x => Task.Run(()=>_logger.LogException(x)));
        }
    }
}

