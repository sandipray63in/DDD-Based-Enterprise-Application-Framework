using System;
using System.Threading.Tasks;

namespace FluentRepository
{
    internal class OperationData
    {
        internal Action Operation { get; set; }

        internal Func<Task> AsyncOperation { get; set; }
    }
}
