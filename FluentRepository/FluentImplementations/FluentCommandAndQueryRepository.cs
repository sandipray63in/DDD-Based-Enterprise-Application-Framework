using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Base.Aggregates;
using FluentRepository.FluentInterfaces;
using Infrastructure.Utilities;
using Repository.Base;

namespace FluentRepository.FluentImplementations
{
    internal class FluentCommandAndQueryRepository : IFluentCommandAndQueryRepository
    {
        protected readonly UnitOfWorkData _unitOfWorkData;
        protected readonly IList<dynamic> _repositoriesList;
        protected readonly Queue<OperationData> _operationsQueue;

        public FluentCommandAndQueryRepository(UnitOfWorkData unitOfWorkData, IList<dynamic> repositoriesList = null, Queue<OperationData> operationsQueue = null)
        {
            _unitOfWorkData = unitOfWorkData;
            _repositoriesList = repositoriesList ?? new List<dynamic>();
            _operationsQueue = operationsQueue ?? new Queue<OperationData>();
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository.IsNotNull(), "commandRepository instance cannot be null");
            CheckForOperations();
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                ((dynamic)commandRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            }
            var commandRepositoryList = new List<dynamic> { commandRepository };
            _repositoriesList.AddRange(commandRepositoryList);
            return new FluentCommandRepository(_unitOfWorkData, commandRepositoryList, _repositoriesList, _operationsQueue);
        }

        public IFluentCommandRepository SetUpCommandRepository(params dynamic[] commandRepositories)
        {
            return SetUpCommandRepository(commandRepositories.ToList());
        }

        public IFluentCommandRepository SetUpCommandRepository(IList<dynamic> commandRepositories)
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepositories.IsNotNull(), "commandRepositories cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(commandRepositories.IsNotEmpty(), "commandRepositories cannot be empty");
            ContractUtility.Requires<ArgumentException>(commandRepositories.All(x => x.GetType().GetGenericTypeDefinition().GetInterface(typeof(ICommandRepository<>).Name) != null), "All repositories should be of type ICommandRepository<>");
            ContractUtility.Requires<ArgumentException>(commandRepositories.Count() == commandRepositories.Distinct().Count(), "One or more Command Repository has been repeated");
            CheckForOperations();
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                commandRepositories.ToList().ForEach(x => x.SetUnitOfWork(_unitOfWorkData.UnitOfWork));
            }
            _repositoriesList.AddRange(commandRepositories);
            return new FluentCommandRepository(_unitOfWorkData, commandRepositories, _repositoriesList, _operationsQueue);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository.IsNotNull(), "queryRepository instance cannot be null");
            CheckForOperations();
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                ((dynamic)queryRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            }
            var queryRepositoryList = new List<dynamic> { queryRepository };
            _repositoriesList.AddRange(queryRepositoryList);
            return new FluentQueryRepository(_unitOfWorkData, queryRepositoryList, _repositoriesList, _operationsQueue);
        }

        public IFluentQueryRepository SetUpQueryRepository(params dynamic[] queryRepositories)
        {
            return SetUpQueryRepository(queryRepositories.ToList());
        }

        public IFluentQueryRepository SetUpQueryRepository(IList<dynamic> queryRepositories)
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepositories.IsNotNull(), "queryRepositories cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(queryRepositories.IsNotEmpty(), "queryRepositories cannot be empty");
            ContractUtility.Requires<ArgumentException>(queryRepositories.All(x => x.GetType().GetGenericTypeDefinition().GetInterface(typeof(IQueryableRepository<>).Name) != null), "All repositories should be of type IQueryableRepository<>");
            ContractUtility.Requires<ArgumentException>(queryRepositories.Count() == queryRepositories.Distinct().Count(), "One or more Query Repository has been repeated");
            CheckForOperations();
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                queryRepositories.ToList().ForEach(x => x.SetUnitOfWork(_unitOfWorkData.UnitOfWork));
            }
            _repositoriesList.AddRange(queryRepositories);
            return new FluentQueryRepository(_unitOfWorkData, queryRepositories, _repositoriesList, _operationsQueue);
        }

        protected virtual void CheckForOperations() { }
    }
}
