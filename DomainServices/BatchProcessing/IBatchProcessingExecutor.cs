
using System;

namespace DomainServices.BatchProcessing
{
    public interface IBatchProcessingExecutor : IDisposable
    {
        bool ExecuteBatchProcess();
    }
}
