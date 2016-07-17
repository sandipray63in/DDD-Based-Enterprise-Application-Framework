using System;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Infrastructure.Extensions;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandAndQueryRepository : IFluentCommandAndQueryRepository
    {
        private UnitOfWorkData _unitOfWorkData;

        internal FluentCommandAndQueryRepository(UnitOfWorkData unitOfWorkData)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkData instance cannot be null");
            _unitOfWorkData = unitOfWorkData;
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentCommandRepository, FluentCommandRepository>(_unitOfWorkData, commandRepositoryFunc.ConvertFunc<ICommandRepository<TEntity>, dynamic>(),null);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot
        {
            return ContainerUtility.CheckRegistrationAndGetInstance<IFluentQueryRepository, FluentQueryRepository>(_unitOfWorkData, queryRepositoryFunc.ConvertFunc<IQueryableRepository<TEntity>, dynamic>(),null);
        }
    }
}
