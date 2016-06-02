using System;

namespace Domain.Base.Entities
{
    public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
        where TId : struct
    {
        public TId Id { get; private set; }

        public BaseEntity(TId id)
        {
            DomainContractUtility.Requires<ArgumentException>(object.Equals(id, default(TId)), "The ID cannot be the default value.");
            this.Id = id;
        }

        public override bool Equals(object obj)
        {
            var entity = obj as BaseEntity<TId>;
            return entity.IsNotNull()? this.Equals(entity) :  base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public bool Equals(BaseEntity<TId> other)
        {
            return other.IsNull() ? false : this.Id.Equals(other.Id);
        }
    }
}
