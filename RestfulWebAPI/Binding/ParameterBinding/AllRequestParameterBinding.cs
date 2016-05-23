using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;

namespace RestfulWebAPI.Binding.MediaType
{
    public class AllRequestParameterBinding<TEntity> : HttpParameterBinding
        where TEntity : class
    {
        private HttpParameterBinding modelBinding;
        private HttpParameterBinding formatterBinding;

        public AllRequestParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {
            // GetBinding returns ModelBinderParameterBinding
            modelBinding = new ModelBinderAttribute().GetBinding(descriptor);

            // GetBinding returns FormatterParameterBinding
            formatterBinding = new FromBodyAttribute().GetBinding(descriptor);
        }

        public override async Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,HttpActionContext context,CancellationToken cancellationToken)
        {
            // Perform formatter binding
            await formatterBinding.ExecuteBindingAsync(metadataProvider, context, cancellationToken);

            // and store the resulting model
            var entity = GetValue(context) as TEntity;

            // Perform model binding
            await modelBinding.ExecuteBindingAsync(metadataProvider, context, cancellationToken);

            // and store the resulting model
            var entityFromUri = GetValue(context) as TEntity;

            // Apply the delta on top of the entity object resulting from formatter binding
            entity = Merge(entity, entityFromUri);

            // Set the merged model in the context
            SetValue(context, entity);
        }

        private TEntity Merge(TEntity baseEntity, TEntity newEntity)
        {
            var entityType = typeof(TEntity);

            foreach (var property in entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var baseValue = property.GetValue(baseEntity, null);
                var newValue = property.GetValue(newEntity, null);
                var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
                if (baseValue.IsNull() || baseValue.Equals(defaultValue))
                    property.SetValue(baseEntity, newValue);
            }
            return baseEntity;
        }

    }

}