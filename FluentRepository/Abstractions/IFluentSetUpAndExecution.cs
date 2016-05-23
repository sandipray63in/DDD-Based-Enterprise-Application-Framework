using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Repository.Base;

namespace FluentRepository.Abstractions
{
    public interface IFluentSetUpAndExecution
    {
        /// <summary>
        /// Set Up new Command Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandRepositoryFunc"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpNewCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set Up new Query Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryRepositoryFunc"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpNewQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot;

        void Execute();

        Task ExecuteAsync(CancellationToken token = default(CancellationToken));
    }
}
