using System;

namespace DomainServices.Base.BatchProcessing
{
    public interface IBatchProcessingExecutor : IDisposable
    {
        bool ExecuteBatchProcess();
    }
}
