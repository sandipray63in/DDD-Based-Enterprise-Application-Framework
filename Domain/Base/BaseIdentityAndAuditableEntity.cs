
namespace Domain.Base
{
    public abstract class BaseIdentityAndAuditableEntity : BaseAuditableEntity
    {
        public int Id { get; set; }
    }
}
