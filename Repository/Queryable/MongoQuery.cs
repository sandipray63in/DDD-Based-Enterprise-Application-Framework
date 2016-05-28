using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBDomainMaps;
using Domain.Base.Aggregates;
using Domain.Base.Entities.Mongo;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Queryable
{
    public class MongoQuery<TId,TEntity> : DisposableClass, IQuery<TEntity>
        where TId : struct
         where TEntity : BaseMongoEntity<TId>, IQueryableAggregateRoot
    {
        private IMongoQueryable<TEntity> _queryableMongoCollection;

        public MongoQuery(MongoContext mongoContext)
        {
            ContractUtility.Requires<ArgumentNullException>(mongoContext != null, "mongoContext instance cannot be null");
            _queryableMongoCollection = mongoContext.GetMongoCollection<TId,TEntity>().AsQueryable();
        }

        /// Mongo being schemaless, doesn't require the methods below like Include and GetWithRawSql

        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<TEntity> GetWithRawSQL(string getQuery, params object[] parameters)
        {
            throw new NotSupportedException();
        }


        #region Facilitation for LINQ based Selects,JOINs etc from the classes, using the instance of this class
        public IEnumerator<TEntity> GetEnumerator()
        {
            return _queryableMongoCollection.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _queryableMongoCollection.ElementType; }
        }

        public Expression Expression
        {
            get { return _queryableMongoCollection.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _queryableMongoCollection.Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _queryableMongoCollection = null;
        }

        #endregion
    }
}
