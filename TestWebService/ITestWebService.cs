using System.ServiceModel;
using Domain.Base.Aggregates;

namespace TestWebService
{
    [ServiceContract]
    public interface ITestWebService<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        void Insert( TEntity item);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        void Update(TEntity item);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        void Delete(TEntity item);

        //
        //Similarly other repository methods can be added here
        //
    }
}
