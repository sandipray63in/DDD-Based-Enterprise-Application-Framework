using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    internal class OperationData
    {
       internal Action Operation { get; set; }

       internal Func<CancellationToken,Task> AsyncOperation { get; set; }
    }
}
