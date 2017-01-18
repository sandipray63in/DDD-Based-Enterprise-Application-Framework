using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using DomainServices.Base.CommandDomainServices;
using Domain.Base;
using Domain.Base.Aggregates;
using Infrastructure.Utilities;
using Repository.Base;
using Infrastructure.Logging.Loggers;
using Infrastructure.UnitOfWork;

namespace DomainServices.Base.ElasticSearchableDomainServices.Commands
{
    public class ElasticSearchableCommandDomainServiceAsync<TEntity> : CommandDomainServiceAsync<TEntity>, IElasticSearchableCommandDomainServiceAsync<TEntity> where TEntity : ICommandAggregateRoot, IElasticSearchable
    {
        protected readonly ICommandElasticSearchableRepository<TEntity> _elasticSearchableCommandRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ElasticSearchableCommandDomainServiceAsync(ICommandRepository<TEntity> commandRepository, ICommandElasticSearchableRepository<TEntity> elasticSearchableCommandRepository,ILogger logger) : base(commandRepository,logger)
        {
            ContractUtility.Requires<ArgumentNullException>(elasticSearchableCommandRepository != null, "elasticSearchableCommandRepository instance cannot be null");
            _elasticSearchableCommandRepository = elasticSearchableCommandRepository;
        }

        public ElasticSearchableCommandDomainServiceAsync(IUnitOfWork unitOfWork, ICommandRepository<TEntity> commandRepository, ICommandElasticSearchableRepository<TEntity> elasticSearchableCommandRepository, ILogger logger) : base(commandRepository,logger)
        {
            ContractUtility.Requires<ArgumentNullException>(unitOfWork != null, "unitOfWork instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(elasticSearchableCommandRepository != null, "elasticSearchableCommandRepository instance cannot be null");
            _elasticSearchableCommandRepository = elasticSearchableCommandRepository;
        }

        public override async Task<bool> InsertAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.InsertAsync(item, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.InsertAsync(item, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> UpdateAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.UpdateAsync(item, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.UpdateAsync(item, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> DeleteAsync(TEntity item, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.DeleteAsync(item, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.DeleteAsync(item, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> InsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.InsertAsync(items, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.InsertAsync(items, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> UpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.UpdateAsync(items, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.UpdateAsync(items, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> DeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.DeleteAsync(items, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.DeleteAsync(items, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> BulkInsertAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.BulkInsertAsync(items, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> BulkUpdateAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.BulkUpdateAsync(items, token, operationToExecuteBeforeNextOperation);
                }
            );
        }

        public override async Task<bool> BulkDeleteAsync(IList<TEntity> items, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null)
        {
            return await InvokeAfterWrappingWithinExceptionHandling(async () =>
                {
                    //TODO - Incorporate UnitOfWork logic
                    await _repository.BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation);
                    await _elasticSearchableCommandRepository.BulkDeleteAsync(items, token, operationToExecuteBeforeNextOperation);
                }
            );
        }
    }
}
