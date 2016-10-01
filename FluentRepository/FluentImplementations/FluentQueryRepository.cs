using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;
using Repository.Queryable;

namespace FluentRepository.FluentImplementations
{
    internal class FluentQueryRepository : FluentQueries, IFluentQueryRepository
    {
        public FluentQueryRepository(UnitOfWorkData unitOfWorkData, IList<dynamic> repositoriesList, Queue<OperationData> operationsQueue) : base(unitOfWorkData, repositoriesList, operationsQueue)
        {
        }

        public IFluentQueries SetUpQueryPersistance<TEntity>(IQuery<TEntity> query)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(query.IsNotNull(), "query instance cannot be null");
            var queryableRepositoryTypeName = typeof(IQueryableRepository<>).Name;
            var queryRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(queryableRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(queryRepository != null, string.Format("No Query Repository has been set up for {0}.", typeof(TEntity).Name));
            queryRepository.SetCommand(query);
            return new FluentQueries(_unitOfWorkData, _repositoriesList,_operationsQueue);
        }

        public IFluentQueries SetUpQueryPersistance(params dynamic[] queries)
        {
            return SetUpQueryPersistance(queries.ToList());
        }

        public IFluentQueries SetUpQueryPersistance(IList<dynamic> queries)
        {
            throw new NotImplementedException();
        }
    }
}
