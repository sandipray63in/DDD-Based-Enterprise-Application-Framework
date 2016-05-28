using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.Abstractions;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.Command;

namespace FluentRepository.Implementations
{
    internal class FluentCommandRepository : IFluentCommandRepository
    {
        private UnitOfWorkData _unitOfWorkData;
        private Func<dynamic> _commandRepositoryFunc;
        private IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;

        internal FluentCommandRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> commandRepositoryFunc)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkData instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
            _unitOfWorkData = unitOfWorkData;
            _commandRepositoryFunc = commandRepositoryFunc;
            _commandsAndQueriesPersistanceAndRespositoryDataList = new List<CommandsAndQueriesPersistanceAndRespositoryData>();
            SetUpCommandsAndQueriesPersistanceAndRespositoryDataList();
        }

        internal FluentCommandRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> commandRepositoryFunc, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsAndQueriesPersistanceAndRespositoryDataList.IsNotNull(), "commandsPersistanceAndRespositoryDataList instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty(), "commandsPersistanceAndRespositoryDataList cannot be empty");
            _unitOfWorkData = unitOfWorkData;
            _commandRepositoryFunc = commandRepositoryFunc;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsAndQueriesPersistanceAndRespositoryDataList;
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotNull())
            {
                var commandsAndQueriesPersistanceAndRespositoryDataNotNullList = _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepositoryFunc.IsNotNull());
                if (commandsAndQueriesPersistanceAndRespositoryDataNotNullList.IsNotEmpty())
                {
                    ContractUtility.Requires<ArgumentException>(!commandsAndQueriesPersistanceAndRespositoryDataNotNullList
                         .Any(x => x.CommandRepositoryFunc.GetUnderlyingType() == _commandRepositoryFunc.GetUnderlyingType())
                         , string.Format("The repository type {0} has been already Set Up", _commandRepositoryFunc.GetUnderlyingType().ToString()));
                }
            }
            SetUpCommandsAndQueriesPersistanceAndRespositoryDataList();
        }

        public IFluentCommandPersistance SetUpCommandPersistance<TEntity>(Func<ICommand<TEntity>> commandFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().GetGenericArguments().First();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("TEntity must be of type {0} since the" +  
                "last repository that has been Set Up was of type {1}", expectedTEntityType.ToString(),lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().ToString()));
            lastCommandsAndQueriesPersistanceAndRespositoryData.CommandPersistanceFunc = commandFunc.ConvertFunc<ICommand<TEntity>, dynamic>();
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandPersistance, FluentCommandPersistance>(_unitOfWorkData, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        public IFluentCommands WithCommands()
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommands, FluentCommands>(_unitOfWorkData, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        private void SetUpCommandsAndQueriesPersistanceAndRespositoryDataList()
        {
            var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { CommandRepositoryFunc = _commandRepositoryFunc };
            _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
        }
    }
}
