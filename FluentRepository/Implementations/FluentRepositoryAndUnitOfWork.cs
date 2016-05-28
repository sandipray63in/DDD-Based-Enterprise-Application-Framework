using System;
using System.Transactions;
using Domain.Base.Aggregates;
using FluentRepository.Abstractions;
using Infrastructure.Utilities;
using Infrastructure.Extensions;
using Repository.Base;
using Repository.UnitOfWork;

namespace FluentRepository.Implementations
{
    internal class FluentRepositoryAndUnitOfWork : IFluentRepositoryAndUnitOfWork
    {
        private const string WITH_UNIT_OF_WORK = "WithUnitOfWork";
        private UnitOfWorkData _unitOfWorkData;

        internal FluentRepositoryAndUnitOfWork()
        {

        }

        public IFluentRepositoryAndUnitOfWork WithDefaultUnitOfWork(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
        {
            var isoLevel = IsolationLevel.ReadCommitted;
            var scopeOption = TransactionScopeOption.RequiresNew;
            Func<UnitOfWork> unitOfWorkFunc = () => ContainerUtility.CheckRegistrationAndGetInstance<BaseUnitOfWork, UnitOfWork>(isoLevel, scopeOption) as UnitOfWork;
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<UnitOfWork, dynamic>();
            _unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return this;
        }

        public IFluentRepositoryAndUnitOfWork WithUnitOfWork<TUnitOfWork>(IsolationLevel isoLevel, TransactionScopeOption scopeOption, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            Func<TUnitOfWork> unitOfWorkFunc = () => ContainerUtility.CheckRegistrationAndGetInstance<BaseUnitOfWork, TUnitOfWork>(isoLevel, scopeOption) as TUnitOfWork;
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<TUnitOfWork, dynamic>();
            _unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
             return this;
        }

        public IFluentRepositoryAndUnitOfWork WithUnitOfWork<TUnitOfWork>(Func<TUnitOfWork> unitOfWorkFunc, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkFunc.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<TUnitOfWork, dynamic>();
            _unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            return this;
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(WITH_UNIT_OF_WORK, _unitOfWorkData, commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>,dynamic>());
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(WITH_UNIT_OF_WORK, _unitOfWorkData, queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>());
        }
    }
}
