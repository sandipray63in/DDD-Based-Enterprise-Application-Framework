using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.Queryable;

namespace FluentRepository.FluentImplementations
{
    internal class FluentQueryRepository : FluentQueries, IFluentQueryRepository
    {
        /// <summary>
        /// For proper working of this class alongwith Unity(for regstration), the constructor needs to be public.
        /// </summary>
        /// <param name="unitOfWorkData"></param>
        /// <param name="queryRepositoryFunc"></param>
        /// <param name="commandsAndQueriesPersistanceAndRespositoryDataList"></param>
        public FluentQueryRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> queryRepositoryFunc, Type queryRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, queryRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(queryRepositoryType.IsNotNull(), "queryRepositoryType instance cannot be null");
            SetAndCheckRepositoryType(queryRepositoryType, () =>
             {
                 var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { QueryRepositoryFunc = queryRepositoryFunc, QueryRepositoryType = queryRepositoryType };
                 _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
             });
        }

        public IFluentQueries SetUpQueryPersistance<TEntity>(Func<IQuery<TEntity>> queryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryType;
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", expectedTEntityType.Name, typeof(TEntity).Name));
            lastCommandsAndQueriesPersistanceAndRespositoryData.QueryPersistanceFunc = queryFunc.ConvertFunc<IQuery<TEntity>, dynamic>();
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = _unitOfWorkData };
            var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _commandsAndQueriesPersistanceAndRespositoryDataList.GetType(), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = _commandsAndQueriesPersistanceAndRespositoryDataList };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueries, FluentQueries>(unitOfWorkParameterTypeOverrideData, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
        }

        internal protected override void CheckRepositoryType(Type queryRepositoryType)
        {
            ContractUtility.Requires<ArgumentException>(!_commandsAndQueriesPersistanceAndRespositoryDataList
                        .Any(x => x.QueryRepositoryType == queryRepositoryType)
                        , string.Format("The repository type {0} has been already Set Up", queryRepositoryType.Name));
        }
    }
}
