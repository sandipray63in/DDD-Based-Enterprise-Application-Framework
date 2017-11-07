using System.Collections;

namespace Repository.BatchProcessing
{
    public interface IBatchCommandProcesor
    {
        bool Execute(IEnumerable[] batchSelectorEnumerables);
    }
}
