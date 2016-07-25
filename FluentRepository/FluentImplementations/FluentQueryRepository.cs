using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Queryable;

namespace FluentRepository.FluentImplementations
{
    internal class FluentQueryRepository : FluentQueries, IFluentQueryRepository
    {

        public FluentQueryRepository(UnitOfWorkData unitOfWorkData, dynamic queryRepository, Type queryRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, queryRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository != null, "commandRepositoryFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(queryRepositoryType.IsNotNull(), "queryRepositoryType instance cannot be null");
            SetAndCheckRepositoryType(queryRepositoryType, () =>
             {
                 var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { QueryRepository = queryRepository, QueryRepositoryType = queryRepositoryType };
                 _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
             });
        }

        public IFluentQueries SetUpQueryPersistance<TEntity>(IQuery<TEntity> query)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(query.IsNotNull(), "query instance cannot be null");
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryType.GetGenericArguments().First().GetType();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", expectedTEntityType.Name, typeof(TEntity).Name));
            return new FluentQueries(_unitOfWorkData, lastCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryType, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        internal protected override void CheckRepositoryType(Type queryRepositoryType)
        {
            ContractUtility.Requires<ArgumentException>(!_commandsAndQueriesPersistanceAndRespositoryDataList
                        .Any(x => x.QueryRepositoryType == queryRepositoryType)
                        , string.Format("The repository type {0} has been already Set Up", queryRepositoryType.Name));
        }
    }
}
