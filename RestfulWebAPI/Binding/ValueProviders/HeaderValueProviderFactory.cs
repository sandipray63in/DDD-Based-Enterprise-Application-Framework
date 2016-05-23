using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace RestfulWebAPI.Binding.ValueProviders
{
    public class HeaderValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            var request = actionContext.ControllerContext.Request;
            return new HeaderValueProvider(request.Headers);
        }
    }
}