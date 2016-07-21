using System;
using System.Collections.Generic;
using System.Transactions;
using Domain.Base.Aggregates;
using FluentRepository.FluentImplementations;
using FluentRepository.FluentInterfaces;
using Infrastructure.DI;
using Infrastructure.Extensions;
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
            var isoLevelParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = isoLevel.GetType(), ParameterName = "isoLevel", ParameterValue = isoLevel };
            var scopeOptionParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = scopeOption.GetType(), ParameterName = "scopeOption", ParameterValue = scopeOption };
            Func<UnitOfWork> unitOfWorkFunc = () => ContainerUtility.CheckRegistrationAndGetInstance<BaseUnitOfWork, UnitOfWork>(isoLevelParameterTypeOverrideData, scopeOptionParameterTypeOverrideData) as UnitOfWork;
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<UnitOfWork, dynamic>();
            var unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = unitOfWorkData };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandAndQueryRepository, FluentCommandAndQueryRepository>(unitOfWorkParameterTypeOverrideData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(IsolationLevel isoLevel, TransactionScopeOption scopeOption, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            var isoLevelParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = isoLevel.GetType(), ParameterName = "isoLevel", ParameterValue = isoLevel };
            var scopeOptionParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = scopeOption.GetType(), ParameterName = "scopeOption", ParameterValue = scopeOption };
            Func<UnitOfWork> unitOfWorkFunc = () => ContainerUtility.CheckRegistrationAndGetInstance<BaseUnitOfWork, UnitOfWork>(isoLevelParameterTypeOverrideData, scopeOptionParameterTypeOverrideData) as UnitOfWork;
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<UnitOfWork, dynamic>();
            var unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = unitOfWorkData };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandAndQueryRepository, FluentCommandAndQueryRepository>(unitOfWorkParameterTypeOverrideData);
        }

        public static IFluentCommandAndQueryRepository WithUnitOfWork<TUnitOfWork>(Func<TUnitOfWork> unitOfWorkFunc, bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
            where TUnitOfWork : BaseUnitOfWork
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkFunc.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            var unitOfWorkFuncDynamic = unitOfWorkFunc.ConvertFunc<TUnitOfWork, dynamic>();
            var unitOfWorkData = new UnitOfWorkData { UnitOfWorkFunc = unitOfWorkFuncDynamic, ShouldAutomaticallyRollBackOnTransactionException = shouldAutomaticallyRollBackOnTransactionException, ShouldThrowOnException = shouldThrowOnException };
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = unitOfWorkData };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandAndQueryRepository, FluentCommandAndQueryRepository>(unitOfWorkParameterTypeOverrideData);
        }

        public static IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            //var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(UnitOfWorkData), ParameterName = "unitOfWorkData", ParameterValue = null };
            var dynamicCommandRepositoryFunc = commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>();
            //var dynamicCommandRepositoryFuncParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = dynamicCommandRepositoryFunc.GetType(), ParameterName = "commandRepositoryFunc", ParameterValue = dynamicCommandRepositoryFunc };
            //var commandRepositoryType = new ParameterTypeOverrideData { ParameterType = typeof(Type), ParameterName = "commandRepositoryType", ParameterValue = typeof(ICommandRepository<TEntity>) };
            //var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(IList<CommandsAndQueriesPersistanceAndRespositoryData>), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = null };
            //return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(unitOfWorkParameterTypeOverrideData, dynamicCommandRepositoryFuncParameterTypeOverrideData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
            return new FluentCommandRepository(null, dynamicCommandRepositoryFunc, typeof(ICommandRepository<TEntity>),null);
        }

        public static IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            //var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(UnitOfWorkData), ParameterName = "unitOfWorkData", ParameterValue = null };
            var dynamicQueryRepositoryFunc = queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>();
            //var dynamicQueryRepositoryFuncParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = dynamicQueryRepositoryFunc.GetType(), ParameterName = "commandRepositoryFunc", ParameterValue = dynamicQueryRepositoryFunc };
            //var queryRepositoryType = new ParameterTypeOverrideData { ParameterType = typeof(Type), ParameterName = "queryRepositoryType", ParameterValue = typeof(IQueryableRepository<TEntity>) };
            //var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(IList<CommandsAndQueriesPersistanceAndRespositoryData>), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = null };
            //return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(unitOfWorkParameterTypeOverrideData, dynamicQueryRepositoryFuncParameterTypeOverrideData, queryRepositoryType,commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
            return new FluentQueryRepository(null, dynamicQueryRepositoryFunc, typeof(IQueryableRepository<TEntity>), null);
        }
    }
}
