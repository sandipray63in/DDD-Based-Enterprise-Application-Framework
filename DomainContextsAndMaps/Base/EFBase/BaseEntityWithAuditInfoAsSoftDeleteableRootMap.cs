using System.Data.Entity.ModelConfiguration.Configuration;
using Domain.Base.Entities.WithAuditInfo;

namespace DomainContextsAndMaps.Base.EFBase
{
    public abstract class BaseEntityWithAuditInfoAsSoftDeleteableRootMap<TId,TEntity> : BaseEntityWithAuditInfoRootMap<TId, TEntity>
        where TId : struct
        where TEntity : BaseEntityWithAuditInfoAsSoftDeleteableCommandAggregateRoot<TId>
    {
        protected virtual string IsDeletedColumnName { get; } = "col_is_deleted";
        protected virtual string DeletedOnColumnName { get; } = "col_deleted_on";

        protected override void SetSpecificPropertiesForEntity()
        {
            ExtendIsDeletedPropertyWithOtherConfigurations(Property(p => p.IsDeleted).HasColumnName(IsDeletedColumnName));
            ExtendDeletedOnPropertyWithOtherConfigurations(Property(p => p.DeletedOn).HasColumnName(DeletedOnColumnName));
        }

        #region Overrideable Configuration Extensions

        protected virtual void ExtendIsDeletedPropertyWithOtherConfigurations(PrimitivePropertyConfiguration propertyConfig) { }

        protected virtual void ExtendDeletedOnPropertyWithOtherConfigurations(DateTimePropertyConfiguration propertyConfig) { }

        #endregion
    }
}
