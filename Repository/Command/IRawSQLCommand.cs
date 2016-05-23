using System;

namespace Repository.Command
{
    public interface IRawSQLCommand : IDisposable
    {
        /// <summary>
        /// Useful in case of running scripts to create Database,tables,views,stored procedures or executing some sql query
        /// or executing a stored procedure etc.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int RunScriptWithRawSQL(String script, params object[] parameters);

        /// <summary>
        /// Useful in case of bulk delete or update or insert.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="parameters"></param>
        int SaveWithRawSQL(String script, params object[] parameters);
    }
}
