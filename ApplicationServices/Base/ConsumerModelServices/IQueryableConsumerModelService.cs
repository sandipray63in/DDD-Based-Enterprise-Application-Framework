using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Base;

namespace ApplicationServices.Base.ConsumerModelServices
{
    public interface IQueryableConsumerModelService<TConsumerModel> where TConsumerModel : IQueryableConsumerModel
    {
        IList<TConsumerModel> GetAll();
        TConsumerModel GetByID(int id);
        IList<TConsumerModel> GetByFilterExpression(Expression<Func<TConsumerModel, bool>> whereExpression);
        IList<TConsumerModel> GetByOrderExpression<TKey>(Expression<Func<TConsumerModel, TKey>> orderExpression);
        IList<TConsumerModel> GetByExpression<TKey>(Expression<Func<TConsumerModel, bool>> whereExpression, Expression<Func<TConsumerModel, TKey>> orderExpression);

        ///More methods can be added similarly e.g. Async versions of the above methods.
    }
}
