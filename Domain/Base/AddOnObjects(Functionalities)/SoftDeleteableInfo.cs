using System;

namespace Domain.Base.AddOnObjects
{
    public class SoftDeleteableInfo : IAddOnObject
    {
        public DateTimeOffset? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
