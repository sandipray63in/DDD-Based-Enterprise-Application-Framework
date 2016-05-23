
namespace Domain.Base.Mongo
{
    public abstract class BaseMongoIdentityAndAuditableEntity : BaseAuditableEntity
    {
        public string MongoId { get; set; }

        public int? RDBMSId { get; set; }

    }
}
