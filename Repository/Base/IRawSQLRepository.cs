using System;

namespace Repository.Base
{
    public interface IRawSQLRepository : IDisposable
    {
        /// <summary>
        /// Useful in case of running scripts to create Database,tables,views,stored procedures or executing some sql query or executing a stored procedure etc.
        /// Also useful for running queries using plain old ADO.NET.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int RunScriptWithRawSQL(String script, params object[] parameters);

        /// <summary>
        /// Useful for saving data using plain old ADO.NET.
        /// </summary>
        /// <param name="deleteQuery"></param>
        /// <param name="parameters"></param>
        int SaveWithRawSQL(String script, params object[] parameters);
    }
}
