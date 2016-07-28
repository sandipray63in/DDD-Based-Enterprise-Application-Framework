using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentRepository.FluentInterfaces;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace FluentRepository.FluentImplementations
{
    internal abstract class FluentSetUpAndExecution : FluentCommandAndQueryRepository,IFluentSetUpAndExecution
    {

        internal protected FluentSetUpAndExecution(UnitOfWorkData unitOfWorkData, IList<dynamic> repositoriesList, Queue<OperationData> operationsQueue) : base(unitOfWorkData, repositoriesList, operationsQueue)
        {
        }

        public void Execute(Boolean shouldAutomaticallyDisposeAllDisposables = false)
        {
            ContractUtility.Requires<ArgumentOutOfRangeException>(_operationsQueue.IsNotNullOrEmpty(), "Atleast one operation must be there to be executed.");

            ContractUtility.Requires<NotSupportedException>(_operationsQueue.All(y => y.AsyncOperation.IsNull()),
                                    "Async operations are not supported by Execute method.Use ExecuteAsync instead.");
            while (_operationsQueue.Count > 0)
            {
                var operationData = _operationsQueue.Dequeue();
                if (operationData.Operation.IsNotNull())
                {
                    operationData.Operation();
                }
            }
            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                var unitOfWork = _unitOfWorkData.UnitOfWork;
                unitOfWork.Commit(_unitOfWorkData.ShouldAutomaticallyRollBackOnTransactionException, _unitOfWorkData.ShouldThrowOnException);
                if (shouldAutomaticallyDisposeAllDisposables)
                {
                    unitOfWork.Dispose();
                }
            }
            if (shouldAutomaticallyDisposeAllDisposables && _repositoriesList.IsNotNullOrEmpty())
            {
                _repositoriesList.Select(x => x as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }
        
        public async Task ExecuteAsync(CancellationToken token = default(CancellationToken), Boolean shouldAutomaticallyDisposeAllDisposables = false)
        {
            ContractUtility.Requires<ArgumentOutOfRangeException>(_operationsQueue.IsNotNullOrEmpty(), "Atleast one operation must be there to be executed.");
            ContractUtility.Requires<NotSupportedException>(_operationsQueue.Any(y => y.AsyncOperation.IsNotNull()),
                "If ExecuteAsync method is used,there needs to be atleast one async operation exceuted." +
                "Please use Execute method(instead of ExecuteAsync) if there is not " +
                "a single async operation.");

            while (_operationsQueue.Count > 0)
            {
                var operationData = _operationsQueue.Dequeue();
                if (operationData.Operation.IsNotNull())
                {
                    operationData.Operation();
                }
                else if (operationData.AsyncOperation.IsNotNull())
                {
                    await operationData.AsyncOperation();
                }
            }

            if (_unitOfWorkData != null && _unitOfWorkData.UnitOfWork != null)
            {
                var unitOfWork = _unitOfWorkData.UnitOfWork;
                await unitOfWork.CommitAsync(token, _unitOfWorkData.ShouldAutomaticallyRollBackOnTransactionException, _unitOfWorkData.ShouldThrowOnException);
                if (shouldAutomaticallyDisposeAllDisposables)
                {
                    unitOfWork.Dispose();
                }
            }
            if (shouldAutomaticallyDisposeAllDisposables && _repositoriesList.IsNotNullOrEmpty())
            {
                _repositoriesList.Select(x => x as IDisposable).Where(x => x.IsNotNull()).CleanUp();
            }
        }
    }
}
