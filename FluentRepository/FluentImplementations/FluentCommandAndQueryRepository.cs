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

        public FluentCommandAndQueryRepository(UnitOfWorkData unitOfWorkData, IEnumerable<dynamic> repositoriesEnumerable = null, Queue<OperationData> operationsQueue = null)
        {
            _unitOfWorkData = unitOfWorkData;
            _repositoriesList = repositoriesEnumerable.IsNotNullOrEmpty() ? repositoriesEnumerable.ToList() : new List<dynamic>();
            _operationsQueue = operationsQueue ?? new Queue<OperationData>();
        }

        public IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepository.IsNotNull(), "commandRepository instance cannot be null");
            string commandRepositoryTypeName = typeof(ICommandRepository<>).Name;
            dynamic existingCommandRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(commandRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(existingCommandRepository == null, string.Format("Command Repository for {0} has already been set up", typeof(TEntity).Name));
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                ((dynamic)commandRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            }
            _repositoriesList.Add(commandRepository);
            return new FluentCommandRepository(_unitOfWorkData, _repositoriesList, _operationsQueue);
        }

        public IFluentCommandRepository SetUpCommandRepository(params dynamic[] commandRepositories)
        {
            return SetUpCommandRepository(commandRepositories);
        }

        public IFluentCommandRepository SetUpCommandRepository(IEnumerable<dynamic> commandRepositories)
        {
            ContractUtility.Requires<ArgumentNullException>(commandRepositories.IsNotNull(), "commandRepositories cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(commandRepositories.IsNotEmpty(), "commandRepositories cannot be empty");
            ContractUtility.Requires<ArgumentException>(commandRepositories.All(x => x.GetType().GetGenericTypeDefinition().GetInterface(typeof(ICommandRepository<>).Name) != null), "All repositories should be of type ICommandRepository<>");
            ContractUtility.Requires<ArgumentException>(commandRepositories.Count() == commandRepositories.Distinct().Count(), "One or more Command Repository has been repeated");
            string commandRepositoryTypeName = typeof(ICommandRepository<>).Name;
            commandRepositories.ForEach(x =>
            {
                dynamic existingCommandRepository = _repositoriesList.SingleOrDefault(y => y != null && y.GetType().GetGenericTypeDefinition().GetInterface(commandRepositoryTypeName) != null && y.GetType().GenericTypeArguments[0] == x.GetType().GenericTypeArguments[0]);
                ContractUtility.Requires<ArgumentNullException>(existingCommandRepository == null, string.Format("Command Repository for {0} has already been set up", x.GetType().GenericTypeArguments[0].Name));
            });
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                commandRepositories.ForEach(x => x.SetUnitOfWork(_unitOfWorkData.UnitOfWork));
            }

            //TODO- need to check why AddRange is not working as expected.
            // there is some issue of dynamic binding with List Addition as depicted here - 
            // http://stackoverflow.com/questions/15920844/system-collections-generic-ilistobject-does-not-contain-a-definition-for-ad
            commandRepositories.ForEach(x => _repositoriesList.Add((object)x));
            return new FluentCommandRepository(_unitOfWorkData, _repositoriesList, _operationsQueue);
        }

        public IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepository.IsNotNull(), "queryRepository instance cannot be null");
            string queryableRepositoryTypeName = typeof(IQueryableRepository<>).Name;
            dynamic existingQueryRepository = _repositoriesList.SingleOrDefault(x => x != null && x.GetType().GetGenericTypeDefinition().GetInterface(queryableRepositoryTypeName) != null && x.GetType().GenericTypeArguments[0] == typeof(TEntity));
            ContractUtility.Requires<ArgumentNullException>(existingQueryRepository == null, string.Format("Query Repository for {0} has already been set up", typeof(TEntity).Name));
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                ((dynamic)queryRepository).SetUnitOfWork(_unitOfWorkData.UnitOfWork);
            }
            _repositoriesList.Add(queryRepository);
            return new FluentQueryRepository(_unitOfWorkData, _repositoriesList, _operationsQueue);
        }

        public IFluentQueryRepository SetUpQueryRepository(params dynamic[] queryRepositories)
        {
            return SetUpQueryRepository(queryRepositories);
        }

        public IFluentQueryRepository SetUpQueryRepository(IEnumerable<dynamic> queryRepositories)
        {
            ContractUtility.Requires<ArgumentNullException>(queryRepositories.IsNotNull(), "queryRepositories cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(queryRepositories.IsNotEmpty(), "queryRepositories cannot be empty");
            ContractUtility.Requires<ArgumentException>(queryRepositories.All(x => x.GetType().GetGenericTypeDefinition().GetInterface(typeof(IQueryableRepository<>).Name) != null), "All repositories should be of type IQueryableRepository<>");
            ContractUtility.Requires<ArgumentException>(queryRepositories.Count() == queryRepositories.Distinct().Count(), "One or more Query Repository has been repeated");
            string queryableRepositoryTypeName = typeof(IQueryableRepository<>).Name;
            queryRepositories.ForEach(x =>
            {
                dynamic existingQueryRepository = _repositoriesList.SingleOrDefault(y => y != null && y.GetType().GetGenericTypeDefinition().GetInterface(queryableRepositoryTypeName) != null && y.GetType().GenericTypeArguments[0] == x.GetType().GenericTypeArguments[0]);
                ContractUtility.Requires<ArgumentNullException>(existingQueryRepository == null, string.Format("Query Repository for {0} has already been set up", x.GetType().GenericTypeArguments[0].Name));
            });
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                queryRepositories.ForEach(x => x.SetUnitOfWork(_unitOfWorkData.UnitOfWork));
            }
            //TODO- need to check why AddRange is not working as expected.
            // there is some issue of dynamic binding with List Addition as depicted here - 
            // http://stackoverflow.com/questions/15920844/system-collections-generic-ilistobject-does-not-contain-a-definition-for-ad
            queryRepositories.ForEach(x => _repositoriesList.Add((object)x));
            return new FluentQueryRepository(_unitOfWorkData, _repositoriesList, _operationsQueue);
        }
    }
}
