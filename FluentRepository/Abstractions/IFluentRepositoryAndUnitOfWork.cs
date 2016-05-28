using System;
using System.Transactions;
using Domain.Base.Aggregates;
using Repository.Base;
using Repository.UnitOfWork;

namespace FluentRepository.Abstractions
{
    public interface IFluentRepositoryAndUnitOfWork
    {
        /// <summary>
        /// Set up the Unit Of Work Instance with an istance of UnitOfWork class(default implementation 
        /// of Unit Of Work Pattern)
        /// 
        /// Use this API if all the repositories needs to be saved in a single Transaction and 
        /// if Transaction is not required then just don't call it.
        /// </summary>
        /// <returns></returns>
        IFluentRepositoryAndUnitOfWork WithDefaultUnitOfWork(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true);

        /// <summary>
        /// Set up the Unit Of Work Instance providing the Isolation level and TransactionScope option
        /// to be set explicitly.
        /// 
        /// Use this API if all the repositories needs to be saved in a single Transaction and 
        /// if Transaction is not required then just don't call it.
        /// </summary>
        /// <param name="isoLevel"></param>
        /// <param name="scopeOption"></param>
        /// <returns></returns>
        IFluentRepositoryAndUnitOfWork WithUnitOfWork<TUnitOfWork>(IsolationLevel isoLevel, TransactionScopeOption scopeOption, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork;

        /// <summary>
        /// Set up the Unit Of Work Instance with an istance of TUnitOfWork
        /// 
        /// Use this API if all the repositories needs to be saved in a single Transaction and 
        /// if Transaction is not required then just don't call it.
        /// </summary>
        /// <typeparam name="TUnitOfWork"></typeparam>
        /// <param name="unitOfWorkFunc"></param>
        /// <returns></returns>
        IFluentRepositoryAndUnitOfWork WithUnitOfWork<TUnitOfWork>(Func<TUnitOfWork> unitOfWorkFunc, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork;

        /// <summary>
        /// Set Up the Command Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandRepositoryFunc"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set Up the Query Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryRepositoryFunc"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot;
    }
}
