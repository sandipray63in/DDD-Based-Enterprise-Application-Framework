using System;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;

namespace Infrastructure.ExceptionHandling
{
    public class DefaultExceptionHandler : BaseExceptionHandler
    {
        private bool _shouldThrowOnException;
        private ILogger _logger;
        private int _maxNumberOfAllowedRetries;
        private readonly Func<DateTime, bool> _retryConditionFunc;
        private readonly TransientFailureExceptions _transientFailureExceptions;

        public DefaultExceptionHandler(ILogger logger, int maxNumberOfAllowedRetries, bool shouldThrowOnException, int timeOutInMilliSeconds = -1)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _maxNumberOfAllowedRetries = maxNumberOfAllowedRetries;
            _shouldThrowOnException = shouldThrowOnException;
            _retryConditionFunc = x => _maxNumberOfAllowedRetries > 0 && (timeOutInMilliSeconds < 0 || DateTime.Now.Subtract(x).Milliseconds < timeOutInMilliSeconds);
            XDocument xDoc = XDocument.Load(Path.Combine(typeof(DefaultExceptionHandler).Assembly.Location, "TransientFailureExceptions.xml"));
            _transientFailureExceptions = XMLUtility.DeSerialize<TransientFailureExceptions>(xDoc.ToString());
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override void HandleException(Action action, Action onExceptionCompensatingHandler = null)
        {
            DateTime methodEntryTime = DateTime.Now;
            bool isActionToBeRetried = false;
            AggregateException aggregateException = null;
            do
            {
                try
                {
                    isActionToBeRetried = false;
                    action();
                }
                catch (Exception ex)
                {
                    if(aggregateException.IsNull())
                    {
                        aggregateException = new AggregateException();
                    }
                    aggregateException.InnerExceptions.AddIfNotContains(ex);
                    if (_transientFailureExceptions.CommaSeperatedNormalTransientFailures.Contains(ex.GetType().Name) && _retryConditionFunc(methodEntryTime))
                    {
                        isActionToBeRetried = true;
                        _maxNumberOfAllowedRetries--;
                    }
                    else 
                    {
                        isActionToBeRetried = false;
                        if (_transientFailureExceptions.CommaSeperatedTimeConsumingTransientFailures.Contains(ex.GetType().Name))
                        {
                            //TODO - Handle using Circuit breaker approach
                        }
                    }
                }
            }
            while (isActionToBeRetried && _retryConditionFunc(methodEntryTime));
            HandleExceptionCompensation(onExceptionCompensatingHandler, aggregateException);
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override TReturn HandleException<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null)
        {
            DateTime methodEntryTime = DateTime.Now;
            bool isActionToBeRetried = false;
            AggregateException aggregateException = null;
            do
            {
                try
                {
                    isActionToBeRetried = false;
                    return action();
                }
                catch (Exception ex)
                {
                    if (aggregateException.IsNull())
                    {
                        aggregateException = new AggregateException();
                    }
                    aggregateException.InnerExceptions.AddIfNotContains(ex);
                    if (_transientFailureExceptions.CommaSeperatedNormalTransientFailures.Contains(ex.GetType().Name) && _retryConditionFunc(methodEntryTime))
                    {
                        isActionToBeRetried = true;
                        _maxNumberOfAllowedRetries--;
                    }
                    else
                    {
                        isActionToBeRetried = false;
                        if (_transientFailureExceptions.CommaSeperatedTimeConsumingTransientFailures.Contains(ex.GetType().Name))
                        {
                            //TODO - Handle using Circuit breaker approach
                        }
                    }
                }
            }
            while (isActionToBeRetried && _retryConditionFunc(methodEntryTime));
            HandleExceptionCompensation(onExceptionCompensatingHandler, aggregateException);
            return default(TReturn);
        }

        public override async Task HandleExceptionAsync(Func<Task> action, Action onExceptionCompensatingHandler = null)
        {
            await HandleException(action,onExceptionCompensatingHandler);
        }

        public override async Task<TReturn> HandleExceptionAsync<TReturn>(Func<Task<TReturn>> action, Action onExceptionCompensatingHandler = null)
        {
            return await HandleException<Task<TReturn>>(action, onExceptionCompensatingHandler);
        }

        private void HandleExceptionCompensation(Action onExceptionCompensatingHandler, Exception ex)
        {
            if (ex.IsNotNull())
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
}
