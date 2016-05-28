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
        void Insert(IList<TEntity> items);
        void Update(IList<TEntity> items);
        void Delete(IList<TEntity> items);
        void BulkInsert(IList<TEntity> items);
        void BulkUpdate(IList<TEntity> items);
        void BulkDelete(IList<TEntity> items);

        #region Async Versions

        Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken));
        Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken));
        Task InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken));
        Task DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken));
        Task BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken));
        Task BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken));
        Task BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken));

        #endregion

    }
}
