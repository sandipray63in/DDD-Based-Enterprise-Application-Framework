using System.Data.Common;
using System.ServiceModel;
using Effort.Provider;
using Domain.Base;

namespace TestService
{
    /// <summary>
    /// This Service was developed to be used alongwith Effort Framework but EffortConnection related 
    /// DBContext doesn't persist data across cross App Domains.Was needed to test Unit Of Work 
    /// explicit Transaction handling.Anyways, still keeping it.
    /// 
    /// Need to use the transient/persistent DBConnection object of Effort Framework to test from Repository.Testing Library
    /// and that's why need to pass an instance of DbConnection as a parameter in the methods.This should not 
    /// be needed in a real webservice. This is done just for testing purpose.
    /// 
    /// Always better to use ServiceKnowntype as compared to KnownType as suggested here - 
    /// http://blogs.msdn.com/b/domgreen/archive/2009/04/13/wcf-using-interfaces-in-method-signatures.aspx
    /// Here since all the methods need to know the EffortConnection type, the ServiceKnownType attribute 
    /// is applied at the class level.
    /// 
    /// Other good articles on KnownType are http://blogs.msdn.com/b/sowmy/archive/2006/06/06/all-about-knowntypes.aspx
    /// and http://blogs.msdn.com/b/youssefm/archive/2009/06/05/introducing-a-new-datacontractserializer-feature-the-datacontractresolver.aspx
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [ServiceKnownType(typeof(EffortConnection))]
    [ServiceContract]
    public interface ITestService<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Insert(DbConnection dbConnection, TEntity item);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Update(DbConnection dbConnection, TEntity item);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Delete(DbConnection dbConnection, TEntity item);

        //
        //Similarly other repository methods can be added here
        //
    }
}
