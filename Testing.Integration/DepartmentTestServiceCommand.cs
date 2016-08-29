using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repository.Command;
using Infrastructure;
using TestEFDomainAndContext.TestDomains;

namespace Testing.Integration
{
    /// <summary>
    /// Although here it's mostly of a wrapper class but in real scenario there can be complex business
    /// logic incorporated within the methods.But ideally all Business logic should be placed in the 
    /// Business Layer rather than in the Service Layer.
    /// </summary>
    internal class DepartmentTestServiceCommand : DisposableClass, ICommand<Department>
    {
        private TestServiceOf_DepartmentClient _departmentTestService;

        public DepartmentTestServiceCommand()
        {
            _departmentTestService = new TestServiceOf_DepartmentClient();
        }

        public void Insert(Department item)
        {
           _departmentTestService.Insert(item);
        }

        public void Update(Department item)
        {
            _departmentTestService.Update(item);
        }

        public void Delete(Department item)
        {
            _departmentTestService.Delete(item);
        }

        public void BulkDelete(IList<Department> items)
        {
            throw new NotImplementedException();
        }

        public void BulkInsert(IList<Department> items)
        {
            throw new NotImplementedException();
        }

        public void BulkUpdate(IList<Department> items)
        {
            throw new NotImplementedException();
        }

        public void Delete(IList<Department> items)
        {
            throw new NotImplementedException();
        }

        public void Insert(IList<Department> items)
        {
            throw new NotImplementedException();
        }

        public void Update(IList<Department> items)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(Department item, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Department item, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Department item, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(IList<Department> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IList<Department> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(IList<Department> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkInsertAsync(IList<Department> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkUpdateAsync(IList<Department> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task BulkDeleteAsync(IList<Department> items, CancellationToken token)
        {
            throw new NotImplementedException();
        }


        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _departmentTestService.Close();
        }

        #endregion
    }
}
