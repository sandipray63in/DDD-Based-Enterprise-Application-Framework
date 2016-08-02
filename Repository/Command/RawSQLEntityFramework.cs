using System;
using System.Data.Entity;
using Transactions = System.Transactions;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Command
{
    public class RawSQLEntityFramework : DisposableClass,IRawSQLCommand
    {
        private readonly DbContext _context;
        private readonly Transactions.IsolationLevel _isoLevel;
        private readonly Transactions.TransactionScopeOption _scopeOption;
        public RawSQLEntityFramework(DbContext context, Transactions.IsolationLevel isoLevel = Transactions.IsolationLevel.ReadCommitted, Transactions.TransactionScopeOption scopeOption = Transactions.TransactionScopeOption.RequiresNew)
        {
            ContractUtility.Requires<ArgumentNullException>(context.IsNotNull(), "context instance cannot be null");
            _context = context;
            //_context.Database.CreateIfNotExists();//Alongwith creating the DB(if the DB does not exist),
            // this method also creates the tables even if the tables do not exist - also alters the tables if 
            // some changes are there in the structure of domain classes.
            _isoLevel = isoLevel;
            _scopeOption = scopeOption;
        }

        public int RunScriptWithRawSQL(string script, params object[] parameters)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(RawSQLEntityFramework).FullName);
            ContractUtility.Requires<ArgumentNullException>(!script.IsNullOrWhiteSpace(), "script instance cannot be null or empty");
            return _context.Database.ExecuteSqlCommand(script, parameters);
        }

        public int SaveWithRawSQL(string script, params object[] parameters)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(RawSQLEntityFramework).FullName);
            ContractUtility.Requires<ArgumentNullException>(!script.IsNullOrWhiteSpace(), "script instance cannot be null or empty");
            return PerformDMLWithRawSQLWithinTransaction(script, parameters);
        }

        #region Private Members

        private int PerformDMLWithRawSQLWithinTransaction(String query, params Object[] parameters)
        {
            ContractUtility.Requires<ArgumentNullException>(!query.IsNullOrWhiteSpace(), "query instance cannot be null or empty");
            var scope = TransactionUtility.GetTransactionScope(_isoLevel, _scopeOption);
            using (scope)
            {
                var retValue = _context.Database.ExecuteSqlCommand(query, parameters);
                scope.Complete();
                return retValue;
            }
        }

        #endregion

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _context.Dispose();
        }

        #endregion
    }
}
