using System;

namespace Infrastructure.Extensions
{
    public static class MemoizationExtension
    {
        public static Func<TReturn> Memoize<TReturn>(this Func<TReturn> func)
        {
            var returnValue = default(TReturn);
            bool hasValue = false;
            return () =>
            {
                if (!hasValue)
                {
                    hasValue = true;
                    returnValue = func();
                }
                return returnValue;
            };
        }
    }
}
