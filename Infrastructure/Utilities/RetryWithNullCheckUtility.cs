using System;
using System.Threading.Tasks;
using Infrastructure.ExceptionHandling.RetryBasedExceptionHandling;

namespace Infrastructure.Utilities
{
    public static class RetryWithNullCheckUtility
    {
        public static void FireRetryWithNullCheck(Action actionToExecute, Action onExceptionCompensatingHandler = null, IRetryBasedExceptionHandler _retryBasedExceptionHandler = null)
        {
            if(_retryBasedExceptionHandler.IsNotNull())
            {
                _retryBasedExceptionHandler.HandleExceptionAfterAllRetryFailure(actionToExecute, onExceptionCompensatingHandler);
            }
            else
            {
                actionToExecute();
            }
        }

        public static TReturn FireRetryWithNullCheck<TReturn>(Func<TReturn> actionToExecute, Action onExceptionCompensatingHandler = null, IRetryBasedExceptionHandler _retryBasedExceptionHandler = null)
        {
            if (_retryBasedExceptionHandler.IsNotNull())
            {
                return _retryBasedExceptionHandler.HandleExceptionAfterAllRetryFailure(actionToExecute, onExceptionCompensatingHandler);
            }
            else
            {
               return actionToExecute();
            }
        }

        public static async Task FireRetryWithNullCheckAsync(Func<Task> actionToExecute, Action onExceptionCompensatingHandler = null, IRetryBasedExceptionHandler _retryBasedExceptionHandler = null)
        {
            if (_retryBasedExceptionHandler.IsNotNull())
            {
                await _retryBasedExceptionHandler.HandleExceptionAfterAllRetryFailureAsync(actionToExecute, onExceptionCompensatingHandler);
            }
        }
    }
}
