using Domain.Base;
using Repository.Base;
using Repository.Command;
using Repository.UnitOfWork;

namespace Repository
{
    public class CommandElasticSearchableRepository<TEntity> : CommandRepository<TEntity>, ICommandElasticSearchableRepository<TEntity> where TEntity : class, ICommandAggregateRoot,IElasticSearchable
    {
        /// <summary>
        /// Should be used when unit of work instance is not required 
        /// i.e when explicit transactions management is not required
        /// 
        /// Ideally, should pass an instance of ElasticSearchCommand class using the DI container
        /// </summary>
        public CommandElasticSearchableRepository(ICommand<TEntity> command)
            : base(command)
        {

        }

        /// <summary>
        /// The same unit of work instance can be used across different instances of repositories
        /// (if needed)
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="command"></param>
        public CommandElasticSearchableRepository(BaseUnitOfWork unitOfWork, ICommand<TEntity> command)
            : base(unitOfWork,command)
        {

        }
    }
}
