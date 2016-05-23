using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Domain.Base;
using Infrastructure.Utilities;
using MongoDBDomainMaps;
using Repository.UnitOfWork;

namespace Repository
{
    public class MongoRepository<TEntity> : BaseRepository<TEntity> where TEntity : BaseMongoIdentityAndAuditableAggregateRoot
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        public MongoRepository(MongoShoppingBeesContext mongoContext):base(null)
        {
            ContractUtility.Requires<ArgumentNullException>(mongoContext != null, "esContext instance cannot be null");
            _mongoCollection = mongoContext.GetMongoCollection<TEntity>();
        }

        public MongoRepository(BaseUnitOfWork unitOfWork, MongoShoppingBeesContext mongoContext) : base(unitOfWork)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork != null, "unitOfWork instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(mongoContext != null, "esContext instance cannot be null");
            _mongoCollection = mongoContext.GetMongoCollection<TEntity>();
        }


        #region Overrides

        internal override void ActualInsert(TEntity item)
        {
            _mongoCollection.InsertOne(item);
        }

        internal override void ActualUpdate(TEntity item)
        {

            _mongoCollection.ReplaceOne(x => x.Id == item.Id, item);
        }

        internal override void ActualDelete(TEntity item)
        {
            _mongoCollection.DeleteOne(x => x.Id == item.Id);
        }

        internal override void ActualInsert(IList<TEntity> itemList)
        {
            _mongoCollection.InsertMany(itemList);
        }

        internal override void ActualUpdate(IList<TEntity> itemList)
        {
            foreach (var item in itemList)
            {
                _mongoCollection.ReplaceOne(x => x.Id == item.Id, item);
            }
        }

        internal override void ActualDelete(IList<TEntity> itemList)
        {
            foreach (var item in itemList)
            {
                _mongoCollection.DeleteOne(x => x.Id == item.Id);
            }
        }

        #endregion

        #region Facilitation for LINQ based Selects,JOINs etc from the classes, using the instance of this class
        public override IEnumerator<TEntity> GetEnumerator()
        {
            return _mongoCollection.AsQueryable().GetEnumerator();
        }

        public override Type ElementType
        {
            get { return _mongoCollection.AsQueryable().ElementType; }
        }

        public override Expression Expression
        {
            get { return _mongoCollection.AsQueryable().Expression; }
        }

        public override IQueryProvider Provider
        {
            get { return _mongoCollection.AsQueryable().Provider; }
        }

        #endregion

    }
}
