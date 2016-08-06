using System;
using System.Threading;

namespace Infrastructure.Concurrency.Synchronization.SharedDataProtection
{
    public class SpinningWait : BaseLocking
    {
        private readonly int _timeToSpinWait;
        public SpinningWait(int timeToSpinWait)
        {
            _timeToSpinWait = timeToSpinWait;
        }

        public override void LockOrSpin(Action action)
        {
            SpinWait.SpinUntil(() =>
            {
                action();
                return true;
            }, _timeToSpinWait);
        }

        public override TReturn LockOrSpin<TReturn>(Func<TReturn> func)
        {
            var returnObject = default(TReturn);
            SpinWait.SpinUntil(() =>
            {
                returnObject = func();
                return true;
            }, _timeToSpinWait);
            return returnObject;
        }

    }
}
