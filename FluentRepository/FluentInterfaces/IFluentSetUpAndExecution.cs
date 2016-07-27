using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentSetUpAndExecution : IFluentCommandAndQueryRepository
    {

        /// <param name="shouldAutomaticallyDisposeAllDisposables">This is set to false 
        /// since, ideally, all the disposables should be disposed from the entry point 
        /// of the application which can be ASP.NET Web API or ASP.NET MVC or ASP.NET Web 
        /// Forms or WCF Service or Workflow Service etc</param>
        void Execute(Boolean shouldAutomaticallyDisposeAllDisposables = false);

        /// <param name="shouldAutomaticallyDisposeAllDisposables">This is set to false 
        /// since, ideally, all the disposables should be disposed from the entry point 
        /// of the application which can be ASP.NET Web API or ASP.NET MVC or ASP.NET Web 
        /// Forms or WCF Service or Workflow Service etc</param>
        Task ExecuteAsync(CancellationToken token = default(CancellationToken), Boolean shouldAutomaticallyDisposeAllDisposables = false);
    }
}
