using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDBDomainMaps;
using Domain.Base.AddOnObjects;
using Domain.Base.Aggregates;
using Domain.Base.Entities.Composites;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Command
{
    public class MongoCommand<TId,TEntity> : DisposableClass, ICommand<TEntity>
        where TId : struct
        where TEntity : BaseEntityComposite<TId, MongoInfo>, ICommandAggregateRoot
    {
        private IMongoCollection<TEntity> _mongoCollection;

        public MongoCommand(MongoContext mongoContext)
        {
            ContractUtility.Requires<ArgumentNullException>(mongoContext != null, "mongoContext instance cannot be null");
            _mongoCollection = mongoContext.GetMongoCollection<TId,TEntity>();
        }

        public void Insert(TEntity item)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            _mongoCollection.InsertOne(item);
        }

        public void Update(TEntity item)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            ///TODO - Need to test this.
            _mongoCollection.UpdateOne(x => x.T1Data.MongoId == item.T1Data.MongoId, null);
        }

        public void Delete(TEntity item)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            _mongoCollection.DeleteOne(x => x.T1Data.MongoId == item.T1Data.MongoId);
        }

        public void Insert(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            _mongoCollection.InsertMany(items);
        }

        public void Update(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            ///TODO - Need to test this.
            _mongoCollection.UpdateMany(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId), null);
        }

        public void Delete(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            _mongoCollection.DeleteMany(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId));
        }

        public void BulkInsert(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            this.Insert(items);
        }

        public void BulkUpdate(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            this.Update(items);
        }

        public void BulkDelete(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            this.Delete(items);
        }

        public async Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await _mongoCollection.InsertOneAsync(item,null,token);
        }

        public async Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            ///TODO - Need to test this.
            await _mongoCollection.UpdateOneAsync(x => x.T1Data.MongoId == item.T1Data.MongoId, null,cancellationToken:token);
        }

        public async Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await _mongoCollection.DeleteOneAsync(x => x.T1Data.MongoId == item.T1Data.MongoId,token);
        }

        public async Task InsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await _mongoCollection.InsertManyAsync(items,cancellationToken:token);
        }

        public async Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            ///TODO - Need to test this.
            await _mongoCollection.UpdateManyAsync(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId), null,cancellationToken:token);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await _mongoCollection.DeleteManyAsync(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId),token);
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await this.InsertAsync(items, token);
        }

        public async Task BulkUpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await this.UpdateAsync(items, token);
        }

        public async Task BulkDeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await this.DeleteAsync(items, token);
        }

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            _mongoCollection = null;
        }

        #endregion
    }
}
