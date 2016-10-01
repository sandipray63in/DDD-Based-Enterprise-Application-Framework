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

        public void BulkDelete(IList<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void BulkInsert(IList<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void BulkUpdate(IList<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Delete(IList<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Insert(IList<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Update(IList<TEntity> items)
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

        public async Task InsertAsync(IList<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IList<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(IList<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkInsertAsync(IList<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkUpdateAsync(IList<TEntity> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkDeleteAsync(IList<TEntity> items, CancellationToken token)
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
