using System;
using System.Collections;

namespace Repository.BatchProcessing
{
    public interface IBatchSeedSelector : IEnumerator , IDisposable
    {
        void Execute();
    }
}
