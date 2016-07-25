using Domain.Base.Aggregates;
using Repository.Base;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentCommandAndQueryRepository
    {
        /// <summary>
        /// Set Up the Command Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandRepository"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set Up the Query Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryRepository"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot; 
    }
}
