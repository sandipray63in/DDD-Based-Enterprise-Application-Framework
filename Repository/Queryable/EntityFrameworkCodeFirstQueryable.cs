using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository.Queryable
{
    /// <summary>
    /// Please keep a note that DBContext is not thread safe and so be cautious while using EF DB Context in multi threaded scenarios
    /// as indicated here - http://mehdi.me/ambient-dbcontext-in-ef6/
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityFrameworkCodeFirstQueryable<TEntity> : DisposableClass, IQuery<TEntity> where TEntity : class, IQueryableAggregateRoot
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public EntityFrameworkCodeFirstQueryable(DbContext dbContext)
        {
            ContractUtility.Requires<ArgumentNullException>(dbContext.IsNotNull(), "dbContext instance cannot be null");
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region IQueryable<T> Members

        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            ContractUtility.Requires<ArgumentNullException>(subSelector.IsNotNull(), "subSelector instance cannot be null");
            return _dbSet.Include(subSelector);
        }

        public IEnumerable<TEntity> GetWithRawSQL(string getQuery, params object[] parameters)
        {
            ContractUtility.Requires<ArgumentNullException>(getQuery.IsNotNullOrWhiteSpace(), "getQuery instance cannot be null or empty");
            return _dbContext.Database.SqlQuery<TEntity>(getQuery, parameters);
        }

        #endregion

        #region Facilitation for LINQ based Selects,JOINs etc from the classes, using the instance of this class
        public IEnumerator<TEntity> GetEnumerator()
        {
            return _dbSet.AsQueryable().GetEnumerator();
        }

        public Type ElementType
        {
            get { return _dbSet.AsQueryable().ElementType; }
        }

        public Expression Expression
        {
            get { return _dbSet.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _dbSet.AsQueryable().Provider; }
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
            _dbContext.Dispose();
        }

        #endregion
    }
}
