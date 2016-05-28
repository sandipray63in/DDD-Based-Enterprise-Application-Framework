using System.Configuration;
using MongoDB.Driver;
using Domain.Base.Entities.Mongo;

namespace MongoDBDomainMaps
{
    /// <summary>
    /// To register Maps using some logic like that used for EF, one can go through -
    /// https://mongodb-documentation.readthedocs.org/en/latest/ecosystem/tutorial/serialize-documents-with-the-csharp-driver.html.
    /// 
    /// Other good reads on Mongo DB C# Driver are https://darkiri.wordpress.com/2013/02/26/bson-serialization-with-mongodb-c-driver/
    /// and http://mikaelkoskinen.net/post/mongodb-aggregation-framework-examples-in-c 
    /// </summary>
    public class MongoContext
    {
        public const string CONNECTION_STRING_NAME = "App_Connection_String";
        //<connectionStrings>
        //    <add name = "Blog" connectionString="mongodb://localhost:27017" />
        //</connectionStrings>
           
        public const string DATABASE_NAME = "App_DB_Name";
        private static readonly IMongoDatabase _database;

        static MongoContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(DATABASE_NAME);
        }

        #region Mongo Collection   

        public IMongoCollection<TEntity> GetMongoCollection<TId,TEntity>()
         where TId : struct
         where TEntity : BaseMongoEntity<TId>
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        #endregion
    }
}
