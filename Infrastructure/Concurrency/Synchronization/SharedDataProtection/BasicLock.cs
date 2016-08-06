using System;

namespace Infrastructure.Concurrency.Synchronization.SharedDataProtection
{
    public class BasicLock : BaseLocking
    {
        private readonly Object _lockObject;

        //private static readonly Object _lockObject = new Object();

        /// <summary>
        /// Above code is commented and now need to inject the instance from outside since if the above code is used 
        /// and there are n consumers of this class in a large application(considering n to be quite large) then 
        /// n-1 consumers will be waiting for the lock to get released which can be a big hindrance to performance.
        /// </summary>
        /// <param name="lockObject">ideally, the consumers of this class should pass a static readonly instance of 
        /// type Object. There is still a challenge to inject static readonly dependencies via only unity and a 
        /// corresponding discussion have started on the SO post viz.
        /// http://stackoverflow.com/questions/34725958/ms-unity-container </param>
        
        public BasicLock(Object lockObject)
        {
            _lockObject = lockObject;
        }

        public override void LockOrSpin(Action action)
        {
            lock (_lockObject)
            {
                action();
            }
        }

        public override TReturn LockOrSpin<TReturn>(Func<TReturn> func)
        {
            lock (_lockObject)
            {
                return func();
            }
        }

    }
}
