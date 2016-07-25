using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Repository.Base;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentSetUpAndExecution
    {
        /// <summary>
        /// Set Up Command Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandRepository"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set Up Query Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryRepository"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot;

        /// <param name="shouldAutomaticallyDisposeAllDisposables">This is set to false 
        /// since, ideally, all the disposables should be disposed from the entry point 
        /// of the application which can be ASP.NET Web API or ASP.NET MVC or ASP.NET Web 
        /// Forms or WCF Service or Workflow Service etc</param>
        void Execute(Boolean shouldAutomaticallyDisposeAllDisposables = false);

        /// <param name="shouldAutomaticallyDisposeAllDisposables">This is set to false 
        /// since, ideally, all the disposables should be disposed from the entry point 
        /// of the application which can be ASP.NET Web API or ASP.NET MVC or ASP.NET Web 
        /// Forms or WCF Service or Workflow Service etc</param>
        Task ExecuteAsync(CancellationToken token = default(CancellationToken), Boolean shouldAutomaticallyDisposeAllDisposables = false);
    }
}
