using System;

namespace Infrastructure.Observers
{
    public interface ISubscribeUnsubscribe<TSubject,TObservable>
        where TObservable : IObservable<TSubject>
    {
        void Subscribe(TObservable provider);

        void Unsubscribe();
    }
}
