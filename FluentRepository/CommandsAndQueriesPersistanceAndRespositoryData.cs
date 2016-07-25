using System;
using System.Collections.Generic;

namespace FluentRepository
{
    internal class CommandsAndQueriesPersistanceAndRespositoryData
    {
        internal dynamic QueryRepository { get; set; }

        internal Type QueryRepositoryType { get; set; }

        internal dynamic CommandRepository { get; set; }

        internal Type CommandRepositoryType { get; set; }

        internal dynamic QueryPersistance { get; set; }

        internal dynamic CommandPersistance { get; set; }

        internal Queue<OperationData> OpreationsQueue { get; set; }
    }
}
