using System;
using System.Threading.Tasks;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;

namespace Infrastructure.ExceptionHandling.RetryBasedExceptionHandling
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/azure/architecture/patterns/retry
    /// </summary>
    public class BasicRetryBasedExceptionHandler : BaseRetryBasedExceptionHandler
    {
        private readonly bool _shouldThrowOnException;
        private readonly ILogger _logger;
        private int _maxNumberOfAllowedRetries;
        private readonly Func<DateTime, bool> _retryConditionFunc;
        private readonly int _retryDelayInMilliSeconds;
        private Func<Exception, bool> _isTransientFunc;

        public BasicRetryBasedExceptionHandler(ILogger logger, int maxNumberOfAllowedRetries, bool shouldThrowOnException, int timeOutInMilliSeconds = -1, int retryDelayInMilliSeconds = 0)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _maxNumberOfAllowedRetries = maxNumberOfAllowedRetries;
            _shouldThrowOnException = shouldThrowOnException;
            _retryConditionFunc = x => _maxNumberOfAllowedRetries > 0 && (timeOutInMilliSeconds < 0 || DateTime.Now.Subtract(x).Milliseconds < timeOutInMilliSeconds);
            _retryDelayInMilliSeconds = retryDelayInMilliSeconds;
        }

        public override void SetIsTransientFunc(Func<Exception,bool> isTransientFunc)
        {
            _isTransientFunc = isTransientFunc;
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override void HandleExceptionAfterAllRetryFailure(Action action, Action onExceptionCompensatingHandler = null)
        {
            ContractUtility.Requires<ArgumentException>(_isTransientFunc.IsNull(), "the isTransientFunc needs to set explicitly and cannot be null");
            DateTime methodEntryTime = DateTime.Now;
            bool isActionInvokedSuceessfullyWithoutAnyException = true;
            do
            {
                try
                {
                    isActionInvokedSuceessfullyWithoutAnyException = true;
                    action();
                }
                catch (Exception ex)
                {
                    isActionInvokedSuceessfullyWithoutAnyException = false;
                    _maxNumberOfAllowedRetries--;
                    if (!_isTransientFunc(ex))
                    {
                        if (!_retryConditionFunc(methodEntryTime))
                        {
                            HandleExceptionCompensation(onExceptionCompensatingHandler, ex);
                            break;
                        }
                    }
                    else
                    {
                        //TODO - Handle Circuit breaker based Exception
                    }
                }
                Task.Delay(_retryDelayInMilliSeconds);
            }
            while (!isActionInvokedSuceessfullyWithoutAnyException && _retryConditionFunc(methodEntryTime));
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override TReturn HandleExceptionAfterAllRetryFailure<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null)
        {
            ContractUtility.Requires<ArgumentException>(_isTransientFunc.IsNull(), "the isTransientFunc needs to set explicitly and cannot be null");
            DateTime methodEntryTime = DateTime.Now;
            bool isActionInvokedSuceessfullyWithoutAnyException = true;
            do
            {
                try
                {
                    isActionInvokedSuceessfullyWithoutAnyException = true;
                    return action();
                }
                catch (Exception ex)
                {
                    isActionInvokedSuceessfullyWithoutAnyException = false;
                    _maxNumberOfAllowedRetries--;
                    if (!_isTransientFunc(ex))
                    {
                        if (!_retryConditionFunc(methodEntryTime))
                        {
                            HandleExceptionCompensation(onExceptionCompensatingHandler, ex);
                            break;
                        }
                    }
                    else
                    {
                        //TODO - Handle Circuit breaker based Exception
                    }
                }
                Task.Delay(_retryDelayInMilliSeconds);
            }
            while (!isActionInvokedSuceessfullyWithoutAnyException && _retryConditionFunc(methodEntryTime));
            return default(TReturn);
        }

        public override async Task HandleExceptionAfterAllRetryFailureAsync(Func<Task> action, Action onExceptionCompensatingHandler = null)
        {
            await HandleExceptionAfterAllRetryFailure(action, onExceptionCompensatingHandler);
        }

        public override async Task<TReturn> HandleExceptionAfterAllRetryFailureAsync<TReturn>(Func<Task<TReturn>> action, Action onExceptionCompensatingHandler = null)
        {
            return await HandleExceptionAfterAllRetryFailure<Task<TReturn>>(action, onExceptionCompensatingHandler);
        }

        private void HandleExceptionCompensation(Action onExceptionCompensatingHandler, Exception ex)
        {
            _logger.LogException(ex);
            if (onExceptionCompensatingHandler != null)
            {
                onExceptionCompensatingHandler();
            }
            if (_shouldThrowOnException)
            {
                throw new Exception("Check Inner Exception", ex);
            }
        }
    }
}
