using System;
using System.Collections;

namespace Repository.BatchProcessing
{
    public static class BatchExecutor 
    {
        public static bool ExecuteBatch(BatchConfiguration batchConfiguration)
        {
            Boolean result = false;
            IBatchSeedSelector batchSeedSelector = batchConfiguration.BatchSeedSelector;
            batchSeedSelector.Reset();
            do
            {
                if (batchSeedSelector.Current.IsNotNull())
                {
                    IEnumerable[] batchSelectorEnumerables = batchSeedSelector.Current as IEnumerable[];
                    /// Process the batchSelectorEnumerables
                    result = batchConfiguration.BatchCommandProcesor.Execute(batchSelectorEnumerables);
                }
            }
            while (batchSeedSelector.MoveNext());
            return result;
        }
    }
}
