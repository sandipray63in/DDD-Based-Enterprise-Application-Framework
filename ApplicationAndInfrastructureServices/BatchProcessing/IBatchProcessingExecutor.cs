using System;

namespace ApplicationAndInfrastructureServices.BatchProcessing
{
    public interface IBatchProcessingExecutor : IDisposable
    {
        bool ExecuteBatchProcess();
    }
}
