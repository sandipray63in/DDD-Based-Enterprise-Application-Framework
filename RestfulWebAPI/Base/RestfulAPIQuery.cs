using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using DomainServices.Base.QueryableDomainServices;
using Infrastructure.Utilities;

namespace RestfulWebAPI.Base
{
    /// <summary>
    /// Go through  
    /// http://www.davepaquette.com/archive/2015/07/19/cancelling-long-running-queries-in-asp-net-mvc-and-web-api.aspx 
    /// to check how to send cancellation request from client side and check
    /// http://www.asp.net/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4#CancelToken 
    /// to set AsyncTimeOut.Ideally AsyncTimeOut should be set in the DB for the Controler/Action and probably cached and retrieved from the cache
    /// and then retrieve it from cache and then use it using some Global Custom Filter. Infact any Filter in general for which we apply some decorative 
    /// attribute should follow this approach.
    /// 
    /// TODO - To write a custom HTTPControllerSelector and HttpControllerActivator which will invoke RestfulAPI<TEntity> as per TEntity.May need a Lookup based
    /// approach.
    /// 
    /// TODO - Need to come up with a way to send the appropriate response(not only Ok or Badrequest) back to the client
    /// as per the content that needs to be sent or the exception thrown.May need to change Domain Services accordingly.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RestfulAPIQuery<TId,TEntity> : BaseDisposableAPIController
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot
    {
        protected readonly IQueryableDomainServiceAsync<TId,TEntity> _queryableDomainServiceAsync;

        public RestfulAPIQuery(IQueryableDomainServiceAsync<TId,TEntity> queryableDomainServiceAsync)
        {
            ContractUtility.Requires<ArgumentNullException>(queryableDomainServiceAsync != null, "queryableDomainServiceAsync instance cannot be null");
            _queryableDomainServiceAsync = queryableDomainServiceAsync;
        }

        /// <summary>
        /// OData enabled Query
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        public virtual async Task<IQueryable<TEntity>> Get(CancellationToken token = default(CancellationToken))
        {
            return await _queryableDomainServiceAsync.GetAsync(token);
        }

        [HttpGet]
        public virtual async Task<IList<TEntity>> Index(CancellationToken token = default(CancellationToken))
        {
            return await _queryableDomainServiceAsync.GetAllAsync(token);
        }

        public virtual async Task<TEntity> GetByID(TId id, CancellationToken token = default(CancellationToken))
        {
            return await _queryableDomainServiceAsync.GetByIDAsync(id, token);
        }

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _queryableDomainServiceAsync.Dispose();
        }

        #endregion
    }
}