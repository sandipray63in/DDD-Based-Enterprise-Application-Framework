using Infrastructure.WCFExtensibility.UnityIntegration;
using Repository.Base;
using TestEFDomainAndContext.TestDomains;

namespace TestWebService
{
    [UnityServiceBehavior]
    public class EmployeeTestWebService : TestWebService<Employee>, IEmployeeTestWebService
    {
        public EmployeeTestWebService(ICommandRepository<Employee> commandRepository) : base(commandRepository)
        {

        }
    }
}