using FluentRepository.Abstractions;
using FluentRepository.Implementations;
using Infrastructure.Utilities;

namespace FluentRepository
{
    public static class FluentRepository
    {
        public static IFluentRepositoryAndUnitOfWork Initialize()
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentRepositoryAndUnitOfWork, FluentRepositoryAndUnitOfWork>();
        }
    }
}
