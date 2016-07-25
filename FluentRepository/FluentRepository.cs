using System;
using System.Transactions;
using Domain.Base.Aggregates;
using FluentRepository.FluentImplementations;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;
using Repository.UnitOfWork;

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
            where TUnitOfWork : BaseUnitOfWork
        {
            var unitOfWorkData = new UnitOfWorkData { UnitOfWork = new UnitOfWork(isoLevel, scopeOption), ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return new FluentCommandAndQueryRepository(unitOfWorkData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(TUnitOfWork unitOfWork, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork.IsNotNull(), "unitOfWork instance cannot be null");
            var unitOfWorkData = new UnitOfWorkData { UnitOfWork = unitOfWork, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return new FluentCommandAndQueryRepository(unitOfWorkData);
        }

        public static IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository.IsNotNull(), "commandRepository instance cannot be null");
            return new FluentCommandRepository(null, commandRepository, commandRepository.GetType(), null);
        }

        public static IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository.IsNotNull(), "queryRepository instance cannot be null");
            return new FluentQueryRepository(null, queryRepository, queryRepository.GetType(), null);
        }
    }
}
