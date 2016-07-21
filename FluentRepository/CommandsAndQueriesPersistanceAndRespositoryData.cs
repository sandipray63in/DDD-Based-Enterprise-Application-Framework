using System;
using System.Collections.Generic;

namespace FluentRepository
{
    internal class CommandsAndQueriesPersistanceAndRespositoryData
    {
        internal Func<dynamic> QueryRepositoryFunc { get; set; }

        internal Type QueryRepositoryType { get; set; }

        internal Func<dynamic> CommandRepositoryFunc { get; set; }

        internal Type CommandRepositoryType { get; set; }

        internal Func<dynamic> QueryPersistanceFunc { get; set; }

        internal Func<dynamic> CommandPersistanceFunc { get; set; }

        internal Queue<OperationData> OpreationsQueue { get; set; }
    }
}
