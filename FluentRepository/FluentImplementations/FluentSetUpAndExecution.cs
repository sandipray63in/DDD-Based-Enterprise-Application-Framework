using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.Base;
using Repository.UnitOfWork;

namespace FluentRepository.FluentImplementations
{
    internal abstract class FluentSetUpAndExecution : IFluentSetUpAndExecution
    {
        protected UnitOfWorkData _unitOfWorkData;
        protected IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;

        protected FluentSetUpAndExecution(UnitOfWorkData unitOfWorkData, Type repositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList)
        {
            _unitOfWorkData = unitOfWorkData;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsPersistanceAndRespositoryDataList;
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
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
            {
                var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
                ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command of "
                    + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository"));
            }
            //var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = _unitOfWorkData };
            var dynamicCommandRepositoryFunc = commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>();
            //var dynamicCommandRepositoryFuncParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = dynamicCommandRepositoryFunc.GetType(), ParameterName = "commandRepositoryFunc", ParameterValue = dynamicCommandRepositoryFunc };
            //var commandRepositoryType = new ParameterTypeOverrideData { ParameterType = typeof(Type), ParameterName = "commandRepositoryType", ParameterValue = typeof(ICommandRepository<TEntity>) };
            //var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(IList<CommandsAndQueriesPersistanceAndRespositoryData>), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = null };
            //return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(unitOfWorkParameterTypeOverrideData, dynamicCommandRepositoryFuncParameterTypeOverrideData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
            return new FluentCommandRepository(_unitOfWorkData, dynamicCommandRepositoryFunc, typeof(ICommandRepository<TEntity>), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
            {
                var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
                ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command/query of "
                    + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository"));
            }
            //var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = _unitOfWorkData };
            var dynamicQueryRepositoryFunc = queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>();
            //var dynamicQueryRepositoryFuncParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = dynamicQueryRepositoryFunc.GetType(), ParameterName = "commandRepositoryFunc", ParameterValue = dynamicQueryRepositoryFunc };
            //var queryRepositoryType = new ParameterTypeOverrideData { ParameterType = typeof(Type), ParameterName = "queryRepositoryType", ParameterValue = typeof(IQueryableRepository<TEntity>) };
            //var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = typeof(IList<CommandsAndQueriesPersistanceAndRespositoryData>), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = null };
            //return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(unitOfWorkParameterTypeOverrideData, dynamicQueryRepositoryFuncParameterTypeOverrideData, queryRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
            return new FluentQueryRepository(_unitOfWorkData, dynamicQueryRepositoryFunc, typeof(IQueryableRepository<TEntity>), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public void Execute(Boolean shouldAutomaticallyDisposeAllDisposables = false)
        {
            BaseUnitOfWork unitOfWork = null;
            if (_unitOfWorkData.IsNotNull() && _unitOfWorkData.UnitOfWorkFunc.IsNotNull())
            {
                unitOfWork = _unitOfWorkData.UnitOfWorkFunc();
            }
            SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList(unitOfWork);
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
            if (_unitOfWorkData.IsNotNull() && unitOfWork.IsNotNull())
            {
                unitOfWork.Commit(_unitOfWorkData.ShouldAutomaticallyRollBackOnTransactionException, _unitOfWorkData.ShouldThrowOnException);
                if (shouldAutomaticallyDisposeAllDisposables)
                {
                    unitOfWork.Dispose();
                }
            }
            if (shouldAutomaticallyDisposeAllDisposables)
            {
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepositoryFunc.IsNotNull()).Select(x => x.CommandRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.QueryRepositoryFunc.IsNotNull()).Select(x => x.QueryRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }
        
        public async Task ExecuteAsync(CancellationToken token = default(CancellationToken), Boolean shouldAutomaticallyDisposeAllDisposables = false)
        {
            BaseUnitOfWork unitOfWork = null;
            if (_unitOfWorkData.IsNotNull() && _unitOfWorkData.UnitOfWorkFunc.IsNotNull())
            {
                unitOfWork = _unitOfWorkData.UnitOfWorkFunc();
            }
            SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList(unitOfWork);
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

            if (unitOfWork.IsNotNull() && _unitOfWorkData.IsNotNull())
            {
                await unitOfWork.CommitAsync(token, _unitOfWorkData.ShouldAutomaticallyRollBackOnTransactionException, _unitOfWorkData.ShouldThrowOnException);
                if (shouldAutomaticallyDisposeAllDisposables)
                {
                    unitOfWork.Dispose();
                }
            }
            if (shouldAutomaticallyDisposeAllDisposables)
            {
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepositoryFunc.IsNotNull()).Select(x => x.CommandRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
                _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.QueryRepositoryFunc.IsNotNull()).Select(x => x.QueryRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }

        internal protected virtual void CheckRepositoryType(Type repositoryType) { }

        internal protected CommandsAndQueriesPersistanceAndRespositoryData CheckForProperEntityTypeAndGetCurrentCommandsPersistanceAndRespositoryData<TEntity>(Type repositoryType)
            where TEntity : class
        {
            var currentCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = repositoryType.GetGenericArguments().First();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", expectedTEntityType.Name, typeof(TEntity).Name));
            var operationsQueue = currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue;
            if (currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNull())
            {
                currentCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue = new Queue<OperationData>();
            }
            return currentCommandsAndQueriesPersistanceAndRespositoryData;
        }

        private void SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList(BaseUnitOfWork unitOfWork)
        {
            _commandsAndQueriesPersistanceAndRespositoryDataList.ForEach(x =>
                {
                    var parameterOverrideDataList = new List<ParameterOverrideData>();
                    if (unitOfWork.IsNotNull())
                    {
                        parameterOverrideDataList.Add(new ParameterOverrideData { ParameterName = "unitOfWork", ParameterValue = unitOfWork });
                    }
                    if (x.QueryRepositoryFunc.IsNotNull() && x.QueryPersistanceFunc.IsNotNull())
                    {
                        var queryPersistance = x.QueryPersistanceFunc();
                        parameterOverrideDataList.Add(new ParameterOverrideData { ParameterName = "persistance", ParameterValue = queryPersistance });
                        x.QueryRepositoryFunc = () => ContainerUtility.Resolve(x.QueryRepositoryType, parameterOverrideDataList);
                    }
                    if (x.CommandRepositoryFunc.IsNotNull() && x.CommandPersistanceFunc.IsNotNull())
                    {
                        var commandPersistance = x.CommandPersistanceFunc();
                        parameterOverrideDataList.Add(new ParameterOverrideData { ParameterName = "persistance", ParameterValue = commandPersistance });
                        x.CommandRepositoryFunc = () => ContainerUtility.Resolve(x.CommandRepositoryType, parameterOverrideDataList);
                    }
                }
            );
        }
    }
}
