using System.Data.Entity;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using Repository.Command;

namespace TestWebService
{
    public class EntityFrameworkCodeFirstCommandForIntegerType<TEntity> : EntityFrameworkCodeFirstCommand<TEntity, int>
        where TEntity : BaseEntity<int>, ICommandAggregateRoot
    {
        public EntityFrameworkCodeFirstCommandForIntegerType(DbContext dbContext) : base(dbContext)
        {

        }
    }
}