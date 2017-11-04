using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;

namespace Repository.Command
{
    public interface ICommand<TEntity> : IDisposable where TEntity : ICommandAggregateRoot
    {
        void Insert(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        void Insert(IEnumerable<TEntity> items);
        void Update(IEnumerable<TEntity> items);
        void Delete(IEnumerable<TEntity> items);
        void BulkInsert(IEnumerable<TEntity> items);
        void BulkUpdate(IEnumerable<TEntity> items);
        void BulkDelete(IEnumerable<TEntity> items);

        #region Async Versions

        Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken));
        Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken));
        Task InsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken));
        Task DeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken));
        Task BulkInsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken));
        Task BulkUpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken));
        Task BulkDeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken));

        #endregion

    }
}
