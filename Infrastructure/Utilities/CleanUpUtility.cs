using System;
using Infrastructure.Extensions;

namespace Infrastructure.Utilities
{
    public static class CleanUpUtility
    {
        public static void CleanUp(params IDisposable[] disposables)
        {
            if (disposables.IsNotNull())
            {
                disposables.CleanUp();
            }
        }
    }
}
