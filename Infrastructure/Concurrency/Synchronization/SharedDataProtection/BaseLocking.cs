using System;

namespace Infrastructure.Concurrency.Synchronization.SharedDataProtection
{
    public abstract class BaseLocking : ILocking
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
