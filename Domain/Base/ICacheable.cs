
namespace Domain.Base
{
    /// <summary>
    /// A marker/tagging interface to mark/tag a class(a domain entity) that it is cacheable.
    /// </summary>
    public interface ICacheable : IQueryableAggregateRoot
    {
    }
}
