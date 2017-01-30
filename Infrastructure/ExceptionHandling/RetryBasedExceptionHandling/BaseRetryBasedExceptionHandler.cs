using System;

namespace Infrastructure.ExceptionHandling.RetryBasedExceptionHandling
{
    public abstract class BaseRetryBasedExceptionHandler : DisposableClass, IRetryBasedExceptionHandler
    {
        public abstract void HandleExceptionAfterAllRetryFailure(Action action, Action onExceptionCompensatingHandler = null);

        public virtual void HandleExceptionAfterAllRetryFailure<TParam>(Action<TParam> action, TParam param, Action onExceptionCompensatingHandler = null)
        {
            HandleExceptionAfterAllRetryFailure(() => action(param), onExceptionCompensatingHandler);
        }

        public virtual void HandleExceptionAfterAllRetryFailure<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null)
        {
            HandleExceptionAfterAllRetryFailure(() => action(param1, param2), onExceptionCompensatingHandler);
        }

        public virtual void HandleExceptionAfterAllRetryFailure<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null)
        {
            HandleExceptionAfterAllRetryFailure(() => action(param1, param2, param3), onExceptionCompensatingHandler);
        }

        public abstract TReturn HandleExceptionAfterAllRetryFailure<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null);

        public virtual TReturn HandleExceptionAfterAllRetryFailure<TParam, TReturn>(Func<TParam, TReturn> func, TParam param, Action onExceptionCompensatingHandler = null)
        {
            return HandleExceptionAfterAllRetryFailure(() => func(param), onExceptionCompensatingHandler);
        }

        public virtual TReturn HandleExceptionAfterAllRetryFailure<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> func, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null)
        {
            return HandleExceptionAfterAllRetryFailure(() => func(param1, param2), onExceptionCompensatingHandler);
        }

        public virtual TReturn HandleExceptionAfterAllRetryFailure<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, TReturn> func, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null)
        {
            return HandleExceptionAfterAllRetryFailure(() => func(param1, param2, param3), onExceptionCompensatingHandler);
        }
    }
}
