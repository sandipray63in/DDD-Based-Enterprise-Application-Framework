using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.Base;
using FluentRepository.Abstractions;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace FluentRepository.Implementations
{
    internal class FluentCommands : FluentSetUpAndExecution,IFluentCommands
    {
        internal FluentCommands(UnitOfWorkData unitOfWorkData, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandsAndQueriesPersistanceAndRespositoryDataList)
        {

        }

        public IFluentCommands Insert<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Insert(item, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Update<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Update(item, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Delete<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Delete(item, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Insert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Insert(items, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Update<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Update(items, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Delete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Delete(items, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkInsert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkInsert(items, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkUpdate<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkUpdate(items, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkDelete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { Operation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkDelete(items, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands InsertAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().InsertAsync(item, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands UpdateAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().UpdateAsync(item, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands DeleteAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().DeleteAsync(item, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands InsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().InsertAsync(items, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands UpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().UpdateAsync(items, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands DeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().DeleteAsync(items, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkInsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkUpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands BulkDeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            var commandsAndQueriesPersistanceAndRespositoryData = CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>();
            var operationData = new OperationData { AsyncOperation = () => commandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation) };
            commandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        private CommandsAndQueriesPersistanceAndRespositoryData CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>()
            where TEntity : class, ICommandAggregateRoot
        {
            var currentCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().GetGenericArguments().First();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("TEntity must be of type {0} " +
                " since the last repository that has been Set Up was of type {1}", expectedTEntityType.ToString(), currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().ToString()));
            var operationsQueue = currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue;
            if (operationsQueue.IsNull())
            {
                operationsQueue = new Queue<OperationData>();
            }
            return currentCommandsAndQueriesPersistanceAndRespositoryData;
        }
    }
}
