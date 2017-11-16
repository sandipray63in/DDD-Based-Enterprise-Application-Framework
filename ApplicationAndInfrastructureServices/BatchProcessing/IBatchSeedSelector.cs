using System;
using System.Collections;

namespace ApplicationAndInfrastructureServices.BatchProcessing
{
    public interface IBatchSeedSelector : IEnumerator , IDisposable
    {
        void Execute();
    }
}
