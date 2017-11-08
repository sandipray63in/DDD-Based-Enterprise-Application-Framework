using System;
using System.Data.SqlClient;
using System.Reflection;
using Infrastructure.Utilities;
using Repository.Base;

namespace Repository
{
    public class SqlBulkCopyRepository : ISqlBulkCopyRepository
    {
        private readonly int _batchSize;
        private readonly int _timeout;

        public SqlBulkCopyRepository(int batchSize,int timeout = 0)
        {
            ContractUtility.Requires<ArgumentOutOfRangeException>(batchSize > 0, "batchSize should be greater than 0");
            _batchSize = batchSize;
            _timeout = timeout;
        }

        public int PerformBulkCopy(string sourceConnectionString,string destinationConnectionString,string sourceQuery,string destinationTableName)
        {
            int result = 0;
            // get the source data
            using (SqlConnection sourceConnection =  new SqlConnection(sourceConnectionString))
            {
                SqlCommand myCommand = new SqlCommand(sourceQuery, sourceConnection);
                sourceConnection.Open();
                SqlDataReader reader = myCommand.ExecuteReader();

                // open the destination data
                using (SqlConnection destinationConnection = new SqlConnection(destinationConnectionString))
                {
                    // open the connection
                    destinationConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection.ConnectionString))
                    {
                        bulkCopy.BatchSize = _batchSize;
                        bulkCopy.BulkCopyTimeout = _timeout;
                        bulkCopy.DestinationTableName = destinationTableName;
                        bulkCopy.WriteToServer(reader);
                        FieldInfo rowCopied = typeof(SqlBulkCopy).GetField("_rowsCopied", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                        result = (int)rowCopied.GetValue(bulkCopy);
                    }
                }
                reader.Close();
            }
            return result;
        }
    }
}
