using System;
using System.ServiceModel.Configuration;

namespace Infrastructure.WCFExtensibility
{
    /// <summary>
    /// For basic info on WCF Behaviours refer - 
    /// http://www.codeproject.com/Articles/37156/Focus-on-the-Extension-of-WCF-Behavior and 
    /// for an extensive tour of WCF Extensibility refer - 
    /// https://blogs.msdn.microsoft.com/carlosfigueira/2011/03/14/wcf-extensibility/
    /// </summary>
    /// <typeparam name="TBehaviour"></typeparam>
    public abstract class BehaviorExtensionElement<TBehaviour> : BehaviorExtensionElement where TBehaviour : class
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(TBehaviour);
            }
        }

        /// <summary>
        /// Here it's considered that TBehaviour has a default constructor.
        /// If TBehaviour doesn't have one default constructor, then override this method appropriately 
        /// and build a proper instance(if required, use the Builder pattern) and then return the instance.
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return Activator.CreateInstance<TBehaviour>();
        }
    }
}
