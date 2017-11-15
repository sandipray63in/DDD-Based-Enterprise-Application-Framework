using System;
using System.Collections;

namespace DomainServices.Base.BatchProcessing
{
    public interface IBatchSeedSelector : IEnumerator , IDisposable
    {
        void Execute();
    }
}
