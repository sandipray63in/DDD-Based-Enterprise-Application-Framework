using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ExceptionHandling
{
    public interface IExceptionHandler 
    {
        void HandleException(Action action, Action onExceptionCompensatingHandler = null);

        void HandleException<TParam>(Action<TParam> action, TParam param, Action onExceptionCompensatingHandler = null);

        void HandleException<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null);

        void HandleException<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null);

        TReturn HandleException<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null);

        TReturn HandleException<TParam, TReturn>(Func<TParam, TReturn> func, TParam param, Action onExceptionCompensatingHandler = null);

        TReturn HandleException<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> func, TParam1 param1, TParam2 param2, Action onExceptionCompensatingHandler = null);

        TReturn HandleException<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, TReturn> func, TParam1 param1, TParam2 param2, TParam3 param3, Action onExceptionCompensatingHandler = null);

        Task HandleExceptionAsync(Func<CancellationToken, Task> action, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null,CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task HandleExceptionAsync<TParam>(Func<TParam, CancellationToken, Task> action, TParam param, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task HandleExceptionAsync<TParam1, TParam2>(Func<TParam1, TParam2, CancellationToken, Task> action, TParam1 param1, TParam2 param2, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task HandleExceptionAsync<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, CancellationToken, Task> action, TParam1 param1, TParam2 param2, TParam3 param3, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task<TReturn> HandleExceptionAsync<TReturn>(Func<CancellationToken, Task<TReturn>> action, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task<TReturn> HandleExceptionAsync<TParam, TReturn>(Func<TParam, CancellationToken, Task<TReturn>> func, TParam param, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task<TReturn> HandleExceptionAfterAsync<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, CancellationToken, Task<TReturn>> func, TParam1 param1, TParam2 param2, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));

        Task<TReturn> HandleExceptionAsync<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, CancellationToken, Task<TReturn>> func, TParam1 param1, TParam2 param2, TParam3 param3, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken));
    }
}
