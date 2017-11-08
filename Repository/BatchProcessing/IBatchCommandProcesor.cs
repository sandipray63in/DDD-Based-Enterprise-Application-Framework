using System;
using System.Collections;

namespace Repository.BatchProcessing
{
    public interface IBatchCommandProcesor : IDisposable
    {
        bool Execute(IEnumerable[] batchSelectorEnumerables);
    }
}
