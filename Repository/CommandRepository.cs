using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Repository.Base;
using Repository.Command;
using Repository.UnitOfWork;
using Infrastructure.Utilities;

namespace Repository
{
    /// <summary>
    /// A Repository per Entity 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CommandRepository<TEntity> : BaseCommandRepository<TEntity> where TEntity : class,ICommandAggregateRoot
    {
        protected BaseUnitOfWork _unitOfWork;

        /// <summary>
        /// Should be used when unit of work instance is not required 
        /// i.e when explicit transactions management is not required
        /// </summary>
        public CommandRepository(ICommand<TEntity> command)
            : base(command)
        {
           
        }

        /// <summary>
        /// The same unit of work instance can be used across different instances of repositories
        /// (if needed)
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="command"></param>
        public CommandRepository(BaseUnitOfWork unitOfWork, ICommand<TEntity> command)
            : base(command)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork.IsNotNull(), "unitOfWork instance cannot be null");
            _unitOfWork = unitOfWork;
        }

        internal void SetUnitOfWork<TUnitOfWork>(TUnitOfWork unitOfWork)
            where TUnitOfWork : BaseUnitOfWork
        {
            _unitOfWork = unitOfWork;
        }

        #region ICommandRepository<T> Members
        /// <summary>
        /// While using this API alongwith unit of work instance, this API's return value should 
        /// not be used as the actual return value for the commit.But rather the internal unit of work's 
        /// Commit method's return value should be used.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override void Insert(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterInsert(item, this);
            }
            else
            {
                ActualInsert(item);
            }
        }
        public override void Update(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterUpdate(item, this);
            }
            else
            {
                ActualUpdate(item);
            }
        }
        public override void Delete(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterDelete(item, this);
            }
            else
            {
                ActualDelete(item);
            }
        }

        public override void Insert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterInsertList(items, this);
            }
            else
            {
                ActualInsert(items);
            }
        }

        public override void Update(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterUpdateList(items, this);
            }
            else
            {
                ActualUpdate(items);
            }
        }

        public override void Delete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterDeleteList(items, this);
            }
            else
            {
                ActualDelete(items);
            }
        }

        public override void BulkInsert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterBulkInsertList(items, this);
            }
            else
            {
                ActualBulkInsert(items);
            }
        }

        public override void BulkUpdate(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterBulkUpdateList(items, this);
            }
            else
            {
                ActualBulkUpdate(items);
            }
        }

        public override void BulkDelete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterBulkDeleteList(items, this);
            }
            else
            {
                ActualBulkDelete(items);
            }
        }
        
        public override async Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterInsertForAsync(item, this);
            }
            else
            {
                await ActualInsertAsync(item, token);
            }
        }
        
        public override async Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterUpdateForAsync(item, this);
            }
            else
            {
                await ActualUpdateAsync(item, token);
            }
        }
        
        public override async Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterDeleteForAsync(item, this);
            }
            else
            {
                await ActualDeleteAsync(item, token);
            }
        }
        
        public override async Task InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterInsertListForAsync(items, this);
            }
            else
            {
                await ActualInsertAsync(items, token);
            }
        }
        
        public override async Task UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterUpdateListForAsync(items, this);
            }
            else
            {
                await ActualUpdateAsync(items, token);
            }
        }
        
        public override async Task DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterDeleteListForAsync(items, this);
            }
            else
            {
                await ActualDeleteAsync(items, token);
            }
        }
        
        public override async Task BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterBulkInsertListForAsync(items, this);
            }
            else
            {
                await ActualBulkInsertAsync(items, token);
            }
        }
        
        public override async Task BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterBulkUpdateListForAsync(items, this);
            }
            else
            {
                await ActualBulkUpdateAsync(items, token);
            }
        }
        
        public override async Task BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.RegisterBulkDeleteListAsync(items, this);
            }
            else
            {
                await ActualBulkDeleteAsync(items, token);
            }
        }

        #endregion

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.Dispose();
            }
        }

        #endregion
    }
}

