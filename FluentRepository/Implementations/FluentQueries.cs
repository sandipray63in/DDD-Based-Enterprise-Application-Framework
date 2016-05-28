using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.Abstractions;
using Infrastructure.Utilities;
using Infrastructure.Extensions;
using Repository.Base;

namespace FluentRepository.Implementations
{
    internal class FluentQueries : FluentSetUpAndExecution,IFluentQueries
    {
        internal FluentQueries(UnitOfWorkData unitOfWorkData, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList) : base(unitOfWorkData,commandsPersistanceAndRespositoryDataList)
        {
            
        }

        public IFluentQueries RunQuery<TEntity>(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc().RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentQueries RunQuery<TEntity>(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc().RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentQueries RunQuery<TEntity, TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc().RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        private CommandsAndQueriesPersistanceAndRespositoryData CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>()
            where TEntity : class, IQueryableAggregateRoot
        {
            var currentCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = currentCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc.GetUnderlyingType().GetGenericArguments().First();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("TEntity must be of type {0} " +
                " since the last repository that has been Set Up was of type {1}", expectedTEntityType.ToString(), currentCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc.GetUnderlyingType().ToString()));
            var operationsQueue = currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue;
            if (operationsQueue.IsNull())
            {
                operationsQueue = new Queue<OperationData>();
            }
            return currentCommandsAndQueriesPersistanceAndRespositoryData;
        }
    }
}
