using ApplicationServices.Base.CommandApplicationServices;
using Domain.Base;

namespace ApplicationServices.Base.ElasticSearchableApplicationServices.Commands
{
    public interface IElasticSearchableCommandApplicationServiceAsync<TEntity> : ICommandApplicationServiceAsync<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {

    }
}
