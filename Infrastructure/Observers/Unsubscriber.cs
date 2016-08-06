using System;
using System.Collections.Generic;

namespace Infrastructure.Observers
{
    internal class Unsubscriber<TSubject> : DisposableClass
    {
        private List<IObserver<TSubject>> _observers;
        private IObserver<TSubject> _observer;

        internal Unsubscriber(List<IObserver<TSubject>> observers, IObserver<TSubject> observer)
        {
            _observers = observers;
            _observer = observer;
        }
        
        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
