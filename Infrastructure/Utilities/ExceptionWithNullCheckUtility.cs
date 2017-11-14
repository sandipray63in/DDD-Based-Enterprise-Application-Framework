using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.ExceptionHandling;

namespace Infrastructure.Utilities
{
    public static class ExceptionWithNullCheckUtility
    {
        public static void HandleExceptionWithNullCheck(Action actionToExecute, Action onExceptionCompensatingHandler = null, IExceptionHandler exceptionHandler = null)
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

        public static TReturn HandleExceptionWithNullCheck<TReturn>(Func<TReturn> actionToExecute, Action onExceptionCompensatingHandler = null, IExceptionHandler exceptionHandler = null)
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

        public static async Task HandleExceptionWithNullCheck(Func<Task> actionToExecute, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, IExceptionHandler exceptionHandler = null)
        {
            if (exceptionHandler.IsNotNull())
            {
                await exceptionHandler.HandleExceptionAsync(actionToExecute, onExceptionCompensatingHandler);
            }
            else
            {
                await actionToExecute();
            }
        }

        public static async Task<TReturn> HandleExceptionWithNullCheck<TReturn>(Func<Task<TReturn>> actionToExecute, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, IExceptionHandler exceptionHandler = null)
        {
            if (exceptionHandler.IsNotNull())
            {
                return await exceptionHandler.HandleExceptionAsync(actionToExecute, onExceptionCompensatingHandler);
            }
            else
            {
                return await actionToExecute();
            }
        }
    }
}