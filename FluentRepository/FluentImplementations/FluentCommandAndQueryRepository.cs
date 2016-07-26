using System;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandAndQueryRepository : IFluentCommandAndQueryRepository
    {
        private UnitOfWorkData _unitOfWorkData;

        public FluentCommandAndQueryRepository(UnitOfWorkData unitOfWorkData)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWorkData.IsNotNull(), "unitOfWorkData instance cannot be null");
            _unitOfWorkData = unitOfWorkData;
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository.IsNotNull(), "commandRepository instance cannot be null");
            ((dynamic)commandRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            return new FluentCommandRepository(_unitOfWorkData, commandRepository, commandRepository.GetType(), null);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository.IsNotNull(), "queryRepository instance cannot be null");
            ((dynamic)queryRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            return new FluentQueryRepository(_unitOfWorkData, queryRepository, queryRepository.GetType(), null);
        }
    }
}
