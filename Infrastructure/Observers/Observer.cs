using System;

namespace Infrastructure.Observers
{
    public abstract class Observer<TSubject, TObservable> : IObserver<TSubject>, ISubscribeUnsubscribe<TSubject, TObservable>
        where TObservable : IObservable<TSubject>
    {
        private IDisposable _cancellation;

        public virtual void Subscribe(TObservable provider)
        {
            _cancellation = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            _cancellation.Dispose();
        }

        public abstract void OnNext(TSubject value);

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);
    }
}
