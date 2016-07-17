using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Repository.Queryable;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace FluentRepository.FluentImplementations
{
    internal class FluentQueryRepository : FluentQueries, IFluentQueryRepository
    {
        private Func<dynamic> _queryRepositoryFunc;

        internal FluentQueryRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> queryRepositoryFunc, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
            _queryRepositoryFunc = queryRepositoryFunc;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsAndQueriesPersistanceAndRespositoryDataList;
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotNull())
            {
                var commandsAndQueriesPersistanceAndRespositoryDataNotNullList = _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepositoryFunc.IsNotNull());
                if (commandsAndQueriesPersistanceAndRespositoryDataNotNullList.IsNotEmpty())
                {
                    ContractUtility.Requires<ArgumentException>(!commandsAndQueriesPersistanceAndRespositoryDataNotNullList
                         .Any(x => x.QueryRepositoryFunc.GetUnderlyingType() == _queryRepositoryFunc.GetUnderlyingType())
                         , string.Format("The repository type {0} has been already Set Up", _queryRepositoryFunc.GetUnderlyingType().ToString()));
                }
            }
            SetUpCommandsAndQueriesPersistanceAndRespositoryDataList();
        }

        public IFluentQueries SetUpQueryPersistance<TEntity>(Func<IQuery<TEntity>> queryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().GetGenericArguments().First();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("TEntity must be of type {0} since the" +
                "last repository that has been Set Up was of type {1}", expectedTEntityType.ToString(), lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().ToString()));
            lastCommandsAndQueriesPersistanceAndRespositoryData.QueryPersistanceFunc = queryFunc.ConvertFunc<IQuery<TEntity>, dynamic>();
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueries, FluentQueries>(_unitOfWorkData, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        private void SetUpCommandsAndQueriesPersistanceAndRespositoryDataList()
        {
            var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { QueryRepositoryFunc = _queryRepositoryFunc };
            _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
        }
    }
}
