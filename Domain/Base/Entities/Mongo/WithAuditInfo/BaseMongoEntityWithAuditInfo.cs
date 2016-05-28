using Domain.Base.ValueObjects;

namespace Domain.Base.Entities.Mongo.WithAuditInfo
{
    public abstract class BaseMongoEntityWithAuditInfo<TId> : BaseMongoEntity<TId>
        where TId : struct
    {
        public BaseMongoEntityWithAuditInfo(string MongoId, TId id) : base(MongoId, id)
        {
            if (AuditInfo.IsNull())
            {
                AuditInfo = new AuditInfo();
            }
        }

        public AuditInfo AuditInfo;
    }
}
