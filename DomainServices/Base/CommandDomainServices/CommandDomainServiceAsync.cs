using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Infrastructure;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;
using Repository.Base;

namespace DomainServices.Base.CommandDomainServices
{
    public class CommandDomainServiceAsync<TEntity> : DisposableClass, ICommandDomainServiceAsync<TEntity> where TEntity : ICommandAggregateRoot
    {
        protected readonly ICommandRepository<TEntity> _repository;

        protected readonly ILogger logger;

        public CommandDomainServiceAsync(ICommandRepository<TEntity> repository, ILogger logger)
        {
            ContractUtility.Requires<ArgumentNullException>(repository != null, "repository instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(logger != null, "logger instance cannot be null");
            _repository = repository;
            this.logger = logger;
        }

        public virtual async Task<bool> InsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.InsertAsync(item, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.UpdateAsync(item, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.DeleteAsync(item, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.InsertAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.UpdateAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.DeleteAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public virtual async Task<bool> BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(CommandDomainServiceAsync<TEntity>).FullName);
            return await InvokeAfterWrappingWithinExceptionHandling(async () => await _repository.BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        protected async Task<bool> InvokeAfterWrappingWithinExceptionHandling(Action repositoryAction)
        {
            try
            {
                await Task.Run(repositoryAction);
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
