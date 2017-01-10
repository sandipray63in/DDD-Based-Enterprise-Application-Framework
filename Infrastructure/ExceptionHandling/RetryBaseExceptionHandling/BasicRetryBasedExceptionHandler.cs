using System;
using Infrastructure.ExceptionHandling.SemanticLogging.CrossCuttingEventSources;

namespace Infrastructure.ExceptionHandling.RetryBaseExceptionHandling
{
    public class BasicRetryBasedExceptionHandler : BaseRetryBasedExceptionHandler
    {
        private int numberOfRetries = 0;
        private bool shouldThrowOnException;

        public BasicRetryBasedExceptionHandler()
        {
        }

        public BasicRetryBasedExceptionHandler(bool shouldThrowOnException)
        {
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
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                if (numberOfRetries < maxNumberOfRetriesAllowed)
                {
                    numberOfRetries++;
                    return HandleExceptionAfterAllRetryFailure(action, onExceptionCompensatingHandler, maxNumberOfRetriesAllowed);
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
            ExceptionLogEvents.Log.LogException(ex.ToString());
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
