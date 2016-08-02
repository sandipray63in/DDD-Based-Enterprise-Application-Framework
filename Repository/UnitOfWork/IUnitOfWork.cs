using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Repository.Base;
using Infrastructure;

namespace Repository.UnitOfWork
{
    /// <summary>
    /// An Abstract Class for Unit of Work stuffs.Could have used an Interface 
    /// but an Interface doesn't allow "internal" methods.
    /// 
    /// For Async operations, if Unit of Work(explicit Transactions Management) is used then the 
    /// Cancellation token passed to the CommitAsync method(in UnitOfWork class) will be used 
    /// else Cancellation token passed to individual methods will be used.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IUnitOfWork : DisposableClass
    {
        internal abstract void RegisterQueryOperation(Action operation);
        internal abstract void RegisterInsert<TEntity>(TEntity item, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterUpdate<TEntity>(TEntity item, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterDelete<TEntity>(TEntity item, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterInsertList<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterUpdateList<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterDeleteList<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterBulkInsertList<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterBulkUpdateList<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterBulkDeleteList<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        public abstract void Commit(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true);

        #region Async Versions

        internal abstract void RegisterInsertForAsync<TEntity>(TEntity item, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterUpdateForAsync<TEntity>(TEntity item, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterDeleteForAsync<TEntity>(TEntity item, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterInsertListForAsync<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterUpdateListForAsync<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterDeleteListForAsync<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterBulkInsertListForAsync<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterBulkUpdateListForAsync<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        internal abstract void RegisterBulkDeleteListAsync<TEntity>(IList<TEntity> entityList, BaseUnitOfWorkRepository<TEntity> repository, Action operationToExecuteBeforeNextOperation = null) where TEntity : ICommandAggregateRoot;
        public abstract Task CommitAsync(CancellationToken token = default(CancellationToken),bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true);

        #endregion

        public abstract void Rollback(Exception commitException = null,bool shouldThrowOnException = true);
    }
}
