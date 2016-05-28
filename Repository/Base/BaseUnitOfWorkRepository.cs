using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Infrastructure;

namespace Repository.Base
{
    /// <summary>
    /// An Abstract Class for Repository related Unit of Work stuffs.Could have used an Interface 
    /// but an Interface doesn't allow "internal" methods.
    /// Also cannot declare this class as "internal" because then BaseUnitOfWork will have more
    /// accessibility compared to BaseUnitOfWorkRepository and BaseUnitOfWork internally
    /// uses BaseUnitOfWorkRepository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseUnitOfWorkRepository<TEntity> : DisposableClass where TEntity : ICommandAggregateRoot
    {
        internal abstract void ActualInsert(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualUpdate(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualDelete(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualInsert(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualUpdate(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualDelete(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualBulkInsert(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualBulkUpdate(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null);
        internal abstract void ActualBulkDelete(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null);

        #region Async Versions
        internal abstract Task ActualInsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualUpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualDeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualInsertAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualUpdateAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualDeleteAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualBulkInsertAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualBulkUpdateAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        internal abstract Task ActualBulkDeleteAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        #endregion
    }
}