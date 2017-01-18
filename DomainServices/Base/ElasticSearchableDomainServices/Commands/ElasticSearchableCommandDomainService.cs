using System;
using System.Collections.Generic;
using DomainServices.Base.CommandDomainServices;
using Domain.Base;
using Domain.Base.Aggregates;
using Infrastructure.Utilities;
using Repository.Base;
using Infrastructure.Logging.Loggers;
using Infrastructure.UnitOfWork;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public class ElasticSearchableCommandDomainService<TEntity> : CommandDomainService<TEntity>, IElasticSearchableCommandDomainService<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {
        protected readonly ICommandElasticSearchableRepository<TEntity> _elasticSearchableCommandRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ElasticSearchableCommandDomainService(ICommandRepository<TEntity> commandRepository, ICommandElasticSearchableRepository<TEntity> elasticSearchableCommandRepository, ILogger logger) : base(commandRepository,logger)
        {
            ContractUtility.Requires<ArgumentNullException>(elasticSearchableCommandRepository != null, "elasticSearchableCommandRepository instance cannot be null");
            _elasticSearchableCommandRepository = elasticSearchableCommandRepository;
        }

        public ElasticSearchableCommandDomainService(IUnitOfWork unitOfWork, ICommandRepository<TEntity> commandRepository, ICommandElasticSearchableRepository<TEntity> elasticSearchableCommandRepository, ILogger logger) : base(commandRepository, logger)
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
