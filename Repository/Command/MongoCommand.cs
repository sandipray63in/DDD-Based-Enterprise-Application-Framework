using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDBDomainMaps;
using Domain.Base.Mongo;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Command
{
    public class MongoCommand<TEntity> : DisposableClass, ICommand<TEntity> where TEntity : BaseMongoIdentityAndAuditableCommandAggregateRoot
    {
        private IMongoCollection<TEntity> _mongoCollection;

        public MongoCommand(MongoContext mongoContext)
        {
            ContractUtility.Requires<ArgumentNullException>(mongoContext != null, "mongoContext instance cannot be null");
            _mongoCollection = mongoContext.GetMongoCollection<TEntity>();
        }

        public void Insert(TEntity item)
        {
            _mongoCollection.InsertOne(item);
        }

        public void Update(TEntity item)
        {
            ///TODO - Need to test this.
            _mongoCollection.UpdateOne(x => x.MongoId == item.MongoId, null);
        }

        public void Delete(TEntity item)
        {
            _mongoCollection.DeleteOne(x => x.MongoId == item.MongoId);
        }

        public void Insert(IList<TEntity> items)
        {
            _mongoCollection.InsertMany(items);
        }

        public void Update(IList<TEntity> items)
        {
            ///TODO - Need to test this.
            _mongoCollection.UpdateMany(x => items.Select(y => y.MongoId).Distinct().Contains(x.MongoId), null);
        }

        public void Delete(IList<TEntity> items)
        {
            _mongoCollection.DeleteMany(x => items.Select(y => y.MongoId).Distinct().Contains(x.MongoId));
        }

        public void BulkInsert(IList<TEntity> items)
        {
            this.Insert(items);
        }

        public void BulkUpdate(IList<TEntity> items)
        {
            this.Update(items);
        }

        public void BulkDelete(IList<TEntity> items)
        {
            this.Delete(items);
        }

        public async Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            await _mongoCollection.InsertOneAsync(item,null,token);
        }

        public async Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            ///TODO - Need to test this.
            await _mongoCollection.UpdateOneAsync(x => x.MongoId == item.MongoId, null,cancellationToken:token);
        }

        public async Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            await _mongoCollection.DeleteOneAsync(x => x.MongoId == item.MongoId,token);
        }

        public async Task InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            await _mongoCollection.InsertManyAsync(items,cancellationToken:token);
        }

        public async Task UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            ///TODO - Need to test this.
            await _mongoCollection.UpdateManyAsync(x => items.Select(y => y.MongoId).Distinct().Contains(x.MongoId), null,cancellationToken:token);
        }

        public async Task DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
           await _mongoCollection.DeleteManyAsync(x => items.Select(y => y.MongoId).Distinct().Contains(x.MongoId),token);
        }

        public async Task BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            await this.InsertAsync(items, token);
        }

        public async Task BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            await this.UpdateAsync(items, token);
        }

        public async Task BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken))
        {
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
