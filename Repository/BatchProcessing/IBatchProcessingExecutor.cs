
using System;

namespace Repository.BatchProcessing
{
    public interface IBatchProcessingExecutor : IDisposable
    {
        bool ExecuteBatchProcess();
    }
}
