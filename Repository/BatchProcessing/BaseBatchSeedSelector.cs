using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Infrastructure.Extensions;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;
using Repository.Base;

namespace Repository.BatchProcessing
{
    public abstract class BaseBatchSeedSelector<TEntity,TId> : IBatchSeedSelector 
        where TEntity : IQueryableAggregateRoot
        where TId : struct,IComparable<TId>
    {
        private readonly IQueryableRepository<TEntity> _seedQueryableRepository;
        private TId _currentBatchStartPosition;
        private int _batchSize;
        private IEnumerable<TEntity> _currentBatch;
        private readonly TId _maxPropertyalue;
        private readonly ILogger _logger;
        private IEnumerable[] _allBatchSelectorEnumerables;

        public BaseBatchSeedSelector(IQueryableRepository<TEntity> seedQueryableRepository, int batchSize,ILogger logger)
        {
            ContractUtility.Requires<ArgumentNullException>(seedQueryableRepository.IsNotNull(), "seedQueryableRepository cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(batchSize > 0, "batchSize should be greater than 0");
            ContractUtility.Requires<ArgumentNullException>(logger.IsNotNull(), "logger cannot be null");
            _seedQueryableRepository = seedQueryableRepository;
            _batchSize = batchSize;
            _logger = logger;
            _currentBatchStartPosition = GetMinPropertyValue();
            _maxPropertyalue = GetMaxPropertyValue();
        }

        public virtual void Execute()
        {
            TId currentBatchEndPosition = _currentBatchStartPosition.Add((TId)Convert.ChangeType(_batchSize,typeof(TId)));
            Expression<Func<TEntity, TId>> entityPropertyBasedOnWhichMinOrMaxValueShouldBeFetchedExpression = x => EntityPropertyBasedOnWhichMinOrMaxValueShouldBeFetched(x);
            _currentBatch = BatchQueryable.Between(entityPropertyBasedOnWhichMinOrMaxValueShouldBeFetchedExpression, _currentBatchStartPosition,currentBatchEndPosition).ToList();

            if (_currentBatch.IsNotEmpty())
            {
                TId actualBatchEndPosition = _currentBatch.Max(x => EntityPropertyBasedOnWhichMinOrMaxValueShouldBeFetched(x));
                _logger.LogMessage("Batch data selected is between start position of " + _currentBatchStartPosition + " and end position of " + actualBatchEndPosition);
                _logger.LogMessage("Number of rows selected between start position of " + _currentBatchStartPosition + " and end position of " + actualBatchEndPosition + " is : " + _currentBatch.Count());
            }
            ProcessCurrentBatchFurther(_currentBatch);
            _allBatchSelectorEnumerables = GetAllBatchesBasedOnCurrentSeedBatch(_currentBatch);
        }

        public object Current
        {
            get
            {
                return _allBatchSelectorEnumerables;
            }
        }

        public bool MoveNext()
        {
            if (_currentBatch.IsNullOrEmpty())
            {
                return false;
            }
            _currentBatchStartPosition = _currentBatch.Max(x => EntityPropertyBasedOnWhichMinOrMaxValueShouldBeFetched(x));
            _currentBatchStartPosition = _currentBatchStartPosition.Add(ValueToIncrementByToGoToNextBatch);
            Execute();
            return _currentBatch.IsNotNullOrEmpty();
        }

        public void Reset()
        {
            _currentBatchStartPosition = GetMinPropertyValue();
            Execute();
        }

        protected virtual IQueryable<TEntity> BatchQueryable
        {
            get { return _seedQueryableRepository; }
        }

        protected virtual TId ValueToIncrementByToGoToNextBatch
        {
            get { return (TId)Convert.ChangeType(1, typeof(TId)); }
        }

        protected abstract Func<TEntity,TId> EntityPropertyBasedOnWhichMinOrMaxValueShouldBeFetched { get; }

        protected virtual void ProcessCurrentBatchFurther(IEnumerable currentBatch) { }

        protected virtual IEnumerable[] GetAllBatchesBasedOnCurrentSeedBatch(IEnumerable currentSeedBatch)
        {
            IEnumerable[] batchEnumerables = new IEnumerable[1];
            batchEnumerables[0] = currentSeedBatch;
            return batchEnumerables;
        }

        /// <summary>
        /// Gets the Max property value from the repository.
        /// </summary>
        /// <returns></returns>
        private TId GetMaxPropertyValue()
        {
            return _seedQueryableRepository.Max(x => EntityPropertyBasedOnWhichMinOrMaxValueShouldBeFetched(x));
        }

        /// <summary>
        /// Gets the Min property value from the repository.
        /// </summary>
        /// <returns></returns>
        private TId GetMinPropertyValue()
        {
            return _seedQueryableRepository.Min(x => EntityPropertyBasedOnWhichMinOrMaxValueShouldBeFetched(x));
        }
    }
}
