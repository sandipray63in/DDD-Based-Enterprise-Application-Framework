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

        public FluentCommands(UnitOfWorkData unitOfWorkData, Type commandRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
        }

        public IFluentCommands Insert<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.Insert(item, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Update<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.Update(item, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Delete<TEntity>(TEntity item, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.Delete(item, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Insert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.Insert(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Update<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.Update(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands Delete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.Delete(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkInsert<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.BulkInsert(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkUpdate<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.BulkUpdate(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkDelete<TEntity>(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { Operation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.BulkDelete(items, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands InsertAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.InsertAsync(item, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands UpdateAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.UpdateAsync(item, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands DeleteAsync<TEntity>(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.DeleteAsync(item, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands InsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.InsertAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands UpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.UpdateAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands DeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.DeleteAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkInsertAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }

        public IFluentCommands BulkUpdateAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
        
        public IFluentCommands BulkDeleteAsync<TEntity>(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(items.IsNotNull(), "items cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(items.IsNotEmpty(), "items cannot be empty");
            ContractUtility.Requires<ArgumentException>(_expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", _expectedTEntityType.Name, typeof(TEntity).Name));
            var operationData = new OperationData { AsyncOperation = () => _currentCommandsAndQueriesPersistanceAndRespositoryData.CommandRepository.BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation) };
            _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.Enqueue(operationData);
            return this;
        }
    }
}
