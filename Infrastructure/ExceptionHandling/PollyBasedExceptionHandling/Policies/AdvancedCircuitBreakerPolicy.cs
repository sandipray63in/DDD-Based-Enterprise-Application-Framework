using System;
using Polly;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies
{
    class AdvancedCircuitBreakerPolicy : IPolicy
    {
        private readonly ILogger _logger;
        private readonly double _failureThreshold;
        private readonly int _samplingDuration;
        private readonly int _minimumThroughput;
        private readonly int _durationOfBreak;

        public AdvancedCircuitBreakerPolicy(ILogger logger, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _failureThreshold = failureThreshold;
            _samplingDuration = samplingDuration;
            _minimumThroughput = minimumThroughput;
            _durationOfBreak = durationOfBreak;
        }

        public Policy GetPolicy(PolicyBuilder policyBuilder)
        {
            return policyBuilder.AdvancedCircuitBreaker(_failureThreshold, TimeSpan.FromMilliseconds(_samplingDuration), _minimumThroughput,TimeSpan.FromMilliseconds(_durationOfBreak),
                    onBreak: (x, y) => _logger.LogException(new Exception("Advanced Breaker logging: Breaking the circuit for " + y.TotalMilliseconds + " ms!", x)),
                    onReset: () => _logger.LogMessage("Advanced Breaker logging: Call ok! Closed the circuit again!"),
                    onHalfOpen: () => _logger.LogMessage("Advanced Breaker logging: Half-open: Next call is a trial!"));
        }

        public Policy GetPolicyAsync(PolicyBuilder policyBuilder)
        {
            return policyBuilder.AdvancedCircuitBreakerAsync(_failureThreshold, TimeSpan.FromMilliseconds(_samplingDuration), _minimumThroughput, TimeSpan.FromMilliseconds(_durationOfBreak),
                    onBreak: (x, y) => _logger.LogException(new Exception("Advanced Breaker logging: Breaking the circuit for " + y.TotalMilliseconds + " ms!", x)),
                    onReset: () => _logger.LogMessage("Advanced Breaker logging: Call ok! Closed the circuit again!"),
                    onHalfOpen: () => _logger.LogMessage("Advanced Breaker logging: Half-open: Next call is a trial!"));
        }
    }
}


