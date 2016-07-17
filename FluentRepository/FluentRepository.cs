using System;
using System.Transactions;
using Domain.Base.Aggregates;
using FluentRepository.FluentImplementations;
using FluentRepository.FluentInterfaces;
using Repository.Base;
using Repository.UnitOfWork;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace FluentRepository
{
    public static class FluentRepository
    {
        public static IFluentCommandAndQueryRepository WithDefaultUnitOfWork(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
        {
            var isoLevel = IsolationLevel.ReadCommitted;
            var scopeOption = TransactionScopeOption.RequiresNew;
            Func<UnitOfWork> unitOfWorkFunc = () => ContainerUtility.CheckRegistrationAndGetInstance<BaseUnitOfWork, UnitOfWork>(isoLevel, scopeOption) as UnitOfWork;
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<UnitOfWork, dynamic>();
            var unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandAndQueryRepository, FluentCommandAndQueryRepository>(unitOfWorkData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(IsolationLevel isoLevel, TransactionScopeOption scopeOption, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            Func<TUnitOfWork> unitOfWorkFunc = () => ContainerUtility.CheckRegistrationAndGetInstance<BaseUnitOfWork, TUnitOfWork>(isoLevel, scopeOption) as TUnitOfWork;
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<TUnitOfWork, dynamic>();
            var unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandAndQueryRepository, FluentCommandAndQueryRepository>(unitOfWorkData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(Func<TUnitOfWork> unitOfWorkFunc, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkFunc.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<TUnitOfWork, dynamic>();
            var unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandAndQueryRepository, FluentCommandAndQueryRepository>(unitOfWorkData);
        }

        public static IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(null,commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>(),null);
        }

        public static IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(null,queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>(),null);
        }
    }
}
