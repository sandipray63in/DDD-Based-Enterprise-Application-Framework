using TestEFDomainAndContext.TestDomains;

namespace Testing.Integration
{
    /// <summary>
    /// Ideally this factory should be created by getting the type from the name(of TEntity) using Reflection API 
    /// or Expression API and then caching the types and based on the key, getting the instance using the type from the cache
    /// rather than an if-else way as shown below.
    /// </summary>
    internal static class TestServiceCommandFactory
    {
        internal static dynamic GetServiceClientInstance<TEntity>()
        {
            if(typeof(TEntity) == typeof(Department))
            {
                return new DepartmentTestWebServiceClient();
            }
            else if(typeof(TEntity) == typeof(Employee))
            {
                return new EmployeeTestWebServiceClient();
            }
            return null;
        }
    }
}
