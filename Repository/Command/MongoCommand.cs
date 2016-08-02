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

        public void Insert(IList<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            _mongoCollection.InsertMany(items);
        }

        public void Update(IList<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            ///TODO - Need to test this.
            _mongoCollection.UpdateMany(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId), null);
        }

        public void Delete(IList<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            _mongoCollection.DeleteMany(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId));
        }

        public void BulkInsert(IList<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            this.Insert(items);
        }

        public void BulkUpdate(IList<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            this.Update(items);
        }

        public void BulkDelete(IList<TEntity> items)
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

        public async Task InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await _mongoCollection.InsertManyAsync(items,cancellationToken:token);
        }

        public async Task UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            ///TODO - Need to test this.
            await _mongoCollection.UpdateManyAsync(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId), null,cancellationToken:token);
        }

        public async Task DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await _mongoCollection.DeleteManyAsync(x => items.Select(y => y.T1Data.MongoId).Distinct().Contains(x.T1Data.MongoId),token);
        }

        public async Task BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await this.InsertAsync(items, token);
        }

        public async Task BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(MongoCommand<TId,TEntity>).FullName);
            await this.UpdateAsync(items, token);
        }

        public async Task BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
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
