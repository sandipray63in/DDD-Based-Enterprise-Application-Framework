using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ExceptionHandling
{
    public abstract class BaseExceptionHandler : DisposableClass, IExceptionHandler
    {
        public abstract void HandleException(Action action, Action onExceptionCompensatingHandler = null);

        public virtual void HandleException<TParam>(Action<TParam> action, TParam param, Action onExceptionCompensatingHandler = null)
        {
            HandleException(() => action(param), onExceptionCompensatingHandler);
        }

        public virtual void HandleException<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null)
        {
            HandleException(() => action(param1, param2), onExceptionCompensatingHandler);
        }

        public virtual void HandleException<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null)
        {
            HandleException(() => action(param1, param2, param3), onExceptionCompensatingHandler);
        }

        public abstract TReturn HandleException<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null);

        public virtual TReturn HandleException<TParam, TReturn>(Func<TParam, TReturn> func, TParam param, Action onExceptionCompensatingHandler = null)
        {
            return HandleException(() => func(param), onExceptionCompensatingHandler);
        }

        public virtual TReturn HandleException<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> func, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null)
        {
            return HandleException(() => func(param1, param2), onExceptionCompensatingHandler);
        }

        public virtual TReturn HandleException<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, TReturn> func, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null)
        {
            return HandleException(() => func(param1, param2, param3), onExceptionCompensatingHandler);
        }

        public abstract Task HandleExceptionAsync(Func<CancellationToken, Task> action, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        public virtual async Task HandleExceptionAsync<TParam>(Func<TParam, CancellationToken, Task> action, TParam param, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            await HandleExceptionAsync(x => action(param,x), actionCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
        }

        public virtual async Task HandleExceptionAsync<TParam1, TParam2>(Func<TParam1, TParam2, CancellationToken, Task> action, TParam1 param1, TParam2 param2, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            await HandleExceptionAsync(x => action(param1,param2, x), actionCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
        }

        public virtual async Task HandleExceptionAsync<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, CancellationToken, Task> action, TParam1 param1, TParam2 param2, TParam3 param3, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            await HandleExceptionAsync(x => action(param1, param2,param3, x), actionCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
        }

        public abstract Task<TReturn> HandleExceptionAsync<TReturn>(Func<CancellationToken, Task<TReturn>> action, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        public virtual async Task<TReturn> HandleExceptionAsync<TParam, TReturn>(Func<TParam, CancellationToken, Task<TReturn>> func, TParam param, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            return await HandleExceptionAsync(x => func(param, x), funcCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
        }

        public virtual async Task<TReturn> HandleExceptionAfterAsync<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, CancellationToken, Task<TReturn>> func, TParam1 param1, TParam2 param2, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            return await HandleExceptionAsync(x => func(param1,param2, x), funcCancellationToken, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
        }

        public virtual async Task<TReturn> HandleExceptionAsync<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3,CancellationToken, Task<TReturn>> func, TParam1 param1, TParam2 param2, TParam3 param3, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            return await HandleExceptionAsync(x => func(param1, param2,param3, x), funcCancellationToken, onExceptionCompensatingHandler,onExceptionCompensatingHandlerCancellationToken);
        }
    }
}
