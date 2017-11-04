using System.Collections.Generic;

namespace Repository
{
    public class PagingTableResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalCount { get; set; }
    }
}