using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class CleanUpExtensions
    {
        public static void CleanUp(this IEnumerable<IDisposable> disposables)
        {
            disposables.Where(x => x.IsNotNull()).ToList().ForEach(x =>
                {
                    x.Dispose();
                    x = null;
                }
            );
        }
    }
}
