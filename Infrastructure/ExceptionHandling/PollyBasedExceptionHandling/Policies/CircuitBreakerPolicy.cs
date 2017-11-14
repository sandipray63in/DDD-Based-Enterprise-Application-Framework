using System;
using Polly;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies
{
    public class CircuitBreakerPolicy : IPolicy
    {
        private readonly ILogger _logger;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly int _durationOfBreakInMilliseconds;

        public CircuitBreakerPolicy(ILogger logger, int exceptionsAllowedBeforeBreaking, int durationOfBreakInMilliseconds)
        {
            _logger = logger;
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _durationOfBreakInMilliseconds = durationOfBreakInMilliseconds;
        }

        public Policy GetPolicy(PolicyBuilder policyBuilder)
        {
            return policyBuilder.CircuitBreaker(_exceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(_durationOfBreakInMilliseconds),
                    onBreak:(x,y) => _logger.LogException(new Exception("Breaker logging: Breaking the circuit for " + y.TotalMilliseconds + " ms!", x)),
                    onReset:() => _logger.LogMessage("Breaker logging: Call ok! Closed the circuit again!"),
                    onHalfOpen: () => _logger.LogMessage("Breaker logging: Half-open: Next call is a trial!"));
        }

        public Policy GetPolicyAsync(PolicyBuilder policyBuilder)
        {
            return policyBuilder.CircuitBreakerAsync(_exceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(_durationOfBreakInMilliseconds),
                    onBreak: (x, y) => _logger.LogException(new Exception("Breaker logging: Breaking the circuit for " + y.TotalMilliseconds + " ms!", x)),
                    onReset: () => _logger.LogMessage("Breaker logging: Call ok! Closed the circuit again!"),
                    onHalfOpen: () => _logger.LogMessage("Breaker logging: Half-open: Next call is a trial!"));
        }
    }
}


