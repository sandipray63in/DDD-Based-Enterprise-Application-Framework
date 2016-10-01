using System.ServiceModel;
using TestEFDomainAndContext.TestDomains;

namespace TestWebService
{
    [ServiceContract]
    public interface IDepartmentTestWebService : ITestWebService<Department>
    {

    }
}
