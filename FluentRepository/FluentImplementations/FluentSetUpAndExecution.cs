using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Repository.Base;
using Repository.UnitOfWork;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace FluentRepository.FluentImplementations
{
    internal abstract class FluentSetUpAndExecution : IFluentSetUpAndExecution
    {
        private const string WITH_UNIT_OF_WORK_AND_OTHER_DEPENDENCIES = "WithUnitOfWorkAndOtherDependencies";
        protected UnitOfWorkData _unitOfWorkData;
        protected IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;

        internal protected FluentSetUpAndExecution(UnitOfWorkData unitOfWorkData, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList)
        {
            _unitOfWorkData = unitOfWorkData;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsPersistanceAndRespositoryDataList;
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
            {
                var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
                var lastRepositoryType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType();
                ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command of "
                    + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository which was of type {0}", lastRepositoryType.ToString()));
            }
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(_unitOfWorkData, commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>(), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty())
            {
                var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
                var lastRepositoryType = lastCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc.GetUnderlyingType();
                ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command/query of "
                    + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository which was of type {0}", lastRepositoryType.ToString()));
            }
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(WITH_UNIT_OF_WORK_AND_OTHER_DEPENDENCIES, _unitOfWorkData, queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>(), _commandsAndQueriesPersistanceAndRespositoryDataList);
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
                _commandsAndQueriesPersistanceAndRespositoryDataList.Select(x => x.CommandRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
                _commandsAndQueriesPersistanceAndRespositoryDataList.Select(x => x.QueryRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
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
                _commandsAndQueriesPersistanceAndRespositoryDataList.Select(x => x.CommandRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
                _commandsAndQueriesPersistanceAndRespositoryDataList.Select(x => x.QueryRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }

        private void SetUpAllRespositoriesForCommandsAndQueriesRespositoryDataList(BaseUnitOfWork unitOfWork)
        {
            _commandsAndQueriesPersistanceAndRespositoryDataList.ForEach(x =>
                {
                    var parameterOverrideDataLis = new List<ParameterOverrideData>();
                    if (unitOfWork.IsNotNull())
                    {
                        parameterOverrideDataLis.Add(new ParameterOverrideData { ParameterName = "unitOfWork", ParameterValue = unitOfWork });
                    }
                    if (x.QueryRepositoryFunc.IsNotNull())
                    {
                        if (x.QueryPersistanceFunc.IsNotNull())
                        {
                            var queryPersistance = x.QueryPersistanceFunc();
                            parameterOverrideDataLis.Add(new ParameterOverrideData { ParameterName = "persistance", ParameterValue = queryPersistance });
                        }
                        dynamic dynamicqueryRepository = parameterOverrideDataLis.IsEmpty() ? ContainerUtility.Resolve(x.QueryRepositoryFunc.GetUnderlyingType(), null) :
                                        ContainerUtility.Resolve(x.QueryRepositoryFunc.GetUnderlyingType(), parameterOverrideDataLis);
                        x.QueryRepositoryFunc = () => dynamicqueryRepository;
                    }
                    if (x.CommandRepositoryFunc.IsNotNull())
                    {
                        if (x.CommandPersistanceFunc.IsNotNull())
                        {
                            var commandPersistance = x.CommandPersistanceFunc();
                            parameterOverrideDataLis.Add(new ParameterOverrideData { ParameterName = "persistance", ParameterValue = commandPersistance });
                        }
                        dynamic dynamicCommandRepository = parameterOverrideDataLis.IsEmpty() ? ContainerUtility.Resolve(x.CommandRepositoryFunc.GetUnderlyingType(), null) :
                                            ContainerUtility.Resolve(x.CommandRepositoryFunc.GetUnderlyingType(), parameterOverrideDataLis);
                        x.CommandRepositoryFunc = () => dynamicCommandRepository;
                    }
                }
            );
        }
    }
}
