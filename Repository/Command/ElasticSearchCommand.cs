using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElasticsearchCRUD;
using Domain.Base;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using DomainContextsAndMaps;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Command
{
    public class ElasticSearchCommand<TEntity, TId> : DisposableClass, ICommand<TEntity>
        where TEntity : BaseEntity<TId>, ICommandAggregateRoot, IElasticSearchable
        where TId : struct
    {
        private readonly ElasticsearchContext _elasticsearchContext;

        public ElasticSearchCommand(ElasticSearchContext esContext)
        {
            ContractUtility.Requires<ArgumentNullException>(esContext != null, "esContext instance cannot be null");
            _elasticsearchContext = esContext.ElasticsearchContext;
        }

        public void Insert(TEntity item)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            _elasticsearchContext.AddUpdateDocument(item, item.Id);
            SaveChanges();
        }

        public void Update(TEntity item)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            _elasticsearchContext.AddUpdateDocument(item, item.Id);
            SaveChanges();
        }

        public void Delete(TEntity item)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            _elasticsearchContext.DeleteDocument<TEntity>(item.Id);
            SaveChanges();
        }

        public void Insert(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            items.ToList().ForEach(item =>
                {
                    _elasticsearchContext.AddUpdateDocument(item, item.Id);
                }
             );
            SaveChanges();
        }

        public void Update(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            items.ToList().ForEach(item =>
                {
                    _elasticsearchContext.AddUpdateDocument(item, item.Id);
                }
            );
            SaveChanges();
        }

        public void Delete(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            items.ToList().ForEach(item =>
                {
                    _elasticsearchContext.DeleteDocument<TEntity>(item.Id);
                }
            );
            SaveChanges();
        }

        public void BulkInsert(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            this.Insert(items);
        }

        public void BulkUpdate(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            this.Update(items);
        }

        public void BulkDelete(IEnumerable<TEntity> items)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            this.Delete(items);
        }

        public async Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            _elasticsearchContext.AddUpdateDocument(item, item.Id);
            await SaveChangesAsync(token);
        }

        public async Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            _elasticsearchContext.AddUpdateDocument(item, item.Id);
            await SaveChangesAsync(token);
        }

        public async Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            _elasticsearchContext.DeleteDocument<TEntity>(item.Id);
            await SaveChangesAsync(token);
        }

        public async Task InsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            items.ToList().ForEach(item =>
                {
                    _elasticsearchContext.AddUpdateDocument(item, item.Id);
                }
            );
            await SaveChangesAsync(token);
        }

        public async Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            items.ToList().ForEach(item =>
            {
                _elasticsearchContext.AddUpdateDocument(item, item.Id);
            }
            );
            await SaveChangesAsync(token);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            items.ToList().ForEach(item =>
            {
                _elasticsearchContext.DeleteDocument<TEntity>(item.Id);
            }
            );
            await SaveChangesAsync(token);
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            await this.InsertAsync(items, token);
        }

        public async Task BulkUpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            await this.UpdateAsync(items, token);
        }

        public async Task BulkDeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken))
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchCommand<TEntity, TId>).FullName);
            await this.DeleteAsync(items, token);
        }

        #region Private Methods


        private void SaveChanges()
        {
            if (_elasticsearchContext.IndexTypeExists<TEntity>())
            {
                _elasticsearchContext.SaveChanges();
            }
            else
            {
                _elasticsearchContext.SaveChangesAndInitMappings();
            }
        }

        private async Task SaveChangesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_elasticsearchContext.IndexTypeExistsAsync<TEntity>().Result.PayloadResult)
            {
                await _elasticsearchContext.SaveChangesAsync();
            }
            else
            {
                _elasticsearchContext.SaveChangesAndInitMappings();
            }
        }

        #endregion


        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _elasticsearchContext.Dispose();
        }

        #endregion
    }
}
