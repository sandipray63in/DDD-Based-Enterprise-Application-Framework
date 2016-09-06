using System.Data.Entity;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using Repository.Command;

namespace TestWebService
{
    public class EntityFrameworkCodeFirstCommandForIntegerType<TEntity> : EntityFrameworkCodeFirstCommand<int, TEntity>
        where TEntity : BaseEntity<int>, ICommandAggregateRoot
    {
        public EntityFrameworkCodeFirstCommandForIntegerType(DbContext dbContext) : base(dbContext)
        {

        }
    }
}