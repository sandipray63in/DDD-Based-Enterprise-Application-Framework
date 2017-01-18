using System;
using System.Collections.Generic;
using Domain.Base.Aggregates;
using Infrastructure;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;
using Repository.Base;

namespace DomainServices.Base.CommandDomainServices
{
    public class CommandDomainService<TEntity> : DisposableClass, ICommandDomainService<TEntity> where TEntity : ICommandAggregateRoot
    {
        protected readonly ICommandRepository<TEntity> _repository;

        protected readonly ILogger logger;

        public CommandDomainService(ICommandRepository<TEntity> repository, ILogger logger)
        {
            ContractUtility.Requires<ArgumentNullException>(repository != null, "repository instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(logger != null, "logger instance cannot be null");
            _repository = repository;
            this.logger = logger;
        }
        
        public virtual bool Insert(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.Insert(item, operationToExecuteBeforeNextOperation));
        }

        public virtual bool Update(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.Update(item, operationToExecuteBeforeNextOperation));
        }

        public virtual bool Delete(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.Delete(item, operationToExecuteBeforeNextOperation));
        }

        public virtual bool Insert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.Insert(items, operationToExecuteBeforeNextOperation));
        }

        public virtual bool Update(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.Update(items, operationToExecuteBeforeNextOperation));
        }

        public virtual bool Delete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.Delete(items, operationToExecuteBeforeNextOperation));
        }

        public virtual bool BulkInsert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.BulkInsert(items, operationToExecuteBeforeNextOperation));
        }

        public virtual bool BulkUpdate(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.BulkUpdate(items, operationToExecuteBeforeNextOperation));
        }

        public virtual bool BulkDelete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainService<TEntity>).FullName);
            return InvokeAfterWrappingWithinExceptionHandling(() => _repository.BulkDelete(items, operationToExecuteBeforeNextOperation));
        }

        protected bool InvokeAfterWrappingWithinExceptionHandling(Action repositoryAction)
        {
            try
            {
                repositoryAction();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                return false;
            }
        }

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _repository.Dispose();
        }

        #endregion
    }
}
