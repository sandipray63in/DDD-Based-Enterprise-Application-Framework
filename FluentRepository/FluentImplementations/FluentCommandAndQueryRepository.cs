using System;
using System.Collections.Generic;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandAndQueryRepository : IFluentCommandAndQueryRepository
    {
        private UnitOfWorkData _unitOfWorkData;

        /// <summary>
        /// For proper working of this class alongwith Unity(for regstration), the constructor needs to be public.
        /// </summary>
        /// <param name="unitOfWorkData"></param>
        public FluentCommandAndQueryRepository(UnitOfWorkData unitOfWorkData)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkData instance cannot be null");
            _unitOfWorkData = unitOfWorkData;
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = _unitOfWorkData };
            var dynamicCommandRepositoryFunc = commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>();
            var dynamicCommandRepositoryFuncParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = dynamicCommandRepositoryFunc.GetType(), ParameterName = "commandRepositoryFunc", ParameterValue = dynamicCommandRepositoryFunc };
            var commandRepositoryType = new ParameterTypeOverrideData { ParameterType = typeof(Type), ParameterName = "commandRepositoryType", ParameterValue = typeof(ICommandRepository<TEntity>) };
            var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(IList<CommandsAndQueriesPersistanceAndRespositoryData>), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = null };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(unitOfWorkParameterTypeOverrideData, dynamicCommandRepositoryFuncParameterTypeOverrideData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = _unitOfWorkData };
            var dynamicQueryRepositoryFunc = queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>();
            var dynamicQueryRepositoryFuncParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = dynamicQueryRepositoryFunc.GetType(), ParameterName = "commandRepositoryFunc", ParameterValue = dynamicQueryRepositoryFunc };
            var queryRepositoryType = new ParameterTypeOverrideData { ParameterType = typeof(Type), ParameterName = "queryRepositoryType", ParameterValue = typeof(IQueryableRepository<TEntity>) };
            var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(IList<CommandsAndQueriesPersistanceAndRespositoryData>), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = null };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(unitOfWorkParameterTypeOverrideData, dynamicQueryRepositoryFuncParameterTypeOverrideData, queryRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
        }
    }
}
