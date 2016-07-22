using System;
using System.Collections.Generic;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentQueries : FluentSetUpAndExecution,IFluentQueries
    {

        /// <summary>
        /// For proper working of this class alongwith Unity(for regstration), the constructor needs to be public.
        /// </summary>
        /// <param name="unitOfWorkData"></param>
        /// <param name="commandsPersistanceAndRespositoryDataList"></param>
        public FluentQueries(UnitOfWorkData unitOfWorkData, Type queryRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList) : base(unitOfWorkData, queryRepositoryType, commandsPersistanceAndRespositoryDataList)
        {
        }

        public IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc().RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc().RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentQueries Query<TEntity, TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc().RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
    }
}
