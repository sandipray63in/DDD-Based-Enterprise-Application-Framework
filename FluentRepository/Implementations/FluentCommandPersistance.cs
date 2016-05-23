using System;
using System.Collections.Generic;
using FluentRepository.Abstractions;
using Infrastructure.Utilities;

namespace FluentRepository.Implementations
{
    internal class FluentCommandPersistance : IFluentCommandPersistance
    {
        private UnitOfWorkData _unitOfWorkData;
        private IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;

        internal FluentCommandPersistance(UnitOfWorkData unitOfWorkData, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsPersistanceAndRespositoryDataList.IsNotNull(), "commandsPersistanceAndRespositoryDataList instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsPersistanceAndRespositoryDataList.IsNotEmpty(), "commandsPersistanceAndRespositoryDataList cannot be empty");
            _unitOfWorkData = unitOfWorkData;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsPersistanceAndRespositoryDataList;
        }

        public IFluentCommands WithCommands()
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommands, FluentCommands>(_unitOfWorkData, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }
    }
}
