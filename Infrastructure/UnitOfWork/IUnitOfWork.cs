using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    /// <summary>
    /// For Async operations, if Unit of Work(explicit Transactions Management) is used then the 
    /// Cancellation token passed to the CommitAsync method(in UnitOfWork class) will be used 
    /// else Cancellation token passed to individual methods will be used.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork
    {
        void AddOperation(Action operation);

        void AddOperation(Func<CancellationToken,Task> asyncOperation);

        void Commit(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true);

        Task CommitAsync(CancellationToken token = default(CancellationToken),bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true);

        void Rollback(Exception commitException = null,bool shouldThrowOnException = true);
    }
}
