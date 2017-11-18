using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.ExceptionHandling;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.Utilities
{
    public static class ExceptionHandlingUtility
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

        public static async Task HandleExceptionWithNullCheck(Func<CancellationToken,Task> actionToExecute, CancellationToken actionCancellationToken = default(CancellationToken), IExceptionHandler exceptionHandler = null, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            if (exceptionHandler.IsNotNull())
            {
                await exceptionHandler.HandleExceptionAsync(actionToExecute, actionCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            else
            {
                await actionToExecute(actionCancellationToken);
            }
        }

        public static async Task<TReturn> HandleExceptionWithNullCheck<TReturn>(Func<CancellationToken,Task<TReturn>> funcToExecute, CancellationToken funcCancellationToken = default(CancellationToken), IExceptionHandler exceptionHandler = null, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            if (exceptionHandler.IsNotNull())
            {
                return await exceptionHandler.HandleExceptionAsync(funcToExecute, funcCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            else
            {
                return await funcToExecute(funcCancellationToken);
            }
        }

        public static void WrapActionWithExceptionHandling(Action action, ILogger logger,bool shouldThrowOnException = true)
        {
            try
            {
                action();
            }
            catch(Exception ex)
            {
                logger.LogException(ex);
                if (shouldThrowOnException)
                {
                    throw;
                }
            }
        }

        public static TReturn WrapFuncWithExceptionHandling<TReturn>(Func<TReturn> func, ILogger logger, bool shouldThrowOnException = true)
        {
            TReturn returnValue = default(TReturn);
            try
            {
                returnValue =  func();
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                if (shouldThrowOnException)
                {
                    throw;
                }
            }
            return returnValue;
        }
    }
}