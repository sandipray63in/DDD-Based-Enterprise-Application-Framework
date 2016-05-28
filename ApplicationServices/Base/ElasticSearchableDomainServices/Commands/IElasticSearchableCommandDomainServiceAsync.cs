using DomainServices.Base.CommandDomainServices;
using Domain.Base;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public interface IElasticSearchableCommandDomainServiceAsync<TEntity> : ICommandDomainServiceAsync<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {

    }
}
