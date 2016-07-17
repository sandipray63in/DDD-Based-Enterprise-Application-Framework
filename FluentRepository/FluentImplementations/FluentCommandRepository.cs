using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Repository.Command;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandRepository : FluentCommands, IFluentCommandRepository
    {
        private Func<dynamic> _commandRepositoryFunc;

        internal FluentCommandRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> commandRepositoryFunc, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
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

        public IFluentCommands SetUpCommandPersistance<TEntity>(Func<ICommand<TEntity>> commandFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().GetGenericArguments().First();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("TEntity must be of type {0} since the" +  
                "last repository that has been Set Up was of type {1}", expectedTEntityType.ToString(),lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryFunc.GetUnderlyingType().ToString()));
            lastCommandsAndQueriesPersistanceAndRespositoryData.CommandPersistanceFunc = commandFunc.ConvertFunc<ICommand<TEntity>, dynamic>();
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommands, FluentCommands>(_unitOfWorkData, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        private void SetUpCommandsAndQueriesPersistanceAndRespositoryDataList()
        {
            var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { CommandRepositoryFunc = _commandRepositoryFunc };
            _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
        }
    }
}
