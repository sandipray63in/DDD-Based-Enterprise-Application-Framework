using System;
using System.Collections;

namespace DomainServices.Base.BatchProcessing
{
    public interface IBatchCommandProcesor : IDisposable
    {
        bool Execute(IEnumerable[] batchSelectorEnumerables);
    }
}
