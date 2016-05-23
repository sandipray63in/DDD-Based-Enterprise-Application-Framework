
namespace FluentRepository.Abstractions
{
    public interface IFluentCommandPersistance
    {
        /// <summary>
        /// Get all the available Commands which can be fired next
        /// </summary>
        /// <returns></returns>
        IFluentCommands WithCommands();
    }
}
