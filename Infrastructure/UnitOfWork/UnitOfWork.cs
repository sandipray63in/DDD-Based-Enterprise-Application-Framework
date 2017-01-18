using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : DisposableClass, IUnitOfWork
    {
        private readonly ILogger logger;
        private TransactionScope _scope;
        private readonly IsolationLevel _isoLevel;
        private readonly TransactionScopeOption _scopeOption;
        private Queue<OperationData> _operationsQueue;

        public UnitOfWork(IsolationLevel isoLevel = IsolationLevel.ReadCommitted, TransactionScopeOption scopeOption = TransactionScopeOption.RequiresNew, ILogger logger = null)
        {
            ContractUtility.Requires<ArgumentNullException>(logger.IsNotNull(), "logger instance cannot be null");
            _isoLevel = isoLevel;
            _scopeOption = scopeOption;
            _operationsQueue = new Queue<OperationData>();
        }

        /// <summary>
        /// Change the build Mode to "Test" to execute all "#if TEST" sections
        /// </summary>
#if TEST
        //Ideally, Action delegate should be used here but Unity Container doesn't suport Action delegate and only
        //supports Func<T> or Func<IEnumerable<T>> and that's why using Func<bool>
        //(since that's seems to be the simplest Func supported by Unity Container)
        private readonly Func<bool> _throwExceptionActionToTestRollback;
        private bool _isProcessDataMethodExecutedAtleastOnce;

        public UnitOfWork(Func<bool> throwExceptionActionToTestRollback, IsolationLevel isoLevel = IsolationLevel.ReadCommitted, TransactionScopeOption scopeOption = TransactionScopeOption.RequiresNew) : this(isoLevel, scopeOption,LoggerFactory.GetLogger(LoggerType.Default))
        {
            _throwExceptionActionToTestRollback = throwExceptionActionToTestRollback;
        }
#endif

        public void AddOperation(Action operation)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(UnitOfWork).FullName);
            _operationsQueue.Enqueue(new OperationData { Operation = operation });
        }

        public void AddOperation(Func<CancellationToken, Task> asyncOperation)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(UnitOfWork).FullName);
            _operationsQueue.Enqueue(new OperationData { AsyncOperation = asyncOperation });
        }

        /// <summary>
        /// Comits all the data within this unit of work instance in an atomic way i.e. all or none get transacted.
        /// Order of operations of different instances of same type or different types needs to be handled at 
        /// the Business or Service Layer.
        /// </summary>
        /// <param name="shouldAutomaticallyRollBackOnTransactionException">when set to true(default value) 
        /// the RollBack method need not be called from the consumer class</param>
        public void Commit(bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(UnitOfWork).FullName);
            ContractUtility.Requires<ArgumentOutOfRangeException>(_operationsQueue.Count > 0, "Atleast one operation must be there to be executed.");

            ContractUtility.Requires<NotSupportedException>(_operationsQueue.All(x => x.AsyncOperation.IsNull()),
                                    "Async operations are not supported by Commit method.Use CommitAsync instead.");

            _scope = TransactionUtility.GetTransactionScope(_isoLevel, _scopeOption);
            try
            {
                while (_operationsQueue.Count > 0)
                {
#if TEST
                    ThrowExceptionForRollbackCheck();
#endif
                    var operationData = _operationsQueue.Dequeue();
                    if (operationData.Operation.IsNotNull())
                    {
                        operationData.Operation();
                    }
                }
                CompleteScope(() =>
                {
                    _scope.Complete(); // this just completes the transaction.Not yet committed here.
                    _scope.Dispose();  // After everthing runs successfully within the transaction 
                                       // and after completion, this should be called to actually commit the data 
                                       // within the transaction scope.
                }, shouldAutomaticallyRollBackOnTransactionException, shouldThrowOnException);
            }
            catch (Exception ex)
            {
                //Although ex is not exactly a commit exception but still passing it to reuse Rollback method.Using the Rollback
                //method to reuse exception handling and dispose the transaction scope object(else it can cause issues for the 
                //future transactions).
                Rollback(ex);
            }
        }

        /// <summary>
        /// This method can run synchronous as well as asynchronous operations within a transaction
        /// in the order in which the operations are written in the consumer class.Here synchronous 
        /// as well as asynchronous operations are hadnled since "Task" also inherently handles both 
        /// synchronous and asynchronous scenarios.
        /// 
        /// </summary>
        /// <param name="shouldCommitSynchronousOperationsFirst"></param>
        /// <param name="shouldAutomaticallyRollBackOnTransactionException"></param>
        /// <param name="shouldThrowOnException"></param>
        /// <returns></returns>
        public async Task CommitAsync(CancellationToken token = default(CancellationToken), bool shouldAutomaticallyRollBackOnTransactionException = true, bool shouldThrowOnException = true)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(UnitOfWork).FullName);
            ContractUtility.Requires<ArgumentOutOfRangeException>(_operationsQueue.Count > 0, "Atleast one operation must be there to be executed.");
            ContractUtility.Requires<NotSupportedException>(_operationsQueue.Any(x => x.AsyncOperation.IsNotNull()),
                "If CommitAsync method is used,there needs to be atleast one async operation exceuted." +
                "Please use Commit method(instead of CommitAsync) if there is not " +
                "a single async operation.");

            _scope = TransactionUtility.GetTransactionScope(_isoLevel, _scopeOption, true);
            try
            {
                while (_operationsQueue.Count > 0)
                {
#if TEST
                    ThrowExceptionForRollbackCheck();
#endif
                    var operationData = _operationsQueue.Dequeue();
                    if (operationData.Operation.IsNotNull())
                    {
                        operationData.Operation();
                    }
                    else if (operationData.AsyncOperation.IsNotNull())
                    {
                        await operationData.AsyncOperation(token);
                    }
                }
                CompleteScope(() =>
                {
                    _scope.Complete(); // this just completes the transaction.Not yet committed here.
                    _scope.Dispose();  // After everthing runs successfully within the transaction 
                                       // and after completion, this should be called to actually commit the data 
                                       // within the transaction scope.
                }, shouldAutomaticallyRollBackOnTransactionException, shouldThrowOnException);
            }
            catch (Exception ex)
            {
                //Although ex is not exactly a commit exception but still passing it to reuse Rollback method.Using the Rollback
                //method to reuse exception handling and dispose the transaction scope object(else it can cause issues for the 
                //future transactions).
                Rollback(ex);
            }
        }

        /// <summary>
        /// Explicitly call the Dispose method in a Rollback method and call this Rollback method on transaction exceptions if required.
        /// </summary>
        public void Rollback(Exception commitException = null, bool shouldThrowOnException = true)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(UnitOfWork).FullName);
            try
            {
                if (_scope.IsNotNull())
                {
                    _scope.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (shouldThrowOnException)
                {
                    var rollBackException = new Exception("Rollback failed for the current transaction.Please check inner exception.", ex);
                    if (commitException.IsNull())
                    {
                        logger.LogException(rollBackException);
                        throw rollBackException;
                    }
                    else
                    {
                        var exceptionOccurredWhileCommitting = new Exception("Commit failed for the current transaction.Please check inner exception.", commitException);
                        var commitAndRollbackException = new AggregateException("Both commit and rollback failed for the current transaction.Please check inner exceptions.", exceptionOccurredWhileCommitting, rollBackException);
                        logger.LogException(commitAndRollbackException);
                        throw commitAndRollbackException;
                    }
                }
                else
                {
                    logger.LogException(ex);
                }
            }
        }


        #region Private Methods

        private void CompleteScope(Action action, bool shouldAutomaticallyRollBackOnTransactionException, bool shouldThrowOnException)
        {
            if (shouldAutomaticallyRollBackOnTransactionException)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Rollback();
                    logger.LogException(ex);
                    if (shouldThrowOnException)
                    {
                        throw new Exception("Commit failed for the current transaction.Please check inner exception", ex);
                    }
                }
            }
            else
            {
                action();
            }
        }

#if TEST
        private void ThrowExceptionForRollbackCheck()
        {
            if (!_isProcessDataMethodExecutedAtleastOnce)
            {
                _isProcessDataMethodExecutedAtleastOnce = true;
            }
            else
            {
                if (_throwExceptionActionToTestRollback.IsNotNull())
                {
                    _throwExceptionActionToTestRollback();
                }
            }
        }
#endif

        #endregion Private Methods

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            if (_operationsQueue.IsNotNull())
            {
                _operationsQueue.Clear();
                _operationsQueue = null;
            }
        }

        #endregion
    }
}
