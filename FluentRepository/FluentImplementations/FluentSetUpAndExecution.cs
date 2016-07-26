using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal abstract class FluentSetUpAndExecution : IFluentSetUpAndExecution
    {
        protected UnitOfWorkData _unitOfWorkData;
        protected IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;
        protected CommandsAndQueriesPersistanceAndRespositoryData _currentCommandsAndQueriesPersistanceAndRespositoryData;
        protected Type _expectedTEntityType;

        internal protected FluentSetUpAndExecution(UnitOfWorkData unitOfWorkData, Type repositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList)
        {
            _unitOfWorkData = unitOfWorkData;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsPersistanceAndRespositoryDataList;
            _expectedTEntityType = repositoryType.GetGenericArguments().First();
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository.IsNotNull(), "commandRepository instance cannot be null");
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
            {
                var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
                ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command of "
                    + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository"));
            }
            if(_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                ((dynamic)commandRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            }
            return new FluentCommandRepository(_unitOfWorkData, commandRepository, commandRepository.GetType(), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository.IsNotNull(), "queryRepository instance cannot be null");
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
            {
                var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
                ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command/query of "
                    + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository"));
            }
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                ((dynamic)queryRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            }
            return new FluentQueryRepository(_unitOfWorkData, queryRepository, queryRepository.GetType(), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public void Execute(Boolean shouldAutomaticallyDisposeAllDisposables = false)
        {
            SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList();
            _commandsAndQueriesPersistanceAndRespositoryDataList.ForEach(x =>
                {
                    var operationsQueue = x.OpreationsQueue;
                    ContractUtility.Requires<ArgumentOutOfRangeException>(operationsQueue.Count > 0, "Atleast one operation must be there to be executed.");

                    ContractUtility.Requires<NotSupportedException>(operationsQueue.All(y => y.AsyncOperation.IsNull()),
                                            "Async operations are not supported by Execute method.Use ExecuteAsync instead.");
                    if (operationsQueue.IsNotNull())
                    {
                        while (operationsQueue.Count > 0)
                        {
                            var operationData = operationsQueue.Dequeue();
                            if (operationData.Operation.IsNotNull())
                            {
                                operationData.Operation();
                            }
                        }
                    }
                }
            );
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                var unitOfWork = _unitOfWorkData.UnitOfWork;
                unitOfWork.Commit(_unitOfWorkData.ShouldAutomaticallyRollBackOnTransactionException, _unitOfWorkData.ShouldThrowOnException);
                if (shouldAutomaticallyDisposeAllDisposables)
                {
                    unitOfWork.Dispose();
                }
            }
            if (shouldAutomaticallyDisposeAllDisposables)
            {
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepository != null).Select(x => x.CommandRepository as IDisposable).Where(x => x.IsNotNull()).CleanUp();
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.QueryRepository != null).Select(x => x.QueryRepository as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }
        
        public async Task ExecuteAsync(CancellationToken token = default(CancellationToken), Boolean shouldAutomaticallyDisposeAllDisposables = false)
        {
            SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList();
            foreach (var x in _commandsAndQueriesPersistanceAndRespositoryDataList)
            {
                var operationsQueue = x.OpreationsQueue;
                ContractUtility.Requires<ArgumentOutOfRangeException>(operationsQueue.Count > 0, "Atleast one operation must be there to be executed.");
                ContractUtility.Requires<NotSupportedException>(operationsQueue.Any(y => y.AsyncOperation.IsNotNull()),
                    "If ExecuteAsync method is used,there needs to be atleast one async operation exceuted." +
                    "Please use Execute method(instead of ExecuteAsync) if there is not " +
                    "a single async operation.");

                if (operationsQueue.IsNotNull())
                {
                    while (operationsQueue.Count > 0)
                    {
                        var operationData = operationsQueue.Dequeue();
                        if (operationData.Operation.IsNotNull())
                        {
                            operationData.Operation();
                        }
                        else if (operationData.AsyncOperation.IsNotNull())
                        {
                            await operationData.AsyncOperation();
                        }
                    }
                }
            }

            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                var unitOfWork = _unitOfWorkData.UnitOfWork;
                await unitOfWork.CommitAsync(token, _unitOfWorkData.ShouldAutomaticallyRollBackOnTransactionException, _unitOfWorkData.ShouldThrowOnException);
                if (shouldAutomaticallyDisposeAllDisposables)
                {
                    unitOfWork.Dispose();
                }
            }
            if (shouldAutomaticallyDisposeAllDisposables)
            {
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepository != null).Select(x => x.CommandRepository as IDisposable).Where(x => x.IsNotNull()).CleanUp();
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.QueryRepository != null).Select(x => x.QueryRepository as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }

        internal protected void SetAndCheckRepositoryType(Type repositoryType, Action addRepositoryFunc)
        {
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotNull())
            {
                if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
                {
                    CheckRepositoryType(repositoryType);
                }
            }
            else
            {
                _commandsAndQueriesPersistanceAndRespositoryDataList = new List<CommandsAndQueriesPersistanceAndRespositoryData>();
            }
            addRepositoryFunc();
            SetCurrentCommandsPersistanceAndRespositoryData();
        }

        internal protected virtual void CheckRepositoryType(Type repositoryType) { }

        private void SetCurrentCommandsPersistanceAndRespositoryData()
        {
             _currentCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            if (_currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNull())
            {
                _currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue = new Queue<OperationData>();
            }
        }

        private void SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList()
        {
            _commandsAndQueriesPersistanceAndRespositoryDataList.ForEach(x =>
                {
                    if (x.QueryRepository != null && x.QueryPersistance != null)
                    {
                        x.QueryRepository = new FluentQueryRepository(_unitOfWorkData, x.QueryRepository, x.QueryRepository.GetType(), _commandsAndQueriesPersistanceAndRespositoryDataList);
                    }
                    if (x.CommandRepository != null && x.CommandPersistance != null)
                    {
                        x.CommandRepository = new FluentCommandRepository(_unitOfWorkData, x.CommandRepository, x.CommandRepository.GetType(), _commandsAndQueriesPersistanceAndRespositoryDataList);
                    }
                }
            );
        }
    }
}
