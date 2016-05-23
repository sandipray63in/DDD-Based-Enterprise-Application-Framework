using System;

namespace Infrastructure.Locking
{
    /// <summary>
    /// The concept used here is somewhat similar to that of currying
    /// (a functional programming concept) although ideally Curry and UnCurry 
    /// extensions should be seperated out into different Extension Methods alltogether 
    /// as suggested here  - 
    /// https://weblogs.asp.net/dixin/lambda-calculus-via-c-sharp-1-fundamentals-closure-currying-and-partial-application
    /// </summary>
    public abstract class BaseLockOrSpin : ILockOrSpin
    {
        public abstract void LockOrSpin(Action action);

        public virtual void LockOrSpin<TParam>(Action<TParam> action, TParam param)
        {
            LockOrSpin(() => action(param));
        }

        public virtual void LockOrSpin<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2)
        {
            LockOrSpin(() => action(param1, param2));
        }

        public virtual void LockOrSpin<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            LockOrSpin(() => action(param1, param2, param3));
        }

        public abstract TReturn LockOrSpin<TReturn>(Func<TReturn> func);
       
        public virtual TReturn LockOrSpin<TParam, TReturn>(Func<TParam, TReturn> func, TParam param)
        {
            return LockOrSpin(() => func(param));
        }

        public virtual TReturn LockOrSpin<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> func, TParam1 param1, TParam2 param2)
        {
            return LockOrSpin(() => func(param1, param2));
        }

        public virtual TReturn LockOrSpin<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, TReturn> func, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return LockOrSpin(() => func(param1, param2, param3));
        }
    }
}
