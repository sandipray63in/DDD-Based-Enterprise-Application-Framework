using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repository.Command;
using Infrastructure;
using Domain.Base.Aggregates;

namespace Testing.Integration
{
    /// <summary>
    /// Although here it's mostly of a wrapper class but in real scenario there can be complex business
    /// logic incorporated within the methods.But ideally all Business logic should be placed in the 
    /// Business Layer rather than in the Service Layer.
    /// </summary>
    internal class TestServiceCommand<TEntity> : DisposableClass, ICommand<TEntity>
        where TEntity : ICommandAggregateRoot
    {
        private dynamic _testServiceClient;

        public TestServiceCommand()
        {
            _testServiceClient = TestServiceCommandFactory.GetServiceClientInstance<TEntity>();
        }

        public void Insert(TEntity item)
        {
           _testServiceClient.Insert(item);
        }

        public void Update(TEntity item)
        {
            _testServiceClient.Update(item);
        }

        public void Delete(TEntity item)
        {
            _testServiceClient.Delete(item);
        }

        public void BulkDelete(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void BulkInsert(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void BulkUpdate(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(TEntity item, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(TEntity item, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(TEntity item, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(IEnumerable<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkUpdateAsync(IEnumerable<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkDeleteAsync(IEnumerable<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }


        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _testServiceClient.Close();
        }

        #endregion
    }
}
