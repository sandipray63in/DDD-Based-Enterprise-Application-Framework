using System.Collections.Generic;
using Domain.Base.Aggregates;
using Repository.Command;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentCommandRepository : IFluentCommands
    {
        /// <summary>
        /// Set up Command Persistance instance if needed
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IFluentCommands SetUpCommandPersistance<TEntity>(ICommand<TEntity> command)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set up Command Persistances if needed
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        IFluentCommands SetUpCommandPersistance(params dynamic[] commands);

        /// <summary>
        /// Set up Command Persistances if needed
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        IFluentCommands SetUpCommandPersistance(IList<dynamic> commands);
    }
}
