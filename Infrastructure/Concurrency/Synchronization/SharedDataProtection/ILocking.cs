using System;

namespace Infrastructure.Concurrency.Synchronization.SharedDataProtection
{
    /// <summary>
    /// As per industry standards, at most a function should have 3 parameters and if it's more than that, one should consider 
    /// to wrap the parameters into some class and then pass an instance of the class to the function and that's why for Action or Func 
    /// at most 3 parameters are considered.
    /// 
    /// Here also, if needed, the Action/Func alongwith the parameters can be wraped into some class and the class instance can be passed to 
    /// the methods.
    /// </summary>
    
    public interface ILocking
    {
        void LockOrSpin(Action action);

        void LockOrSpin<TParam>(Action<TParam> action, TParam param);

        void LockOrSpin<TParam1,TParam2>(Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2);

        void LockOrSpin<TParam1, TParam2,TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3);

        TReturn LockOrSpin<TReturn>(Func<TReturn> func);

        TReturn LockOrSpin<TParam,TReturn>(Func<TParam, TReturn> func, TParam param);

        TReturn LockOrSpin<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> func, TParam1 param1, TParam2 param2);

        TReturn LockOrSpin<TParam1, TParam2, TParam3, TReturn>(Func<TParam1, TParam2, TParam3, TReturn> func, TParam1 param1, TParam2 param2, TParam3 param3);
    }
}
