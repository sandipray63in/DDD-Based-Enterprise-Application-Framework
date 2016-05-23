using System.Transactions;

namespace Infrastructure.Utilities
{
    public static class TransactionUtility
    {
        public static TransactionScope GetTransactionScope(IsolationLevel isoLevel, TransactionScopeOption scopeOption,bool isAsyncEnabled = false)
        {
            var transOptions = new TransactionOptions();
            transOptions.IsolationLevel = isoLevel;
            var scope = new TransactionScope(scopeOption, transOptions, isAsyncEnabled ? TransactionScopeAsyncFlowOption.Enabled : TransactionScopeAsyncFlowOption.Suppress);
            return scope;
        }
    }
}
