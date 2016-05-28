using DomainServices.Base.CommandDomainServices;
using Domain.Base;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public interface IElasticSearchableCommandDomainService<TEntity> : ICommandDomainService<TEntity> where TEntity : ICommandAggregateRoot,IElasticSearchable
    {

    }
}
