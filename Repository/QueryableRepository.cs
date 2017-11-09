using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Infrastructure;
using Infrastructure.ExceptionHandling.RetryBasedExceptionHandling;
using Infrastructure.Utilities;
using Infrastructure.UnitOfWork;
using Repository.Base;
using Repository.Queryable;

namespace Repository
{
    public class QueryableRepository<TEntity>
        : DisposableClass, IQueryableRepository<TEntity> 
        where TEntity : IQueryableAggregateRoot
    {
        #region Private Fields

        protected IQuery<TEntity> _queryable;
        private IUnitOfWork _unitOfWork;
        private readonly IRetryBasedExceptionHandler _retryBasedExceptionHandler;

        #endregion

        #region Constructors

        public QueryableRepository(IQuery<TEntity> queryable)
        {
            ContractUtility.Requires<ArgumentNullException>(queryable.IsNotNull(), "queryable instance cannot be null");
            _queryable = queryable;
        }

        public QueryableRepository(IQuery<TEntity> queryable, IRetryBasedExceptionHandler retryBasedExceptionHandler)
            : this(queryable)
        {
            ContractUtility.Requires<ArgumentNullException>(retryBasedExceptionHandler.IsNotNull(), "retryBasedExceptionHandler instance cannot be null");
            _retryBasedExceptionHandler = retryBasedExceptionHandler;
            _retryBasedExceptionHandler.SetIsTransientFunc(x => false); //TODO - Need to set the transientFunc properly
        }

        public QueryableRepository(IUnitOfWork unitOfWork, Queryable.IQuery<TEntity> queryable)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork.IsNotNull(), "unitOfWork instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(queryable.IsNotNull(), "queryable instance cannot be null");
            _unitOfWork = unitOfWork;
            _queryable = queryable;
        }

        #endregion

        internal void SetQuery(dynamic query)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableRepository<TEntity>).FullName);
            _queryable = query as IQuery<TEntity>;
        }

        internal void SetUnitOfWork<TUnitOfWork>(TUnitOfWork unitOfWork)
            where TUnitOfWork : IUnitOfWork
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableRepository<TEntity>).FullName);
            _unitOfWork = unitOfWork;
        }

        public virtual void RunQuery(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
        {
            HandleRunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation);
        }

        public virtual void RunQuery(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
        {
            HandleRunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation);
        }

        public virtual void RunQuery<TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation = null)
        {
            HandleRunQuery(queryableRepositoryOperation, operationToExecuteBeforeNextOperation);
        }

        private void HandleRunQuery<TNextActionType>(Func<IQueryableRepository<TEntity>, TNextActionType> queryableRepositoryOperation, Action<TNextActionType> operationToExecuteBeforeNextOperation)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation instance cannot be null");
            Action operation = () =>
            {
                TNextActionType queryReturnValue = _unitOfWork.IsNull()? 
                                                    RetryWithNullCheckUtility.FireRetryWithNullCheck(
                                                        ()=> queryableRepositoryOperation(this),null,_retryBasedExceptionHandler)
                                                   : queryableRepositoryOperation(this);
                                                   //TODO - proper exception handling compensating handler needs to be here
                if (operationToExecuteBeforeNextOperation.IsNotNull())
                {
                    operationToExecuteBeforeNextOperation(queryReturnValue);
                }
            };
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(operation);
            }
            else
            {
                operation();
            }
        }

        public virtual IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(subSelector.IsNotNull(), "subSelector instance cannot be null");
            //TODO - proper exception handling compensating handler needs to be here
            return RetryWithNullCheckUtility.FireRetryWithNullCheck(() => _queryable.Include(subSelector), null, _retryBasedExceptionHandler);
        }

        public virtual IEnumerable<TEntity> GetWithRawSQL(string query, params object[] parameters)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(!query.IsNullOrWhiteSpace(), "query instance cannot be null or empty");
            return _queryable.GetWithRawSQL(query, parameters);
        }

        #region Facilitation for LINQ based Selects,JOINs etc from the classes, using the instance of this class
        public IEnumerator<TEntity> GetEnumerator()
        {
            return (_queryable as IQueryable<TEntity>).GetEnumerator();
        }

        public Type ElementType
        {
            get { return (_queryable as IQueryable<TEntity>).ElementType; }
        }

        public Expression Expression
        {
            get { return (_queryable as IQueryable<TEntity>).Expression; }
        }

        public IQueryProvider Provider
        {
            get { return (_queryable as IQueryable<TEntity>).Provider; }
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
            _queryable.Dispose();
        }

        #endregion
    }
}
