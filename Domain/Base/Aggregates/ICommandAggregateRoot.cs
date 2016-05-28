
namespace Domain.Base.Aggregates
{
    /// <summary>
    /// Defines an entity type that can be an aggregate root
    /// </summary>
    public interface ICommandAggregateRoot
    {
        /// <summary>
        /// Gets wether the current aggregate can be saved
        /// </summary>
        bool CanBeSaved { get; }

        /// <summary>
        /// Gets wether the current aggregate can be deleted
        /// </summary>
        bool CanBeDeleted { get; }
    }
}
