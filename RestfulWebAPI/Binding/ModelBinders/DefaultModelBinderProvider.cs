using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace RequestBinding
{
    /// <summary>
    /// Basically you will use a custom model binder if the client JSON model mismatches with the server model
    /// (for some reason) or if you want to bind the model using some Session object so that the controller 
    /// methods need not have the converter logic to convert from Session object to the model or vice versa.
    /// </summary>
    /// <typeparam name="TModelBinder"></typeparam>
    public abstract class DefaultModelBinderProvider<TModelBinder> : ModelBinderProvider
        where TModelBinder : IModelBinder,new()
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return new TModelBinder();
        }
    }
}