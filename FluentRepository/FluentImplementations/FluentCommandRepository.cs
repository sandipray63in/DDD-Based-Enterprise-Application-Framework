using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Command;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandRepository : FluentCommands, IFluentCommandRepository
    {
        public FluentCommandRepository(UnitOfWorkData unitOfWorkData, dynamic commandRepository, Type commandRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository != null, "commandRepositoryFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandRepositoryType.IsNotNull(), "commandRepositoryType instance cannot be null");
            SetAndCheckRepositoryType(commandRepositoryType, () =>
             {
                 var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { CommandRepository = commandRepository, CommandRepositoryType = commandRepositoryType };
                 _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
             });
        }

        public IFluentCommands SetUpCommandPersistance<TEntity>(ICommand<TEntity> command)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(command.IsNotNull(), "command instance cannot be null");
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryType.GetGenericArguments().First().GetType();
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", expectedTEntityType.Name, typeof(TEntity).Name));
            return new FluentCommands(_unitOfWorkData, lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryType, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        internal protected override void CheckRepositoryType(Type commandRepositoryType)
        {
            ContractUtility.Requires<ArgumentException>(!_commandsAndQueriesPersistanceAndRespositoryDataList
                          .Any(x => x.CommandRepositoryType == commandRepositoryType)
                          , string.Format("The repository type {0} has been already Set Up", commandRepositoryType.Name));
        }
    }
}
