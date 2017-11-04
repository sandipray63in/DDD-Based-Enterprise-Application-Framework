using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Base.ConsumerModels;

namespace DomainServices.Base.ConsumerModelServices
{
    public interface IQueryableConsumerModelService<TConsumerModel> : IDisposable where TConsumerModel : IQueryableConsumerModel
    {
        IEnumerable<TConsumerModel> GetAll();
        TConsumerModel GetByID(int id);
        IEnumerable<TConsumerModel> GetByFilterExpression(Expression<Func<TConsumerModel, bool>> whereExpression);
        IEnumerable<TConsumerModel> GetByOrderExpression<TKey>(Expression<Func<TConsumerModel, TKey>> orderExpression);
        IEnumerable<TConsumerModel> GetByExpression<TKey>(Expression<Func<TConsumerModel, bool>> whereExpression, Expression<Func<TConsumerModel, TKey>> orderExpression);

        ///More methods can be added similarly e.g. Async versions of the above methods.
    }
}
