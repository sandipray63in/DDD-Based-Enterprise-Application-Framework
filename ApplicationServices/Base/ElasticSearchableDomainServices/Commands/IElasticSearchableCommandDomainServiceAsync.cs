using DomainServices.Base.CommandDomainServices;
using Domain.Base;
using Domain.Base.Aggregates;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public interface IElasticSearchableCommandDomainServiceAsync<TEntity> : ICommandDomainServiceAsync<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {

    }
}
