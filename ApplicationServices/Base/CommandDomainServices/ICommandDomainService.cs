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
        bool Insert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool Update(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool Delete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool BulkInsert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool BulkUpdate(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
        bool BulkDelete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);
    }
}
