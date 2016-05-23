
namespace FluentRepository.Abstractions
{
    public interface IFluentQueryRepository
    {
        /// <summary>
        /// Get all the available Queries which can be fired next
        /// </summary>
        /// <returns></returns>
        IFluentQueries WithQueries();
    }
}
