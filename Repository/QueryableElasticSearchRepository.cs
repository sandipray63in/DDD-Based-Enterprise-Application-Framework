using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Base.Aggregates;
using Infrastructure.UnitOfWork;
using Repository.Base;
using Repository.Queryable;

namespace Repository
{
    public class QueryableElasticSearchRepository<TEntity> : QueryableRepository<TEntity>,IQueryableElasticSearchRepository<TEntity> 
        where TEntity : IQueryableAggregateRoot, IElasticSearchable
    {
        #region Constructors

        public QueryableElasticSearchRepository(IElasticSearchQuery<TEntity> queryable) : base(queryable)
        {

        }

        public QueryableElasticSearchRepository(IUnitOfWork unitOfWork, IElasticSearchQuery<TEntity> queryable) : base(unitOfWork,queryable)
        {

        }

        #endregion

        public PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null)
        {
            var pagingTableResult =  (_queryable as IElasticSearchQuery<TEntity>).GetAllPagedResult(id, startIndex, pageSize, sorting);
            if (operationToExecuteBeforeNextOperation.IsNotNull())
            {
                operationToExecuteBeforeNextOperation();
            }
            return pagingTableResult;
        }

        public async Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            token.ThrowIfCancellationRequested();
            return await Task.Run(() => this.GetAllPagedResult(id, startIndex, pageSize, sorting, operationToExecuteBeforeNextOperation));
        }

        public IList<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null)
        {
            var entities = (_queryable as IElasticSearchQuery<TEntity>).QueryString(term);
            if (operationToExecuteBeforeNextOperation.IsNotNull())
            {
                operationToExecuteBeforeNextOperation();
            }
            return entities;
        }

        public async Task<IList<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            token.ThrowIfCancellationRequested();
            return await Task.Run(() => this.QueryString(term, operationToExecuteBeforeNextOperation));
        }
    }
}
