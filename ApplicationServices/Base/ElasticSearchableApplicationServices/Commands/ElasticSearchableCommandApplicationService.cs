using System;
using System.Collections.Generic;
using ApplicationServices.Base.CommandApplicationServices;
using Domain.Base;
using Infrastructure.Utilities;
using Repository.Base;
using Repository.UnitOfWork;

namespace ApplicationServices.Base.ElasticSearchableApplicationServices.Commands
{
    public class ElasticSearchableCommandApplicationService<TEntity> : CommandApplicationService<TEntity>, IElasticSearchableCommandApplicationService<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {
        protected readonly ICommandElasticSearchableRepository<TEntity> _elasticSearchableCommandRepository;
        protected readonly BaseUnitOfWork _unitOfWork;

        public ElasticSearchableCommandApplicationService(ICommandRepository<TEntity> commandRepository, ICommandElasticSearchableRepository<TEntity> elasticSearchableCommandRepository) : base(commandRepository)
        {
            ContractUtility.Requires<ArgumentNullException>(elasticSearchableCommandRepository != null, "elasticSearchableCommandRepository instance cannot be null");
            _elasticSearchableCommandRepository = elasticSearchableCommandRepository;
        }

        public ElasticSearchableCommandApplicationService(BaseUnitOfWork unitOfWork, ICommandRepository<TEntity> commandRepository, ICommandElasticSearchableRepository<TEntity> elasticSearchableCommandRepository) : base(commandRepository)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork != null, "unitOfWork instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(elasticSearchableCommandRepository != null, "elasticSearchableCommandRepository instance cannot be null");
            _elasticSearchableCommandRepository = elasticSearchableCommandRepository;
        }

        public override bool Insert(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.Insert(item, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.Insert(item, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool Update(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.Update(item, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.Update(item, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool Delete(TEntity item, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.Delete(item, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.Delete(item, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool Insert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.Insert(items, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.Insert(items, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool Update(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.Update(items, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.Update(items, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool Delete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.Delete(items, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.Delete(items, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool BulkInsert(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.BulkInsert(items, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.BulkInsert(items, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool BulkUpdate(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.BulkUpdate(items, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.BulkUpdate(items, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override bool BulkDelete(IList<TEntity> items, Action operationToExecuteBeforeNextOperation = null)
        {
            return InvokeAfterWrappingWithinExceptionHandling(() =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    _repository.BulkDelete(items, operationToExecuteBeforeNextOperation);
                    _elasticSearchableCommandRepository.BulkDelete(items, operationToExecuteBeforeNextOperation);
                }
            );
        }
    }
}
