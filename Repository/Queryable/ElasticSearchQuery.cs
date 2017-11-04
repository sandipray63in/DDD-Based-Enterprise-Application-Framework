using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElasticsearchCRUD;
using ElasticsearchCRUD.Model;
using ElasticsearchCRUD.ContextSearch.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Sorting;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Queries;
using Domain.Base;
using Domain.Base.Aggregates;
using DomainContextsAndMaps;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Queryable
{
    public class ElasticSearchQuery<TEntity> : DisposableClass, IElasticSearchQuery<TEntity> 
        where TEntity : IQueryableAggregateRoot,IElasticSearchable
    {

        private readonly ElasticsearchContext _elasticsearchContext;
        private IQueryable<TEntity> _elasticsearchQuery;

        public ElasticSearchQuery(ElasticSearchContext esContext)
        {
            ContractUtility.Requires<ArgumentNullException>(esContext != null, "esContext instance cannot be null");
            _elasticsearchContext = esContext.ElasticsearchContext;
            _elasticsearchQuery = esContext.Query<TEntity>();
        }

        #region IQueryable<T> Members

        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchQuery<TEntity>).FullName);
            throw new NotSupportedException("This API is not supported for Elastic Search");
        }

        public IEnumerable<TEntity> GetWithRawSQL(string getQuery, params object[] parameters)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchQuery<TEntity>).FullName);
            throw new NotSupportedException("This API is not supported for Elastic Search");
        }

        #endregion

        public IEnumerable<TEntity> QueryString(string term)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchQuery<TEntity>).FullName);
            ResultDetails<SearchResult<TEntity>> results = _elasticsearchContext.Search<TEntity>(BuildQueryStringSearch(term));
            return results.PayloadResult.Hits.HitsResult.Select(t => t.Source);
        }

        public PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(ElasticSearchQuery<TEntity>).FullName);
            var result = new PagingTableResult<TEntity>();
            ResultDetails<SearchResult<TEntity>> data = _elasticsearchContext.Search<TEntity>(
                            BuildSearchForChildDocumentsWithIdAndParentType(
                                id,
                                typeof(TEntity).Name,
                                startIndex,
                                pageSize,
                                sorting)
                        );
            result.Items = data.PayloadResult.Hits.HitsResult.Select(t => t.Source);
            result.TotalCount = data.PayloadResult.Hits.Total;
            return result;
        }


        #region Facilitation for LINQ based Selects,JOINs etc from the classes, using the instance of this class
        public IEnumerator<TEntity> GetEnumerator()
        {
            return _elasticsearchQuery.AsQueryable().GetEnumerator();
        }

        public Type ElementType
        {
            get { return _elasticsearchQuery.AsQueryable().ElementType; }
        }

        public Expression Expression
        {
            get { return _elasticsearchQuery.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _elasticsearchQuery.AsQueryable().Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private Methods

        private Search BuildQueryStringSearch(string term)
        {
            string names = string.Empty;
            if (term != null)
            {
                names = term.Replace("+", " OR *");
            }
            var search = new Search
            {
                Query = new Query(new QueryStringQuery(names + "*"))
            };
            return search;
        }

        private Search BuildSearchForChildDocumentsWithIdAndParentType(object id, string type, int startIndex, int pageSize, string sorting)
        {
            var search = new Search
            {
                From = startIndex,
                Size = pageSize,
                Query = new Query(new TermQuery("_id", type + "#" + id))
            };
            string[] sorts = sorting.Split(' ');
            if (sorts.Length == 2)
            {
                var order = OrderEnum.asc;
                if (sorts[1].ToLower() == "desc")
                {
                    order = OrderEnum.desc;
                }
                search.Sort = CreateSortQuery(sorts.First().ToLower(), order);
            }
            return search;
        }

        private SortHolder CreateSortQuery(string sort, OrderEnum order)
        {
            return new SortHolder(
                new List<ISort>
                {
                    new SortStandard(sort)
                    {
                        Order = order
                    }
                }
            );
        }

        #endregion

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _elasticsearchContext.Dispose();
            _elasticsearchQuery = null;
        }

        #endregion

    }
}
