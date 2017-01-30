using System;

namespace Infrastructure.ExceptionHandling.RetryBasedExceptionHandling
{
    public interface IRetryBasedExceptionHandler : IDisposable
    {
        void HandleExceptionAfterAllRetryFailure(Action action, Action onExceptionCompensatingHandler = null);

        void HandleExceptionAfterAllRetryFailure<TParam>(Action<TParam> action, TParam param, Action onExceptionCompensatingHandler = null);

        void HandleExceptionAfterAllRetryFailure<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null);

        void HandleExceptionAfterAllRetryFailure<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null);

        TReturn HandleExceptionAfterAllRetryFailure<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null);

        TReturn HandleExceptionAfterAllRetryFailure<TParam, TReturn>(Func<TParam, TReturn> func, TParam param, Action onExceptionCompensatingHandler = null);

        TReturn HandleExceptionAfterAllRetryFailure<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> func, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null);

        TReturn HandleExceptionAfterAllRetryFailure<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, TReturn> func, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null);
    }
}
