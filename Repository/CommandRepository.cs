using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Infrastructure.ExceptionHandling;
using Infrastructure.UnitOfWork;
using Infrastructure.Utilities;
using Repository.Base;
using Repository.Command;

namespace Repository
{
    /// <summary>
    /// A Repository per Entity 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CommandRepository<TEntity> : BaseCommandRepository<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        protected IUnitOfWork _unitOfWork;
        private readonly IExceptionHandler _exceptionHandler;

        /// <summary>
        /// Should be used when unit of work instance is not required 
        /// i.e when explicit transactions management is not required
        /// </summary>
        public CommandRepository(ICommand<TEntity> command)
            : base(command)
        {

        }

        public CommandRepository(ICommand<TEntity> command, IExceptionHandler exceptionHandler)
            : base(command)
        {
            ContractUtility.Requires<ArgumentNullException>(exceptionHandler.IsNotNull(), "exceptionHandler instance cannot be null");
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// The same unit of work instance can be used across different instances of repositories
        /// (if needed)
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="command"></param>
        public CommandRepository(IUnitOfWork unitOfWork, ICommand<TEntity> command)
            : base(command)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork.IsNotNull(), "unitOfWork instance cannot be null");
            _unitOfWork = unitOfWork;
        }

        internal void SetUnitOfWork<TUnitOfWork>(TUnitOfWork unitOfWork)
            where TUnitOfWork : IUnitOfWork
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
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualInsert(item, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualInsert(item, operationToExecuteBeforeNextOperation),_exceptionHandler);
            }
        }
        public override void Update(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualUpdate(item, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualUpdate(item, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }
        public override void Delete(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualDelete(item, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualDelete(item, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override void Insert(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualInsert(items, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualInsert(items, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override void Update(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualUpdate(items, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualUpdate(items, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override void Delete(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualDelete(items, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualDelete(items, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override void BulkInsert(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualBulkInsert(items, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualBulkInsert(items, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override void BulkUpdate(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualBulkUpdate(items, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualBulkUpdate(items, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override void BulkDelete(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(() => ActualBulkDelete(items, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualBulkDelete(items, operationToExecuteBeforeNextOperation), _exceptionHandler);
            }
        }

        public override async Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualInsertAsync(item, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualInsertAsync(item, token, operationToExecuteBeforeNextOperation), _exceptionHandler,null);
            }
        }

        public override async Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualUpdateAsync(item, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualUpdateAsync(item, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item instance cannot be null");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualDeleteAsync(item, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualDeleteAsync(item, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task InsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualInsertAsync(items, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualInsertAsync(items, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualUpdateAsync(items, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualUpdateAsync(items, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task DeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualDeleteAsync(items, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualDeleteAsync(items, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task BulkInsertAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualBulkInsertAsync(items, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualBulkInsertAsync(items, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task BulkUpdateAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualBulkUpdateAsync(items, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualBulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        public override async Task BulkDeleteAsync(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandRepository<TEntity>).FullName);
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items count should be greater than 0");
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.AddOperation(x => ActualBulkDeleteAsync(items, token == default(CancellationToken) ? x : token, operationToExecuteBeforeNextOperation));
            }
            else
            {
                //TODO - proper exception handling compensating handler needs to be here
                await ExceptionWithNullCheckUtility.HandleExceptionWithNullCheck(() => ActualBulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation), _exceptionHandler, null);
            }
        }

        #endregion

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            if (_unitOfWork.IsNotNull())
            {
                var dynamicUnitOfWork = (dynamic)_unitOfWork;
                dynamicUnitOfWork.Dispose();
            }
        }

        #endregion
    }
}
