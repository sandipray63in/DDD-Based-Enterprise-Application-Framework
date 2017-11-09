using System;
using System.Collections;

namespace DomainServices.BatchProcessing
{
    public interface IBatchCommandProcesor : IDisposable
    {
        bool Execute(IEnumerable[] batchSelectorEnumerables);
    }
}
