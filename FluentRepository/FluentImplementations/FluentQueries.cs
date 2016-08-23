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

        public FluentQueries(UnitOfWorkData unitOfWorkData, IList<dynamic> repositoriesList, Queue<OperationData> operationsQueue) : base(unitOfWorkData, repositoriesList, operationsQueue)
        {
        }

        public IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            return GetFluentQueriesAfterSettingQueryRepositoryAndPersistanceQueueData<TEntity>(x => x.RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation));
        }

        public IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            return GetFluentQueriesAfterSettingQueryRepositoryAndPersistanceQueueData<TEntity>(x => x.RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation));
        }

        public IFluentQueries Query<TEntity, TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation cannot be null");
            return GetFluentQueriesAfterSettingQueryRepositoryAndPersistanceQueueData<TEntity>(x => x.RunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation));
        }

        private IFluentQueries GetFluentQueriesAfterSettingQueryRepositoryAndPersistanceQueueData<TEntity>(Action<dynamic> queryRepositoryAction)
            where TEntity : class, IQueryableAggregateRoot
        {
            var queryableRepositoryTypeName = typeof(IQueryableRepository<>).Name;
            var queryRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(queryableRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(queryRepository != null, string.Format("No Query Repository has been set up for {0}.", typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => queryRepositoryAction(queryRepository) };
            _operationsQueue.Enqueue(operationData);
            return this;
        }
    }
}
