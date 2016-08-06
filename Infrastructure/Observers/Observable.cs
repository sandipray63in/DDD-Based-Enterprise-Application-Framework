using System;
using System.Collections.Generic;

namespace Infrastructure.Observers
{
    public abstract class Observable<TSubject> : IObservable<TSubject>
    {
        private List<IObserver<TSubject>> _observers;
        private List<TSubject> _subjects;

        public Observable()
        {
            _observers = new List<IObserver<TSubject>>();
            _subjects = new List<TSubject>();
        }

        public virtual IDisposable Subscribe(IObserver<TSubject> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                // Provide observer with existing data.
                foreach (var item in _subjects)
                    observer.OnNext(item);
            }
            return new Unsubscriber<TSubject>(_observers, observer);
        }
    }
}
