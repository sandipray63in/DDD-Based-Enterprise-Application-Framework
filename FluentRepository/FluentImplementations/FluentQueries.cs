using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentQueries : FluentSetUpAndExecution,IFluentQueries
    {
        protected IList<dynamic> _queryRepositories;

        public FluentQueries(UnitOfWorkData unitOfWorkData, IList<dynamic> queryRepositories, IList<dynamic> repositoriesList, Queue<OperationData> operationsQueue) : base(unitOfWorkData, repositoriesList, operationsQueue)
        {
            _queryRepositories = queryRepositories;
        }

        public IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation));
        }

        public IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation));
        }

        public IFluentQueries Query<TEntity, TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation));
        }

        private IFluentQueries GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(Action<dynamic> commandRepositoryAction)
            where TEntity : class, IQueryableAggregateRoot
        {
            var queryRepository = _queryRepositories.SingleOrDefault(x => x != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(queryRepository != null, string.Format("Last Query Repository has not been set up for {0}.", typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => commandRepositoryAction(queryRepository) };
            _operationsQueue.Enqueue(operationData);
            _isAnyOperationExecutedLast = true;
            return this;
        }
    }
}
