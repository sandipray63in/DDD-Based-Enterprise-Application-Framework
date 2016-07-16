using System;
using System.Collections.Generic;
using System.Threading;
using Domain.Base.Aggregates;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentCommands : IFluentSetUpAndExecution
    {
        IFluentCommands Insert<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Update<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Delete<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Insert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Update<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Delete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands BulkInsert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands BulkUpdate<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands BulkDelete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        #region Async Versions

        IFluentCommands InsertAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands UpdateAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands DeleteAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands InsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands UpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands DeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands BulkInsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands BulkUpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands BulkDeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        #endregion
    }
}
