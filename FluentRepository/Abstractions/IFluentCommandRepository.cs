using System;
using Domain.Base.Aggregates;
using Repository.Command;

namespace FluentRepository.Abstractions
{
    public interface IFluentCommandRepository
    {
        /// <summary>
        /// Set up Command Persistance instance if needed
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IFluentCommandPersistance SetUpCommandPersistance<TEntity>(Func<ICommand<TEntity>> commandFunc)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Get all the available Commands which can be fired next
        /// </summary>
        /// <returns></returns>
        IFluentCommands WithCommands();
    }
}
