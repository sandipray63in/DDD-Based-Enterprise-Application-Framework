using System;
using Repository.Base;
using Repository.Command;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository
{
    public class RawSQLRepository : DisposableClass, IRawSQLRepository
    {
        private readonly IRawSQLCommand _command;

        public RawSQLRepository(IRawSQLCommand command)
        {
            ContractUtility.Requires<ArgumentNullException>(command.IsNotNull(), "Command instance cannot be null");
            _command = command;
        }

        public int RunScriptWithRawSQL(String script, params object[] parameters)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(RawSQLRepository).FullName);
            ContractUtility.Requires<ArgumentNullException>(!script.IsNullOrWhiteSpace(), "script instance cannot be null or empty");
            return _command.RunScriptWithRawSQL(script, parameters);
        }

        public int SaveWithRawSQL(String script, params object[] parameters)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(RawSQLRepository).FullName);
            ContractUtility.Requires<ArgumentNullException>(!script.IsNullOrWhiteSpace(), "script instance cannot be null or empty");
            return _command.SaveWithRawSQL(script, parameters);
        }

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _command.Dispose();
        }

        #endregion
    }
}
