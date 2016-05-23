using System.ServiceModel;
using Domain.Base;

namespace TestService
{
    [ServiceContract]
    public interface ITestServiceBasedOnSQLLite<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        [OperationContract]
        void Insert( TEntity item);

        [OperationContract]
        void Update(TEntity item);

        [OperationContract]
        void Delete(TEntity item);

        //
        //Similarly other repository methods can be added here
        //
    }
}
