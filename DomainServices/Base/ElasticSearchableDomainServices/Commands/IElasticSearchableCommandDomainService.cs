using Domain.Base;
using Domain.Base.Aggregates;
using DomainServices.Base.CommandDomainServices;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public interface IElasticSearchableCommandDomainService<TEntity> : ICommandDomainService<TEntity> where TEntity : ICommandAggregateRoot,IElasticSearchable
    {

    }
}
