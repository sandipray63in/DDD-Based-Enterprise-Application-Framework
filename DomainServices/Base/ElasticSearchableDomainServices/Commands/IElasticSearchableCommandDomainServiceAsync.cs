using Domain.Base;
using Domain.Base.Aggregates;
using DomainServices.Base.CommandDomainServices;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public interface IElasticSearchableCommandDomainServiceAsync<TEntity> : ICommandDomainServiceAsync<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {

    }
}
