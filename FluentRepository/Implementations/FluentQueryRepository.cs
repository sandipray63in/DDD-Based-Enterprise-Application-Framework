using System;
using System.Collections.Generic;
using System.Linq;
using FluentRepository.Abstractions;
using Infrastructure.Utilities;
using Infrastructure.Extensions;

namespace FluentRepository.Implementations
{
    internal class FluentQueryRepository : IFluentQueryRepository
    {
        private UnitOfWorkData _unitOfWorkData;
        private Func<dynamic> _queryRepositoryFunc;
        private IList<CommandsAndQueriesPersistanceAndRespositoryData> _commandsAndQueriesPersistanceAndRespositoryDataList;

        internal FluentQueryRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> queryRepositoryFunc)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkData instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(queryRepositoryFunc.IsNotNull(), "queryRepositoryFunc instance cannot be null");
            _unitOfWorkData = unitOfWorkData;
            _queryRepositoryFunc = queryRepositoryFunc;
            _commandsAndQueriesPersistanceAndRespositoryDataList = new List<CommandsAndQueriesPersistanceAndRespositoryData>();
            SetUpCommandsAndQueriesPersistanceAndRespositoryDataList();
        }

        internal FluentQueryRepository(UnitOfWorkData unitOfWorkData, Func<dynamic> queryRepositoryFunc, IList<CommandsAndQueriesPersistanceAndRespositoryData> commandsAndQueriesPersistanceAndRespositoryDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(queryRepositoryFunc.IsNotNull(), "commandRepositoryFunc instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsAndQueriesPersistanceAndRespositoryDataList.IsNotNull(), "commandsAndQueriesPersistanceAndRespositoryDataList instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(commandsAndQueriesPersistanceAndRespositoryDataList.IsNotEmpty(), "commandsAndQueriesPersistanceAndRespositoryDataList cannot be empty");
            _unitOfWorkData = unitOfWorkData;
            _queryRepositoryFunc = queryRepositoryFunc;
            _commandsAndQueriesPersistanceAndRespositoryDataList = commandsAndQueriesPersistanceAndRespositoryDataList;
            if (_commandsAndQueriesPersistanceAndRespositoryDataList.IsNotNull())
            {
                var commandsAndQueriesPersistanceAndRespositoryDataNotNullList = _commandsAndQueriesPersistanceAndRespositoryDataList.Where(x => x.CommandRepositoryFunc.IsNotNull());
                if (commandsAndQueriesPersistanceAndRespositoryDataNotNullList.IsNotEmpty())
                {
                    ContractUtility.Requires<ArgumentException>(!commandsAndQueriesPersistanceAndRespositoryDataNotNullList
                         .Any(x => x.QueryRepositoryFunc.GetUnderlyingType() == _queryRepositoryFunc.GetUnderlyingType())
                         , string.Format("The repository type {0} has been already Set Up", _queryRepositoryFunc.GetUnderlyingType().ToString()));
                }
            }
            SetUpCommandsAndQueriesPersistanceAndRespositoryDataList();
        }

        public IFluentQueries WithQueries()
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueries, FluentQueries>(_unitOfWorkData, _commandsAndQueriesPersistanceAndRespositoryDataList);
        }

        private void SetUpCommandsAndQueriesPersistanceAndRespositoryDataList()
        {
            var commandsAndQueriesPersistanceAndRespositoryData = new CommandsAndQueriesPersistanceAndRespositoryData { CommandRepositoryFunc = _queryRepositoryFunc };
            _commandsAndQueriesPersistanceAndRespositoryDataList.Add(commandsAndQueriesPersistanceAndRespositoryData);
        }
    }
}
