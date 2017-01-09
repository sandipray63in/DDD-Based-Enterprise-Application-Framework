using System;
using Infrastructure.ExceptionHandling.SemanticLogging.CrossCuttingEventSources;

namespace Infrastructure.ExceptionHandling.RetryBaseExceptionHandling
{
    public class BasicRetryBasedExceptionHandler : BaseRetryBasedExceptionHandler
    {
        private int numberOfRetries = 0;

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
                    HandleExceptionAfterAllRetryFailure(action);
                }
                else
                {
                    HandleExceptionCompensatingHandler(onExceptionCompensatingHandler, ex);
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
                    return HandleExceptionAfterAllRetryFailure(action);
                }
                else
                {
                    HandleExceptionCompensatingHandler(onExceptionCompensatingHandler, ex);
                    return default(TReturn);
                }
            }
        }

        private void HandleExceptionCompensatingHandler(Action onExceptionCompensatingHandler,Exception ex)
        {
            if (onExceptionCompensatingHandler == null)
            {
                onExceptionCompensatingHandler = () => ExceptionLogEvents.Log.LogException(ex.ToString());
            }
            onExceptionCompensatingHandler();
        }
    }
}
