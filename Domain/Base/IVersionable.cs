
namespace Domain.Base
{
    public interface IVersionable
    {
        byte[] RowVersion { get; set; }
    }
}
