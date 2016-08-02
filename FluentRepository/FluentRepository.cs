using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Domain.Base.Aggregates;
using FluentRepository.FluentImplementations;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;
using Infrastructure.UnitOfWork;

namespace FluentRepository
{
    public static class FluentRepository
    {
        public static IFluentCommandAndQueryRepository WithDefaultUnitOfWork(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
        {
            var isoLevel = IsolationLevel.ReadCommitted;
            var scopeOption = TransactionScopeOption.RequiresNew;
            var unitOfWorkData = new UnitOfWorkData { UnitOfWork = new UnitOfWork(isoLevel, scopeOption), ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return new FluentCommandAndQueryRepository(unitOfWorkData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(IsolationLevel isoLevel, TransactionScopeOption scopeOption, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : IUnitOfWork
        {
            var unitOfWorkData = new UnitOfWorkData { UnitOfWork = new UnitOfWork(isoLevel, scopeOption), ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return new FluentCommandAndQueryRepository(unitOfWorkData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(TUnitOfWork unitOfWork, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : class, IUnitOfWork
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork.IsNotNull(), "unitOfWork instance cannot be null");
            var unitOfWorkData = new UnitOfWorkData { UnitOfWork = unitOfWork, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return new FluentCommandAndQueryRepository(unitOfWorkData);
        }

        public static IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository.IsNotNull(), "commandRepository instance cannot be null");
            var commandRepositoryList = new List<dynamic> { commandRepository };
            return new FluentCommandRepository(null, commandRepositoryList, null);
        }

        public static IFluentCommandRepository SetUpCommandRepository(params dynamic[] commandRepositories)
        {
            return SetUpCommandRepository(commandRepositories.ToList());
        }

        public static IFluentCommandRepository SetUpCommandRepository(IList<dynamic> commandRepositories)
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepositories.IsNotNull(), "commandRepositories cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(commandRepositories.IsNotEmpty(), "commandRepositories cannot be empty");
            ContractUtility.Requires<ArgumentException>(commandRepositories.All(x => x.GetType().GetGenericTypeDefinition().GetInterface(typeof(ICommandRepository<>).Name) != null), "All repositories should be of type ICommandRepository<>");
            ContractUtility.Requires<ArgumentException>(commandRepositories.Count() == commandRepositories.Distinct().Count(), "One or more Command Repository has been repeated");
            return new FluentCommandRepository(null, commandRepositories, null);
        }

        public static IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository.IsNotNull(), "queryRepository instance cannot be null");
            var queryRepositoryList = new List<dynamic> { queryRepository };
            return new FluentQueryRepository(null, queryRepositoryList, null);
        }

        public static IFluentQueryRepository SetUpQueryRepository(params dynamic[] queryRepositories)
        {
            return SetUpQueryRepository(queryRepositories.ToList());
        }

        public static IFluentQueryRepository SetUpQueryRepository(IList<dynamic> queryRepositories)
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepositories.IsNotNull(), "queryRepositories cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(queryRepositories.IsNotEmpty(), "queryRepositories cannot be empty");
            ContractUtility.Requires<ArgumentException>(queryRepositories.All(x => x.GetType().GetGenericTypeDefinition().GetInterface(typeof(IQueryableRepository<>).Name) != null), "All repositories should be of type IQueryableRepository<>");
            ContractUtility.Requires<ArgumentException>(queryRepositories.Count() == queryRepositories.Distinct().Count(), "One or more Query Repository has been repeated");
            return new FluentQueryRepository(null, queryRepositories, null);
        }
    }
}
