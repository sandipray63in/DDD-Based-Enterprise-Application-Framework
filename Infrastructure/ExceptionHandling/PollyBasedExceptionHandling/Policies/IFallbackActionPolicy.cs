using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies
{
    internal interface IFallbackActionPolicy
    {
        void SetFallbackAction(Action fallbackAction);
        void SetFallbackAction(Func<CancellationToken, Task> fallbackAction);
    }
}
