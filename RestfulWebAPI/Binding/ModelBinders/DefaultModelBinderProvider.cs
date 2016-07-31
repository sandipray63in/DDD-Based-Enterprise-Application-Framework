using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace RequestBinding
{
    public abstract class DefaultModelBinderProvider<TModelBinder> : ModelBinderProvider
        where TModelBinder : IModelBinder,new()
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return new TModelBinder();
        }
    }
}