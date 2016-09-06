using Infrastructure.WCFExtensibility.UnityIntegration;
using Repository.Base;
using TestEFDomainAndContext.TestDomains;

namespace TestWebService
{
    [UnityServiceBehavior]
    public class DepartmentTestWebService : TestWebService<Department>, IDepartmentTestWebService
    {
        public DepartmentTestWebService(ICommandRepository<Department> commandRepository) : base(commandRepository)
        {

        }
    }
}