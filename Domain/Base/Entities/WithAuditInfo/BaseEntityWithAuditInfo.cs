using Domain.Base.ValueObjects;

namespace Domain.Base.Entities.WithAuditInfo
{
    public abstract class BaseEntityWithAuditInfo<TId> : Entities.BaseEntity<TId>
        where TId : struct
    {
        public BaseEntityWithAuditInfo(TId id) : base(id)
        {
            if(AuditInfo.IsNull())
            {
                AuditInfo = new AuditInfo();
            }
        }

        public AuditInfo AuditInfo;
    }
}
