using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Repository.Command;
using Respository.Testing.ServiceReferences;
using TestEFDomainAndContext.TestDomains;

namespace Respository.Testing
{
    /// <summary>
    /// Although here it's mostly of a wrappaer class but in real scenario there can be complex business
    /// logic incorporated within the methods.
    /// </summary>
    internal class DepartmentTestServiceBasedOnSQLCECommand : DisposableClass, ICommand<Department>
    {
        private TestServiceBasedOnSQLCEOf_DepartmentClient _departmentTestService;

        public DepartmentTestServiceBasedOnSQLCECommand()
        {
            _departmentTestService = new TestServiceBasedOnSQLCEOf_DepartmentClient();
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
