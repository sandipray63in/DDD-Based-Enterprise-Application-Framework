using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Domain.Base.AddOnObjects;
using Repository.Command;
using Infrastructure.Utilities;

namespace Repository.Base
{
    public abstract class BaseCommandRepository<TEntity> : BaseUnitOfWorkRepository<TEntity>, ICommandRepository<TEntity> 
        where TEntity : ICommandAggregateRoot
    {
        #region Private Fields

        private ICommand<TEntity> _command;

        #endregion

        #region Constructors

        /// <summary>
        /// Should be used when unit of work instance is not required 
        /// i.e when explicit transactions management is not required
        /// </summary>
        protected BaseCommandRepository(ICommand<TEntity> command)
        {
            ContractUtility.Requires<ArgumentNullException>(command.IsNotNull(), "Command instance cannot be null");
            _command = command;
        }

        #endregion

        internal void SetCommand(dynamic command)
        {
            _command = command as ICommand<TEntity>;
        }

        internal override void ActualInsert(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            _command.Insert(item);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualUpdate(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            _command.Update(item);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualDelete(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            if (item.GetType().GetProperties().Any(x => x.GetType() == typeof(SoftDeleteableInfo)))
            {
                ActualUpdate(item);
            }
            else
            {
                _command.Delete(item);
            }
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualInsert(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            _command.Insert(itemList);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualUpdate(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            _command.Update(itemList);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualDelete(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            var softDeleteableItems = itemList.Where(x => x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo))).ToList();
            var nonSoftDeleteableItems = itemList.Where(x => !(x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo)))).ToList();
            if (softDeleteableItems.IsNotEmpty())
            {
                ActualUpdate(softDeleteableItems);
            }
            if (nonSoftDeleteableItems.IsNotEmpty())
            {
                _command.Delete(itemList);
            }
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualBulkInsert(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            _command.BulkInsert(itemList);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualBulkUpdate(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            _command.BulkUpdate(itemList);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override void ActualBulkDelete(IList<TEntity> itemList, Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            var softDeleteableItems = itemList.Where(x => x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo))).ToList();
            var nonSoftDeleteableItems = itemList.Where(x => !(x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo)))).ToList();
            if (softDeleteableItems.IsNotEmpty())
            {
                ActualBulkUpdate(softDeleteableItems);
            }
            if (nonSoftDeleteableItems.IsNotEmpty())
            {
                _command.BulkDelete(itemList);
            }
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualInsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            await _command.InsertAsync(item, token);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualUpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            await _command.UpdateAsync(item, token);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualDeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            if (item.GetType().GetProperties().Any(x => x.GetType() == typeof(SoftDeleteableInfo)))
            {
                await ActualUpdateAsync(item, token);
            }
            else
            {
                await _command.DeleteAsync(item, token);
            }
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualInsertAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            await _command.InsertAsync(itemList, token);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualUpdateAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            await _command.UpdateAsync(itemList, token);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualDeleteAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            var softDeleteableItems = itemList.Where(x => x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo))).ToList();
            var nonSoftDeleteableItems = itemList.Where(x => !(x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo)))).ToList();
            if (softDeleteableItems.IsNotEmpty())
            {
                await ActualUpdateAsync(softDeleteableItems, token);
            }
            if (nonSoftDeleteableItems.IsNotEmpty())
            {
                await _command.DeleteAsync(itemList, token);
            }
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualBulkInsertAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            await _command.BulkInsertAsync(itemList, token);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualBulkUpdateAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            await _command.BulkUpdateAsync(itemList, token);
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        internal override async Task ActualBulkDeleteAsync(IList<TEntity> itemList, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(BaseCommandRepository<TEntity>).FullName);
            var softDeleteableItems = itemList.Where(x => x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo))).ToList();
            var nonSoftDeleteableItems = itemList.Where(x => !(x.GetType().GetProperties().Any(y => y.GetType() == typeof(SoftDeleteableInfo)))).ToList();
            if (softDeleteableItems.IsNotEmpty())
            {
                await ActualBulkUpdateAsync(softDeleteableItems, token);
            }
            if (nonSoftDeleteableItems.IsNotEmpty())
            {
                await _command.BulkDeleteAsync(itemList, token);
            }
            ExecuteOperationBeforeNextOperation(operationToExecuteBeforeNextOperation);
        }

        public abstract void Insert(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        public abstract void Update(TEntity item, Action operationToExecuteBeforeNextOperation = null);

        public abstract void Delete(TEntity item, Action operationToExecuteBeforeNextOperation = null);
        public abstract void Insert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);

        public abstract void Update(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);

        public abstract void Delete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);

        public abstract void BulkInsert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);

        public abstract void BulkUpdate(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);

        public abstract void BulkDelete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null);

        public abstract Task InsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);
        public abstract Task BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        #region Private Methods

        private void ExecuteOperationBeforeNextOperation(Action operationToExecuteBeforeNextOperation = null)
        {
            if (operationToExecuteBeforeNextOperation.IsNotNull())
            {
                operationToExecuteBeforeNextOperation();
            }
        }

        #endregion

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _command.Dispose();
        }
        #endregion

    }
}
