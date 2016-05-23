using System;

namespace FluentRepository
{
    internal class UnitOfWorkData
    {
        internal Func<dynamic> UnitOfWorkFunc { get; set; }

        internal bool ShouldAutomaticallyRollBackOnTransactionException { get; set; }

        internal bool ShouldThrowOnException { get; set; }
    }
}
