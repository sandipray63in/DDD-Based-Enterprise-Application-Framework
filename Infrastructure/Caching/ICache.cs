
namespace Infrastructure.Caching
{
    public interface ICache<TKey,TValue> 
    {
        TValue this[TKey key] { get; set; }

        bool Contains(TKey key);

        TValue Get(TKey key);

        bool Add(TKey key, TValue value);

        bool Remove(TKey key);
    }
}
