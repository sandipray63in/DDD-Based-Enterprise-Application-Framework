using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Base.Aggregates;
using Infrastructure.ExceptionHandling.RetryBasedExceptionHandling;
using Infrastructure.UnitOfWork;
using Infrastructure.Utilities;
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

        public QueryableElasticSearchRepository(IElasticSearchQuery<TEntity> queryable, IRetryBasedExceptionHandler retryBasedExceptionHandler)
            : base(queryable,retryBasedExceptionHandler)
        {
        }

        public QueryableElasticSearchRepository(IUnitOfWork unitOfWork, IElasticSearchQuery<TEntity> queryable) : base(unitOfWork,queryable)
        {

        }

        #endregion

        public PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null)
        {
            PagingTableResult<TEntity> pagingTableResult =  (_queryable as IElasticSearchQuery<TEntity>).GetAllPagedResult(id, startIndex, pageSize, sorting);
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

        public IEnumerable<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null)
        {
            IEnumerable<TEntity> entities = (_queryable as IElasticSearchQuery<TEntity>).QueryString(term);
            if (operationToExecuteBeforeNextOperation.IsNotNull())
            {
                operationToExecuteBeforeNextOperation();
            }
            return entities;
        }

        public async Task<IEnumerable<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            token.ThrowIfCancellationRequested();
            return await Task.Run(() => this.QueryString(term, operationToExecuteBeforeNextOperation));
        }
    }
}
