using System;
using System.Collections.Generic;
using Domain.Base.Aggregates;

namespace DomainServices.Base.CommandDomainServices
{
    public interface ICommandDomainService<TEntity> : IDisposable where TEntity : ICommandAggregateRoot
    {
        bool Insert(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        bool Update(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        bool Delete(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        bool Insert(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool Update(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool Delete(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool BulkInsert(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool BulkUpdate(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool BulkDelete(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
    }
}
