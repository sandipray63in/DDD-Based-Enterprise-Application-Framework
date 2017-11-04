using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;

namespace DomainServices.Base.CommandDomainServices
{
    public interface ICommandDomainServiceAsync<TEntity> : IDisposable where TEntity : ICommandAggregateRoot
    {
        Task<bool> InsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> InsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> DeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> BulkInsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> BulkUpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        Task<bool> BulkDeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
    }
}
