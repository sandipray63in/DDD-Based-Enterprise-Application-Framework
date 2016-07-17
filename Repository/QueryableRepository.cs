using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Repository.Base;
using Repository.Queryable;
using Repository.UnitOfWork;
using Infrastructure;
using Infrastructure.Utilities;

namespace Repository
{
    public class QueryableRepository<TEntity>
        : DisposableClass, IQueryableRepository<TEntity> 
        where TEntity : IQueryableAggregateRoot
    {
        #region Private Fields

        protected readonly IQuery<TEntity> _queryable;
        private readonly BaseUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public QueryableRepository(IQuery<TEntity> queryable)
        {
            ContractUtility.Requires<ArgumentNullException>(queryable.IsNotNull(), "queryable instance cannot be null");
            _queryable = queryable;
        }

        public QueryableRepository(BaseUnitOfWork unitOfWork, Queryable.IQuery<TEntity> queryable)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork.IsNotNull(), "unitOfWork instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(queryable.IsNotNull(), "queryable instance cannot be null");
            _unitOfWork = unitOfWork;
            _queryable = queryable;
        }

        #endregion

        public virtual void RunQuery(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation instance cannot be null");
            Action operation = () =>
                {
                    var queryReturnValue = queryableRepositoryOperation(this);
                    if (operationToExecuteBeforeNextOperation.IsNotNull())
                    {
                        operationToExecuteBeforeNextOperation(queryReturnValue);
                    }
                };
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterQueryOperation(operation);
            }
            else
            {
                operation();
            }
        }

        public virtual void RunQuery(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation instance cannot be null");
            Action operation = () =>
                {
                    var queryReturnValue = queryableRepositoryOperation(this);
                    if (operationToExecuteBeforeNextOperation.IsNotNull())
                    {
                        operationToExecuteBeforeNextOperation(queryReturnValue);
                    }
                };
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterQueryOperation(operation);
            }
            else
            {
                operation();
            }
        }

        public virtual void RunQuery<TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(queryableRepositoryOperation.IsNotNull(), "queryableRepositoryOperation instance cannot be null");
            Action operation = () =>
            {
                var queryReturnValue = queryableRepositoryOperation(this);
                if (operationToExecuteBeforeNextOperation.IsNotNull())
                {
                    operationToExecuteBeforeNextOperation(queryReturnValue);
                }
            };
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterQueryOperation(operation);
            }
            else
            {
                operation();
            }
        }

        public virtual IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            ContractUtility.Requires<ArgumentNullException>(subSelector.IsNotNull(), "subSelector instance cannot be null");
            return _queryable.Include(subSelector);
        }

        public virtual IEnumerable<TEntity> GetWithRawSQL(string query, params object[] parameters)
        {
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
