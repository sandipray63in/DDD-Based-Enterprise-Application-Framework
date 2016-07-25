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
    }
}
