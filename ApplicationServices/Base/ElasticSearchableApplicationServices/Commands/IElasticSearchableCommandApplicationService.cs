using ApplicationServices.Base.CommandApplicationServices;
using Domain.Base;

namespace ApplicationServices.Base.ElasticSearchableApplicationServices.Commands
{
    public interface IElasticSearchableCommandApplicationService<TEntity> : ICommandApplicationService<TEntity> where TEntity : ICommandAggregateRoot,IElasticSearchable
    {

    }
}
