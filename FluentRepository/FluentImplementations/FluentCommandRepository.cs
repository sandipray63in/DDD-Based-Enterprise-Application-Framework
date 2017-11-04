using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;
using Repository.Command;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandRepository : FluentCommands, IFluentCommandRepository
    {
        public FluentCommandRepository(UnitOfWorkData unitOfWorkData, IEnumerable<dynamic> repositoriesEnumerable, Queue<OperationData> operationsQueue) : base(unitOfWorkData, repositoriesEnumerable, operationsQueue)
        {
        }

        public IFluentCommands SetUpCommandPersistance<TEntity>(ICommand<TEntity> command)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(command.IsNotNull(), "command instance cannot be null");
            string commandRepositoryTypeName = typeof(ICommandRepository<>).Name;
            dynamic commandRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(commandRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(commandRepository != null, string.Format("No Command Repository has been set up for {0}.", typeof(TEntity).Name));
            commandRepository.SetCommand(command);
            return new FluentCommands(_unitOfWorkData, _repositoriesList, _operationsQueue);
        }

        public IFluentCommands SetUpCommandPersistance(params dynamic[] commands)
        {
            return SetUpCommandRepository(commands);
        }

        /// <summary>
        /// TODO- Provide proper implementation for the below method.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public IFluentCommands SetUpCommandPersistance(IEnumerable<dynamic> commands)
        {
            throw new NotImplementedException();
        }
    }
}
