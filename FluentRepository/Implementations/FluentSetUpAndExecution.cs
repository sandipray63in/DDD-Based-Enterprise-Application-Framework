using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using FluentRepository.Abstractions;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.UnitOfWork;
using Repository.Base;

namespace FluentRepository.Implementations
{
    internal abstract class FluentSetUpAndExecution : IFluentSetUpAndExecution
    {
        private const string WITH_UNIT_OF_WORK_AND_OTHER_DEPENDENCIES = "WithUnitOfWorkAndOtherDependencies";
        protected UnitOfWorkData _unitOfWorkData;
        protected IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;

        protected FluentSetUpAndExecution(UnitOfWorkData unitOfWorkData, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsPersistanceAndRespositoryDataList.IsNotNull(), "commandsPersistanceAndRespositoryDataList instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsPersistanceAndRespositoryDataList.IsNotEmpty(), "commandsPersistanceAndRespositoryDataList cannot be empty");
            _unitOfWorkData = unitOfWorkData;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsPersistanceAndRespositoryDataList;
        }

        public IFluentCommandRepository SetUpNewCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var lastRepositoryType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType();
            ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command of "
                + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository which was of type {0}", lastRepositoryType.ToString()));
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(WITH_UNIT_OF_WORK_AND_OTHER_DEPENDENCIES, _unitOfWorkData, commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>(), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public IFluentQueryRepository SetUpNewQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var lastRepositoryType = lastCommandsAndQueriesPersistanceAndRespositoryData.QueryRepositoryFunc.GetUnderlyingType();
            ContractUtility.Requires<ArgumentException>(lastCommandsAndQueriesPersistanceAndRespositoryData.OpreationsQueue.IsNotNullOrEmpty(), string.Format("Atleast one command/query of "
                + "IFluentCommands/IFluentQueries needs to be Set Up for the last repository which was of type {0}", lastRepositoryType.ToString()));
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(WITH_UNIT_OF_WORK_AND_OTHER_DEPENDENCIES, _unitOfWorkData, queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>(), _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public void Execute()
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
                unitOfWork.Dispose();
            }
            _commandsAndQueriesPersistanceAndRespositoryDataList.Select(x => x.CommandRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
        }
        
        public async Task ExecuteAsync(CancellationToken token = default(CancellationToken))
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
                unitOfWork.Dispose();
            }
            _commandsAndQueriesPersistanceAndRespositoryDataList.Select(x => x.CommandRepositoryFunc() as IDisposable).Where(x => x.IsNotNull()).CleanUp();
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
