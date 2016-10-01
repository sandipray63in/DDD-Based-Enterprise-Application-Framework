
namespace Infrastructure.WCFExtensibility.UnityIntegration
{
    /// <summary>
    /// Can be used for WCF or Workflow Services(since Workflow Services are just a WCF wrapper over 
    /// .NET Workflow)
    /// 
    /// Workflow Services doesn't inherently support Dependency Injection(rather supports Service 
    /// Locator pattern) as indicated in these articles viz. 
    /// http://stackoverflow.com/questions/3825791/dependency-injection-ioc-in-workflow-foundation-4 
    /// and http://www.neovolve.com/2010/09/15/dependency-injection-options-for-windows-workflow-4/
    /// 
    /// Also, Workflow Services won't allow to define some attribute like that used in this project 
    /// (e.g.one such usage can be found in TestServiceBasedOnSQLCE class) and using 
    /// BehaviourExtensionElement is the most feasible approach.An implementation of using Unity 
    /// alongwith Workflow Services(not using DI but Service Locator pattern) is available here - 
    /// http://www.cauldwell.net/patrick/blog/UnityAndWorkflowServicesHostedInIIS.aspx. Something in 
    /// similar lines using MEF(Managed Extensibility Framework) can be found here - 
    /// http://www.codeproject.com/Articles/114472/MEF-and-Workflow-Services-Integration.
    /// </summary>
    public class UnityBehaviourExtensionElement : BehaviorExtensionElement<UnityServiceBehavior>
    {

    }
}
