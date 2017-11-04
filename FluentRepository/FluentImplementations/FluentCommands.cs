using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommands : FluentSetUpAndExecution,IFluentCommands
    {

        public FluentCommands(UnitOfWorkData unitOfWorkData, IEnumerable<dynamic> repositoriesEnumerable, Queue<OperationData> operationsQueue) : base(unitOfWorkData, repositoriesEnumerable, operationsQueue)
        {
        }

        public IFluentCommands Insert<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.Insert(item, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands Update<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.Update(item, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands Delete<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.Delete(item, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands Insert<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.Insert(items, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands Update<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.Update(items, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands Delete<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.Delete(items, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands BulkInsert<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.BulkInsert(items, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands BulkUpdate<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.BulkUpdate(items, operationToExecuteBeforeNextOperation));
       }

        public IFluentCommands BulkDelete<TEntity>(IEnumerable<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(x => x.BulkDelete(items, operationToExecuteBeforeNextOperation));
        }
        
        public IFluentCommands InsertAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.InsertAsync(item, token, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands UpdateAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.UpdateAsync(item, token, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands DeleteAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.DeleteAsync(item, token, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands InsertAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.InsertAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands UpdateAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.UpdateAsync(items, token, operationToExecuteBeforeNextOperation));
        }
        
        public IFluentCommands DeleteAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.DeleteAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands BulkInsertAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        public IFluentCommands BulkUpdateAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation));
        }
        
        public IFluentCommands BulkDeleteAsync<TEntity>(IEnumerable<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            return GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(x => x.BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation));
        }

        private IFluentCommands GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueData<TEntity>(Action<dynamic> commandRepositoryAction)
            where TEntity : class, ICommandAggregateRoot
        {
            string commandRepositoryTypeName = typeof(ICommandRepository<>).Name;
            dynamic commandRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(commandRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(commandRepository != null, string.Format("No Command Repository has been set up for {0}.", typeof(TEntity).Name));
            OperationData operationData = new OperationData { Operation = () => commandRepositoryAction(commandRepository) };
            _operationsQueue.Enqueue(operationData);
            return this;
        }

        private IFluentCommands GetFluentCommandsAfterSettingCommandRepositoryAndPersistanceQueueDataForAsync<TEntity>(Func<dynamic,Task> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            string commandRepositoryTypeName = typeof(ICommandRepository<>).Name;
            dynamic commandRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(commandRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(commandRepository != null, string.Format("No Command Repository has been set up for {0}.", typeof(TEntity).Name));
            OperationData operationData = new OperationData { AsyncOperation = () => commandRepositoryFunc(commandRepository) };
            _operationsQueue.Enqueue(operationData);
            return this;
        }
    }
}
