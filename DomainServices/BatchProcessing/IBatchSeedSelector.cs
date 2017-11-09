using System;
using System.Collections;

namespace DomainServices.BatchProcessing
{
    public interface IBatchSeedSelector : IEnumerator , IDisposable
    {
        void Execute();
    }
}
