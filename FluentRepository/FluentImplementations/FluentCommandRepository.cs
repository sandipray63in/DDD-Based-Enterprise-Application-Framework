using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using Repository.Command;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandRepository : FluentCommands, IFluentCommandRepository
    {
        /// <summary>
        /// For proper working of this class alongwith Unity(for regstration), the constructor needs to be public.
        /// </summary>
        /// <param name="unitOfWorkData"></param>
        /// <param name="commandRepositoryFunc"></param>
        /// <param name="commandsAndQueriesPersistanceAndRespositoryDataList"></param>
        public FluentCommandRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> commandRepositoryFunc, Type commandRepositoryType, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList) : base(unitOfWorkData, commandRepositoryType, commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandRepositoryType.IsNotNull(), "commandRepositoryType instance cannot be null");
            SetAndCheckRepositoryType(commandRepositoryType, () =>
             {
                 var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { CommandRepositoryFunc = commandRepositoryFunc, CommandRepositoryType = commandRepositoryType };
                 _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
             });
        }

        public IFluentCommands SetUpCommandPersistance<TEntity>(Func<ICommand<TEntity>> commandFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            var lastCommandsAndQueriesPersistanceAndRespositoryData = _commandsAndQueriesPersistanceAndRespositoryDataList.Last();
            var expectedTEntityType = lastCommandsAndQueriesPersistanceAndRespositoryData.CommandRepositoryType;
            ContractUtility.Requires<ArgumentException>(expectedTEntityType == typeof(TEntity), string.Format("Type Mismatch: Expected Generic Type is {0} but the Supplied Generic Type is {1}", expectedTEntityType.Name, typeof(TEntity).Name));
            lastCommandsAndQueriesPersistanceAndRespositoryData.CommandPersistanceFunc = commandFunc.ConvertFunc<ICommand<TEntity>, dynamic>();
            var unitOfWorkParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _unitOfWorkData.GetType(), ParameterName = "unitOfWorkData", ParameterValue = _unitOfWorkData };
            var commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData = new ParameterTypeOverrideData { ParameterType = _commandsAndQueriesPersistanceAndRespositoryDataList.GetType(), ParameterName = "commandsAndQueriesPersistanceAndRespositoryDataList", ParameterValue = _commandsAndQueriesPersistanceAndRespositoryDataList };
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommands, FluentCommands>(unitOfWorkParameterTypeOverrideData, commandsAndQueriesPersistanceAndRespositoryDataListParameterTypeOverrideData);
        }

        internal protected override void CheckRepositoryType(Type commandRepositoryType)
        {
            ContractUtility.Requires<ArgumentException>(!_commandsAndQueriesPersistanceAndRespositoryDataList
                          .Any(x => x.CommandRepositoryType == commandRepositoryType)
                          , string.Format("The repository type {0} has been already Set Up", commandRepositoryType.Name));
        }
    }
}
