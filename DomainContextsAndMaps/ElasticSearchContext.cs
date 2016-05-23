using System;
using System.Configuration;
using ElasticLinq;
using ElasticsearchCRUD;
using Domain.Base;

namespace DomainContextsAndMaps
{
    public class ElasticSearchContext : ElasticContext
    {
        private const string INDEX_NAME = "Index_Name_As_Per_Application";
        private static string _connectionString = ConfigurationManager.ConnectionStrings["ElasticSearchConnectionString"].ConnectionString;
        private static readonly ElasticsearchContext _elasticSearchContext;

        static ElasticSearchContext()
        {
            _elasticSearchContext = new ElasticsearchContext(_connectionString + INDEX_NAME, new ElasticsearchMappingResolver());
            CreateIndexesAndThenIndexDataAfterFetchFromActualDataRepository();
        }

        public ElasticSearchContext() : base(new ElasticConnection(new Uri(_connectionString), index: INDEX_NAME))
        {
        }

        public ElasticsearchContext ElasticsearchContext
        {
            get { return _elasticSearchContext; }
        }

        private static void CreateIndexesAndThenIndexDataAfterFetchFromActualDataRepository()
        {
            //Call the method for all domain entities that needs to be elastic searchable
            //CreateIndexAndThenIndexDataAfterFetchFromActualDataRepository<AddressModel>();
        }

        private static void CreateIndexAndThenIndexDataAfterFetchFromActualDataRepository<TEntity>() where TEntity : IElasticSearchable
        {
           _elasticSearchContext.DeleteIndex<TEntity>();
           _elasticSearchContext.IndexCreate<TEntity>();
            ///TODO - Migrate the data from existing main DB to Elastc Search Store.
        }
    }
}
