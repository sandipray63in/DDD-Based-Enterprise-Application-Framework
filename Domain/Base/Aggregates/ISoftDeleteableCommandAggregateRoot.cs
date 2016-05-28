using System;

namespace Domain.Base.Aggregates
{
    /// <summary>
    /// Should not be implemented if the deletion related archival process is taken care in the DB side using some 
    /// trigger.
    /// </summary>
    public interface ISoftDeleteableCommandAggregateRoot : ICommandAggregateRoot
    {
        bool IsDeleted { get; set; }

        DateTimeOffset? DeletedOn { get; set; }
    }
}
