using System;

namespace Domain.Base.Entities
{
    /// <summary>
    /// Really wanted to remove all the duplications in this Entities folder to make things cleaner.
    /// A question has been raised in stackoverflow regarding the same viz. 
    /// http://stackoverflow.com/questions/37510656/c-sharp-generic-dynamic-inheritance.
    /// If some good concrete solution is provided there then I will try to update the 
    /// same in this code.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
        where TId : struct
    {
        public TId Id { get; private set; }

        public BaseEntity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the default value.", "id");
            }
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
