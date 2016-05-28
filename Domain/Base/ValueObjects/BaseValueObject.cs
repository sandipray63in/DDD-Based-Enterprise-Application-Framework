using System.Collections.Generic;
using System.Linq;

namespace Domain.Base.ValueObjects
{
    public abstract class BaseValueObject<TValue> where TValue : BaseValueObject<TValue>
    {
        public override bool Equals(object other)
        {
            return Equals(other as TValue);
        }

        public bool Equals(TValue other)
        {
            return other.IsNull() ? false : GetAttributesToIncludeInEqualityCheck()
            .SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public static bool operator ==(BaseValueObject<TValue> left, BaseValueObject<TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseValueObject<TValue> left, BaseValueObject<TValue> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            this.GetAttributesToIncludeInEqualityCheck().ToList().ForEach(obj =>
            {
                hash = hash * 31 + (obj.IsNull() ? 0 : obj.GetHashCode());
            });
            return hash;
        }

        protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();
    }
}
