using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.ExceptionHandling;

namespace Infrastructure.Utilities
{
    public static class ExceptionWithNullCheckUtility
    {
        public static void HandleExceptionWithNullCheck(Action actionToExecute,IExceptionHandler exceptionHandler = null, Action onExceptionCompensatingHandler = null)
        {
            if (exceptionHandler.IsNotNull())
            {
                exceptionHandler.HandleException(actionToExecute, onExceptionCompensatingHandler);
            }
            else
            {
                actionToExecute();
            }
        }

        public static TReturn HandleExceptionWithNullCheck<TReturn>(Func<TReturn> actionToExecute, IExceptionHandler exceptionHandler = null, Action onExceptionCompensatingHandler = null)
        {
            if (exceptionHandler.IsNotNull())
            {
                return exceptionHandler.HandleException(actionToExecute, onExceptionCompensatingHandler);
            }
            else
            {
                return actionToExecute();
            }
        }

        public static async Task HandleExceptionWithNullCheck(Func<Task> actionToExecute, IExceptionHandler exceptionHandler = null, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            if (exceptionHandler.IsNotNull())
            {
                await exceptionHandler.HandleExceptionAsync(actionToExecute, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            else
            {
                await actionToExecute();
            }
        }

        public static async Task<TReturn> HandleExceptionWithNullCheck<TReturn>(Func<Task<TReturn>> actionToExecute, IExceptionHandler exceptionHandler = null, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            if (exceptionHandler.IsNotNull())
            {
                return await exceptionHandler.HandleExceptionAsync(actionToExecute, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            else
            {
                return await actionToExecute();
            }
        }
    }
}