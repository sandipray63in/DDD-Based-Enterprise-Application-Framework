using System;
using Infrastructure.Logging.Loggers;
using Infrastructure.Logging;

namespace Infrastructure.ExceptionHandling.RetryBasedExceptionHandling
{
    public class BasicRetryBasedExceptionHandler : BaseRetryBasedExceptionHandler
    {
        private bool _shouldThrowOnException;
        private ILogger _logger;
        private int _maxNumberOfAllowedRetries;
        private readonly Func<DateTime, bool> _retryConditionFunc;

        public BasicRetryBasedExceptionHandler(ILogger logger, int maxNumberOfAllowedRetries, bool shouldThrowOnException, int timeOutInMilliSeconds = -1)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _maxNumberOfAllowedRetries = maxNumberOfAllowedRetries;
            _shouldThrowOnException = shouldThrowOnException;
            _retryConditionFunc = x => _maxNumberOfAllowedRetries > 0 && (timeOutInMilliSeconds < 0 || DateTime.Now.Subtract(x).Milliseconds < timeOutInMilliSeconds);
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override void HandleExceptionAfterAllRetryFailure(Action action, Action onExceptionCompensatingHandler = null)
        {
            DateTime methodEntryTime = DateTime.Now;
            bool isActionInvokedSuceessfullyWithoutAnyException = true;
            do
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    isActionInvokedSuceessfullyWithoutAnyException = false;
                    _maxNumberOfAllowedRetries--;
                    if (!_retryConditionFunc(methodEntryTime))
                    {
                        HandleExceptionCompensation(onExceptionCompensatingHandler, ex);
                        break;
                    }
                }
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
            DateTime methodEntryTime = DateTime.Now;
            bool isActionInvokedSuceessfullyWithoutAnyException = true;
            do
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    isActionInvokedSuceessfullyWithoutAnyException = false;
                    _maxNumberOfAllowedRetries--;
                    if (!_retryConditionFunc(methodEntryTime))
                    {
                        HandleExceptionCompensation(onExceptionCompensatingHandler, ex);
                        break;
                    }
                }
            }
            while (!isActionInvokedSuceessfullyWithoutAnyException && _retryConditionFunc(methodEntryTime));
            return default(TReturn);
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
