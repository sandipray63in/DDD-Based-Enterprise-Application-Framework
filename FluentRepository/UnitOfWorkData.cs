
namespace FluentRepository
{
    internal class UnitOfWorkData
    {
        internal dynamic UnitOfWork { get; set; }

        internal bool ShouldAutomaticallyRollBackOnTransactionException { get; set; }

        internal bool ShouldThrowOnException { get; set; }
    }
}
