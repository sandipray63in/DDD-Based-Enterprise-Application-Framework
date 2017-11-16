using System;
using System.Collections;

namespace ApplicationAndInfrastructureServices.BatchProcessing
{
    public interface IBatchCommandProcesor : IDisposable
    {
        bool Execute(IEnumerable[] batchSelectorEnumerables);
    }
}
