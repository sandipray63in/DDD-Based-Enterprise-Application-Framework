
namespace Repository.Base
{
    /// <summary>
    /// Can perform bulk copy (without any intermediate preocessing) from one sql server db to
    /// another sql server db.If intermediate processing is required then go for Batch processing.
    /// </summary>
    public interface ISqlBulkCopyRepository
    {
        int PerformBulkCopy(string sourceConnectionString, string destinationConnectionString, string sourceQuery, string destinationTableName);
    }
}
