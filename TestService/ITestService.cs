using System.ServiceModel;
using Domain.Base.Aggregates;

namespace TestService
{
    [ServiceContract]
    public interface ITestService<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Insert( TEntity item);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Update(TEntity item);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Delete(TEntity item);

        //
        //Similarly other repository methods can be added here
        //
    }
}
