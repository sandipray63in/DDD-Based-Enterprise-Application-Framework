using System.Data.Entity.ModelConfiguration.Configuration;
using Domain.Base.AddOnObjects;
using Domain.Base.Entities.Composites;

namespace DomainContextsAndMaps.Base.EFBase
{
    public abstract class BaseEntityAsSoftDeleteableRootMap<TId,TEntity> : BaseEntityRootMap<TId,TEntity>
        where TId : struct
        where TEntity : BaseEntityComposite<TId, SoftDeleteableInfo>
    {
        protected virtual string IsDeletedColumnName { get; } = "col_is_deleted";
        protected virtual string DeletedOnColumnName { get; } = "col_deleted_on";

        protected override void SetEntitySpecificProperties()
        {
            ExtendIsDeletedPropertyWithOtherConfigurations(Property(p => p.T1Data.IsDeleted).HasColumnName(IsDeletedColumnName));
            ExtendDeletedOnPropertyWithOtherConfigurations(Property(p => p.T1Data.DeletedOn).HasColumnName(DeletedOnColumnName));
        }

        #region Overrideable Configuration Extensions

        protected virtual void ExtendIsDeletedPropertyWithOtherConfigurations(PrimitivePropertyConfiguration propertyConfig) { }

        protected virtual void ExtendDeletedOnPropertyWithOtherConfigurations(DateTimePropertyConfiguration propertyConfig) { }

        #endregion
    }
}