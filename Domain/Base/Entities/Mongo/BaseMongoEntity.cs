
namespace Domain.Base.Entities.Mongo
{
    public abstract class BaseMongoEntity<TId> : BaseEntity<TId>
        where TId : struct
    {
        public string MongoId { get; private set; }

        public BaseMongoEntity(string MongoId, TId id) : base(id)
        {
            this.MongoId = MongoId;
        }
    }
}
