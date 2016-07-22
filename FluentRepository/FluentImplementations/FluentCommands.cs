using System;
using System.Collections.Generic;
using System.Threading;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommands : FluentSetUpAndExecution,IFluentCommands
    {
        /// <summary>
        /// For proper working of this class alongwith Unity(for regstration), the constructor needs to be public.
        /// </summary>
        /// <param name="unitOfWorkData"></param>
        /// <param name="commandsAndQueriesPersistanceAndRespositoryDataList"></param>
        public FluentCommands(UnitOfWorkData unitOfWorkData, Type commandRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
        }

        public IFluentCommands Insert<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Insert(item, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Update<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Update(item, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Delete<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Delete(item, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Insert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Insert(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Update<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Update(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Delete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().Delete(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkInsert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkInsert(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkUpdate<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkUpdate(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkDelete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkDelete(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands InsertAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().InsertAsync(item, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands UpdateAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().UpdateAsync(item, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands DeleteAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().DeleteAsync(item, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands InsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().InsertAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands UpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().UpdateAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands DeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().DeleteAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkInsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkUpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands BulkDeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc().BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
    }
}
