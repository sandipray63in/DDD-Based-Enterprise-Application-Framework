using System;
using System.Collections.Generic;
using Domain.Base.ConsumerModels;

namespace DomainServices.Base.ConsumerModelServices
{
    /// <summary>
    /// Implement this class to pass on the Consumer Model and transform it accordingly into 
    /// Domain Entities and save the data and similarly transform the Domain objects into Consumer 
    /// Model and and fetch the required data. Auto Mapper can be used for Transformation between 
    /// Domain Model and Consumer Model and vice versa.
    /// 
    /// Also the implementations should use Repository or Fluent Repository to actually save/retrieve 
    /// the Domain objects.
    /// </summary>
    /// <typeparam name="TConsumerModel"></typeparam>
    public interface IConsumerModelService<TConsumerModel> : IDisposable where TConsumerModel : ICommandConsumerModel
    {
        void Insert(TConsumerModel item, Action operationToExecuteBeforeNextOperation = null);
        void Update(TConsumerModel item, Action operationToExecuteBeforeNextOperation = null);
        void Delete(TConsumerModel item, Action operationToExecuteBeforeNextOperation = null);
        void Insert(IEnumerable<TConsumerModel> items, Action operationToExecuteBeforeNextOperation = null);
        void Update(IEnumerable<TConsumerModel> items, Action operationToExecuteBeforeNextOperation = null);
        void Delete(IEnumerable<TConsumerModel> items, Action operationToExecuteBeforeNextOperation = null);
        void BulkInsert(IEnumerable<TConsumerModel> items, Action operationToExecuteBeforeNextOperation = null);
        void BulkUpdate(IEnumerable<TConsumerModel> items, Action operationToExecuteBeforeNextOperation = null);
        void BulkDelete(IEnumerable<TConsumerModel> items, Action operationToExecuteBeforeNextOperation = null);

        ///More methods can be added similarly e.g. Async versions of the above methods.
    }
}
