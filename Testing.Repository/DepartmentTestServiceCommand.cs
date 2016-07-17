using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Repository.Command;
using Infrastructure;
using Testing.Respository.ServiceReferences;
using TestEFDomainAndContext.TestDomains;

namespace Testing.Respository
{
    /// <summary>
    /// Although here it's mostly of a wrappaer class but in real scenario there can be complex business
    /// logic incorporated within the methods.
    /// 
    /// Add the DataContractSerializer for EffortConnection KnownType registration in config as done here in 
    /// App.config.
    /// 
    /// This is actually Adapter Pattern implementation.If needed groups of Adapters can be implemented
    /// (may be using Channel Factory) and each group can act as a generic Repository..
    /// </summary>
    public class DepartmentTestServiceCommand : DisposableClass, ICommand<Department>
    {
        private DbConnection _connection;
        private TestServiceOf_DepartmentClient _departmentTestService;

        public DepartmentTestServiceCommand(DbConnection connection)
        {
            _connection = connection;
            _departmentTestService = new TestServiceOf_DepartmentClient();
        }

        public void Insert(Department item)
        {
            _departmentTestService.Insert(_connection,item);
        }

        public void Update(Department item)
        {
            _departmentTestService.Update(_connection,item);
        }

        public void Delete(Department item)
        {
            _departmentTestService.Delete(_connection,item);
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
