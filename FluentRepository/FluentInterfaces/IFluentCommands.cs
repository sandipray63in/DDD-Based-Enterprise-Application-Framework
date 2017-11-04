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

        IFluentCommands Insert<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Update<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands Delete<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands BulkInsert<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands BulkUpdate<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        IFluentCommands BulkDelete<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        #region Async Versions

        IFluentCommands InsertAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands UpdateAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands DeleteAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands InsertAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands UpdateAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands DeleteAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands BulkInsertAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands BulkUpdateAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;
        IFluentCommands BulkDeleteAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot;

        #endregion
    }
}
