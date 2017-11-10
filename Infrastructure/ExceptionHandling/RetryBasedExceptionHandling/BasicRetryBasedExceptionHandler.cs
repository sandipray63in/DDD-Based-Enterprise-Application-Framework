using System;
using System.Threading.Tasks;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;

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
                    isActionInvokedSuceessfullyWithoutAnyException = true;
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
                    isActionInvokedSuceessfullyWithoutAnyException = true;
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

        public override async Task HandleExceptionAfterAllRetryFailureAsync(Action action, Action onExceptionCompensatingHandler = null)
        {
            await Task.Run(() => HandleExceptionAfterAllRetryFailure(action,onExceptionCompensatingHandler));
        }

        public override async Task<TReturn> HandleExceptionAfterAllRetryFailureAsync<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null)
        {
            return await Task.Run<TReturn>(() => HandleExceptionAfterAllRetryFailure<TReturn>(action, onExceptionCompensatingHandler));
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
