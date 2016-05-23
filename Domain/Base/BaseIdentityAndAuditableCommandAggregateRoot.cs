
namespace Domain.Base
{
    public abstract class BaseIdentityAndAuditableCommandAggregateRoot : BaseIdentityAndAuditableEntity,ICommandAggregateRoot
    {
        #region IAggregateRoot Members

        /// <summary>
        /// Gets wether the current aggregate can be saved
        /// </summary>
        public virtual bool CanBeSaved { get; } = true;

        /// <summary>
        /// Gets wether the current aggregate can be deleted
        /// </summary>
        public virtual bool CanBeDeleted { get; } = true;

        #endregion
    }
}
