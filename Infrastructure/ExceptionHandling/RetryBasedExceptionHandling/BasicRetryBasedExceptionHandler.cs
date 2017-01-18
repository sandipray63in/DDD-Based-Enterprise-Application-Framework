using System;
using Infrastructure.Extensions;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;

namespace Infrastructure.ExceptionHandling.RetryBasedExceptionHandling
{
    public class BasicRetryBasedExceptionHandler : BaseRetryBasedExceptionHandler
    {
        private int numberOfRetries = 0;
        private bool shouldThrowOnException;

        private readonly ILogger logger;

        public BasicRetryBasedExceptionHandler(ILogger logger,bool shouldThrowOnException = false)
        {
            ContractUtility.Requires<ArgumentNullException>(logger.IsNotNull(), "logger instance cannot be null");
            this.logger = logger;
            this.shouldThrowOnException = shouldThrowOnException;
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override void HandleExceptionAfterAllRetryFailure(Action action, Action onExceptionCompensatingHandler = null, int maxNumberOfRetriesAllowed = 0)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (numberOfRetries < maxNumberOfRetriesAllowed)
                {
                    numberOfRetries++;
                    HandleExceptionAfterAllRetryFailure(action,onExceptionCompensatingHandler,maxNumberOfRetriesAllowed);
                }
                else
                {
                    HandleExceptionCompensation(onExceptionCompensatingHandler, ex);
                }
            }
        }

        /// <summary>
        /// Retries for the maxNumberOfRetriesAllowed value and if still some exception occurs
        /// then handles it
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxNumberOfRetriesAllowed"></param>
        public override TReturn HandleExceptionAfterAllRetryFailure<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null, int maxNumberOfRetriesAllowed = 0)
        {
            Func<TReturn> retryBasedExceptionHandler = () => HandleExceptionAfterAllRetryFailureForMemoize(action, onExceptionCompensatingHandler, maxNumberOfRetriesAllowed);
            return retryBasedExceptionHandler.Memoize()();
        }

        private TReturn HandleExceptionAfterAllRetryFailureForMemoize<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null, int maxNumberOfRetriesAllowed = 0)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                if (numberOfRetries < maxNumberOfRetriesAllowed)
                {
                    numberOfRetries++;
                    return HandleExceptionAfterAllRetryFailureForMemoize(action, onExceptionCompensatingHandler, maxNumberOfRetriesAllowed);
                }
                else
                {
                    HandleExceptionCompensation(onExceptionCompensatingHandler, ex);
                    return default(TReturn);
                }
            }
        }

        private void HandleExceptionCompensation(Action onExceptionCompensatingHandler,Exception ex)
        {
            logger.LogException(ex);
            if (onExceptionCompensatingHandler.IsNotNull())
            {
                onExceptionCompensatingHandler();
            }
            if(shouldThrowOnException)
            {
                throw new Exception("Check Inner Exception",ex);
            }
        }
    }
}
